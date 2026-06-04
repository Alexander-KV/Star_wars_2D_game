using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10; // урон по боссу и метеоритам

    void Update()
    {
        // летим вправо каждый кадр
        transform.position += Vector3.right * speed * Time.deltaTime;

        // улетела за правый край — удаляем
        if (transform.position.x > 15f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // попали в босса — снимаем хп и уничтожаем пулю
        if (other.CompareTag("Boss"))
        {
            other.GetComponent<Boss>().TakeDamage(damage);
            Destroy(gameObject);
        }

        // попали в метеорит — просто уничтожаем оба объекта
        if (other.CompareTag("Meteor"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
