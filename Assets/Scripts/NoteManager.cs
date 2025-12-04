using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ADD THIS

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject filesMenuUI;
    [SerializeField] private GameObject noteReadingUI;

    [Header("Input Settings")]
    [SerializeField] private KeyCode filesHotkey = KeyCode.Tab;

    private HashSet<string> collectedNoteIDs = new HashSet<string>();
    private List<NoteData> collectedNotes = new List<NoteData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        InitializeUI();
    }

    private void OnDestroy()
    {
        // Unsubscribe when destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ADD THIS NEW METHOD
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[NoteManager] Scene loaded: {scene.name}, re-finding UI references...");
        
        // Re-find UI references in the new scene
        FindUIReferences();
        InitializeUI();
    }

    // ADD THIS NEW METHOD
    private void FindUIReferences()
    {
        // Find FilesMenuUI
        GameObject filesMenu = GameObject.Find("FilesMenuUI");
        if (filesMenu != null)
        {
            filesMenuUI = filesMenu;
            Debug.Log("[NoteManager] Found FilesMenuUI");
        }
        else
        {
            Debug.LogWarning("[NoteManager] Could not find FilesMenuUI in scene!");
        }

        // Find NoteReadingUI
        GameObject noteReading = GameObject.Find("NoteReadingUI");
        if (noteReading != null)
        {
            noteReadingUI = noteReading;
            Debug.Log("[NoteManager] Found NoteReadingUI");
        }
        else
        {
            Debug.LogWarning("[NoteManager] Could not find NoteReadingUI in scene!");
        }
    }

    // ADD THIS NEW METHOD (extracted from Awake)
    private void InitializeUI()
    {
        // Initialize UI as inactive
        if (filesMenuUI != null)
        {
            filesMenuUI.SetActive(false);
            Debug.Log("[NoteManager] Disabled FilesMenuUI");
        }
        
        if (noteReadingUI != null)
        {
            noteReadingUI.SetActive(false);
            Debug.Log("[NoteManager] Disabled NoteReadingUI");
        }
    }

    private void Update()
    {
        // Hotkey to open FILES menu (only if not paused and not reading a note)
        if (Input.GetKeyDown(filesHotkey) && Time.timeScale > 0f && noteReadingUI != null && !noteReadingUI.activeSelf)
        {
            OpenFilesMenu();
        }
    }

    public void CollectNote(NoteData note)
    {
        if (note == null || collectedNoteIDs.Contains(note.noteID))
            return;

        collectedNoteIDs.Add(note.noteID);
        collectedNotes.Add(note);
        
        Debug.Log($"Note collected: {note.noteTitle} ({collectedNotes.Count} total)");
    }

    public bool IsNoteCollected(string noteID)
    {
        return collectedNoteIDs.Contains(noteID);
    }

    public List<NoteData> GetCollectedNotes()
    {
        return new List<NoteData>(collectedNotes);
    }

    public void ShowNoteReading(NoteData note)
    {
        if (noteReadingUI != null)
        {
            noteReadingUI.GetComponent<NoteReadingUI>().DisplayNote(note);
            noteReadingUI.SetActive(true);
        }
        else
        {
            Debug.LogError("[NoteManager] Cannot show note - noteReadingUI is null!");
        }
    }

    public void CloseNoteReading()
    {
        if (noteReadingUI != null)
        {
            noteReadingUI.SetActive(false);
        }
        
        // Resume game
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenFilesMenu()
    {
        Debug.Log("[OpenFilesMenu] Called!");
        Debug.Log($"[OpenFilesMenu] filesMenuUI null? {filesMenuUI == null}");
        
        if (filesMenuUI != null)
        {
            Debug.Log($"[OpenFilesMenu] Activating {filesMenuUI.name}");
            filesMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogError("[OpenFilesMenu] filesMenuUI is NULL! Trying to re-find...");
            FindUIReferences();
            
            if (filesMenuUI != null)
            {
                Debug.Log("[OpenFilesMenu] Found UI, trying again...");
                filesMenuUI.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void CloseFilesMenu()
    {
        if (filesMenuUI != null)
        {
            filesMenuUI.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}