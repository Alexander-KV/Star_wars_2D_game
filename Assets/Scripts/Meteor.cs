using UnityEngine;

public class Meteor : MonoBehaviour
{
    // скорость задаётся снаружи через MeteorSpawner, поэтому public
    public float speed = 3f;

    // маленький сдвиг по Y — чтобы метеориты не летели ровно горизонтально
    private float randomY;

    void Start()
    {
        // при создании один раз рандомим отклонение, потом используем в Update
        randomY = Random.Range(-0.3f, 0.3f);
    }

    void Update()
    {
        // летим влево с небольшим дрейфом по вертикали
        transform.position += new Vector3(-speed, randomY * 0.1f, 0) * Time.deltaTime;

        // ушёл за левый край — удаляем, игрок его уже не увидит
        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
}
