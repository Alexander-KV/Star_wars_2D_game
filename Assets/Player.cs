using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Движение")]
    public float speed = 5f;
    public float minY = -4f;
    public float maxY = 4f;
    public float minX = -8f;
    public float maxX = -3f;

    [Header("HP")]
    public int maxHP = 100;
    private int currentHP;

    // Неуязвимость на короткое время после удара
    // чтобы один метеорит не снял всё HP сразу
    private bool isInvincible = false;
    public float invincibleDuration = 1.5f;
    private float invincibleTimer = 0f;

    void Start()
    {
        // В начале игры HP полное
        currentHP = maxHP;
    }

    void Update()
    {
        // Движение
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        transform.position += new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;

        // Ограничение в пределах экрана
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        // Таймер неуязвимости
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }

    // Этот метод вызывается когда метеорит касается корабля
    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем что это именно метеорит (по тегу)
        if (other.CompareTag("Meteor") && !isInvincible)
        {
            TakeDamage(20);

            // Уничтожаем метеорит при столкновении
            Destroy(other.gameObject);
        }
    }

    // Метод получения урона
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // Включаем неуязвимость на короткое время
        isInvincible = true;
        invincibleTimer = invincibleDuration;

        Debug.Log("HP: " + currentHP + "/" + maxHP);

        // Если HP кончилось — игрок погиб
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Игрок погиб!");
        // Пока просто выводим в консоль
        // Позже сюда добавим экран проигрыша
    }

    // Метод для получения текущего HP (для UI)
    public int GetCurrentHP() { return currentHP; }
    public int GetMaxHP() { return maxHP; }
}
