using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class startMenu : MonoBehaviour
{
    public Button mainButton;
    Boolean gameStarted = false;
    float currentY = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (mainButton != null)
        {
            mainButton.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button not assigned in the Inspector");
        }
    }

    // Method to be called when the button is clicked
    void OnButtonClick()
    {
        TextMeshProUGUI buttonText = mainButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null && gameStarted == false)
        {
            buttonText.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            gameStarted = true;
        } 
        else
        {
            Debug.LogError("Button has no text child");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If you want to log something every frame, place it here.
    }
}
