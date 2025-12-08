using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteReadingUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Image noteImage;
    [SerializeField] private GameObject imageContainer;
    [SerializeField] private Button closeButton;

    [Header("Styling")]
    [SerializeField] private Color paperColor = new Color(0.95f, 0.93f, 0.85f);

    private NoteData currentNote;   // <--- store the note being read

    private void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseNote);
        }
    }

    public void DisplayNote(NoteData note)
    {
        currentNote = note; // remember which note is open

        titleText.text = note.noteTitle;
        contentText.text = note.noteContent;

        // Handle optional image
        if (note.noteImage != null)
        {
            if (imageContainer != null) imageContainer.SetActive(true);

            if (noteImage != null)
            {
                noteImage.gameObject.SetActive(true);
                noteImage.sprite = note.noteImage;
            }
        }
        else
        {
            if (imageContainer != null) imageContainer.SetActive(false);
            if (noteImage != null) noteImage.gameObject.SetActive(false);
        }

        // Pause game and show cursor
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseNote()
    {
        // 🔥 Trigger inner dialogue if this note has one
        if (currentNote != null &&
            !string.IsNullOrEmpty(currentNote.innerDialogueAfterClose))
        {
            DialogueUI.Instance.ShowMessage(currentNote.innerDialogueAfterClose);
        }

        // Continue normal closing behavior
        NoteManager.Instance.CloseNoteReading();
    }

    private void Update()
    {
        // ESC or E to close
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            CloseNote();
        }
    }
}
