using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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


    private void Update()
    {


        //Updates the players linearVelocity with new (inputted) values
        rb.linearVelocity = new Vector3(xMovement * moveSpeed, jumpMovement * jumpPower, zMovement * moveSpeed);

        //part of the jump to ensure that the player doesn't jump forever
        //not fully implemented yet
        if (transform.position.y <= 1.9)
        {

            jumpsRemaining = maxJumps;

        }

    }

    public void PlayerMove(InputAction.CallbackContext context)
    {

        //prints the input action and its details
        Debug.Log(context.action.ToString());

        //reads the x and z (left to right and forward and back) parts of the Vector3
        //assigns them to the corresponding movement variables
        xMovement = context.ReadValue<Vector3>().x;
        zMovement = context.ReadValue<Vector3>().z;

        /*
         * 
         *   More incomplete jumping logic
         *   
         *   ISSUE: player can hold the jump button (space) to infinitely jump until releasing the button
         * 
         *   if(jumpsRemaining > 0)
         *   {
         *       if(context.performed)
         *       {
         *
         *           jumpMovement = context.ReadValue<Vector3>().y;
         *           jumpsRemaining--;
         *
         *       }
         *   
         *   }
         */

    }

}
