/*
    This the the central state machine class that creates the factory and sets initial state/update state checks

    Inspired by IHeartGameDev's Hierachal State Machine

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/

using UnityEngine;
public class PlayerStateMachine : MonoBehaviour
{

    // private setters
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InteractController playerInteract;

    // public getters/setters
    public PlayerBaseState CurrentState { get {return _currentState;} set {_currentState = value; }}

    public PlayerJump PlayerJump {get {return playerJump;}}
    public PlayerMovement PlayerMovement {get {return playerMovement;}}
    public InteractController PlayerInteract {get {return playerInteract;}}
    
    // Creates factory and establishes inital state
    void Awake()
    {
        _states = new PlayerStateFactory(this);

        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    // Checks if the current state for any update
    private void LateUpdate()
    {
        _currentState.UpdateStates();
    }

}
