using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{

    public Button WinButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is called when the player collides with the finish line
    void OnTriggerEnter(Collider other)
    {
        // Turn on the Win button
        WinButton.gameObject.SetActive(true);
        // Log a message to the console bc i needed to check if the trigger worked
        Debug.Log("You win!");
    }
}
