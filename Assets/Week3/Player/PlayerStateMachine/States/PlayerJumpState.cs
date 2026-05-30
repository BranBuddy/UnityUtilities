/*
    Jump player state that handles what occurs when player is jumping. In the future a "Airborne" root state will erase this
    and make this and fall into substates.

    Inspired by IHeartGameDev's Hierachal State Machine

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/

using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class PlayerJumpState : PlayerBaseState, IRootState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    // If jump reaches ground, switch state back
    public override void CheckSwitchState()
    {
        if (Ctx.PlayerMovement.CharacterController.isGrounded)
            SwitchState(Factory.Grounded());
    }

    public override void EnterState()
    {
        Debug.Log("Entering");
        InitializeSubState();
        HandleJump();
    }

    public override void ExitState()
    {
        if (CurrentSubState != null) 
            CurrentSubState.ExitState();

        if (Ctx.PlayerJump.IsJumpPressed)
            Ctx.PlayerJump.RequireNewJumpPress = true;

        if (Ctx.PlayerJump.CurrenJumpResetCoroutine == null && Ctx.PlayerJump.JumpCount > 0)
        {
            Ctx.PlayerJump.CurrenJumpResetCoroutine = Ctx.StartCoroutine(JumpResetCouroutine(Ctx.PlayerJump.WaitForJumpReset));
        }
        else
        {
            if (Ctx.PlayerJump.CurrenJumpResetCoroutine != null)
            {
                Ctx.StopCoroutine(Ctx.PlayerJump.CurrenJumpResetCoroutine);
                Ctx.PlayerJump.CurrenJumpResetCoroutine = null;
            }
        }

        if (Ctx.PlayerJump.JumpCount == 3)
            Ctx.PlayerJump.JumpCount = 0;
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

    private void HandleJump()
    {
        if (Ctx.PlayerJump.JumpCount < 3 && Ctx.PlayerJump.CurrenJumpResetCoroutine != null)
        {
            Ctx.StopCoroutine(Ctx.PlayerJump.CurrenJumpResetCoroutine);
        }

        Ctx.PlayerJump.IsJumping = true;

        Ctx.PlayerJump.JumpCount = Mathf.Clamp(Ctx.PlayerJump.JumpCount + 1, 1, 3);

        float newVelocity = Ctx.PlayerJump.InitialJumpValues[Ctx.PlayerJump.JumpCount];
        
        Ctx.PlayerMovement.CurrentMovementY = newVelocity;
        Ctx.PlayerMovement.AppliedMovement = newVelocity;
    }
    private IEnumerator JumpResetCouroutine(float waitTime) 
    {
        yield return new WaitForSeconds(waitTime);
        Ctx.PlayerJump.JumpCount = 0;
        Ctx.PlayerJump.CurrenJumpResetCoroutine = null;
    }

    // Inherits from IRootState, applies force to player can ascend then fall back down akin to a real jump
    public void HandleGravity()
    {
        bool isFalling = Ctx.PlayerMovement.CurrentMovementY <= 0.0f || !Ctx.PlayerJump.IsJumpPressed;
        float fallMultiplier = 2.0f;
        
        float currentGravity = Ctx.PlayerJump.JumpGravities[Ctx.PlayerJump.JumpCount];
        float prevYVel = Ctx.PlayerMovement.CurrentMovementY;

        if (isFalling)
        {
            Ctx.PlayerMovement.CurrentMovementY += currentGravity * fallMultiplier * Time.deltaTime;
        }
        else
        {
            Ctx.PlayerMovement.CurrentMovementY += currentGravity * Time.deltaTime;
        }
        float finalApplied = (prevYVel + Ctx.PlayerMovement.CurrentMovementY) * 0.5f;
        Ctx.PlayerMovement.AppliedMovement = Mathf.Max(finalApplied, -20.0f);
    }
}
