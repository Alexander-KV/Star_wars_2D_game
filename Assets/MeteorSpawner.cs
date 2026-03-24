using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    // Сюда перетащим префаб метеорита
    public GameObject[] meteorPrefabs;

    // Как часто появляется новый метеорит (в секундах)
    public float spawnInterval = 1.5f;

    // Минимальная и максимальная скорость метеоритов
    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    [Header("Границы появления")]
    // По Y — где могут появляться метеориты
    public float minY = -4f;
    public float maxY = 4f;

    // Таймер
    private float timer = 0f;

    // Здесь храним позиции метеоритов которые уже летят
    // чтобы не спавнить новый туда где уже есть
    void Update()
    {
        // Считаем время
        timer += Time.deltaTime;

        // Когда таймер дошёл до интервала — спавним метеорит
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnMeteor();
        }
    }

    void SpawnMeteor()
    {
        // Делим экран на 3 зоны по высоте
        // Верх: 1.3 до 4
        // Центр: -1.3 до 1.3
        // Низ: -4 до -1.3

        float[] zones = new float[] {
            Random.Range(1.3f, 4f),    // верхняя зона
            Random.Range(-1.3f, 1.3f), // центральная зона
            Random.Range(-4f, -1.3f)   // нижняя зона
        };

        // Выбираем случайную зону
        float spawnY = zones[Random.Range(0, zones.Length)];

        // Позиция появления — за правым краем экрана
        Vector3 spawnPos = new Vector3(12f, spawnY, 0f);

        // Выбираем случайный префаб из массива
        int randomIndex = Random.Range(0, meteorPrefabs.Length);
        GameObject meteor = Instantiate(meteorPrefabs[randomIndex], spawnPos, Quaternion.identity);

        // Задаём случайную скорость
        meteor.GetComponent<Meteor>().speed = Random.Range(minSpeed, maxSpeed);

        // Случайный размер для разнообразия
        float randomScale = Random.Range(1f, 2f);
        meteor.transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }
}