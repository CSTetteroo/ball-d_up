using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class death : MonoBehaviour
{
    public Button DeathButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        // Turn on the Death button
        DeathButton.gameObject.SetActive(true);
        // Log a message to the console bc i needed to check if the trigger worked
        Debug.Log("You die!");
    }
}
