using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // синглтон — чтобы другие скрипты могли дотянуться через GameManager.instance
    public static GameManager instance;

    [Header("Босс")]
    public GameObject bossPrefab;

    // если 0 — игнорируем очки и спавним босса по таймеру
    public int scoreToSpawnBoss = 0;

    [Header("Время до босса (секунды)")]
    public float timeToSpawnBoss = 60f; // через сколько секунд появится босс

    private float bossTimer = 0f;    // сколько времени уже прошло
    private bool bossSpawned = false; // флаг чтобы не спавнить дважды

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // считаем время пока босс не появился
        if (!bossSpawned)
        {
            bossTimer += Time.deltaTime;
            if (bossTimer >= timeToSpawnBoss)
            {
                bossSpawned = true;
                SpawnBoss();
            }
        }
    }

    // спавним босса справа по центру экрана, повёрнутого на 270 градусов
    void SpawnBoss()
    {
        if (bossPrefab == null) return;
        Vector3 spawnPos = new Vector3(6f, 0f, 0f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, 270f);
        Instantiate(bossPrefab, spawnPos, rotation);
    }

    // вызывается из Boss.cs когда здоровье упало до 0
    public void BossDefeated()
    {
        FindObjectOfType<UIManager>().ShowVictory();
    }
}
