using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Remove if using legacy UI Text

public class ScriptGameManager : MonoBehaviour
{
    public static ScriptGameManager Instance { get; private set; }

    [Header("UI References")]
    public TMP_Text coinText;          // Use 'public Text coinText;' if using legacy UI
    public GameObject gameOverPanel;   // The UI panel with Retry / Exit buttons

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
            
        Time.timeScale = 1f; // Ensure game is unpaused on start
    }

    public void AddCoin()
    {
        if (isGameOver) return;
        coins++;
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
        Time.timeScale = 0f; // Freeze game movement

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // Call this from the "Retry" button OnClick()
    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Call this from the "Exit to Main Menu" button OnClick()
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Matches the "MainMenu" scene in your project
    }
}