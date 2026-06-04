using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Характеристики")]
    public int maxHP = 500;
    private int currentHP; // текущее хп, меняется при получении урона
    public float speed = 1.5f;

    [Header("Пули")]
    // три типа атак — обычная, луч в игрока, веер
    public GameObject normalBulletPrefab;
    public GameObject beamBulletPrefab;
    public GameObject spreadBulletPrefab;

    [Header("Таймеры атак")]
    // как часто стреляет каждым типом (в секундах)
    public float normalAttackRate = 2f;
    public float beamAttackRate = 8f;
    public float spreadAttackRate = 5f;

    // накапливают время с последней атаки, сбрасываются при выстреле
    private float normalTimer = 0f;
    private float beamTimer = 0f;
    private float spreadTimer = 0f;

    [Header("HP Bar")]
    // слайдер над боссом — найдётся автоматически через GetComponentInChildren
    private Slider hpBar;

    // направление движения по Y: -1 вниз, +1 вверх
    private int moveDirection = -1;

    // насколько далеко от стартовой позиции может уйти вверх/вниз
    public float moveRange = 3.5f;

    // запоминаем Y при старте чтобы отсчитывать от него
    private float startY;

    void Start()
    {
        currentHP = maxHP;
        startY = transform.position.y;

        // ищем слайдер среди дочерних объектов — не нужно тащить руками
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

    // простое движение вверх-вниз, при достижении границы меняет направление
    void Move()
    {
        transform.position += Vector3.up * moveDirection * speed * Time.deltaTime;
        if (transform.position.y > startY + moveRange) moveDirection = -1;
        if (transform.position.y < startY - moveRange) moveDirection = 1;
    }

    // обычная атака — три пули рядом, стреляет влево
    void AttackNormal()
    {
        normalTimer += Time.deltaTime;
        if (normalTimer < normalAttackRate) return;
        normalTimer = 0f;

        // небольшой разброс по Y чтобы три пули не летели в одну точку
        float[] offsets = { 0f, 0.5f, -0.5f };
        foreach (float offset in offsets)
        {
            Vector3 pos = transform.position + new Vector3(0, offset, 0);
            GameObject bullet = Instantiate(normalBulletPrefab, pos, Quaternion.identity);
            bullet.GetComponent<BossBullet>().SetDirection(Vector2.left);
        }
    }

    // лучевая атака — летит прямо в игрока, чуть быстрее и больнее
    void AttackBeam()
    {
        beamTimer += Time.deltaTime;
        if (beamTimer < beamAttackRate) return;
        beamTimer = 0f;

        // если игрока нет — просто пропускаем
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        // считаем направление от босса к игроку
        Vector2 dir = (player.transform.position - transform.position).normalized;
        GameObject beam = Instantiate(beamBulletPrefab, transform.position, Quaternion.identity);
        BossBullet bb = beam.GetComponent<BossBullet>();
        bb.SetDirection(dir);
        bb.speed = 3f;
        bb.damage = 35;
    }

    // веерная атака — 5 пуль широким конусом влево
    void AttackSpread()
    {
        spreadTimer += Time.deltaTime;
        if (spreadTimer < spreadAttackRate) return;
        spreadTimer = 0f;

        int bulletCount = 5;
        float angleRange = 120f;       // суммарный угол конуса
        float startAngle = 180f - angleRange / 2f; // начальный угол (левая сторона)

        for (int i = 0; i < bulletCount; i++)
        {
            // равномерно распределяем пули по конусу
            float angle = startAngle + (angleRange / (bulletCount - 1)) * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            GameObject bullet = Instantiate(spreadBulletPrefab,
                transform.position, Quaternion.identity);
            bullet.GetComponent<BossBullet>().SetDirection(dir);
        }
    }

    // вызывается когда пуля игрока попадает в босса
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // синхронизируем полоску хп
        if (hpBar != null)
            hpBar.value = currentHP;

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        // сообщаем менеджеру что босс умер — он покажет победный экран
        if (GameManager.instance != null)
            GameManager.instance.BossDefeated();
        Destroy(gameObject);
    }

    // если игрок врезался в самого босса — тоже получает урон
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerController>().TakeDamage(25);
    }
}
