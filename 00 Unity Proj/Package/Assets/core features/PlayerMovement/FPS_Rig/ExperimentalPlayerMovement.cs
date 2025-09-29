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
        private Player player; // Player.cs
        private PlayerControls playerControls; // PlayerControls.cs
        private CapsuleCollider playerCapsule; // Capsule Collider
        private PlayerInputProcessor inputProcessor; // PlayerInputProcessor.cs
        private PlayerInput playerInput; // PlayerInput component
        public CharacterController characterController; // CharacterController component
        // ================================

        // ========== Basic Movement ==========
        [Header("Move Variables")]
        private float playerSpeed; // The current speed of the Player
        private float SpeedChangeRate = 10.0f; // Acceleration rate
        public float moveSpeed = 5.0f; // Normal walking speed
        public float sprintSpeed = 7.0f; // Sprinting speed
        public Vector3 movement; //stores the movement input
        public float xMovement; //left and right
        public float zMovement; //forward and back
        public float yMovement;
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
                    return playerInput.currentControlScheme == "KeyboardMouse";
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
        }
        

        private void Start()
        {
            GetPlayerInfo(); // Get the Player's info
            
            // Get the PlayerInput component
            #if ENABLE_INPUT_SYSTEM
                playerInput = GetComponent<PlayerInput>();
            #else
			    Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
            #endif
        }
        
        private void GetPlayerInfo()
        {
            // Get the following information about the Player
            playerInput = player.GetPlayerInput(); // PlayerInput Component
            playerCapsule = player.GetPlayerCollider(); // Capsule Collider
            characterController = GetComponent<CharacterController>(); // CharacterController component
            inputProcessor = GetComponent<PlayerInputProcessor>(); // PlayerInputProcessor.cs
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
            movement = playerControls.Player.Move.ReadValue<Vector3>();
            xMovement = playerControls.Player.Move.ReadValue<Vector3>().x;
            zMovement = playerControls.Player.Move.ReadValue<Vector3>().z;
            jumpMovement = playerControls.Player.Move.ReadValue<Vector3>().y;
            yMovement = jumpMovement;
            
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = inputProcessor.sprint ? sprintSpeed : moveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (playerControls.Player.Move.ReadValue<Vector3>() == Vector3.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = inputProcessor.analogMovement ? inputProcessor.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                playerSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                playerSpeed = Mathf.Round(playerSpeed * 1000f) / 1000f;
            }
            else
            {
                playerSpeed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(inputProcessor.move.x, 0.0f, inputProcessor.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (inputProcessor.move != Vector3.zero)
            {
                // move
                inputDirection = transform.right * inputProcessor.move.x + transform.forward * inputProcessor.move.y;
            }

            // move the player
            characterController.Move(inputDirection.normalized * (playerSpeed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            
        }
        
        // Handles the rotation of the camera
        private void CameraRotation()
        {
            // if there is an input
            if (playerControls.Player.Look.ReadValue<Vector2>().sqrMagnitude >= threshold)
            {
                //Don't multiply mouse input by Time.deltaTime
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
                //_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
                _cinemachineTargetPitch += playerControls.Player.Look.ReadValue<Vector2>().y * RotationSpeed * deltaTimeMultiplier;
                _rotationVelocity = playerControls.Player.Look.ReadValue<Vector2>().x * RotationSpeed * deltaTimeMultiplier;

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
                    // Apply jump velocity
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