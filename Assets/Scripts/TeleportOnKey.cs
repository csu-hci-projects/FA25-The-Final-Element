using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportOnKey : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad = "Lab1";
    
    [Header("UI References")]
    public GameObject pressEText;
    
    [Header("Lock Settings")]
    public bool isLocked = true; // Start locked
    public string unlockSaveKey = "LabDoorUnlocked"; // Unique key for this door
    
    [Header("Keypad Reference")]
    public KeypadController keypadController; // Reference to the keypad

    private bool playerInZone = false;

    void Start()
    {
        // Hide the text at start
        if (pressEText != null)
            pressEText.SetActive(false);
        
        // Check if door was previously unlocked
        if (PlayerPrefs.GetInt(unlockSaveKey, 0) == 1)
        {
            isLocked = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            if (pressEText != null)
                pressEText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            if (pressEText != null)
                pressEText.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            if (isLocked)
            {
                // Door is locked, open keypad
                if (keypadController != null)
                {
                    keypadController.OpenKeypad();
                }
                else
                {
                    Debug.LogWarning("Keypad Controller not assigned!");
                }
            }
            else
            {
                // Door is unlocked, teleport
                TeleportToScene();
            }
        }
    }
    
    private void TeleportToScene()
    {
        // Save spawn location
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("SpawnX", transform.position.x);
        PlayerPrefs.SetFloat("SpawnY", transform.position.y);
        PlayerPrefs.SetFloat("SpawnZ", transform.position.z);
        PlayerPrefs.Save();
        
        // Load the scene
        SceneManager.LoadScene(sceneToLoad);
    }
    
    // Called by KeypadController when code is correct
    public void UnlockDoor()
    {
        isLocked = false;
        PlayerPrefs.SetInt(unlockSaveKey, 1);
        PlayerPrefs.Save();
        Debug.Log("Door unlocked!");
    }
}