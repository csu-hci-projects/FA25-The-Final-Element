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
        // DON'T deactivate the keypadUI GameObject!
        // The KeypadUI script handles its own visibility
        
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
                TeleportToScene();
            }
            else
            {
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
        Debug.Log("=== OPENING KEYPAD ===");
        minigameOpen = true;
        
        // Disable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            Debug.Log("Player movement disabled");
        }
        
        // Show keypad UI - let KeypadUI handle its own activation
        keypadUI.OpenKeypad(this);
        Debug.Log("Called OpenKeypad on KeypadUI");
        
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Hide prompt
        promptUI.SetActive(false);
        
        Debug.Log("=== KEYPAD OPENED ===");
    }

    public void CloseKeypad()
    {
        minigameOpen = false;
        
        // Re-enable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        
        // Hide keypad UI
        keypadUI.CloseKeypad();
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnCodeCorrect()
    {
        isUnlocked = true;
        
        string unlockKey = $"Keypad_Unlocked_{targetSceneName}";
        PlayerPrefs.SetInt(unlockKey, 1);
        PlayerPrefs.Save();
        
        Debug.Log("Keypad unlocked! Teleporting...");
        
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
