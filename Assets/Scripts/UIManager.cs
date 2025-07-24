using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject startPanel;
    public GameObject tutorialPanel1;
    public GameObject tutorialPanel2;
    public Image[] heartImages;

    private static bool tutorialShown = false;

    public void UpdateHearts(int currentHP)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < currentHP;
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowWin()
    {
        winPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        Debug.Log("▶️ StartGame вызван");
        startPanel.SetActive(false);

        if (!tutorialShown)
        {
            tutorialShown = true;
            tutorialPanel1.SetActive(true); // Показываем 1й туториал
        }
        else
        {
            FindObjectOfType<GameManager>().StartGameplay();
        }
    }

    public void OnTutorialClick()
    {
        if (tutorialPanel1.activeSelf)
        {
            tutorialPanel1.SetActive(false);
            tutorialPanel2.SetActive(true);
        }
        else if (tutorialPanel2.activeSelf)
        {
            tutorialPanel2.SetActive(false);
            FindObjectOfType<GameManager>().StartGameplay();
        }
    }
}



