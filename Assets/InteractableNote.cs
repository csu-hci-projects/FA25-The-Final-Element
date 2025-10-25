using UnityEngine;

public class InteractableNote : MonoBehaviour
{
    [Header("Note Information")]
    public string noteID = "Note1";
    public string noteTitle = "Day 1 - Arrival";
    [TextArea(3, 10)]
    public string noteContent = "We've arrived on the island. The government sent us here to investigate strange readings. Something feels wrong...";
    
    private bool playerInRange = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Show "Press E" prompt
            if (NoteUIManager.instance != null)
                NoteUIManager.instance.ShowInteractionPrompt(true);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Hide prompt
            if (NoteUIManager.instance != null)
                NoteUIManager.instance.ShowInteractionPrompt(false);
        }
    }
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ReadNote();
        }
    }
    
    void ReadNote()
    {
        // Show note UI
        if (NoteUIManager.instance != null)
        {
            NoteUIManager.instance.DisplayNote(noteTitle, noteContent);
            // Hide the prompt while reading
            NoteUIManager.instance.ShowInteractionPrompt(false);
        }
        
        // Disable this note so it can't be picked up again
        gameObject.SetActive(false);
    }
}