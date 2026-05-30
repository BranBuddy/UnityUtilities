/*
    This is class holds all variables and functions for player movement. Also handles player rotation,
    along with changing variabes based on if in run or walk state

    Inspired by IHeartGameDev's Movement Setup

    URL to Playlist: https://www.youtube.com/playlist?list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy 
*/

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MasterInputClass
{
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private Vector3 appliedMovement;

    private bool isRunPressed;
    private bool isMovementPressed;

    public bool IsMovementPressed {get {return isMovementPressed; }}
    public bool IsRunPressed {get {return isRunPressed;}}

    public float CurrentMovementY { get { return currentMovement.y; } set {currentMovement.y = value; } }
    public float AppliedMovement { get { return appliedMovement.y; } set {appliedMovement.y = value; } }
    public float AppliedMovementX {get {return appliedMovement.x;} set {appliedMovement.x = value;}}
    public float AppliedMovementZ {get {return appliedMovement.z;} set {appliedMovement.z = value;}}
    public float RunSpeed {get {return runSpeed;}}
    public float WalkSpeed {get {return walkSpeed;}}
    public bool PauseMovement {get {return pauseMovement;}}

    public Vector2 CurrentMovementInput { get {return currentMovementInput;}}

    [Header("Movement Variables")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    internal bool pauseMovement;

    [Space(10), Header("Rotation Variables")]
    [SerializeField] private float rotFactorPerFrame = 1.0f;

    [Space(10), Header("Ground Check Variables")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private float groundCheckRadius = 0.3f; 

    public override void Awake()
    {
        base.Awake(); 
        playerInput.Gameplay.Move.started += OnMovementInput;
        playerInput.Gameplay.Move.canceled += OnMovementInput;
        playerInput.Gameplay.Move.performed += OnMovementInput;

        playerInput.Gameplay.Run.started += OnRun;
        playerInput.Gameplay.Run.canceled += OnRun;
    }

    private void Start()
    {
        characterController.Move(appliedMovement * Time.deltaTime);
    }

    private void Update()
    {
        // if the movement is paused, return. used for certain interactions
        if (pauseMovement)
            return;

        HandleRotation();
        
        // if in run state the applied movement gets increased
        if (isRunPressed)
        {
            appliedMovement.x = currentRunMovement.x;
            appliedMovement.z = currentRunMovement.z;
        }
        else
        {
            appliedMovement.x = currentMovement.x;
            appliedMovement.z = currentMovement.z;  
        }

        characterController.Move(appliedMovement * Time.deltaTime);
    }

    private void OnMovementInput (InputAction.CallbackContext ctx)
    {
        currentMovementInput = ctx.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkSpeed;
        currentMovement.z = currentMovementInput.y * walkSpeed;

        currentRunMovement.x = currentMovementInput.x * runSpeed;
        currentRunMovement.z = currentMovementInput.y * runSpeed;

        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void OnRun(InputAction.CallbackContext ctx)
    {
        isRunPressed = ctx.ReadValueAsButton();
    }

    private void HandleRotation()
    {
        Vector3 posToLook;

        posToLook.x = currentMovement.x;
        posToLook.y = 0.0f;
        posToLook.z = currentMovement.z;

    
        Quaternion currentRot = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRot = Quaternion.LookRotation(posToLook);
            transform.rotation = Quaternion.Slerp(currentRot, targetRot, rotFactorPerFrame * Time.deltaTime);
        }
    }

    public bool CheckIsGrounded()
    {
        Vector3 sphereOrigin = transform.position + CharacterController.center;
        float rayDistance = (CharacterController.height / 2f) - CharacterController.radius + groundCheckDistance;
        return Physics.SphereCast(sphereOrigin, groundCheckRadius, Vector3.down, out RaycastHit hit, rayDistance, groundLayer);
    }
}
