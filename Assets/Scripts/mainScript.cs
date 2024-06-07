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
    public new CameraController camera;
    public TextMeshProUGUI buttonText;
    public Material placedMaterial;

    // Script variables
    private bool gameStarted = false;
    
    private float currentY = 1;
    private float previousPlatformX = 0;

    private Platform platform;
    private Platform previousPlatform;
    private float nextPlatformScale;

    private float platformSpeed = 5.0f;

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
            SpawnPlatform(5);
        } else {
            PlacePlatform();
        }
    }

    void PlacePlatform() {
        GameObject previousPlatformObject = GameObject.Find($"platform-{currentY - 1}");
        if (previousPlatformObject != null) {
            previousPlatform = previousPlatformObject.GetComponent<Platform>();
            float previousPlatformPos = previousPlatform.transform.position.x;
            float previousPlatformScale = previousPlatform.transform.localScale.x;

            platform.Place();
            Vector3 platformPosition = platform.transform.position;
            float currentPlatformScale = platform.transform.localScale.x;

            // Snap the platform to the previous one if it's close enough
            if (platformPosition.x < previousPlatformPos + currentPlatformScale / 10 && platformPosition.x > previousPlatformPos - currentPlatformScale / 10) {
                platformPosition.x = previousPlatformPos;
                platform.transform.position = platformPosition;
            }

            // Calculate the start and end positions of both platforms
            float previousPlatformStart = previousPlatformPos - previousPlatformScale / 2;
            float previousPlatformEnd = previousPlatformPos + previousPlatformScale / 2;
            float currentPlatformStart = platformPosition.x - currentPlatformScale / 2;
            float currentPlatformEnd = platformPosition.x + currentPlatformScale / 2;

            // Calculate the overlap
            float overlapStart = Mathf.Max(previousPlatformStart, currentPlatformStart);
            float overlapEnd = Mathf.Min(previousPlatformEnd, currentPlatformEnd);

            if (overlapStart < overlapEnd) {
                nextPlatformScale = overlapEnd - overlapStart;
            } else {
                nextPlatformScale = 0;
            }

            // Center the new platform on the overlap
            platformPosition.x = (overlapStart + overlapEnd) / 2;
            platform.transform.position = platformPosition;

            // Set the new scale
            Vector3 newScale = platform.transform.localScale;
            newScale.x = nextPlatformScale;
            platform.transform.localScale = newScale;

            // Update material to indicate placement
            Renderer platformRenderer = platform.GetComponent<Renderer>();
            platformRenderer.material = placedMaterial;

            // Move to next position
            platform = null;
            currentY += 1;
            platformSpeed += 0.3f;
            camera.MoveUp();
            SpawnPlatform(nextPlatformScale);
        }
    }

    void SpawnPlatform(float platformScale) {
        if (platformScale == 0) {
            GameOver();
        } else {
            GameObject previousPlatformObject = GameObject.Find($"platform-{currentY - 1}");
            if (previousPlatformObject != null) {
                previousPlatform = previousPlatformObject.GetComponent<Platform>();
                previousPlatformX = previousPlatform.transform.position.x;
            }

            platform = Instantiate(platformTemplate);
            platform.name = $"platform-{currentY}";
            platform.speed = platformSpeed;
            platform.transform.localScale = new Vector3(platformScale, 1, 5);
            platform.transform.position = new Vector3(previousPlatformX, currentY, 0);
        }
    }



    void CleanGame() {
        if (currentY >= 20) {
            GameObject oldPlatform = GameObject.Find($"platform-{currentY - 19}");
            Destroy(oldPlatform);
        }
    }

    void GameOver() {
        Application.Quit();
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
