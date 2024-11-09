
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour
{
    public Button startButton;
    public Button restartButton;
    public Button quitButton;
    public Button playAgainButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoText;
    public Canvas uiCanvas;
    public ARPlaneManager planeManager;
    public TargetSpawner targetSpawner;

    private int score = 0;
    private bool gameStarted = false;
    private List<GameObject> activeTargets = new List<GameObject>();

    // Long Tap Variables
    private bool isTouching = false;
    private float touchTime = 0f;
    private float longTapThreshold = 1.0f; // 1 second for long tap

    private void Start()
    {
        startButton.gameObject.SetActive(false);
        uiCanvas.gameObject.SetActive(false); 
        UpdateScore(0);
        CheckForPlanes();
    }

    private void Update()
    {
        // Detect a long tap
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isTouching = true;
                touchTime = 0f;
            }
            else if (touch.phase == TouchPhase.Stationary && isTouching)
            {
                touchTime += Time.deltaTime;

                // If the touch time exceeds the threshold, toggle UI visibility
                if (touchTime > longTapThreshold)
                {
                    ToggleGameUI();
                    isTouching = false; // Reset after toggling
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
            }
        }
    }

    private void CheckForPlanes()
    {
        if (!gameStarted)
        {
            startButton.gameObject.SetActive(planeManager.trackables.count > 0);
        }
    }

    public void OnPlaneSelected()
    {
        if (!gameStarted)
        {
            Debug.Log("Plane selected; displaying Start button and UI canvas.");
            uiCanvas.gameObject.SetActive(true);
            startButton.gameObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        HideGameUI();

        if (targetSpawner != null)
        {
            targetSpawner.SpawnTargets();
            activeTargets = targetSpawner.GetActiveTargets();
        }
        else
        {
            Debug.LogError("TargetSpawner not assigned in GameUIManager.");
        }
    }

    public void ShowGameUI()
    {
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    public void HideGameUI()
    {
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    private void ToggleGameUI()
    {
        bool isUIActive = startButton.gameObject.activeSelf;
        if (isUIActive)
        {
            HideGameUI();
        }
        else
        {
            ShowGameUI();
        }
    }

    public void RestartGame()
    {
        gameStarted = false;
        score = 0;
        UpdateScore(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayAgain()
    {
        score = 0;
        UpdateScore(0);
        ResetTargets();
        playAgainButton.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        ammoText.text = "Ammo: " + ammoCount;
        if (ammoCount <= 0 || AllTargetsEliminated())
        {
            DisplayPlayAgainButton();
        }
    }

    private void DisplayPlayAgainButton()
    {
        playAgainButton.gameObject.SetActive(true);
    }

    private void ResetTargets()
    {
        foreach (GameObject target in activeTargets)
        {
            if (target != null)
            {
                Destroy(target);
            }
        }
        activeTargets.Clear();

        if (targetSpawner != null)
        {
            targetSpawner.SpawnTargets();
            activeTargets = targetSpawner.GetActiveTargets();
        }
    }

    private bool AllTargetsEliminated()
    {
        foreach (GameObject target in activeTargets)
        {
            if (target != null && target.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }
}
