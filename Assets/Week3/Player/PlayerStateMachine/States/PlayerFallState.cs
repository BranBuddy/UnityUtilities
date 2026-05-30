/*
    Fall player state that handles what occurs when player is falling. In the future a "Airborne" root state will usurp this
    making fall and jump substates.

    Inspired by IHeartGameDev's Hierachal State Machine

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/

using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    // If the player reaches the ground switch state
    public override void CheckSwitchState()
    {
        if (Ctx.PlayerMovement.CheckIsGrounded())
            SwitchState(Factory.Grounded());
    }

    public override void EnterState()
    {
        InitializeSubState();
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
        HandleGravity();
        CheckSwitchState();        
    }

    // Func inherited from IRootState. This causes a downward force to force players to the ground.
    public void HandleGravity()
    {
        float prevYVel = Ctx.PlayerMovement.CurrentMovementY;
         Ctx.PlayerMovement.CurrentMovementY += Ctx.PlayerJump.Gravity * Time.deltaTime;
        float smoothedY = (prevYVel + Ctx.PlayerMovement.CurrentMovementY) * Ctx.PlayerJump.FallSpeed;
        Ctx.PlayerMovement.AppliedMovement = Mathf.Max(smoothedY, -20.0f); 
    }

}
