/*
    Walk player state that handles what occurs when player is walking. 

    Inspired by IHeartGameDev's Hierachal State Machine

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/

using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}

    // If conditions met switch into the proper substate
    public override void CheckSwitchState()
    {
        if (Ctx.PlayerInteract.IsInteractPressed && !Ctx.PlayerMovement.IsMovementPressed)
        {
            Ctx.PlayerInteract.CanInteract = false;
            SwitchState(Factory.Interact());
            return;
        }

        if (!Ctx.PlayerMovement.IsMovementPressed && !Ctx.PlayerInteract.IsInteractPressed)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.PlayerMovement.IsMovementPressed && Ctx.PlayerMovement.IsRunPressed && !Ctx.PlayerInteract.IsInteractPressed)
        {
            SwitchState(Factory.Run());
        }

    }

    public override void EnterState()
    {
        Debug.Log("Entering Walk");
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
        Ctx.PlayerMovement.AppliedMovementX = Ctx.PlayerMovement.CurrentMovementInput.x * Ctx.PlayerMovement.WalkSpeed;
        Ctx.PlayerMovement.AppliedMovementZ = Ctx.PlayerMovement.CurrentMovementInput.y * Ctx.PlayerMovement.WalkSpeed;
        CheckSwitchState();
    }

}
