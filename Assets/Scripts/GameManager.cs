using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private List<Plushie> attackingPlushies = new List<Plushie>();
    private bool isGameOver = false;
    private int destroyedPlushies = 0;

    private UIManager uiManager;

    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject pauseButton;

    public int totalPlushiesToSpawn = 20; // можно задать через инспектор
    public int playerHP = 3; // Кол-во жизней

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void RegisterAttackingPlush(Plushie plushie)
    {
        if (plushie == null || attackingPlushies.Contains(plushie) || plushie.IsDead()) return;

        attackingPlushies.Add(plushie);
        Debug.Log($"✅ Зарегистрирована плюша: {plushie.name}. Всего: {attackingPlushies.Count}");

        CheckGameOver();
    }


    public void UnregisterAttackingPlush(Plushie plushie)
    {
        if (attackingPlushies.Contains(plushie))
        {
            attackingPlushies.Remove(plushie);
            Debug.Log($"❌ Удалена плюша: {plushie.name} ({plushie.GetInstanceID()})");
        }
    }

    void CheckGameOver()
    {
            attackingPlushies.RemoveAll(p => p == null || p.IsDead());

            int crushingCount = 0;
            foreach (var plush in attackingPlushies)
            {
                if (plush != null && plush.IsCrushing())
                {
                    crushingCount++;
                }
            }

            if (crushingCount >= 3)
            {
                GameOver("Три плюши придавили игрока!");
            }
    }
    

    public void GameOver(string reason)
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("🟥 GAME OVER вызван ИЗ GameManager: " + reason);

        Plushie[] allPlushies = FindObjectsOfType<Plushie>();
        foreach (Plushie plush in allPlushies)
        {
            plush.Freeze(); 
        }

        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
    }

    public int GetAttackingCount()
    {
        return attackingPlushies.Count;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void ReduceHP()
    {
        playerHP--;

        if (uiManager != null)
        {
            uiManager.UpdateHearts(playerHP); // 💔 обновляем интерфейс
        }

        Debug.Log($"💔 Игрок потерял жизнь! Осталось: {playerHP}");

        if (playerHP <= 0)
        {
            GameOver("У игрока кончились жизни!");
        }
    }

    public void NotifyPlushDestroyed()
    {
        destroyedPlushies++;

        Debug.Log($"☠️ Уничтожено плюш: {destroyedPlushies}/{totalPlushiesToSpawn}");

        if (!isGameOver && destroyedPlushies >= totalPlushiesToSpawn)
        {
            Win();
        }
    }

    void Win()
    {
        isGameOver = true;
        Debug.Log("🏆 Победа! Все плюши уничтожены.");

        Plushie[] allPlushies = FindObjectsOfType<Plushie>();
        foreach (Plushie plush in allPlushies)
        {
            plush.Freeze();
        }

        if (uiManager != null)
        {
            uiManager.ShowWin();
        }
    }

    public void StartGameplay()
    {
        Time.timeScale = 1f;
        Debug.Log("▶️ Игра началась");
    }
}

