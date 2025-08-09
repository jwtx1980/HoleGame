using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    public float timeLeft = 60f; // Timer duration
    public TextMeshProUGUI timerText; // UI text for time
    public TextMeshProUGUI remainingBoxesText; // UI text for remaining boxes
    public AudioSource gameOverSound; // Sound when time runs out
    public AudioSource winSound; // Sound when player wins
    public PlayerMovement playerMovement; // Reference to the player's movement (the hole)
    public Button continueButton; // Continue button
    private bool gameEnded = false;

    public CubeSpawner cubeSpawner; // Reference to CubeSpawner

    void Start()
    {
        // Hide the Continue button at the start
        continueButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameEnded)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
            {
                timeLeft = 0;
                EndGame(false); // Player loses
            }

            timerText.text = "Time Left: " + Mathf.Ceil(timeLeft).ToString();
            UpdateRemainingBoxes();
        }

        // Check win condition: if no cubes remain, the player wins
        if (!gameEnded && GameObject.FindGameObjectsWithTag("Swallowable").Length == 0)
        {
            EndGame(true); // Player wins
        }
    }

    void UpdateRemainingBoxes()
    {
        int remaining = GameObject.FindGameObjectsWithTag("Swallowable").Length;
        remainingBoxesText.text = "Remaining: " + remaining;
    }

    void EndGame(bool playerWon)
    {
        gameEnded = true;
        playerMovement.enabled = false; // Disable player movement

        if (playerWon)
        {
            timerText.text = "You Win!";
            if (winSound != null)
                winSound.Play();
            // Show the Continue button when the player wins
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            timerText.text = "Time’s Up!";
            if (gameOverSound != null)
                gameOverSound.Play();
            ResetDifficulty(); // Reset difficulty on loss
            // Restart the game after a delay for a loss as well
            StartCoroutine(RestartGameAfterDelay());
        }

        Debug.Log(playerWon ? "Player Wins!" : "Game Over!");
    }

    public void ContinueGame()
    {
        Debug.Log("Continue button pressed!");
        IncreaseDifficulty(); // Increase difficulty for the next round
        StartCoroutine(RestartGameAfterDelay());
    }

    IEnumerator RestartGameAfterDelay()
    {
        Debug.Log("Restarting game after delay...");
        yield return new WaitForSeconds(1f); // Delay for 1 second
        timeLeft = 60f; // Reset timer
        gameEnded = false;
        playerMovement.enabled = true; // Re-enable player movement
        continueButton.gameObject.SetActive(false); // Hide Continue button

        ResetHoleSize();   // Reset the hole's size
        ResetCameraZoom(); // Reset the camera's zoom
        cubeSpawner.SpawnCubes(); // Spawn new cubes
    }

    void ResetHoleSize()
    {
        if (playerMovement != null)
        {
            // Reset the hole's scale to its default value
            playerMovement.transform.localScale = new Vector3(1, 0.1f, 1);
        }
    }

    void ResetCameraZoom()
    {
        // Find the CameraFollow script and call its ResetZoom method
        CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.ResetZoom();
        }
    }

    void IncreaseDifficulty()
    {
        // Increase cube count and spawn area for the next round
        cubeSpawner.numberOfCubes += 5;
        cubeSpawner.spawnAreaSize += 2f;
    }

    void ResetDifficulty()
    {
        // Reset cube count and spawn area to default values
        cubeSpawner.numberOfCubes = 10;
        cubeSpawner.spawnAreaSize = 5f;
    }
}
