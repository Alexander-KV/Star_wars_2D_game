using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;    // главный экран с кнопками сложности
    public GameObject levelSelectPanel; // экран выбора уровня после выбора сложности

    [Header("UI Elements")]
    public TextMeshProUGUI levelTitleText; // заголовок над списком уровней

    // хранит выбранную сложность между сценами (Easy / Normal / Hard)
    public static string CurrentDifficulty = "Easy";

    void Start()
    {
        // на случай если вернулись из игры с замороженным временем
        Time.timeScale = 1f;

        // показываем главное меню, скрываем выбор уровней
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(false);
    }

    // вызывается при нажатии на кнопку сложности, переходим к выбору уровня
    public void SelectDifficulty(string difficulty)
    {
        CurrentDifficulty = difficulty;

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(true);

        // показываем что выбрали, например "Hard - Выбор уровня"
        if (levelTitleText != null)
            levelTitleText.text = difficulty + " - Выбор уровня";
    }

    // загружает нужную сцену по номеру уровня и сложности
    // например Level1_Easy или Level2_Hard
    public void StartLevel(int levelNumber)
    {
        string sceneName = "Level" + levelNumber + "_" + CurrentDifficulty;
        SceneManager.LoadScene(sceneName);
    }

    // кнопка "назад" — возвращаемся к выбору сложности
    public void GoBack()
    {
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(false);
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
