using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x > 15f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            other.GetComponent<Boss>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.CompareTag("Meteor"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}