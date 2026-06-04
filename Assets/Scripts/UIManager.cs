using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("HP игрока")]
    public Slider hpBar;           // полоска здоровья вверху экрана
    public TextMeshProUGUI hpText; // текст рядом с полоской, например "HP: 80/100"

    [Header("Экраны")]
    public GameObject gameOverPanel; // панель проигрыша
    public GameObject victoryPanel;  // панель победы

    // ссылка на игрока — берём автоматически в Start
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        // оба экрана скрыты пока игра идёт
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        // настраиваем максимум полоски один раз
        if (hpBar != null && player != null)
            hpBar.maxValue = player.maxHP;
    }

    void Update()
    {
        // каждый кадр обновляем UI — слайдер и текст с текущим хп
        if (player == null) return;
        if (hpBar != null)
            hpBar.value = player.GetCurrentHP();
        if (hpText != null)
            hpText.text = "HP: " + player.GetCurrentHP() + "/" + player.maxHP;
    }

    // вызывается из PlayerController когда хп <= 0
    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // замораживаем игру
    }

    // вызывается из GameManager когда босс умер
    public void ShowVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f; // замораживаем игру
    }

    // кнопка "Заново" на обоих экранах
    public void RestartGame()
    {
        Time.timeScale = 1f; // размораживаем перед загрузкой
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // кнопка "В меню" на обоих экранах
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
