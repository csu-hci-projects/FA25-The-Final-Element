using UnityEngine;

public class InteractableCard : MonoBehaviour, IInteractable
{
    [Header("Card Info")]
    [SerializeField] private string cardName = "Keycard";
    [SerializeField] private string cardID = "Keycard_01"; // Unique ID for this card
    
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private string interactionPrompt = "Press E to pick up";

    [Header("Optional Effects")]
    [SerializeField] private GameObject visualEffect;
    [SerializeField] private AudioClip pickupSound;

    private void Start()
    {
        // If card was already picked up, destroy it
        if (PlayerPrefs.GetInt(cardID, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    public string GetInteractionPrompt()
    {
        return $"{interactionPrompt}: {cardName}";
    }

    public void Interact(GameObject player)
    {
        // Mark card as collected
        PlayerPrefs.SetInt(cardID, 1);
        PlayerPrefs.Save();
        
        Debug.Log($"Picked up {cardName}! Door is now unlocked.");

        // Play effects if you want them
        PlayPickupEffects();

        // Destroy the card
        Destroy(gameObject);
    }

    public float GetInteractionDistance()
    {
        return interactionDistance;
    }

    public bool CanInteract()
    {
        return true;
    }

    private void PlayPickupEffects()
    {
        if (visualEffect != null)
        {
            GameObject effect = Instantiate(visualEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
    }
}