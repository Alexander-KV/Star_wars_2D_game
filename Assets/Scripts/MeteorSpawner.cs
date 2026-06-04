using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    // массив префабов — можно добавить несколько видов метеоритов
    public GameObject[] meteorPrefabs;

    // пауза между появлением новых метеоритов
    public float spawnInterval = 1.5f;

    // разброс скоростей, у каждого метеора будет своя рандомная скорость
    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    [Header("Границы появления")]
    // вертикальные границы — за этими пределами метеориты не спавнятся
    public float minY = -4f;
    public float maxY = 4f;

    // накапливает время между спавнами
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        // время вышло — создаём новый метеорит и сбрасываем таймер
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnMeteor();
        }
    }

    void SpawnMeteor()
    {
        // делим экран на три горизонтальные зоны
        // чтобы метеориты не кучковались все в одном месте
        float[] zones = new float[] {
            Random.Range(1.3f, 4f),    // верхняя треть
            Random.Range(-1.3f, 1.3f), // центр
            Random.Range(-4f, -1.3f)   // нижняя треть
        };

        // выбираем случайную зону из трёх
        float spawnY = zones[Random.Range(0, zones.Length)];

        // появляется за правым краем экрана и летит влево
        Vector3 spawnPos = new Vector3(12f, spawnY, 0f);

        // случайный префаб из массива — разные виды метеоритов
        int randomIndex = Random.Range(0, meteorPrefabs.Length);
        GameObject meteor = Instantiate(meteorPrefabs[randomIndex], spawnPos, Quaternion.identity);

        // своя скорость у каждого
        meteor.GetComponent<Meteor>().speed = Random.Range(minSpeed, maxSpeed);

        // случайный размер — маленькие и большие вперемешку
        float randomScale = Random.Range(1f, 2f);
        meteor.transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }
}
