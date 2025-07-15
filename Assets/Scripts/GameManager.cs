using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private List<Plushie> attackingPlushies = new List<Plushie>();
    private bool isGameOver = false;
    private int destroyedPlushies = 0;

    private UIManager uiManager;

    public int totalPlushiesToSpawn = 20; // можно задать через инспектор
    public GameObject gameOverPanel;

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
            plush.Freeze(); // ← вот это должно быть обязательно
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
}

