using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float groundDrag;
    public float airDrag;
    public float airMultiplier;
    bool readyToJump = true; // Initialize to true
    public float jumpCooldown;
    public float rollSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode backKey = KeyCode.S;
    public KeyCode rightKey = KeyCode.D;

    [Header("SFX")]
    public AudioSource jumpSFX;
    public AudioSource landSFX;
    public AudioClip jumpClip; // Add this line to declare an AudioClip

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;

    public Transform orientation;
    public Transform playerObj; // Reference to the PlayerObj

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Freeze only X and Z rotation

        // Assign the AudioClip to the AudioSource
        if (jumpSFX != null && jumpClip != null)
        {
            jumpSFX.clip = jumpClip;
        }
    }

    //UPDATE-----------------------------------------------------------------------------------------------------------
    void Update()
    {
        GroundCheck();
        MyInput();

        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }

        // Visual rotation
        RotateBall();
    }

    void GroundCheck()
    {
        // Perform multiple raycasts to check if the player is grounded
        Vector3[] raycastOrigins = new Vector3[]
        {
            transform.position,
            transform.position + new Vector3(playerHeight / 2, 0, 0),
            transform.position - new Vector3(playerHeight / 2, 0, 0),
            transform.position + new Vector3(0, 0, playerHeight / 2),
            transform.position - new Vector3(0, 0, playerHeight / 2)
        };

        isGrounded = false;
        foreach (var origin in raycastOrigins)
        {
            if (Physics.Raycast(origin, Vector3.down, playerHeight / 2 + 0.1f, whatIsGround))
            {
                isGrounded = true;
                break;
            }
        }
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

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void Jump()
    {
        if (jumpSFX != null)
        {
            jumpSFX.Play();
        }
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce * 10f, ForceMode.Impulse);
        Debug.Log("Jumped");
    }

    public void ResetJump()
    {
        readyToJump = true;
    }
    //-----------------------------------------------------------------------------------------------------------


    //FIXED UPDATE-----------------------------------------------------------------------------------------------------------
    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    void RotateBall()
    {
        if (rb.velocity != Vector3.zero)
        {
            float rotationSpeed = rb.velocity.magnitude * rollSpeed * 10; // Adjust the multiplier as needed
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, rb.velocity.normalized);
            playerObj.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
    //-----------------------------------------------------------------------------------------------------------
}