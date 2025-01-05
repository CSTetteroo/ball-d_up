using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{
    public Button RestartButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetTheGame()
    {
        // Reset the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Turn off the Restart button
        RestartButton.gameObject.SetActive(false);
    }
}
