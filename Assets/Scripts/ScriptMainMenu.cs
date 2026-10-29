using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptMainMenu : MonoBehaviour
{
    [Header("Scenes")]
    public string gameSceneName = "GamePlay"; // Name of your gameplay scene

    [Header("UI Panels")]
    public GameObject creditsPanel; // Drag the Credits Popup Panel here

    void Start()
    {
        // Make sure time flows normally and credits panel is hidden on start
        Time.timeScale = 1f;
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    // Called by the Start Button
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Called by the Credits Button
    public void OpenCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    // Called by the Close Button inside the Credits Panel
    public void CloseCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    // Called by the Exit Button
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Quits Play Mode in Editor
        #else
            Application.Quit(); // Closes the build executable
        #endif
    }
}