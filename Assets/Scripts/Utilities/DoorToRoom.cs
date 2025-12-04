using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToRoom : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    [SerializeField] private string doorName = "Lab Entrance";
    [SerializeField] private string targetSceneName = "Lab_Room1";
    
    [Header("Spawn Settings")]
    [SerializeField] private string spawnPointName = ""; // Name of spawn point in target scene (leave empty for default)
    
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    
    [Header("Lock Settings")]
    [SerializeField] private bool isLocked = false;
    [SerializeField] private string lockedMessage = "This door is locked. Find a way to open it.";
    
    // IInteractable implementation
    public string GetInteractionPrompt()
    {
        if (isLocked)
        {
            return lockedMessage;
        }
        return $"Press E to enter {doorName}";
    }
    
    public void Interact(GameObject player)
    {
        if (isLocked)
        {
            Debug.Log($"Door is locked: {lockedMessage}");
            return;
        }
        
        LoadRoom();
    }
    
    public float GetInteractionDistance()
    {
        return interactionDistance;
    }
    
    public bool CanInteract()
    {
        // Door can always be interacted with (unless you want to add conditions)
        return true;
    }
    
    // Door functionality
    private void LoadRoom()
    {
        Debug.Log($"Loading room: {targetSceneName}");
        
        // Save spawn point name for next scene
        if (!string.IsNullOrEmpty(spawnPointName))
        {
            PlayerPrefs.SetString("SpawnPointName", spawnPointName);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.DeleteKey("SpawnPointName");
        }
        
        SceneManager.LoadScene(targetSceneName);
    }
    
    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log($"{doorName} has been unlocked!");
    }
}