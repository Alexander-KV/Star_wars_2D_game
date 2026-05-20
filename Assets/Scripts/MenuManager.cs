using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;

    [Header("UI Elements")]
    public TextMeshProUGUI levelTitleText;

    public static string CurrentDifficulty = "Easy";

    void Start()
    {
        // Гарантируем что время идёт нормально
        Time.timeScale = 1f;

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(false);
    }

    public void SelectDifficulty(string difficulty)
    {
        CurrentDifficulty = difficulty;
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(true);
        if (levelTitleText != null)
            levelTitleText.text = difficulty + " - Выбор уровня";
    }

    public void StartLevel(int levelNumber)
    {
        string sceneName = "Level" + levelNumber + "_" + CurrentDifficulty;
        SceneManager.LoadScene(sceneName);
    }

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