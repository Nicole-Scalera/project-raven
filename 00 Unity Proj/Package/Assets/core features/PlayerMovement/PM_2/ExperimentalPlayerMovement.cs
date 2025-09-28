using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BasicMovement2_cf
{

    public class ExperimentalPlayerMovement : MonoBehaviour
    {

        [Header("Components")]
        public Rigidbody rb; //player object rigidbody

        [Header("Move Variables")]
        public float moveSpeed = 5.0f;
        public float xMovement; //left and right
        public float zMovement; //forward and back

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


        private void Update()
        {


            //Updates the players linearVelocity with new (inputted) values
            rb.linearVelocity = new Vector3(xMovement * moveSpeed, rb.linearVelocity.y, zMovement * moveSpeed);

            GroundCheck();

        }

        public void PlayerMove(InputAction.CallbackContext context)
        {

            //prints the input action and its details
            Debug.Log(context.action.ToString());

            //reads the x and z (left to right and forward and back) parts of the Vector3
            //assigns them to the corresponding movement variables
            xMovement = context.ReadValue<Vector3>().x;
            zMovement = context.ReadValue<Vector3>().z;

            if (isGrounded && jumpsRemaining > 0)
            {
                if (context.performed && context.ReadValue<Vector3>().y != 0)
                {

                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpPower, rb.linearVelocity.z);
                    jumpsRemaining--;

                }

            }

        }

        private void GroundCheck()
        {
            //if the player's y position is less than or equal to the ground coord
            if (groundCheckPos.position.y <= groundCoord)
            {
                //reset the jump amount and say the object is on the ground
                isGrounded = true;
                jumpsRemaining = maxJumps;

            }
            else
            {

                isGrounded = false;

            }
        }

    }
}