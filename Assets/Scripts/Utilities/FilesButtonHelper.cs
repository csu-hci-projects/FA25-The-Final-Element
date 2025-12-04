using UnityEngine;

public class FilesButtonHelper : MonoBehaviour
{
    // This method is called by the FILES button
    // It dynamically finds NoteManager.Instance instead of using a serialized reference
    public void OpenFilesMenu()
    {
        Debug.Log("[FilesButtonHelper] Button clicked!");
        
        if (NoteManager.Instance != null)
        {
            NoteManager.Instance.OpenFilesMenu();
        }
        else
        {
            Debug.LogError("[FilesButtonHelper] NoteManager.Instance is null!");
        }
    }
}