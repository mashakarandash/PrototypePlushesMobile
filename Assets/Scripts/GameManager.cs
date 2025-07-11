using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private List<Plushie> attackingPlushies = new List<Plushie>();
    private bool isGameOver = false;

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
        if (attackingPlushies.Count >= 3)
        {
            GameOver("На игрока давит 3 плюши!");
        }
    }

    public void GameOver(string reason)
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("🟥 GAME OVER вызван ИЗ GameManager: " + reason);

       /*  Здесь можно добавить финальную логику:
         - остановка времени
         - показ экрана проигрыша
         - перезапуск сцены и т.п.*/
       // Time.timeScale = 0f;
    }

    public int GetAttackingCount()
    {
        return attackingPlushies.Count;
    }
}

