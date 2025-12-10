using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TeleportOnKey : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad = "card_swipe";
    
    [Header("UI")]
    public GameObject pressEText;
    public GameObject lockedText; // Optional: "Door is locked" message
    
    [Header("Card Requirement")]
    [SerializeField] private string requiredCardID = "Keycard_01"; // Must match the card's ID
    [SerializeField] private bool requiresCard = true;

    private bool playerInZone = false;

    void Start()
    {
        // Hide the text at start
        if (pressEText != null)
            pressEText.SetActive(false);
        if (lockedText != null)
            lockedText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            UpdateUI();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            if (pressEText != null)
                pressEText.SetActive(false);
            if (lockedText != null)
                lockedText.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            if (CanUseDoor())
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.Log("Door is locked! You need the keycard.");
            }
        }
    }

    private bool CanUseDoor()
    {
        // If door doesn't require a card, always allow
        if (!requiresCard) return true;
        
        // Check if player has the required card
        return PlayerPrefs.GetInt(requiredCardID, 0) == 1;
    }

    private void UpdateUI()
    {
        if (CanUseDoor())
        {
            // Show "Press E" prompt
            if (pressEText != null)
                pressEText.SetActive(true);
            if (lockedText != null)
                lockedText.SetActive(false);
        }
        else
        {
            // Show "Locked" message
            if (pressEText != null)
                pressEText.SetActive(false);
            if (lockedText != null)
                lockedText.SetActive(true);
        }
    }
}