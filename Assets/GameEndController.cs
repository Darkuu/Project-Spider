using UnityEngine;

public class GameEndController : MonoBehaviour
{
    public GameObject gameOverUI; // Assign your popup panel here

    private bool gameEnded = false;

    void Update()
    {
        if (gameEnded && Input.GetKeyDown(KeyCode.Space))
        {
            ResumeGame();
        }
    }

    public void EndGame()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
        gameEnded = true;
    }

    void ResumeGame()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        Time.timeScale = 1f;
        gameEnded = false;
    }
}
