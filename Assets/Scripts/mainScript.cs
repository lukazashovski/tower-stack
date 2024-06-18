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
    public TextMeshPro scoreText;
    public Material placedMaterial;
    public GameObject scoreObject;

    // Game Over
    public GameObject gameOverFrame;
    public TextMeshProUGUI highScoreValue;
    public Button tryAgainButton;
    public Button toMenuButton;

    // Script variables
    private float currentY = 1;
    private float previousPlatformX = 0;

    private float scoreTextY = 957;

    private Platform platform;
    private Platform previousPlatform;
    private float nextPlatformScale;

    private float platformSpeed = 5.0f;

    private bool gameStarted = false;
    private float gameScore = 0;

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
            if (nextPlatformScale == 0) {
                newScale.z = 0;
            }
            platform.transform.localScale = newScale;

            // Update material to indicate placement
            Renderer platformRenderer = platform.GetComponent<Renderer>();
            platformRenderer.material = placedMaterial;

            // Move to next position
            platform = null;
            platformSpeed += 0.3f;
            currentY += 1;

            if (nextPlatformScale > 0) {
                SpawnPlatform(nextPlatformScale);
                gameScore += 1;
                scoreTextY += 1;
                scoreObject.transform.position = new Vector3(540, scoreTextY);
                camera.MoveUp();
            } else {
                GameOver();
            }
        }
    }

    void SpawnPlatform(float platformScale) {
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



    void CleanGame() {
        if (currentY >= 20) {
            GameObject oldPlatform = GameObject.Find($"platform-{currentY - 19}");
            Destroy(oldPlatform);
        }
    }

void GameOver() {
    Debug.Log("Game Over");
    if (currentY > highScoreValue) {
        highScoreValue = currentY;
    }
    gameOverFrame.SetActive(true);
    mainButton.enabled = false;
}


    void Update() {
        scoreText.text = gameScore.ToString();
        CleanGame();
    }
}
