/*
    Character interaction class which inherits from master class. Initiates dialouge from week 2 and pauses player movement
    while active. 
*/

using System.Collections;
using UnityEngine;

public class CharacterInteraction : InteractableMasterClass
{
    [SerializeField] private CharacterSO character;
    [SerializeField] private MasterDialougeClass dialougeTree;
    private Coroutine pauseMovement = null;

    public override void Start()
    {   
        base.Start();
    }

    public override void OnValidate()
    {
        base.OnValidate();

        #if UNITY_EDITOR
            interactionName = character.characterName;
            this.name = interactionName;
        #endif
    }

    public override void InteractAction(GameObject player)
    {
        base.InteractAction(player);

        dialougeTree.InitializeDialougeUI();

        pauseMovement = StartCoroutine(OnlyUnpausePlayWhenFinished(dialougeTree, player));

    }

    private IEnumerator OnlyUnpausePlayWhenFinished(MasterDialougeClass dialougeTree, GameObject player)
    {
        PausePlayerMovement(player, true);

        while (!dialougeTree.conservationEnded)
            yield return null;

        PausePlayerMovement(player, false);

        pauseMovement = null;
    }
}
