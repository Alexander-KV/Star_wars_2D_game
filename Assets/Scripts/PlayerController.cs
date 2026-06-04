using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Движение")]
    public float speed = 5f;

    // ограничиваем зону движения — игрок не выходит за эти границы
    public float minY = -4f;
    public float maxY = 4f;
    public float minX = -8f;
    public float maxX = -3f; // максимум по X — игрок держится в левой части

    [Header("HP")]
    public int maxHP = 100;
    [HideInInspector] public int currentHP; // скрыто в инспекторе, читается через GetCurrentHP()

    // временная неуязвимость после получения урона
    private bool isInvincible = false;
    public float invincibleDuration = 0f; // сколько секунд неуязвим (0 = отключено)
    private float invincibleTimer = 0f;

    [Header("Стрельба")]
    public GameObject bulletPrefab;
    public Transform firePoint;         // точка откуда вылетает пуля
    public float fireRate = 0.2f;       // пауза между выстрелами в секундах
    private float fireTimer = 0f;       // накапливает время с последнего выстрела

    [Header("Перегрев")]
    public float maxHeat = 100f;        // максимальный нагрев
    public float heatPerShot = 10f;     // сколько тепла добавляет один выстрел
    public float cooldownRate = 15f;    // как быстро остывает в секунду
    public float overheatCooldown = 3f; // сколько ждём после перегрева
    private float currentHeat = 0f;
    private bool isOverheated = false;  // пока true — стрелять нельзя
    private float overheatTimer = 0f;   // обратный отсчёт перегрева

    [Header("UI перегрева")]
    public Slider heatBar;             // полоска нагрева
    public TextMeshProUGUI overheatText; // надпись "ПЕРЕГРЕВ" — прячем когда не нужна

    void Start()
    {
        currentHP = maxHP;

        // прячем предупреждение о перегреве при старте
        if (overheatText != null)
            overheatText.gameObject.SetActive(false);
    }

    void Update()
    {
        Move();
        HandleShooting();
        HandleHeat();
        HandleInvincibility();
    }

    // читаем ввод и двигаем корабль, потом зажимаем в границах
    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        transform.position += new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;

        // не даём выйти за пределы игровой зоны
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    // зажат пробел или ЛКМ — стреляем, если не перегрелись и таймер готов
    void HandleShooting()
    {
        if (isOverheated) return;

        fireTimer += Time.deltaTime;

        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            && fireTimer >= fireRate)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    // спавним пулю и добавляем тепло
    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        currentHeat += heatPerShot;
        if (currentHeat >= maxHeat)
        {
            currentHeat = maxHeat;
            isOverheated = true;
            overheatTimer = overheatCooldown;

            // показываем надпись что перегрелись
            if (overheatText != null)
                overheatText.gameObject.SetActive(true);
        }
    }

    // управляет нагревом: при перегреве отсчитывает паузу, иначе постепенно остывает
    void HandleHeat()
    {
        if (isOverheated)
        {
            overheatTimer -= Time.deltaTime;
            if (overheatTimer <= 0f)
            {
                // перегрев закончился — сбрасываем и разрешаем стрелять
                isOverheated = false;
                currentHeat = 0f;
                if (overheatText != null)
                    overheatText.gameObject.SetActive(false);
            }
        }
        else
        {
            // постепенно остываем если не стреляем
            currentHeat -= cooldownRate * Time.deltaTime;
            currentHeat = Mathf.Clamp(currentHeat, 0f, maxHeat);
        }

        // обновляем полоску нагрева в UI (0 до 1)
        if (heatBar != null)
            heatBar.value = currentHeat / maxHeat;
    }

    // считает таймер неуязвимости и снимает её когда время вышло
    void HandleInvincibility()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
                isInvincible = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // врезались в метеорит — урон и уничтожаем метеор
        if (other.CompareTag("Meteor"))
        {
            TakeDamage(20);
            Destroy(other.gameObject);
        }

        // попала пуля босса — берём урон из самой пули
        if (other.CompareTag("EnemyBullet"))
        {
            BossBullet bb = other.GetComponent<BossBullet>();
            if (bb != null) TakeDamage(bb.damage);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (currentHP <= 0)
            Die();
    }

    // хп кончилось — показываем экран game over
    void Die()
    {
        FindObjectOfType<UIManager>().ShowGameOver();
    }

    // публичный геттер для UIManager чтобы не открывать currentHP напрямую
    public int GetCurrentHP() { return currentHP; }
}
