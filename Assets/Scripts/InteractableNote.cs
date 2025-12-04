using UnityEngine;

public class InteractableNote : MonoBehaviour, IInteractable
{
    [Header("Note Data")]
    [SerializeField] private NoteData noteData;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private string interactionPrompt = "Press E to read";

    [Header("Visual Effects")]
    [SerializeField] private GameObject visualEffect; // Optional particle effect on pickup
    [SerializeField] private AudioClip pickupSound; // Optional sound

    private bool hasBeenCollected = false;

    private void Start()
    {
        // Check if this note was already collected (persistence between scenes)
        if (NoteManager.Instance != null && NoteManager.Instance.IsNoteCollected(noteData.noteID))
        {
            // Note was already collected, destroy it
            Destroy(gameObject);
        }
    }

    public string GetInteractionPrompt()
    {
        return $"{interactionPrompt}: {noteData.noteTitle}";
    }

    public void Interact(GameObject player)
    {
        if (hasBeenCollected) return;

        hasBeenCollected = true;

        // Add note to collection
        NoteManager.Instance.CollectNote(noteData);

        // Show the note reading UI
        NoteManager.Instance.ShowNoteReading(noteData);

        // Play pickup effects
        PlayPickupEffects();

        // Destroy the note object after a short delay
        Destroy(gameObject, 0.1f);
    }

    public float GetInteractionDistance()
    {
        return interactionDistance;
    }

    public bool CanInteract()
    {
        return !hasBeenCollected && noteData != null;
    }

    private void PlayPickupEffects()
    {
        // Play visual effect
        if (visualEffect != null)
        {
            GameObject effect = Instantiate(visualEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // Play sound
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
    }

    // Optional: Highlight effect when player looks at it
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}