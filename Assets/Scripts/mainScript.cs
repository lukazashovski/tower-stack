using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class mainScript : MonoBehaviour
{
    // Variables from Scene
    public Button mainButton;
    public Platform platformTemplate;
    public CameraController camera;
    public TextMeshProUGUI buttonText;
    public Material placedMaterial;

    // Script variables
    private bool gameStarted = false;
    
    private float currentY = 1;
    private float previousPlatformX = 0;

    private Platform platform;
    private Platform previousPlatform;

    void Start()
    {
        mainButton.onClick.AddListener(OnBtnClick);
    }
    
    void OnBtnClick()
    {
        if (!gameStarted)
        {
            buttonText.gameObject.SetActive(false);
            gameStarted = true;
            SpawnPlatform();
        } else {
            PlacePlatform();
        }
    }

    void SpawnPlatform() {
        GameObject previousPlatformObject = GameObject.Find($"platform-{currentY - 1}");
        previousPlatform = previousPlatformObject.GetComponent<Platform>();
        previousPlatformX = previousPlatform.transform.position.x;

        // get platform size based on placed position - NEXT TO-DO


        platform = Instantiate(platformTemplate);
        platform.name = $"platform-{currentY}";
        platform.transform.position = new Vector3(previousPlatformX, currentY, 0);
    }

    void PlacePlatform() {
        float previousPlatformPos = 0f;

        GameObject previousPlatformObject = GameObject.Find($"platform-{currentY - 1}");
        previousPlatform = previousPlatformObject?.GetComponent<Platform>();
        previousPlatformPos = previousPlatform.transform.position.x;

        platform.Place();
        Vector3 platformPosition = platform.transform.position;
        if (platformPosition.x < previousPlatformPos + 0.4f && platformPosition.x > previousPlatformPos - 0.4f) {
            platformPosition.x = previousPlatformPos;
            platform.transform.position = platformPosition;
        }
        
        Renderer platformRenderer = platform.GetComponent<Renderer>();
        platformRenderer.material = placedMaterial;
        
        platform = null;
        currentY += 1;
        camera.MoveUp();
        SpawnPlatform();
    }

    void CleanGame() {
        if (currentY >= 20) {
            GameObject oldPlatform = GameObject.Find($"platform-{currentY - 19}");
            Destroy(oldPlatform);
        }
    }

    void Update() {
        CleanGame();
    }

    /* OLD SCRIPT
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

        if (platformTemplate == null)
        {
            Debug.LogError("Platform template not assigned in the Inspector");
        }

        if (buttonText == null)
        {
            Debug.LogError("Button text not assigned in the Inspector");
        }
    }

    void OnButtonClick()
    {
        if (!gameStarted)
        {
            StartGame();
        }
        else
        {
            PlacePlatform();
        }
    }

    void StartGame()
    {
        if (buttonText != null)
        {
            buttonText.gameObject.SetActive(false);
        }

        gameStarted = true;
        SpawnNewPlatform();
    }

    void PlacePlatform()
    {
        if (currentPlatform != null)
        {
            currentPlatform.StopMoving();
            // position of platform when placed Debug.Log(currentPlatform.transform.position.x);
            currentPlatform = null;
            currentY += 1;
            camera.MoveUp();
            SpawnNewPlatform();
        }
    }

    void SpawnNewPlatform()
    {
        currentPlatform = Instantiate(platformTemplate);
        currentPlatform.transform.position = new Vector3(0, currentY, 0);
        // Check if previousPlatform is not null before using it
        if (previousPlatform != null)
        {
            if (currentPlatform.transform.position.x > previousPlatform.transform.position.x)
            {
                Vector3 newScale = currentPlatform.transform.localScale;
                newScale.x -= currentPlatform.transform.position.x - previousPlatform.transform.position.x;
                currentPlatform.transform.localScale = newScale;
            }
            else
            {
                Vector3 newScale = currentPlatform.transform.localScale;
                newScale.x -= previousPlatform.transform.position.x + currentPlatform.transform.position.x;
                currentPlatform.transform.localScale = newScale;
            }
        }
    }
    */
}
