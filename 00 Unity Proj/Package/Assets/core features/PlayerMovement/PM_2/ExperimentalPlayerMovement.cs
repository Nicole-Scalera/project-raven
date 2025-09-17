using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BasicMovement2_cf
{

    public class ExperimentalPlayerMovement : MonoBehaviour
    {

        // ========== Components ==========
        [Header("Components")]
        public Rigidbody rb; // Rigidbody
        // ================================

        // ========== Basic Movement ==========
        [Header("Move Variables")]
        public float moveSpeed = 5.0f;
        public float xMovement; //left and right
        public float zMovement; //forward and back
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
        
        private void Update()
        {
            // Updates linearVelocity with new (inputted) values
            rb.linearVelocity = new Vector3(xMovement * moveSpeed, rb.linearVelocity.y, zMovement * moveSpeed);

            // Check if the player is on the ground
            GroundCheck();
        }

        // Controls movement and jumping system
        public void PlayerMove(InputAction.CallbackContext context)
        {

            // Prints the input action and its details
            Debug.Log(context.action.ToString());

            // Reads the X (left/right) and Z (forward/back) variables from the
            // Vector3 & assigns them to the corresponding movement variables
            xMovement = context.ReadValue<Vector3>().x;
            zMovement = context.ReadValue<Vector3>().z;

            if (isGrounded && jumpsRemaining > 0)
            {
                if (context.performed && context.ReadValue<Vector3>().y != 0)
                {

                    Debug.Log("JUMP");

                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpPower, rb.linearVelocity.z);
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