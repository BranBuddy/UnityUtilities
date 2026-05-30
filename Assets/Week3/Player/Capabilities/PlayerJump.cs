/*
    This is the class that handles player jump and the physics involved. Also sets up conditions for root states

    Inspired by IHeartGameDev's Jump video

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class PlayerJump : MasterInputClass
{
    [Space(10), Header("Gravity Variables")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float fallSpeed = .75f;

    [Space(10), Header("Jump Variables")]
    [SerializeField] private float intialJumpVelocity;
    [SerializeField] private float maxJumpHeight = 1.0f;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float waitForJumpReset;

    private bool requireNewJumpPress;
    private bool isJumpPressed = false;
    private bool isJumping = false;

    private int jumpCount;

    internal Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> jumpGravities = new Dictionary<int, float>();

    private Coroutine currentJumpResetCoroutine = null;

    // These dictionaries are used for if you want varied jump values and gravities for consecutive jumps
    public Dictionary<int, float> InitialJumpValues { get {return initialJumpVelocities; }}
    public Dictionary<int, float> JumpGravities { get {return jumpGravities; }}

    public Coroutine CurrenJumpResetCoroutine {get { return currentJumpResetCoroutine; } set {currentJumpResetCoroutine = value; }}

    public int JumpCount {get {return jumpCount;} set {jumpCount = value; }}
    
    public bool IsJumping { set { isJumping = value;}}
    public bool IsJumpPressed { get {return isJumpPressed;}}
    public bool RequireNewJumpPress {get {return requireNewJumpPress;} set {requireNewJumpPress = value;}}
    
    public float WaitForJumpReset {get {return waitForJumpReset; } set {waitForJumpReset = value;}}
    public float Gravity {get {return gravity; } set {gravity = value; }}
    public float FallSpeed {get {return fallSpeed;}}

    public override void Awake()
    {
        base.Awake();

        playerInput.Gameplay.Jump.started += OnJump;
        playerInput.Gameplay.Jump.canceled += OnJump;

        SetUpJumpVariables();
    }

    private void OnJump (InputAction.CallbackContext ctx)
    {
        isJumpPressed = ctx.ReadValueAsButton(); 
        requireNewJumpPress = false;
        Debug.Log($"isJumpPressed : {IsJumpPressed} and requireNewJump {RequireNewJumpPress}");
    }

    // Gravity, velocity, etc. get applied once at runtime based on real life calculations
    private void SetUpJumpVariables()
    {
         float timeToApex = maxJumpTime / 2.0f;

        float initialGravity = (-2 * maxJumpHeight) / (timeToApex * timeToApex);
        intialJumpVelocity = (2 * maxJumpHeight) / timeToApex;

        initialJumpVelocities.Clear();
        initialJumpVelocities.Add(1, intialJumpVelocity);


        jumpGravities.Clear();
        jumpGravities.Add(0, initialGravity);
        jumpGravities.Add(1, initialGravity);

    }

}
