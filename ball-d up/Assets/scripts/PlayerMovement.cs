using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float groundDrag;
    public float airMultiplier;
    bool readyToJump = true;
    public float JumpCooldown;
    public Vector3 delta;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;



    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f, whatIsGround);
        MyInput();

        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
   
        Move();
    }

    public void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), JumpCooldown);
        }
    }

    public void Move()
    {
        if (horizontalInput != 0 || verticalInput != 0)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            delta = moveDirection.normalized * moveSpeed;

            if (isGrounded)
            {
                delta = moveDirection.normalized * moveSpeed;
            }
            else
            {
                delta = moveDirection.normalized * moveSpeed * airMultiplier;
            }
        }
        else
        {
            delta = Vector3.zero; // Set delta to zero when no input is detected
        }

        rb.velocity = delta;
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jumped");
        }
    }

    public void ResetJump()
    {
        readyToJump = true;
    }

}
