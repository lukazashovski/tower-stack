using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startMenu : MonoBehaviour
{
    public Button myButton;

    // Start is called before the first frame update
    void Start()
    {
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button not assigned in the Inspector");
        }
    }

    // Method to be called when the button is clicked
    void OnButtonClick()
    {
        Debug.Log("Button clicked!");
        // Perform your desired action here
    }

    // Update is called once per frame
    void Update()
    {
        // If you want to log something every frame, place it here.
    }
}
