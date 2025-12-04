using UnityEngine;

public interface IInteractable
{
    string GetInteractionPrompt(); // "Press E to read note", "Press E to open door", etc.
    void Interact(GameObject player);
    float GetInteractionDistance();
    bool CanInteract(); // Can this be interacted with right now?
}