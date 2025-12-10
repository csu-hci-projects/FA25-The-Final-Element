using UnityEngine;
using UnityEngine.SceneManagement;

public class KeypadInteraction : MonoBehaviour
{
    [Header("References")]
    public KeypadUI keypadUI;
    public FirstPersonController playerMovement;
    public GameObject promptUI;
    
    [Header("Teleport Settings")]
    public string targetSceneName = "Storage";
    
    private bool playerInRange = false;
    private bool minigameOpen = false;
    private bool isUnlocked = false;

    void Start()
    {
        promptUI.SetActive(false);
        keypadUI.gameObject.SetActive(false);
        
        // Check if already unlocked
        string unlockKey = $"Keypad_Unlocked_{targetSceneName}";
        isUnlocked = (PlayerPrefs.GetInt(unlockKey, 0) == 1);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isUnlocked)
            {
                // Already unlocked, just teleport
                TeleportToScene();
            }
            else
            {
                // Open keypad
                OpenKeypad();
            }
        }

        if (minigameOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
        }
    }

    void OpenKeypad()
    {
        minigameOpen = true;
        
        // Disable player movement
        playerMovement.enabled = false;
        
        // Show keypad UI
        keypadUI.gameObject.SetActive(true);
        keypadUI.OpenKeypad(this);
        
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Hide prompt
        promptUI.SetActive(false);
    }

    public void CloseKeypad()
    {
        minigameOpen = false;
        
        // Re-enable player movement
        playerMovement.enabled = true;
        
        // Hide keypad UI
        keypadUI.gameObject.SetActive(false);
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Called by KeypadUI when code is correct
    public void OnCodeCorrect()
    {
        isUnlocked = true;
        
        // Save unlock state
        string unlockKey = $"Keypad_Unlocked_{targetSceneName}";
        PlayerPrefs.SetInt(unlockKey, 1);
        PlayerPrefs.Save();
        
        Debug.Log("Keypad unlocked! Teleporting...");
        
        // Wait a moment then teleport
        Invoke(nameof(TeleportToScene), 1f);
    }

    void TeleportToScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            if (isUnlocked)
            {
                promptUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"Press E to enter {targetSceneName}";
            }
            else
            {
                promptUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Press E to use keypad";
            }
            
            promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptUI.SetActive(false);
        }
    }
}