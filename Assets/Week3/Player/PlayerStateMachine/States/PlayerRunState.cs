using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
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
        else if (Ctx.PlayerMovement.IsMovementPressed && !Ctx.PlayerMovement.IsRunPressed 
        && !Ctx.PlayerMovement.PauseMovement && !Ctx.PlayerInteract.IsInteractPressed)
        {
            SwitchState(Factory.Walk());
        }

    }

    public override void EnterState()
    {
        
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
        Ctx.PlayerMovement.AppliedMovementX = Ctx.PlayerMovement.CurrentMovementInput.x * Ctx.PlayerMovement.RunSpeed;
        Ctx.PlayerMovement.AppliedMovementZ = Ctx.PlayerMovement.CurrentMovementInput.y * Ctx.PlayerMovement.RunSpeed;
        CheckSwitchState();
    }

}
