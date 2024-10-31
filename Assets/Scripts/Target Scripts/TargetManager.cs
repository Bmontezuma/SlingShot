using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    public GameObject startButton; // Assign the Start Button in Inspector

    void Start()
    {
        if (startButton == null)
        {
            Debug.LogError("Start Button not assigned in Inspector!");
            return;
        }

        // Subscribe to the static plane selection event to show the Start button
        PlaneSelectionManager.onPlaneSelected += ShowStartButton;

        // Ensure the Start button is hidden at the beginning
        startButton.SetActive(false);
    }

    private void ShowStartButton(ARPlane selectedPlane)
    {
        startButton.SetActive(true); // Show the Start button after plane selection
        Debug.Log("Start button is now visible on selected plane: " + selectedPlane.trackableId);
    }

    // This method will be called when the Start button is pressed
    public void OnStartButtonPressed()
    {
        Debug.Log("Start button pressed; game starting.");
        // Add your logic here to begin the game or load a new scene
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        PlaneSelectionManager.onPlaneSelected -= ShowStartButton;
    }
}
