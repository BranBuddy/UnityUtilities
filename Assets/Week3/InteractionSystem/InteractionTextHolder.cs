/*
   Holds player's interaction text and handles activation/deactivation
*/

using TMPro;
using UnityEngine;

public class InteractionTextHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactText;

    public void ActivateInteractText(string interactionText)
    {
        interactText.gameObject.SetActive(true);

        interactText.text = interactionText;
    }

    public void DeactivateInteractText()
    {
        interactText.text = "";

        interactText.gameObject.SetActive(false);
    }
}
