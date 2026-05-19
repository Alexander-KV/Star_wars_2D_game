using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("HP игрока")]
    public Slider hpBar;
    public TextMeshProUGUI hpText;

    [Header("HP босса")]
    public GameObject bossHPContainer;
    public Slider bossHPBar;
    public TextMeshProUGUI bossHPText;

    [Header("Экраны")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        // Скрываем все панели в начале
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (bossHPContainer != null) bossHPContainer.SetActive(false);

        if (hpBar != null && player != null)
            hpBar.maxValue = player.maxHP;
    }

    void Update()
    {
        if (player == null) return;
        if (hpBar != null)
            hpBar.value = player.GetCurrentHP();
        if (hpText != null)
            hpText.text = "HP: " + player.GetCurrentHP() + "/" + player.maxHP;
    }

    // ---- Босс ----

    public void ShowBossHP(int maxHP)
    {
        if (bossHPContainer != null) bossHPContainer.SetActive(true);
        if (bossHPBar != null)
        {
            bossHPBar.maxValue = maxHP;
            bossHPBar.value = maxHP;
        }
    }

    public void UpdateBossHP(int currentHP)
    {
        if (bossHPBar != null) bossHPBar.value = currentHP;
        if (bossHPText != null) bossHPText.text = "Босс: " + currentHP;
    }

    public void HideBossHP()
    {
        if (bossHPContainer != null) bossHPContainer.SetActive(false);
    }

    // ---- Экраны ----

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // ---- Кнопки ----

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        // Обязательно возобновляем время перед переходом
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}