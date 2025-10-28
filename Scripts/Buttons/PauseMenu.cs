using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameInput gameInput;
    [SerializeField] private GameObject pauseMenuUI;

    private bool isPaused = false;

    void Start()
    {
        gameInput.OnPause += GameInput_OnPause;
        isPaused = false;
    }

    private void GameInput_OnPause(object sender, System.EventArgs e)
    {
        if(isPaused)
        {
            Resume();

        } else
        {
            Pause();
        }
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);  // Show UI
        Time.timeScale = 0f;          // Freeze game time
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide UI
        Time.timeScale = 1f;          // Resume game time
        isPaused = false;
    }

    public void QuitGame()
    {
        // If in editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
