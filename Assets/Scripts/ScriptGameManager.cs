
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScriptGameManager : MonoBehaviour
{
    public static ScriptGameManager Instance { get; private set; }

    [Header("UI References")]
    public TMP_Text coinText;
    public GameObject gameOverPanel;

    [Header("Player Stats")]
    public int coins = 0;
    public int playerHealth = 1;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateCoinUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void AddCoin(int amount = 1)
    {
        if (isGameOver) return;

        coins += amount;
        UpdateCoinUI();
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver) return;

        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coins;
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
