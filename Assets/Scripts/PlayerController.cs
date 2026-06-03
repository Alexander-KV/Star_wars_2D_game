using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [HideInInspector] public int currentHP;

    private bool isInvincible = false;
    public float invincibleDuration = 0f;
    private float invincibleTimer = 0f;

    [Header("Стрельба")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    private float fireTimer = 0f;

    [Header("Перегрев")]
    public float maxHeat = 100f;
    public float heatPerShot = 10f;
    public float cooldownRate = 15f;
    public float overheatCooldown = 3f;
    private float currentHeat = 0f;
    private bool isOverheated = false;
    private float overheatTimer = 0f;

    [Header("UI перегрева")]
    public Slider heatBar;
    public TextMeshProUGUI overheatText;

    void Start()
    {
        currentHP = maxHP;
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

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        transform.position += new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

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
            if (overheatText != null)
                overheatText.gameObject.SetActive(true);
        }
    }

    void HandleHeat()
    {
        if (isOverheated)
        {
            overheatTimer -= Time.deltaTime;
            if (overheatTimer <= 0f)
            {
                isOverheated = false;
                currentHeat = 0f;
                if (overheatText != null)
                    overheatText.gameObject.SetActive(false);
            }
        }
        else
        {
            currentHeat -= cooldownRate * Time.deltaTime;
            currentHeat = Mathf.Clamp(currentHeat, 0f, maxHeat);
        }

        if (heatBar != null)
            heatBar.value = currentHeat / maxHeat;
    }

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
        if (other.CompareTag("Meteor"))
        {
            TakeDamage(20);
            Destroy(other.gameObject);
        }

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

    void Die()
    {
        FindObjectOfType<UIManager>().ShowGameOver();
    }

    public int GetCurrentHP() { return currentHP; }
}