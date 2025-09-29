using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementTutorial : MonoBehaviour
{
    // [Header("Movement")]
    // // public float moveSpeed; // Already defined
    // //
    // // public float groundDrag;
    // //
    // // public float jumpForce; // Already defined
    // // public float jumpCooldown; // Already defined
    // // public float airMultiplier; // Already defined
    // // bool readyToJump; // Already defined
    // //
    // // [HideInInspector] public float walkSpeed; // Already defined
    // // [HideInInspector] public float sprintSpeed; // Already defined
    //
    // [Header("Keybinds")]
    // public KeyCode jumpKey = KeyCode.Space; // Already defined
    //
    // [Header("Ground Check")]
    // public float playerHeight;
    // public LayerMask whatIsGround; // Already defined
    // bool grounded; // Already defined

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection; // Already defined

    Rigidbody rb; // Already defined

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // readyToJump = true;
    }

    private void Update()
    {
        // // ground check
        // grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        //SpeedControl();

        // // handle drag
        // if (grounded)
        //     rb.linearDamping = groundDrag;
        // else
        //     rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // // when to jump
        // if(Input.GetKey(jumpKey) && readyToJump && grounded)
        // {
        //     readyToJump = false;
        //
        //     Jump();
        //
        //     Invoke(nameof(ResetJump), jumpCooldown);
        // }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // // on ground
        // if(grounded)
        //     rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        //
        // // in air
        // else if(!grounded)
        //     rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    // private void SpeedControl()
    // {
    //     Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    //
    //     // limit velocity if needed
    //     if(flatVel.magnitude > moveSpeed)
    //     {
    //         Vector3 limitedVel = flatVel.normalized * moveSpeed;
    //         rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
    //     }
    // }

    // private void Jump()
    // {
    //     // reset y velocity
    //     rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    //
    //     rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    // }
    // private void ResetJump()
    // {
    //     readyToJump = true;
    // }
}