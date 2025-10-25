using UnityEngine;
using TMPro;

public class NoteUIManager : MonoBehaviour
{
    public static NoteUIManager instance;
    
    [Header("UI References")]
    public GameObject noteDisplayPanel;
    public TextMeshProUGUI noteTitleText;
    public TextMeshProUGUI noteContentText;
    public GameObject interactionPrompt;
    
    void Awake()
    {
        // Singleton pattern so other scripts can access this
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    void Start()
    {
        // Make sure everything is hidden at start
        if (noteDisplayPanel != null)
            noteDisplayPanel.SetActive(false);
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
    
    public void DisplayNote(string title, string content)
    {
        // Show the note
        noteTitleText.text = title;
        noteContentText.text = content;
        noteDisplayPanel.SetActive(true);
        
        // Pause game and show cursor
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void CloseNote()
    {
        // Hide the note
        noteDisplayPanel.SetActive(false);
        
        // Resume game and hide cursor
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void ShowInteractionPrompt(bool show)
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(show);
    }
    
    void Update()
    {
        // Allow ESC to close note
        if (noteDisplayPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseNote();
        }
    }
}