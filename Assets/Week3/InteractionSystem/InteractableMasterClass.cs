/*
    Master class which all current/future interaction classes will inherit from. 
    
    Allows for Guid id generation and player movement pausing.
*/

using UnityEditor;
using UnityEngine;

public class InteractableMasterClass : MonoBehaviour, IInteractable
{
    [field: SerializeField, Tooltip("If you switched from sequential ids to not using it then back to using it, to get the id back highlight the id then press delete")]
    public int id {get; set; }
    [field: SerializeField] public bool interactable {get; set; }
    [field: SerializeField] public string interactionName {get; set;}

    [SerializeField] internal string interactText = $"Press E to interact";

    [Space(10), Header("Debugging")]
    [SerializeField] private bool useSequentialIds = true;

    public virtual void Start()
    {
        if (string.IsNullOrEmpty(Name))
            interactionName = this.name;

        interactText = $"Press E to interact with {Name}";
    }

    public bool Interactable => interactable = true;
    public int ID => id;
    public string Name => interactionName;
    public virtual void InteractAction(GameObject player)
    {
        InteractionTextHolder holder = player.GetComponent<InteractionTextHolder>();

        holder.DeactivateInteractText();
    }

    internal void PausePlayerMovement(GameObject player, bool pause)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        movement.pauseMovement = pause;
    }

    // Only generates id if bool is true
    public virtual void OnValidate()
    {
        #if UNITY_EDITOR
            if (useSequentialIds)
            {
                if (id == 0)
                    GenerateID();
            }
        #endif
    }

    // Creates new guid, gets the hash code, translates it into an int, then sets the id dirty so it is not reused
    public void GenerateID()
    {
       string tempId = System.Guid.NewGuid().ToString();

       id = tempId.GetHashCode() & 0x7FFFFFFF;

       if (id == 0) 
        id = 1;

        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
        #endif
    }
}
