using UnityEngine;

public class Electrical_Minigame_Interaction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject minigameUI;
    public Camera minigameCamera;
    public Camera playerCamera;
    public FirstPersonController playerMovement; 
    public GameObject promptUI;
    bool playerInRange = false;
    private bool minigameOpen = false;

    void Start()
    {
        
        promptUI.SetActive(false);
        
        minigameUI.SetActive(false);
        minigameCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenMinigame();
        }
        if (minigameOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMinigame();
        }
    }

    void OpenMinigame()
    {
        minigameOpen = true;
        // lock player so they can't move while inside minigame
        playerMovement.enabled = false;

        // switch cameras
        playerCamera.gameObject.SetActive(false);
        minigameCamera.gameObject.SetActive(true);

        // show UI / wires / puzzle
        minigameUI.SetActive(true);

        // optional: unlock mouse cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // EXIT function (you call this when puzzle ends)
    public void CloseMinigame()
    {
        minigameOpen = false;
        playerMovement.enabled = true;

        minigameCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        minigameUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
            promptUI.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
            promptUI.SetActive(false);
    }
}
