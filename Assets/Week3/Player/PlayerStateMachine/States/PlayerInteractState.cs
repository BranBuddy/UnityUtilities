/*
    Interact player state that handles what occurs when player interacts with something. 

    While using IHeartDevs machine, this state was created by yours truly

*/

using UnityEngine;
using System.Collections;
public class PlayerInteractState : PlayerBaseState
{
    public PlayerInteractState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}

    // If conditions met switch into the proper substate
    public override void CheckSwitchState()
    {
         if (Ctx.PlayerInteract.CanInteract && !Ctx.PlayerInteract.IsInteractPressed)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.PlayerMovement.IsMovementPressed && !Ctx.PlayerInteract.IsInteractPressed)
        {
            SwitchState(Factory.Walk());
        }
    }


    // When entering the state initiate the interaction, reset InteractPressed bool, and start input delay
    public override void EnterState()
    {
        Ctx.PlayerInteract.CurrentInteractable.InteractAction(UniversalPlayerReference.Instance.player);

        Ctx.PlayerInteract.IsInteractPressed = false;

        if (Ctx.PlayerInteract.InputBusyCoroutine != null)
            Ctx.PlayerInteract.StopCoroutine(Ctx.PlayerInteract.InputBusyCoroutine);
        
        Ctx.PlayerInteract.InputBusyCoroutine = Ctx.PlayerInteract.StartCoroutine(StartInputBusy(Ctx.PlayerInteract.InputBusyDelay));

    }

    public override void ExitState()
    {
        if (CurrentSubState != null) 
            CurrentSubState.ExitState();
    }

    public override void InitializeSubState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    private IEnumerator StartInputBusy(float waitTime)
    {
        Ctx.PlayerInteract.CanInteract = false;
        Debug.Log("Input busy");
        yield return new WaitForSeconds(waitTime);
        Ctx.PlayerInteract.CanInteract = true;
        Debug.Log("Can Interact");
        Ctx.PlayerInteract.InputBusyCoroutine = null;
    }
}
