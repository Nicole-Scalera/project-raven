using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



namespace FPS_Rig_cf
{
    public class ExperimentalPlayerMovement : MonoBehaviour
    {

        // ========== Components ==========
        [Header("Components")]
        public Rigidbody rb; // Rigidbody
        [SerializeField] public PlayerControls playerControls;
        private Player player; // Player.cs
        public GameObject camera; // Main Camera
        // ================================

        // ========== Basic Movement ==========
        [Header("Move Variables")]
        public float moveSpeed = 5.0f;
        public float xMovement; //left and right
        public float zMovement; //forward and back

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
        public float jumpMovement;
        public int maxJumps = 1;
        public int jumpsRemaining;

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
        // =============================
        
        private void Awake()
        {
            // ===== Player =====
            player = Player.Instance; // Access Player.cs
            playerControls = Player.Controls; // Access movement controls
        }
        
        private void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
        }
        
        private void FixedUpdate()
        {
            // Updates linearVelocity with new (inputted) values
            rb.linearVelocity = new Vector3(xMovement * moveSpeed, rb.linearVelocity.y, zMovement * moveSpeed);
            rb.transform.rotation = Quaternion.Euler(rotationX * lookDirection.x, rotationY * lookDirection.y, 0);
            camera.transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);

            // Adjust the camera angle
            // transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
            
            // Check if the player is on the ground
            GroundCheck();
            
            // Check for movement
            PlayerMove();
        }

        // Controls movement and jumping system
        public void PlayerMove()
        {
            // Prints the input action and its details
            // Debug.Log(context.action.ToString());
            camera.transform.position = new Vector3(transform.position.x, player.transform.position.y + 3.17f, transform.position.z);

            // Reads the X (left/right) and Z (forward/back) variables from the
            // Vector3 & assigns them to the corresponding movement variables
            xMovement = playerControls.PlayerMove.Move.ReadValue<Vector3>().x;
            zMovement = playerControls.PlayerMove.Move.ReadValue<Vector3>().z;
            jumpMovement = playerControls.PlayerMove.Move.ReadValue<Vector3>().y;
            
            rotationX = playerControls.PlayerMove.Look.ReadValue<Vector2>().x;
            rotationY = playerControls.PlayerMove.Look.ReadValue<Vector2>().y;

            if (isGrounded && jumpsRemaining > 0)
            {
                if (jumpMovement != 0)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpPower, rb.linearVelocity.z);
                    camera.transform.position = new Vector3(transform.position.x, player.transform.position.y + 3.17f, transform.position.z);
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
                // Player is not on the ground
                isGrounded = false;
            }
        }

    }
}