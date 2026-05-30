/*
    Master class that establishes player input and character controller for all capabilites classes.
    Also enables and disables Gameplay AM
*/

using UnityEngine;

public class MasterInputClass : MonoBehaviour
{
    internal PlayerInput playerInput;
    internal CharacterController characterController;

    public CharacterController CharacterController {get {return characterController;}}

    public virtual void Awake()
    {
        playerInput = new PlayerInput();

        if (characterController == null)
        {
            characterController = this.GetComponent<CharacterController>();
        }
    }

    void OnEnable()
    {   
        playerInput.Gameplay.Enable();
    }

    void OnDisable()
    {
        playerInput.Gameplay.Disable();       
    }
}
