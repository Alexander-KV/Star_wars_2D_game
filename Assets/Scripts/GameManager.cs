using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Босс")]
    public GameObject bossPrefab;
    public int scoreToSpawnBoss = 0;
    // scoreToSpawnBoss = 0 означает что босс
    // появляется через время, не через очки

    [Header("Время до босса (секунды)")]
    public float timeToSpawnBoss = 60f;
    private float bossTimer = 0f;
    private bool bossSpawned = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Считаем время и спавним босса
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

    void SpawnBoss()
    {
        if (bossPrefab == null) return;
        Vector3 spawnPos = new Vector3(6f, 0f, 0f);
        Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    }

    public void BossDefeated()
    {
        FindObjectOfType<UIManager>().ShowVictory();
    }
}