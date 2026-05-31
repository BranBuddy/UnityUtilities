/*
    Master dialouge tree managing class. Handles the setting up of UI, text assignments, node handling, etc.
    Will eventually be the base for other types of dialouge types like shops and quests.
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using Unity.Mathematics;

public class MasterDialougeClass : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField, Tooltip("Assign the desired dialouge tree SO")] public DialougeTreeSO dialougeSO;

    [SerializeField, Tooltip("Assign the desired character SO")] public CharacterSO characterSO;

    [Header("Temporary Go to Next Node Wait")]
    [SerializeField] private float waitTime = 1.5f;


    // UI Controller
    private DialougeUIController dialougeUIController;
    // All nodes in current dialouge tree
    private List<DialougeNode> nodes = new List<DialougeNode>();
    // Response buttons for current node
    private List<GameObject> responseButtons = new List<GameObject>();

    private int currentNode = 0;
    // The selected response node
    private int responseNode = 0;
    private int maxNodes;

    internal bool conservationEnded = false;

    protected virtual void Start()
    {
        if (dialougeSO.dialougeTree.Length != 0)
            maxNodes = dialougeSO.dialougeTree.Length;

        // If you dont want to load the feeling score, reload character SO
        if (!characterSO.loadFeelingScore)
        {
            characterSO.feelingScore = characterSO.defaultFeelingScore;
            characterSO.feelingTowardPlayer = characterSO.defaultFeelingTowardPlayer;
            GiveCharacterFeelingScoreBaseOnDefault(characterSO);
        }
        
    }

    // Go to next dialouge node
    private void GoToNextNode()
    {
        if (conservationEnded)
            return;

        currentNode++;

        StartCoroutine(ShowOptionalTextCoroutine());

        // If max is reached, end here
        if (currentNode == maxNodes)
        {
            CleanUpButtons();
            return;
        }

        RefreshButtons();

    }

    // If using default, set the character feeling score to the baseline for each feeling
    private void GiveCharacterFeelingScoreBaseOnDefault(CharacterSO characterSO)
    {
        var feeling = characterSO.feelingTowardPlayer;
        var feelingScore = characterSO.feelingScore;
        switch (feeling)
        {
            case FeelingTowardPlayer.Neutral:
                feelingScore += 0;
                break;
            case FeelingTowardPlayer.Like:
                feelingScore += 5;
                break;
            case FeelingTowardPlayer.Love:
                feelingScore += 15;
                break;
            case FeelingTowardPlayer.Hate:
                feelingScore += -15;
                break;
            case FeelingTowardPlayer.Dislike:
                feelingScore -= 5;
                break;
        }

        characterSO.feelingScore = feelingScore;
    }

    // Add/Subtracts from character feeling score based on player response
    private void AdjustCharacterFeelingScoreToResponse(CharacterSO characterSO, DialougePlayerResponse response)
    {
        Connotation connotation = response.connotation;

        switch (connotation)
        {
            case Connotation.Neutral:
                break;

            case Connotation.Positive:
                characterSO.feelingScore += 5;
                break;

            case Connotation.Negative:
                characterSO.feelingScore += -5;
                break;

            default:
                break;
        }


        AdjustFeelingToScore(characterSO);
        ChangeSpriteBasedOnConnotation(dialougeSO);

        Debug.Log($"Feeling: {characterSO.feelingTowardPlayer}, Score: {characterSO.feelingScore}");
    }    

    // Change the chatacters sprite based on the response's connotation
    private void ChangeSpriteBasedOnConnotation(DialougeTreeSO treeSO)
    {
        Image characterImage = dialougeUIController.characterImage;
        if (characterSO == null)
            return;

        var connotationToUse = nodes[currentNode].responses[responseNode].connotation;

        switch (connotationToUse)
        {
            case Connotation.Neutral:
                characterImage.sprite = characterSO.defaultCharacterSprite;
                break;
            case Connotation.Positive:
                characterImage.sprite = characterSO.happyCharacterSprite;
                break;
            case Connotation.Negative:
                characterImage.sprite = characterSO.angryCharacterSprite;
                break;
            default:
                characterImage.sprite = characterSO.defaultCharacterSprite;
                break;
        }
    }

    // Change the feeling based on its score
    private void AdjustFeelingToScore(CharacterSO characterSO)
    {
        if (characterSO.feelingScore > -5 && characterSO.feelingScore < 5)
        {
            characterSO.feelingTowardPlayer = FeelingTowardPlayer.Neutral;
        }
        else if (characterSO.feelingScore < -5 && characterSO.feelingScore > -15)
        {
            characterSO.feelingTowardPlayer = FeelingTowardPlayer.Dislike;
        }
        else if (characterSO.feelingScore < -15)
        {
            characterSO.feelingTowardPlayer = FeelingTowardPlayer.Hate;
        }
        else if (characterSO.feelingScore > 5 && characterSO.feelingScore < 15)
        {
            characterSO.feelingTowardPlayer = FeelingTowardPlayer.Like;
        }
        else if (characterSO.feelingScore > 15)
        {
            characterSO.feelingTowardPlayer = FeelingTowardPlayer.Love;            
        }
    }

    // If the conversation is one time only, display the last message always when the players go for another interaction
    private void DisplayOneTimeEndingMessage(DialougeTreeSO treeSO, CharacterSO characterSO)
    {
        DialougeNode lastNode = treeSO.dialougeTree[treeSO.dialougeTree.Length - 1];

        DialougePlayerResponse responseToUse = lastNode.responses[treeSO.finalNode];

        var textToUse = responseToUse.optionialResponseText;

        dialougeUIController.dialougeText.text = textToUse;

        Connotation responseConnotation = responseToUse.connotation;

        switch (responseConnotation)
        {
            case Connotation.Positive:
                dialougeUIController.characterImage.sprite = characterSO.happyCharacterSprite;
                break;
            case Connotation.Neutral:
                dialougeUIController.characterImage.sprite = characterSO.defaultCharacterSprite;
                break;
            case Connotation.Negative:
                dialougeUIController.characterImage.sprite = characterSO.angryCharacterSprite;
                break;
            default:
                dialougeUIController.characterImage.sprite = characterSO.defaultCharacterSprite;
                break;
        }

        
    }

    // Check if the dialouge tree is one time
    private bool CheckIfOneTimeConversation(DialougeTreeSO treeSO)
    {
        if (treeSO.oneTimeConversation)
            return true;
        
        return false;
    }

    #region UI Helpers
    // Gets info from characterSO and assigns it
    private void BringOverCharacterSO(CharacterSO characterSO)
    {
        if (characterSO == null)
            return;


        if (characterSO.defaultCharacterSprite != null) 
            dialougeUIController.characterImage.sprite = characterSO.defaultCharacterSprite;

        if (characterSO.characterName != null)
            dialougeUIController.characterName.text = characterSO.characterName;

    }

    // Startup function that gathers all other helper functions
    public void InitializeDialougeUI()
    {
        GetUIController();

        dialougeUIController.UIVisibility(true);

        BringOverCharacterSO(characterSO);

        // if the one time conversation is up, just show the last message
        if (dialougeSO.oneTimeConversation && dialougeSO.conservationSpent)
        {
            StartCoroutine(ShowOneTimeMessageThenCleanUp());
            return;
        }

        conservationEnded = false;

        dialougeSO.finalNode = 0;

        AddNodesToList(dialougeSO);

        ShowRespectiveNodesText(currentNode);

        RefreshButtons();

    }

    // Gets the dialouge ui controller
    private void GetUIController()
    {
        if (dialougeUIController == null)
            dialougeUIController = DialougeUIController.Instance;
    }

    // Show the correct text based on current node
    private void ShowRespectiveNodesText(int node)
    {
        if (nodes.Count == 0)
            return;

        if (node == maxNodes)
            return;
        
        dialougeUIController.dialougeText.text = nodes[node].text;
    }

    // Adds all of the dialouge tree's nodes to the list
    private void AddNodesToList(DialougeTreeSO nodeSO)
    {
        foreach (DialougeNode node in nodeSO.dialougeTree)
        {
            nodes.Add(node);
        }
    }

    // Creates the reponse buttons for the current node
    private void SetUpResponseButton(GameObject responseButton, DialougeNode currentNode)
    {
        GameObject prefabToSpawn = dialougeUIController.responseButtonPrefab;

        int amountOfButtonsToMake = currentNode.responses.Length; 
        
        for (int i = 0; i < amountOfButtonsToMake; i++) 
        { 
            
            GameObject makeButton = Instantiate(prefabToSpawn, dialougeUIController.responseButtonContainter.transform, worldPositionStays: false); 
            
            Button buttonComponent = makeButton.GetComponent<Button>();

            if (buttonComponent != null)
            {
                AssignOnClickEventForResponseButtons(currentNode.responses[i], buttonComponent); 
                AssignOnClickEvents(buttonComponent); 
            }
            else
            {
                Debug.LogError("The responseButtonPrefab does not have a Button component attached!");
            }

            makeButton.name = "Response_" + i; 
            responseButtons.Add(makeButton); 
        }
    }

    // Fires at the end of the conversation to initiate cleanup
    private void StopConversation()
    {
        if (FindResponseWithEnd(dialougeSO) == null)
            return;

        dialougeSO.finalNode = responseNode;

        if (nodes[currentNode].responses[responseNode].endConversation)
        {
            CleanUpButtons();
            StartCoroutine(ShowOptionalTextThenCleanUp());
            conservationEnded = true;
        }

        if (CheckIfOneTimeConversation(dialougeSO))
            dialougeSO.conservationSpent = true;

    }

    // Central clean up ui function
    private void CleanUpUI()
    {
        CleanUpButtons();
        currentNode = 0;
        dialougeUIController.dialougeContainer.SetActive(false);
        dialougeUIController.dialougeText.text = "";
        dialougeUIController.characterName.text = "";
        dialougeUIController.characterImage.sprite = null;
    }

    // Find if any response will end the conversation
    private DialougePlayerResponse FindResponseWithEnd(DialougeTreeSO tree)
    {
        DialougePlayerResponse response = null;
        foreach (DialougePlayerResponse playerResponse in tree.dialougeTree[currentNode].responses)
        {
            if (playerResponse.endConversation == false)
                continue;
            else    
                response = playerResponse;
        }

        if (response == null)
            return null;

        return response;
    }

    // Refresh button states for next node
    private void RefreshButtons()
    {
        CleanUpButtons();
        SetUpResponseButton(dialougeUIController.responseButtonPrefab, nodes[currentNode]);
        SetResponseButtonText(currentNode);

    }

    // Cleanly replace buttons
    private void CleanUpButtons()
    {
        if (responseButtons.Count == 0)
            return;

        List<GameObject> buttonsToRemove = responseButtons;
        foreach (var button in buttonsToRemove)
        {
            Destroy(button);
        }
        responseButtons.Clear();
    }

    // Assign the text to each response
    private void SetResponseButtonText(int currentNode)
    {
        int responseNode = 0;
        foreach (var button in responseButtons)
        {
            Button buttonComp = button.GetComponent<Button>();
            TextMeshProUGUI text = buttonComp.GetComponentInChildren<TextMeshProUGUI>();
            text.text = nodes[currentNode].responses[responseNode].text;
            nodes[currentNode].responses[responseNode].responseIndex = responseNode;
            responseNode++;
        }
    }

    // After making a response, the character will say something. if it will show
    private bool ShowOptionalText(DialougePlayerResponse optionalText)
    {
        if (!string.IsNullOrEmpty(optionalText.optionialResponseText))
        {
            dialougeUIController.dialougeText.text = optionalText.optionialResponseText;
            return true;
        }
        else
        {
            Debug.Log("Optional text is null or empty or not being found");
            return false;
        }

    }

    // Coroutine so it goes to next one automatically, if theres no optional text, go to next node
    private IEnumerator ShowOptionalTextCoroutine()
    {
        
        if (ShowOptionalText(nodes[currentNode - 1].responses[responseNode]))
        {
            ShowOptionalText(nodes[currentNode - 1].responses[responseNode]);
            yield return new WaitForSeconds(waitTime);
            ShowRespectiveNodesText(currentNode);
        }
        else
        {
            ShowRespectiveNodesText(currentNode);  
        }
        
    }

    private IEnumerator ShowOptionalTextThenCleanUp()
    {
       ShowOptionalText(nodes[currentNode].responses[responseNode]);
       yield return new WaitForSeconds(waitTime); 
       CleanUpUI();
    }

    private IEnumerator ShowOneTimeMessageThenCleanUp()
    {
        DisplayOneTimeEndingMessage(dialougeSO, characterSO);
        yield return new WaitForSeconds(waitTime); 
       CleanUpUI();
    }

    // Assigns the response node based on which response clicked
    private void AssignResponseNodeByClick(GameObject response)
    {
        if (!responseButtons.Contains(response)) {
            Debug.Log("Does not contain response");
            return;
        }

        int indexOfButton = responseButtons.IndexOf(response);

        Debug.Log(indexOfButton);

        responseNode = indexOfButton;
    }

    private void AssignOnClickEventForResponseButtons(DialougePlayerResponse response, Button button)
    {

        foreach (ButtonEvents buttonEvent in response.buttonEvents) 
        { 
            ButtonEvents currentEventData = buttonEvent;
            
            if(currentEventData.buttonEvent.GetPersistentEventCount() == 0)
            {
                Debug.LogWarning($"[Dialogue System] Button '{button.name}' was assigned an event, but the event has 0 functions hooked up in the inspector!");
            }

            button.onClick.AddListener(() => {
                Debug.Log($"[Dialogue System] Button physically clicked! Invoking event now...");
                currentEventData.buttonEvent.Invoke(); 
            }); 
        }
    }

    // Adds listener onclick events to buttons
    protected virtual void AssignOnClickEvents(Button button)
    {
        button.onClick.AddListener(() => AssignResponseNodeByClick(button.gameObject));
        button.onClick.AddListener(() => AdjustCharacterFeelingScoreToResponse(characterSO, nodes[currentNode].responses[responseNode]));
        button.onClick.AddListener(StopConversation);
        button.onClick.AddListener(GoToNextNode);

    }

    #endregion
}
