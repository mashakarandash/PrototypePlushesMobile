using System.Collections;
using UnityEngine;

public class PlushSpawner : MonoBehaviour
{
    [Header("Plushie Prefab")]
    public GameObject plushPrefab;

    [Header("Spawn Timing")]
    public float spawnIntervalMin = 1.5f;
    public float spawnIntervalMax = 3.5f;

    [Header("Spawn Area")]
    public float spawnRangeX = 3f;
    public float spawnZ = 10f;
    public float plushYOffset = 0.5f;

    [Header("Random Scale Settings")]
    public float minScale = 0.6f;
    public float maxScale = 1.4f;

    [Header("Random Speed Settings")]
    public float minSpeed = 1.0f;
    public float maxSpeed = 3.0f;

    [Header("Spawn Limit")]
    public int maxPlushies = 20;
    private int spawnedPlushies = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (spawnedPlushies < maxPlushies)
            {
                SpawnPlushie();
            }
            else
            {
                Debug.Log("✅ Достигнуто максимальное количество плюш");
                yield break; // Остановить корутину
            }

            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
        }
    }

    void SpawnPlushie()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null && gm.IsGameOver())
        {
            Debug.Log("❌ Игра окончена — плюши больше не спавнятся.");
            return;
        }

        if (spawnedPlushies >= maxPlushies) return;

        float x = Random.Range(-spawnRangeX, spawnRangeX);
        float randomScale = Random.Range(minScale, maxScale);

        // Создаём плюшу
        GameObject plush = Instantiate(plushPrefab);
        plush.transform.localScale = Vector3.one * randomScale;

        // Получаем радиус сферы и корректную позицию по Y
        float radius = plush.GetComponent<SphereCollider>().radius * randomScale;
        Vector3 spawnPos = new Vector3(x, radius, spawnZ);
        plush.transform.position = spawnPos;

        // Назначаем скорость
        Plushie plushieScript = plush.GetComponent<Plushie>();
        if (plushieScript != null)
        {
            plushieScript.speed = Random.Range(minSpeed, maxSpeed);
        }

        spawnedPlushies++; 
    }



}
