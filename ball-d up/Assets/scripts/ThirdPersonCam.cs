using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation; // Reference to the orientation transform
    public Transform player; // Reference to the player transform
    public Transform playerObj; // Reference to the PlayerObj transform
    public Rigidbody rb; // Reference to the player's Rigidbody

    public float rotationSpeed; // Speed of rotation for the playerObj

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the view direction based on the player's position and the camera's position
        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        // Set the orientation's forward direction to the view direction
        orientation.forward = viewDirection.normalized;

        // Get input from the horizontal and vertical axes
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // Calculate the input direction based on the orientation and input values
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    }
}