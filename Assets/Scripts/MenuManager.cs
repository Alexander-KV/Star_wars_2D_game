using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;

    [Header("UI Elements")]
    public TextMeshProUGUI levelTitleText;

    // Текущая выбранная сложность
    public static string CurrentDifficulty = "Easy";

    void Awake()
    {
        // Singleton pattern - сохраняем между сценами
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Убедимся что главная панель активна при старте
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(false);
    }

    // Вызывается при нажатии на кнопки сложности
    public void SelectDifficulty(string difficulty)
    {
        CurrentDifficulty = difficulty;

        // Скрываем главное меню, показываем выбор уровня
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(true);

        // Обновляем заголовок
        if (levelTitleText != null)
            levelTitleText.text = difficulty + " - Select Level";
    }

    // Вызывается при нажатии на кнопки уровней
    public void StartLevel(int levelNumber)
    {
        // Формируем имя сцены: Level1_Easy, Level2_Hard и т.д.
        string sceneName = "Level" + levelNumber + "_" + CurrentDifficulty;

        Debug.Log("Loading scene: " + sceneName + " (Difficulty: " + CurrentDifficulty + ")");

        // Проверяем существует ли сцена и загружаем
        SceneManager.LoadScene(sceneName);
    }

    // Кнопка "Назад"
    public void GoBack()
    {
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(false);
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    // Выход из игры
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}