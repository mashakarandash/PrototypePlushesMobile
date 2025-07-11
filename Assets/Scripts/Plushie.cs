
using UnityEngine;

public class Plushie : MonoBehaviour
{
    public float speed = 1f;
    public float tapRange = 5f;
    public float attackRange = 1.5f;
    public float attackDistance = 1f;  // радиус, в котором плюша "придавливает"
    public float closeAttackDistance = 0.3f;
    public float crushDuration = 5f; // сколько секунд есть на спасение
    public int requiredTapsToSurvive = 7;
    public int requiredTaps = 7;        

    public Transform target;

    private int tapCount = 0;
    private bool isAttacking = false;
    private bool isDead = false;
    private float crushTimer = 0f;
    private bool isCrushing = false;

    private GameManager gameManager;
    private Vector3 stopPosition;

    void Start()
    {
        GameObject targetObj = GameObject.Find("PlushieTarget");

        if (targetObj != null)
        {
            target = targetObj.transform;
        }
        else
        {
            Debug.LogWarning("❗ PlushieTarget не найден, используется камера.");
            target = Camera.main.transform;
        }

        gameManager = FindObjectOfType<GameManager>();
    }



    void Update()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, target.position);
        Debug.Log("📏 Расстояние до игрока: " + distance);

        if (isCrushing)
        {
            if (isDead)
            {
                isCrushing = false;
                return;
            }

            crushTimer -= Time.deltaTime;

            // 👉 Можно сделать визуальное давление (плавное "опускание" вниз)
            transform.position = Vector3.Lerp(transform.position, stopPosition, Time.deltaTime * 2f);

            if (crushTimer <= 0f)
            {
                gameManager.GameOver("Плюша придавила!");
                Destroy(gameObject);
                return;
            }

            return;
        }

        // 👉 Если подошла близко и ещё не атаковала
        if (distance <= attackRange && !isAttacking)
        {
            isAttacking = true;
            Debug.Log($"🔥 Плюша начала атаку на расстоянии: {distance:F2}");
            isCrushing = true;
            crushTimer = crushDuration;
            stopPosition = target.position + target.forward * -0.5f;

            stopPosition = target.position + Vector3.up * 0.1f; // немного приподнимается, потом медленно опускается

            if (!isDead)
            {
                gameManager.RegisterAttackingPlush(this);
                Debug.Log($"📌 Плюша начала давить. Всего: {gameManager.GetAttackingCount()}");
            }
        }

        // 👉 Движение к игроку
        if (!isDead)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        Debug.Log($"[👁 Plushie] isDead={isDead}, isCrushing={isCrushing}, position={transform.position}, distance={Vector3.Distance(transform.position, target.position)}");

    }

    public void Tap()
    {
        if (isDead) return;

        if (!isAttacking)
        {
            Die();
        }
        else if (isAttacking)
        {
            tapCount++;
            if (tapCount >= requiredTaps)
            {
                Die();
            }
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        isCrushing = false;

        if (gameManager != null)
        {
            gameManager.UnregisterAttackingPlush(this);
        }

        Destroy(gameObject);
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
