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
    private int ammoCount = 7;
    private int maxAmmoCount = 7;
    private bool gameStarted = false;
    private List<GameObject> activeTargets = new List<GameObject>(); // Track active targets

    private void Start()
    {
        startButton.gameObject.SetActive(false);
        uiCanvas.gameObject.SetActive(false); 
        UpdateScore(0);
        UpdateAmmo();

        CheckForPlanes();
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
        InitializeAmmo();
        startButton.gameObject.SetActive(false);
        gameStarted = true;

        if (targetSpawner != null)
        {
            targetSpawner.SpawnTargets();
            activeTargets = targetSpawner.GetActiveTargets(); // Get active targets from TargetSpawner
        }
        else
        {
            Debug.LogError("TargetSpawner not assigned in GameUIManager.");
        }
    }

    public void RestartGame()
    {
        gameStarted = false;
        score = 0;
        ammoCount = maxAmmoCount;
        UpdateScore(0);
        UpdateAmmo();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayAgain()
    {
        score = 0;
        ammoCount = maxAmmoCount;
        UpdateScore(0);
        UpdateAmmo();
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

    public void UpdateAmmo()
    {
        ammoText.text = "Ammo: " + ammoCount;

        if (ammoCount <= 0 || AllTargetsEliminated())
        {
            DisplayPlayAgainButton();
        }
    }

    public void AmmoLaunched()
    {
        if (ammoCount > 0)
        {
            ammoCount--;
            UpdateAmmo();
        }

        if (ammoCount <= 0)
        {
            DisplayPlayAgainButton();
        }
    }

    private void DisplayPlayAgainButton()
    {
        playAgainButton.gameObject.SetActive(true);
    }

    // Initializes ammo count and other ammo-related settings at the start of the game
    private void InitializeAmmo()
    {
        ammoCount = maxAmmoCount;
        UpdateAmmo();
    }

    // Resets targets by destroying each one in the activeTargets list and clearing the list
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
            targetSpawner.SpawnTargets(); // Respawn targets
            activeTargets = targetSpawner.GetActiveTargets(); // Refresh active targets list
        }
    }

    // Checks if all targets in the activeTargets list have been eliminated
    private bool AllTargetsEliminated()
    {
        // Ensure that all elements are either null or inactive
        foreach (GameObject target in activeTargets)
        {
            if (target != null && target.activeInHierarchy)
            {
                return false; // At least one target is still active
            }
        }
        return true; // All targets have been eliminated
    }
}
