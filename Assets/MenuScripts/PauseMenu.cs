using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject noteReadingUI;
    
    private bool isPaused = false;
    private FirstPersonController playerController;  // Reference to the controller

    void OnEnable()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[PauseMenu] Scene loaded: {scene.name}");
        
        // Re-find the FirstPersonController in the new scene
        playerController = FindAnyObjectByType<FirstPersonController>();
        
        if (playerController == null)
        {
            Debug.LogWarning($"[PauseMenu] Could not find FirstPersonController in {scene.name}!");
        }
        else
        {
            Debug.Log($"[PauseMenu] Found FirstPersonController in {scene.name}");
        }
        
        // Make sure we're unpaused when entering a new scene
        Time.timeScale = 1f;
        isPaused = false;
        
        // Make sure pause menu is hidden
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        
        // Lock cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        // Make sure pause menu is hidden at start
        pauseMenuUI.SetActive(false);
        
        // Find the FirstPersonController (using newer Unity method)
        playerController = FindAnyObjectByType<FirstPersonController>();
        
        if (playerController == null)
        {
            Debug.LogWarning("[PauseMenu] Could not find FirstPersonController!");
        }
    }

    void Update()
    {
        // Press Escape to pause/unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if note is currently being displayed
            if (noteReadingUI != null && noteReadingUI.activeInHierarchy)
            {
                // Note is open, don't trigger pause menu
                return;
            }

            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        Debug.Log("[PauseMenu] ===== RESUME CLICKED =====");
        Debug.Log($"[PauseMenu] pauseMenuUI is null? {pauseMenuUI == null}");
        Debug.Log($"[PauseMenu] pauseMenuUI name: {(pauseMenuUI != null ? pauseMenuUI.name : "NULL")}");
        Debug.Log($"[PauseMenu] pauseMenuUI active? {(pauseMenuUI != null ? pauseMenuUI.activeInHierarchy : false)}");
        
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            Debug.Log("[PauseMenu] Set pauseMenuUI to false");
        }
        else
        {
            Debug.LogError("[PauseMenu] pauseMenuUI is NULL! Cannot hide menu!");
        }
        
        Time.timeScale = 1f;
        isPaused = false;
        
        // Re-enable player movement and camera
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        
        // Lock cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Debug.Log("[PauseMenu] Resume complete");
    }

    public void Pause()
    {
        Debug.Log("[PauseMenu] ===== PAUSE CLICKED =====");
        Debug.Log($"[PauseMenu] pauseMenuUI is null? {pauseMenuUI == null}");
        
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        
        Time.timeScale = 0f;
        isPaused = true;
        
        // Disable player movement and camera
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        // Show cursor so player can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}