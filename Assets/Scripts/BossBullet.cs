using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 15;
    private Vector2 direction = Vector2.left;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Удаляем если улетела за экран
        if (transform.position.x < -12f || transform.position.x > 12f ||
            transform.position.y < -8f || transform.position.y > 8f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}