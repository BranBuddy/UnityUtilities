/*
    Grounded player state that handles what occurs when player is Grounded. 

    Inspired by IHeartGameDev's Hierachal State Machine

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/

using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    // Inherited from IRootState, ensures that a downward force is always applied when grounded to prevent floating
    public void HandleGravity()
    {
        Ctx.PlayerMovement.CurrentMovementY = -2.0f;
        Ctx.PlayerMovement.AppliedMovement = Ctx.PlayerMovement.CurrentMovementY; 
    }

    // If conditions met switch into the proper substate
    public override void CheckSwitchState()
    {
        if (Ctx.PlayerJump.IsJumpPressed && !Ctx.PlayerJump.RequireNewJumpPress && !Ctx.PlayerMovement.PauseMovement)
        {   
            Debug.Log("Jump");
            SwitchState(Factory.Jump());
        }
        else if (!Ctx.PlayerMovement.CharacterController.isGrounded)
        {
            SwitchState(Factory.Fall());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        Debug.Log("Entering Ground");
        HandleGravity();
    }

    public override void ExitState()
    {
        if (CurrentSubState != null) 
            CurrentSubState.ExitState();
    }

    // If conditions met switch into the proper substate
    public override void InitializeSubState()
    {
        if (!Ctx.PlayerMovement.IsMovementPressed && !Ctx.PlayerMovement.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.PlayerMovement.IsMovementPressed && !Ctx.PlayerMovement.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    
}
