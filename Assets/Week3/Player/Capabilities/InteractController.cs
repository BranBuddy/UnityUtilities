/*
    This class holds all setter and getter variables that relate to interacting, as well as setting up crucical steps for interact state
*/

using UnityEngine;
using UnityEngine.InputSystem;

public class InteractController : MasterInputClass
{

    // As seem below, whatever interactable trigger the player enters will be set to this
    [SerializeField] private InteractableMasterClass currentInteractable = null;
    // Delay between consecutive input presses
    [SerializeField] private float inputBusyDelay;
    private Coroutine inputBusyCoroutine;
    private bool canInteract = true;
    private bool isInteractPressed;    
    private InteractionTextHolder interactionTextHolder;

    public InteractableMasterClass CurrentInteractable {get {return currentInteractable;}}
    public float InputBusyDelay {get {return inputBusyDelay;}}
    public Coroutine InputBusyCoroutine {get {return inputBusyCoroutine;} set {inputBusyCoroutine = value;}}
    public bool CanInteract {get {return canInteract;} set {canInteract = value;}}
    public InteractionTextHolder InteractionTextHolder {get {return interactionTextHolder; }}
    public bool IsInteractPressed {get {return isInteractPressed; } set {isInteractPressed = value;}}

    public override void Awake()
    {
        base.Awake();

        playerInput.Gameplay.Interact.performed += OnInteract;
        playerInput.Gameplay.Interact.canceled += OnInteract;

        interactionTextHolder = GetComponent<InteractionTextHolder>();

    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        isInteractPressed = ctx.ReadValueAsButton();

        if (currentInteractable == null)
            return;

        if (!currentInteractable.interactable)
            return;
        
        if (!canInteract)
            return;

        Debug.Log("Interacted with " + currentInteractable);

        
    }

    // Both On Triggers will gather the current interactable and display proper text
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InteractableMasterClass>(out var interactable))
        {
            currentInteractable = interactable;
            interactionTextHolder.ActivateInteractText(currentInteractable.interactText);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<InteractableMasterClass>(out var interactable))
        {
            currentInteractable = interactable;
            interactionTextHolder.ActivateInteractText(currentInteractable.interactText);
        }   
    }

    // Both on exits will turn off text and set current to null
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InteractableMasterClass>(out var interactable))
        {
            interactionTextHolder.DeactivateInteractText();
            currentInteractable = null;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<InteractableMasterClass>(out var interactable))
        {
            interactionTextHolder.DeactivateInteractText();
            currentInteractable = null;
        }
    }

}
