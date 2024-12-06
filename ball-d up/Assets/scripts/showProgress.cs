using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showProgress : MonoBehaviour
{
    public Transform player;
    public Text progressText;
    // Update is called once per frame
    void Update()
    {
        progressText.text = "Progress: " + player.position.y.ToString("0");
    }
}
