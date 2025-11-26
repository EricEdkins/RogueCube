using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject restartText;

    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (gameOverText != null)
            gameOverText.SetActive(true);

        if (restartText != null)
            restartText.SetActive(true);

        Time.timeScale = 0f;
        Debug.Log("Game Over!");
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
    