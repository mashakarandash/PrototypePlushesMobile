using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorial1Panel;
    public GameObject tutorial2Panel;
    public GameObject startPanel; // Панель стартового экрана

    private int tutorialStep = 0;

    public void StartTutorial()
    {
        if (!PlayerPrefs.HasKey("TutorialShown"))
        {
            tutorialStep = 1;
            startPanel.SetActive(false); // скрываем стартовый экран
            tutorial1Panel.SetActive(true);
            tutorial2Panel.SetActive(false);
            Time.timeScale = 0f; // пауза во время туториала
        }
        else
        {
            // Туториал уже показан → сразу начинаем игру
            startPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void OnTutorialClick()
    {
        if (tutorialStep == 1)
        {
            tutorial1Panel.SetActive(false);
            tutorial2Panel.SetActive(true);
            tutorialStep = 2;
        }
        else if (tutorialStep == 2)
        {
            tutorial2Panel.SetActive(false);
            Time.timeScale = 1f;
            PlayerPrefs.SetInt("TutorialShown", 1); // сохранить что туториал уже был
        }
    }

    // (опционально, для теста)
    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("TutorialShown");
    }
}
