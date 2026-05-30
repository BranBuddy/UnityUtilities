/*
    Idle player state that handles what occurs when player is Idle. 

    Inspired by IHeartGameDev's Hierachal State Machine

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
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

        if (Ctx.PlayerMovement.IsMovementPressed && Ctx.PlayerMovement.IsRunPressed && !Ctx.PlayerMovement.PauseMovement 
        && !Ctx.PlayerInteract.IsInteractPressed)
        {
            SwitchState(Factory.Run());
        }
        else if (Ctx.PlayerMovement.IsMovementPressed && !Ctx.PlayerMovement.PauseMovement && !Ctx.PlayerInteract.IsInteractPressed)
        {
            SwitchState(Factory.Walk());
        }
        
    }

    public override void EnterState()
    {
        Ctx.PlayerMovement.AppliedMovementX = 0;
        Ctx.PlayerMovement.AppliedMovementZ = 0;
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


}
