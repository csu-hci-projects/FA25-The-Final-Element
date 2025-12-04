using UnityEngine;

public class OfficeMinigameInteraction : MonoBehaviour
{
    public GameObject minigameRoot;            // Minigame UI + camera
    public GameObject playerController;        // FPS controller root
    public MonoBehaviour movementScript;       // Script that moves the player (drag/drop)
    public GameManager gameManager;
    public GameObject promptUI;

    private bool playerInRange = false;
    private bool minigameOpen = false;

    private Vector3 savedPosition;
    private Quaternion savedRotation;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        if (minigameRoot != null)
            minigameRoot.SetActive(false);
    }

    void Update()
    {
        // Enter minigame
        if (playerInRange && !minigameOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenMinigame();
        }

        // Exit minigame
        if (minigameOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMinigame();
        }
    }

    private void OpenMinigame()
    {
        minigameOpen = true;

        // Save player transform
        savedPosition = playerController.transform.position;
        savedRotation = playerController.transform.rotation;

        // Disable movement, but NOT entire object
        if (movementScript != null)
            movementScript.enabled = false;

        // Show minigame
        minigameRoot.SetActive(true);

        // Unlock cursor to click UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Notify GameManager
        if (gameManager != null)
            gameManager.ActivateMinigame();

        // Hide prompt
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    public void CloseMinigame()
    {
        minigameOpen = false;

        // Hide minigame
        minigameRoot.SetActive(false);

        // Restore position & rotation
        playerController.transform.position = savedPosition;
        playerController.transform.rotation = savedRotation;

        // Re-enable movement
        if (movementScript != null)
            movementScript.enabled = true;

        // Lock cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Stop GameManager from pausing again
        gameManager.minigameActive = false;

        // Restore world time
        Time.timeScale = 1f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}
