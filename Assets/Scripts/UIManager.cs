using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("HP интерфейс")]
    public Slider hpBar;
    public TextMeshProUGUI hpText;

    [Header("Экран смерти")]
    public GameObject gameOverPanel;

    // Ссылка на игрока
    private PlayerController player;

    void Start()
    {
        // Находим игрока на сцене
        player = FindObjectOfType<PlayerController>();

        // Скрываем экран смерти в начале
        gameOverPanel.SetActive(false);

        // Устанавливаем максимальное значение полоски
        hpBar.maxValue = player.maxHP;
        hpBar.value = player.maxHP;
    }

    void Update()
    {
        // Каждый кадр обновляем полоску и текст
        hpBar.value = player.GetCurrentHP();
        hpText.text = "HP: " + player.GetCurrentHP() + "/" + player.maxHP;
    }

    // Этот метод вызовем из PlayerController когда игрок умирает
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        // Останавливаем игру
        Time.timeScale = 0f;
    }

    // Кнопка "Заново" — перезапускает сцену
    public void RestartGame()
    {
        // Возобновляем время (важно!)
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Кнопка "В меню" — пока просто перезапускает
    // Позже сюда добавим переход в главное меню
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

