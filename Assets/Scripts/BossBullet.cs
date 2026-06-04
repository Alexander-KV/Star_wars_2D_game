using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 15; // урон по умолчанию, луч переопределяет его на 35

    // направление полёта, задаётся снаружи через SetDirection
    private Vector2 direction = Vector2.left;

    // вызывается из Boss.cs перед тем как пуля начнёт лететь
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized; // нормализуем на всякий случай, чтобы скорость была одинаковой
    }

    void Update()
    {
        // двигаемся в заданном направлении каждый кадр
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // улетела за экран — удаляем, нет смысла держать в памяти
        if (transform.position.x < -12f || transform.position.x > 12f ||
            transform.position.y < -8f || transform.position.y > 8f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // попали в игрока — наносим урон и уничтожаем пулю
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
