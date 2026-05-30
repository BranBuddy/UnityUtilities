/*
    This the the central state factorty class that creates the states and stores them for future calls

    Inspired by IHeartGameDev's Hierachal State Machine

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/


using System.Collections.Generic;

public class PlayerStateFactory
{
    PlayerStateMachine _context;
    // Holds all the states instead of creating new version for each call
    Dictionary<States, PlayerBaseState> _states = new Dictionary<States, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        if (!_states.ContainsKey(States.idle))
            _states.Add(States.idle, new PlayerIdleState(_context, this));
        return _states[States.idle];
    }

    public PlayerBaseState Walk()
    {
        if (!_states.ContainsKey(States.walk))
            _states.Add(States.walk, new PlayerWalkState(_context, this)); 
        return _states[States.walk];
    }

    public PlayerBaseState Run()
    {
        if (!_states.ContainsKey(States.run))
            _states.Add(States.run, new PlayerRunState(_context, this)); 
        return _states[States.run];
    }

    public PlayerBaseState Jump()
    {
        if (!_states.ContainsKey(States.jump))
            _states.Add(States.jump, new PlayerJumpState(_context, this)); 
        return _states[States.jump];
    }

    public PlayerBaseState Grounded()
    {
        if (!_states.ContainsKey(States.grounded))
            _states.Add(States.grounded, new PlayerGroundedState(_context, this)); 
        return _states[States.grounded];
    }

    public PlayerBaseState Fall()
    {
        if (!_states.ContainsKey(States.fall))
            _states.Add(States.fall, new PlayerFallState(_context, this)); 
        return _states[States.fall];
    }

    public PlayerBaseState Interact()
    {
        if (!_states.ContainsKey(States.interact))
            _states.Add(States.interact, new PlayerInteractState(_context, this)); 
        return _states[States.interact];
    }
}

public enum States { idle, walk, run, jump, grounded, fall, interact }
