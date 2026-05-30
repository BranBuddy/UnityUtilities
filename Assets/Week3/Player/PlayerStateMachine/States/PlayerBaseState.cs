/*
    Base player state that gives these abstract functions and variables to whoever inherits

    Inspired by IHeartGameDev's Hierachal State Machine

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/


public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    protected bool IsRootState {set {_isRootState = value;}}
    protected PlayerStateMachine Ctx {get {return _ctx;}}
    protected PlayerStateFactory Factory {get {return _factory;}}
    protected PlayerBaseState CurrentSubState { get  {return _currentSubState; }}

    public PlayerBaseState(PlayerStateMachine currentCtx, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentCtx;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public abstract void InitializeSubState();

    // Checks current state if update state is necessary
    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    // Changes super/sub state depending on which state is being checked
    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();

        newState.EnterState();

        if (_isRootState)
            _ctx.CurrentState = newState;
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState);

        
    }
    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState)
    {
        if(_currentSubState != null) _currentSubState.ExitState();
    
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

}
