using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.LowLevelPhysics;

#if UNITY_EDITOR
using UnityEngine.InputSystem.Editor;
#endif

namespace FPS_Rig_cf
{
    public class ExperimentalPlayerMovement : MonoBehaviour
    {

        // ========== Components ==========
        [Header("Components")]
        public Rigidbody rb; // Rigidbody
        [SerializeField] public PlayerControls playerControls;
#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private Player player; // Player.cs
        public GameObject camera; // Main Camera
        //private StarterAssetsInputs _input;
        private GameObject mainCamera;
        public Transform orientation;
        public CapsuleCollider playerCapsule; // Capsule Collider
        // ================================

        // ========== Basic Movement ==========
        [Header("Move Variables")]
        public float moveSpeed = 5.0f;
        public Vector3 movement; //stores the movement input
        public float xMovement; //left and right
        public float zMovement; //forward and back
        public float yMovement;

        [Header("Orientation Variables")]
        public Vector2 lookDirection;
        private float rotationX;
        private float rotationY;
        // ====================================

        // ========== Jumping ==========
        //variables to control the players jump
        //not fully implemented yet
        [Header("Jump")]
        public float jumpPower = 5f;
        private float _verticalVelocity;
        public float jumpMovement;
        public int maxJumps = 1;
        public int jumpsRemaining;
        public bool jumpActive; // Am I currently jumping?
        public float JumpHeight = 1.2f; // The height the player can jump
        private float _terminalVelocity = 53.0f;
        
        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        [Header("GroundCheck")]
        public Transform groundCheckPos;
        public Vector3 groundCheckSize = new Vector3(0.5f, 0.05f, 0.5f);
        public LayerMask groundLayer;
        public bool isGrounded;
        public float groundCoord = 1.9f;

        [Header("Gravity")]
        public float baseGravity = 2f;
        public float maxFallSpeed = 18f;
        public float fallMultiplier = 1f;
        
        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.1f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;
        // =============================
        
        // ========== Cinemachine ==========
        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 90.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -90.0f;
        [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 20.0f;
        private float _rotationVelocity;
        
        private const float threshold = 0.01f;
        
        // cinemachine
        private float _cinemachineTargetPitch;
        
        // This is to check if the current control scheme is "KeyboardMouse"
        // which means the user is using a mouse We make this check to ensure
        // we don't multiply mouse input by Time.deltaTime.
        private bool IsCurrentDeviceMouse
        {
            get
            {
                #if ENABLE_INPUT_SYSTEM
                    return _playerInput.currentControlScheme == "KeyboardMouse";
                #else
				    return false;
                #endif
            }
        }
        // =================================
        
        private void Awake()
        {
            // ===== Player =====
            player = Player.Instance; // Access Player.cs
            playerControls = Player.Controls; // Access movement controls
            _playerInput = GetComponent<PlayerInput>(); // Access PlayerInput component
            playerCapsule = GetComponent<CapsuleCollider>();
        }
        
        private void Start()
        {
            #if ENABLE_INPUT_SYSTEM
                _playerInput = GetComponent<PlayerInput>();
            #else
			    Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
            #endif
            
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }

            // Get the rotation of the 
            orientation = playerCapsule.transform;
        }
        
        private void FixedUpdate()
        {
            // Updates linearVelocity with new (inputted) values
            //rb.linearVelocity = new Vector3(xMovement * moveSpeed, rb.linearVelocity.y, zMovement * moveSpeed);
            
            PlayerMove(); // Check for movement
            GroundCheck(); // Check if the player is on the ground
            PlayerJump();
        }
        
        private void LateUpdate()
        {
            CameraRotation();
        }

        // Controls movement and jumping system
        public void PlayerMove()
        {
            // Read movement input
            movement = playerControls.PlayerMove.Move.ReadValue<Vector3>();
            xMovement = playerControls.PlayerMove.Move.ReadValue<Vector3>().x;
            zMovement = playerControls.PlayerMove.Move.ReadValue<Vector3>().z;
            jumpMovement = playerControls.PlayerMove.Move.ReadValue<Vector3>().y;
            yMovement = jumpMovement;
            
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            // Read look input (vertical)
            _cinemachineTargetPitch += playerControls.PlayerMove.Look.ReadValue<Vector2>().y * RotationSpeed * deltaTimeMultiplier;
            // Read look input (horizontal)
            _rotationVelocity = playerControls.PlayerMove.Look.ReadValue<Vector2>().x * RotationSpeed * deltaTimeMultiplier;
            
            //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            
            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            transform.Rotate(Vector3.up * _rotationVelocity);
            
            // // normalise input direction
            // Vector3 inputDirection = new Vector3(xMovement, 0.0f, yMovement).normalized;
            //
            // rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            
            //_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            //rb.Move(inputDirection.normalized * (moveSpeed * Time.deltaTime)3 + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            //rb.MovePosition(transform.position + inputDirection.normalized * (moveSpeed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            
            // Prints the input action and its details
            // Debug.Log(context.action.ToString());
            // camera.transform.position = new Vector3(transform.position.x, player.transform.position.y + 3.17f, transform.position.z);

            // Reads the X (left/right) and Z (forward/back) variables from the
            // Vector3 & assigns them to the corresponding movement variables

            
            // rotationX = playerControls.PlayerMove.Look.ReadValue<Vector2>().x;
            // rotationY = playerControls.PlayerMove.Look.ReadValue<Vector2>().y;
        }
        
        // Handles the rotation of the camera
        private void CameraRotation()
        {
            // if there is an input
            if (playerControls.PlayerMove.Look.ReadValue<Vector2>().sqrMagnitude >= threshold)
            {
                //Don't multiply mouse input by Time.deltaTime
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
                //_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
                _cinemachineTargetPitch += playerControls.PlayerMove.Look.ReadValue<Vector2>().y * RotationSpeed * deltaTimeMultiplier;
                _rotationVelocity = playerControls.PlayerMove.Look.ReadValue<Vector2>().x * RotationSpeed * deltaTimeMultiplier;

                // clamp our pitch rotation
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                // Update Cinemachine camera target pitch
                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                // rotate the player left and right
                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }
        
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void PlayerJump()
        {
            if (isGrounded && jumpsRemaining > 0)
            {
                if (jumpMovement != 0)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpPower, rb.linearVelocity.z);
                    //camera.transform.position = new Vector3(transform.position.x, player.transform.position.y + 3.17f, transform.position.z);
                    jumpsRemaining--;
                }
            }
        }
        
        // Checks if the player is on the ground
        private void GroundCheck()
        {
            // If the Player's Y-position is less than or
            // equal to the ground coordinate
            if (groundCheckPos.position.y <= groundCoord)
            {
                isGrounded = true; // Player is on the ground
                jumpsRemaining = maxJumps; // Reset the jumps
            }
            else
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * baseGravity);
                isGrounded = false; // Player is not on the ground
            }
        }
    }
}