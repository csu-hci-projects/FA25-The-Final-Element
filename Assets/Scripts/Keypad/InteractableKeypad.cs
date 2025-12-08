using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableKeypad : MonoBehaviour, IInteractable
{
    [Header("Keypad Settings")]
    [SerializeField] private string keypadName = "Security Terminal";
    [SerializeField] private float interactionDistance = 2f;
    
    [Header("UI Reference")]
    [SerializeField] private KeypadUI keypadUI; // Your keypad UI controller
    
    [Header("Teleport Settings")]
    [SerializeField] private string targetSceneName = "Lab_Room1";
    
    private bool isUnlocked = false;

    void Start()
    {
        // Check if already unlocked - use a simpler key
        string unlockKey = $"Keypad_Unlocked_{targetSceneName}";
        if (PlayerPrefs.GetInt(unlockKey, 0) == 1)
        {
            isUnlocked = true;
            Debug.Log($"[Keypad] Loaded unlock state: unlocked for {targetSceneName}");
        }
        else
        {
            Debug.Log($"[Keypad] Loaded unlock state: locked");
        }
    }

    public string GetInteractionPrompt()
    {
        if (isUnlocked)
        {
            return $"Press E to enter {targetSceneName}";
        }
        return $"Press E to use {keypadName}";
    }

    public void Interact(GameObject player)
    {
        if (isUnlocked)
        {
            // Already solved, just teleport
            TeleportToNextRoom();
        }
        else
        {
            // Open keypad UI
            if (keypadUI != null)
            {
                keypadUI.OpenKeypad(this);
            }
            else
            {
                Debug.LogError("[InteractableKeypad] KeypadUI not assigned!");
            }
        }
    }

    public float GetInteractionDistance()
    {
        return interactionDistance;
    }

    public bool CanInteract()
    {
        return true;
    }

    // Call this from KeypadUI when code is correct
    public void OnCodeCorrect()
    {
        isUnlocked = true;
        
        // Save unlock state using the same key format
        string unlockKey = $"Keypad_Unlocked_{targetSceneName}";
        PlayerPrefs.SetInt(unlockKey, 1);
        PlayerPrefs.Save();
        
        Debug.Log($"[InteractableKeypad] Access granted! Saved unlock state for {targetSceneName}");
        
        // Teleport after a short delay
        Invoke(nameof(TeleportToNextRoom), 1f);
    }

    private void TeleportToNextRoom()
    {
        // Simply load the target scene
        SceneManager.LoadScene(targetSceneName);
    }
}