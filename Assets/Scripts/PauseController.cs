using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Sprite pauseIcon;
    [SerializeField] private Sprite closeIcon;
    [SerializeField] private Image pauseButtonImage;

    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        pauseButtonImage.sprite = isPaused ? closeIcon : pauseIcon;
    }

    public void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        pauseButtonImage.sprite = pauseIcon;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // обязательно вернуть время в норму
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

