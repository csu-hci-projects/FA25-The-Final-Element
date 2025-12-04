using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FilesMenuUI : MonoBehaviour
{
    [Header("List Panel")]
    [SerializeField] private Transform noteListContainer;
    [SerializeField] private GameObject noteListItemPrefab;

    [Header("Detail Panel")]
    [SerializeField] private TextMeshProUGUI detailTitleText;
    [SerializeField] private TextMeshProUGUI detailContentText;
    [SerializeField] private Image detailImage;
    [SerializeField] private GameObject detailImageContainer;

    [Header("Controls")]
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI noteCountText;

    private void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseFilesMenu);
        }
    }

    private void OnEnable()
    {
        PopulateNoteList();
    }

    private void PopulateNoteList()
    {
        // Clear existing list
        foreach (Transform child in noteListContainer)
        {
            Destroy(child.gameObject);
        }

        List<NoteData> notes = NoteManager.Instance.GetCollectedNotes();

        // Update count
        if (noteCountText != null)
        {
            noteCountText.text = $"Files: {notes.Count}";
        }

        // Create list items
        foreach (NoteData note in notes)
        {
            GameObject listItem = Instantiate(noteListItemPrefab, noteListContainer);

            TextMeshProUGUI itemText = listItem.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null)
            {
                itemText.text = note.noteTitle;
            }

            Button itemButton = listItem.GetComponent<Button>();
            if (itemButton != null)
            {
                NoteData capturedNote = note;
                itemButton.onClick.AddListener(() => DisplayNoteDetails(capturedNote));
            }
        }

        // Auto-select first note
        if (notes.Count > 0)
        {
            DisplayNoteDetails(notes[0]);
        }
        else
        {
            ShowEmptyState();
        }
    }

    private void DisplayNoteDetails(NoteData note)
    {
        detailTitleText.text = note.noteTitle;
        detailContentText.text = note.noteContent;

        if (note.noteImage != null)
        {
            if (detailImageContainer != null) detailImageContainer.SetActive(true);
            if (detailImage != null)
            {
                detailImage.gameObject.SetActive(true);
                detailImage.sprite = note.noteImage;
            }
        }
        else
        {
            if (detailImageContainer != null) detailImageContainer.SetActive(false);
            if (detailImage != null) detailImage.gameObject.SetActive(false);
        }
    }

    private void ShowEmptyState()
    {
        detailTitleText.text = "No Files Collected";
        detailContentText.text = "Explore the island to find research notes and documents.";
        if (detailImageContainer != null) detailImageContainer.SetActive(false);
    }

    private void CloseFilesMenu()
    {
        NoteManager.Instance.CloseFilesMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            CloseFilesMenu();
        }
    }
}