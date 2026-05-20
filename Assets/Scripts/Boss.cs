using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Характеристики")]
    public int maxHP = 500;
    private int currentHP;
    public float speed = 1.5f;

    [Header("Пули")]
    public GameObject normalBulletPrefab;
    public GameObject beamBulletPrefab;
    public GameObject spreadBulletPrefab;

    [Header("Таймеры атак")]
    public float normalAttackRate = 2f;
    public float beamAttackRate = 8f;
    public float spreadAttackRate = 5f;
    private float normalTimer = 0f;
    private float beamTimer = 0f;
    private float spreadTimer = 0f;

    [Header("HP Bar")]
    private Slider hpBar; // перетащи BossHPBar сюда

    private int moveDirection = -1;
    public float moveRange = 3.5f;
    private float startY;

    void Start()
    {
        currentHP = maxHP;
        startY = transform.position.y;

        // Автоматически находим слайдер внутри босса
        // не нужно ничего перетаскивать вручную
        hpBar = GetComponentInChildren<Slider>();

        if (hpBar != null)
        {
            hpBar.maxValue = maxHP;
            hpBar.value = maxHP;
        }
    }

    void Update()
    {
        Move();
        AttackNormal();
        AttackBeam();
        AttackSpread();
    }

    void Move()
    {
        transform.position += Vector3.up * moveDirection * speed * Time.deltaTime;
        if (transform.position.y > startY + moveRange) moveDirection = -1;
        if (transform.position.y < startY - moveRange) moveDirection = 1;
    }

    void AttackNormal()
    {
        normalTimer += Time.deltaTime;
        if (normalTimer < normalAttackRate) return;
        normalTimer = 0f;

        float[] offsets = { 0f, 0.5f, -0.5f };
        foreach (float offset in offsets)
        {
            Vector3 pos = transform.position + new Vector3(0, offset, 0);
            GameObject bullet = Instantiate(normalBulletPrefab, pos, Quaternion.identity);
            bullet.GetComponent<BossBullet>().SetDirection(Vector2.left);
        }
    }

    void AttackBeam()
    {
        beamTimer += Time.deltaTime;
        if (beamTimer < beamAttackRate) return;
        beamTimer = 0f;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        Vector2 dir = (player.transform.position - transform.position).normalized;
        GameObject beam = Instantiate(beamBulletPrefab, transform.position, Quaternion.identity);
        BossBullet bb = beam.GetComponent<BossBullet>();
        bb.SetDirection(dir);
        bb.speed = 3f;
        bb.damage = 35;
    }

    void AttackSpread()
    {
        spreadTimer += Time.deltaTime;
        if (spreadTimer < spreadAttackRate) return;
        spreadTimer = 0f;

        int bulletCount = 5;
        float angleRange = 120f;
        float startAngle = 180f - angleRange / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + (angleRange / (bulletCount - 1)) * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            GameObject bullet = Instantiate(spreadBulletPrefab,
                transform.position, Quaternion.identity);
            bullet.GetComponent<BossBullet>().SetDirection(dir);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // Обновляем полоску
        if (hpBar != null)
            hpBar.value = currentHP;

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        if (GameManager.instance != null)
            GameManager.instance.BossDefeated();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerController>().TakeDamage(25);
    }
}