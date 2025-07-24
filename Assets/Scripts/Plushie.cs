using UnityEngine;

public class Plushie : MonoBehaviour
{
    public float speed = 1f;
    public float tapRange = 5f;
    public float attackRange = 0.5f;
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
        if (isDead || gameManager.IsGameOver()) return;

        float distance = Vector3.Distance(transform.position, target.position);

        // 👉 Фаза давления (плюша опускается на игрока)
        if (isCrushing)
        {
            crushTimer -= Time.deltaTime;

            // Плавное приближение к позиции давления
            transform.position = Vector3.MoveTowards(transform.position, stopPosition, Time.deltaTime * 2f);

            // Когда время давления закончилось — урон игроку и уничтожение
            if (crushTimer <= 0f && !isDead)
            {
                gameManager.ReduceHP();
                Destroy(gameObject);
            }

            return;
        }

        // 👉 Начало атаки — плюша подошла достаточно близко
        if (distance <= attackRange && !isAttacking)
        {
            isAttacking = true;
            isCrushing = true;
            crushTimer = crushDuration;

            // Ставим точку давления немного перед игроком и чуть выше
            stopPosition = target.position + target.forward * -0.5f + Vector3.up * 0.1f;

            gameManager.ReduceHP(); // 💔 Снимаем жизнь сразу при начале атаки
            gameManager.RegisterAttackingPlush(this);

            Debug.Log($"🔥 Плюша начала атаку. Расстояние: {distance:F2}");
            return;
        }

        // 👉 Если не атакует и не мёртвая — двигаемся к игроку
        if (!isDead)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }


    /* void Update()
     {
         if (isDead || gameManager.IsGameOver()) return;

         float distance = Vector3.Distance(transform.position, target.position);
        // Debug.Log("📏 Расстояние до игрока: " + distance);

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
                 gameManager.ReduceHP();
                 // gameManager.GameOver("Плюша придавила!");
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
             stopPosition = target.position + target.forward * -0.5f + Vector3.up * 0.1f;

             if (!isDead)
             {
                 gameManager.ReduceHP(); // 💔 Снимаем жизнь, когда плюша начинает атаку
                 gameManager.RegisterAttackingPlush(this);

             }
         }

         // 👉 Движение к игроку
         if (!isDead)
         {
             transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
         }


     }*/


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

        Debug.Log($"💀 Die() вызван для плюши {name} ({GetInstanceID()})");

        if (gameManager != null)
        {
            gameManager.UnregisterAttackingPlush(this);
            gameManager.NotifyPlushDestroyed(); // ✅ Добавили
        }

        Destroy(gameObject); // ✅ Не забудь!
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void Freeze()
    {
        Debug.Log("❄ Плюша заморожена: " + gameObject.name);
        // Останавливаем любые действия плюшки
        isDead = true;
        isCrushing = false;

        // Если нужно остановить анимации или эффекты, можешь их отключить здесь

        // Отключаем скрипт, чтобы Update больше не вызывался
        enabled = false;
    }

    public bool IsCrushing()
    {
        return isCrushing;
    }
}
