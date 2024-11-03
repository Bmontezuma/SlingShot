using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class GameUIManager : MonoBehaviour
{
    public Button startButton;
    public Button restartButton;
    public Button quitButton;
    public Button playAgainButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoText;
    public ARPlaneManager planeManager;
    public TargetSpawner targetSpawner;

    private int score = 0;
    private int ammoCount = 7;
    private int maxAmmoCount = 7;
    private bool gameStarted = false; // Track if the game has started

    private void Start()
    {
        startButton.gameObject.SetActive(false);
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
            startButton.gameObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        InitializeAmmo();
        startButton.gameObject.SetActive(false);
        gameStarted = true; // Prevent further activation of the start button

        // Call SpawnTargets from TargetSpawner to ensure targets spawn on start
        if (targetSpawner != null)
        {
            targetSpawner.SpawnTargets();
        }
        else
        {
            Debug.LogError("TargetSpawner not assigned in GameUIManager.");
        }
    }

    public void RestartGame()
    {
        gameStarted = false; // Reset game state
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

    private void InitializeAmmo()
    {
        // Logic to initialize ammo
    }

    private void ResetTargets()
    {
        // Logic to reset all targets
    }

    private bool AllTargetsEliminated()
    {
        return false; // Replace with actual logic
    }
}
