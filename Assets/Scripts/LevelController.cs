using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    // Возврат в главное меню
    public void BackToMainMenu()
    {
        Debug.Log("Загрузка главного меню...");
        SceneManager.LoadScene("MainMenu");
    }

    // Перезапуск текущего уровня
    public void RestartLevel()
    {
        Debug.Log("Перезапуск уровня...");
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    // Выход из игры
    public void QuitGame()
    {
        Debug.Log("Выход из игры");
        Application.Quit();
    }
}