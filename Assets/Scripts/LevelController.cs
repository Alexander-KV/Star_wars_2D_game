using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    // привязываем к кнопке "В главное меню" на экране паузы или конца игры
    public void BackToMainMenu()
    {
        Debug.Log("Загрузка главного меню...");
        SceneManager.LoadScene("MainMenu");
    }

    // перезапускает сцену с нуля — удобно на экране Game Over
    public void RestartLevel()
    {
        Debug.Log("Перезапуск уровня...");
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    // закрывает приложение, в редакторе не работает — это нормально
    public void QuitGame()
    {
        Debug.Log("Выход из игры");
        Application.Quit();
    }
}
