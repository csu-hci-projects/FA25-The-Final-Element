using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    void Start()
    {
        // Check if there's a saved spawn point
        if (PlayerPrefs.HasKey("SpawnPointName"))
        {
            string spawnPointName = PlayerPrefs.GetString("SpawnPointName");
            
            // Find the spawn point in the scene
            GameObject spawnPoint = GameObject.Find(spawnPointName);
            
            if (spawnPoint != null)
            {
                // Move player to spawn point
                transform.position = spawnPoint.transform.position;
                transform.rotation = spawnPoint.transform.rotation;
                
                Debug.Log($"Player spawned at: {spawnPointName}");
            }
            else
            {
                Debug.LogWarning($"Spawn point '{spawnPointName}' not found in scene!");
            }
            
            // Clear the spawn point so it doesn't interfere next time
            PlayerPrefs.DeleteKey("SpawnPointName");
        }
    }
}