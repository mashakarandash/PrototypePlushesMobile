using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScreenController : MonoBehaviour
{
    public GameObject startPanel;
    public Button playButton;
    public float flySpeed = 300f;

    private bool isFlyingAway = false;
    private RectTransform buttonRect;

    private void Start()
    {
        buttonRect = playButton.GetComponent<RectTransform>();
        Time.timeScale = 0f; // стопим игру на старте
    }

    public void OnPlayButtonClicked()
    {
        isFlyingAway = true;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (isFlyingAway)
        {
            buttonRect.anchoredPosition += Vector2.up * flySpeed * Time.unscaledDeltaTime;

            if (buttonRect.anchoredPosition.y > 1500f)
            {
                startPanel.SetActive(false);
                isFlyingAway = false;
            }
        }
    }
}

