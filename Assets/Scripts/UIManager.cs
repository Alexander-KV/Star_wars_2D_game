using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("HP игрока")]
    public Slider hpBar;
    public TextMeshProUGUI hpText;

    [Header("Ёкраны")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

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

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}