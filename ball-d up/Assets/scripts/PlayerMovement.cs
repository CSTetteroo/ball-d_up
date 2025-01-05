using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; // Speed of the player
    public float jumpForce; // Force applied for jumping
    public float groundDrag; // Drag applied when on the ground
    public float airDrag; // Drag applied when in the air
    public float airMultiplier; // Multiplier for movement speed in the air
    bool readyToJump = true; // Flag to check if the player is ready to jump
    public float jumpCooldown; // Cooldown time between jumps
    public float rollSpeed; // Speed of the ball's visual roll

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space; // Key to jump
    public KeyCode forwardKey = KeyCode.W; // Key to move forward
    public KeyCode leftKey = KeyCode.A; // Key to move left
    public KeyCode backKey = KeyCode.S; // Key to move backward
    public KeyCode rightKey = KeyCode.D; // Key to move right
    public KeyCode pauseKey = KeyCode.Escape; // Key to pause the game

    [Header("SFX")]
    public AudioSource jumpSFX; // Audio source for jump sound effect
    public AudioSource landSFX; // Audio source for landing sound effect
    public AudioClip jumpClip; // Audio clip for jump sound effect

    [Header("Ground Check")]
    public float playerHeight; // Height of the player for ground check
    public LayerMask whatIsGround; // Layer mask to identify ground
    bool isGrounded; // Flag to check if the player is grounded

    public Transform orientation; // Reference to the orientation transform
    public Transform playerObj; // Reference to the PlayerObj

    float horizontalInput; // Horizontal input value
    float verticalInput; // Vertical input value

    Vector3 moveDirection; // Direction of movement

    Rigidbody rb; // Reference to the Rigidbody component

    public Button RestartButton; // Reference to the Restart button

    // Start is called before the first frame update
    void Start()
    {
        //get rigidbudy component
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Freeze only X and Z rotation

        // Assign the AudioClip to the AudioSource
        if (jumpSFX != null && jumpClip != null)
        {
            jumpSFX.clip = jumpClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded
        GroundCheck();

        // Handle all player input
        MyInput();

        // Apply drag based on whether the player is grounded
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

    // Perform multiple raycasts to check if the player is grounded
    void GroundCheck()
    {
        // Array of raycast origins
        Vector3[] raycastOrigins = new Vector3[]
        {
            // Four corners of the player
            transform.position,
            transform.position + new Vector3(playerHeight / 2, 0, 0),
            transform.position - new Vector3(playerHeight / 2, 0, 0),
            transform.position + new Vector3(0, 0, playerHeight / 2),
            transform.position - new Vector3(0, 0, playerHeight / 2)
        };

        isGrounded = false;
        // Do raycasts from all origins help this took too long to figure out
        foreach (var origin in raycastOrigins)
        {
            if (Physics.Raycast(origin, Vector3.down, playerHeight / 2 + 0.1f, whatIsGround))
            {
                isGrounded = true;
                break;
            }
        }
    }

    // Handle player input
    public void MyInput()
    {
        // This does all the input lmao i didn't know it was that easy
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Pause
        if (Input.GetKeyDown(pauseKey))
        {
            // Toggle the Restart button
            if (RestartButton.gameObject.activeSelf)
            {
                // Restart the game
                RestartButton.gameObject.SetActive(false);
                // make cursor invis
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                // make cursor visible
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                RestartButton.gameObject.SetActive(true);
            }
        }
    }

    // Perform the jump action
    public void Jump()
    {
        if (jumpSFX != null)
        {
            // Play the jump sound effect
            jumpSFX.Play();
        }
        // Add force to the rigidbody to jump
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce * 10f, ForceMode.Impulse);
        Debug.Log("Jumped");
    }

    // Reset the jump flag
    public void ResetJump()
    {
        // Set the flag to true
        readyToJump = true;
    }

    // FixedUpdate is for physics -------------------------------------------
    void FixedUpdate()
    {
        Move();
    }

    // Handle player movement
    public void Move()
    {
        // get the move direction based on the orientation of the player
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
        {
            // Apply force to the rigidbody to move the player on floor
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            // Apply force to the rigidbody to move the player in the air
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    // Rotate the ball visually based on its velocity
    void RotateBall()
    {
        // if the player is moving
        if (rb.velocity != Vector3.zero)
        {
            // Rotate the player object based on the velocity
            float rotationSpeed = rb.velocity.magnitude * rollSpeed * 10; // Adjust the multiplier as needed
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, rb.velocity.normalized);
            // Rotate the visible player object
            playerObj.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}