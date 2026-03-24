using UnityEngine;

public class Meteor : MonoBehaviour
{
    // —корость метеорита (будет задаватьс€ спавнером)
    public float speed = 3f;

    // Ќебольшой случайный угол движени€ по Y
    private float randomY;

    void Start()
    {
        // ѕри создании задаЄм случайное отклонение по вертикали
        // Ќебольшое Ч чтобы метеориты не летели строго горизонтально
        randomY = Random.Range(-0.3f, 0.3f);
    }

    void Update()
    {
        // ƒвигаем метеорит влево + небольшое отклонение по Y
        transform.position += new Vector3(-speed, randomY * 0.1f, 0) * Time.deltaTime;

        // ≈сли метеорит улетел за левый край экрана Ч удал€ем его
        // -12 это точно за пределами экрана
        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
}