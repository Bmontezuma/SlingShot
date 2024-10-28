using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public Button startButton;
    public Button restartButton;
    public Button quitButton;
    public Button playAgainButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoText;

    private int score = 0;
    private int ammoCount = 7;
    private int maxAmmoCount = 7;
    private bool planeSelected = false;

    private void Start()
    {
        startButton.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(false); // Hide Play Again button initially
        UpdateScore(0);
        UpdateAmmo();
    }

    public void OnPlaneSelected()
    {
        planeSelected = true;
        startButton.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        InitializeAmmo();
        startButton.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        planeSelected = false;
        score = 0;
        ammoCount = maxAmmoCount;
        UpdateScore(0);
        UpdateAmmo();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the scene
    }

    public void PlayAgain()
    {
        score = 0;
        ammoCount = maxAmmoCount;
        UpdateScore(0);
        UpdateAmmo();
        ResetTargets(); // Reset all targets without reselecting the plane
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
        // Instantiate the first Ammo instance in the center of the screen or handle Ammo initialization here
        // For example: Instantiate(ammoPrefab, initialPosition, Quaternion.identity);
    }

    private void ResetTargets()
    {
        // Logic to reset all targets, either by repositioning them or reinitializing them as needed.
    }

    private bool AllTargetsEliminated()
    {
        // Implement a check here to return true if all targets are eliminated, otherwise false.
        return false; // Replace with actual logic
    }
}
