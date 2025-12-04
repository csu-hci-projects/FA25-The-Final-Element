using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{
    [Header("Monologue Settings")]
    [SerializeField] private MonologueData monologueData;
    [SerializeField] private MonologueUI monologueUI;
    
    [Header("Trigger Settings")]
    [SerializeField] private bool triggerOnStart = true;
    [SerializeField] private float delayBeforeTrigger = 1f;
    
    [Header("One-Time Trigger (Optional)")]
    [SerializeField] private bool triggerOnlyOnce = true;
    [SerializeField] private string monologueID = ""; // Unique ID for this monologue
    
    private void Start()
    {
        Debug.Log("[MonologueTrigger] Starting...");
        
        // Auto-find MonologueUI if not assigned
        if (monologueUI == null)
        {
            Debug.Log("[MonologueTrigger] Searching for MonologueUI...");
            monologueUI = FindFirstObjectByType<MonologueUI>();
            
            if (monologueUI != null)
            {
                Debug.Log("[MonologueTrigger] Found MonologueUI!");
            }
            else
            {
                Debug.LogError("[MonologueTrigger] Could not find MonologueUI!");
            }
        }
        
        if (monologueData == null)
        {
            Debug.LogError("[MonologueTrigger] No monologue data assigned!");
        }
        else
        {
            Debug.Log($"[MonologueTrigger] Monologue data assigned: {monologueData.name}");
        }
        
        if (triggerOnStart)
        {
            Debug.Log($"[MonologueTrigger] Will trigger in {delayBeforeTrigger} seconds");
            Invoke(nameof(TriggerMonologue), delayBeforeTrigger);
        }
    }
        
    public void TriggerMonologue()
    {
        Debug.Log("[MonologueTrigger] TriggerMonologue() called!");
        
        // Check if already played
        if (triggerOnlyOnce && !string.IsNullOrEmpty(monologueID))
        {
            string key = "Monologue_" + monologueID;
            if (PlayerPrefs.GetInt(key, 0) == 1)
            {
                Debug.Log($"[MonologueTrigger] Monologue '{monologueID}' already played, skipping");
                return;
            }
        }
        
        Debug.Log("[MonologueTrigger] Checking references...");
        Debug.Log($"[MonologueTrigger] monologueData null? {monologueData == null}");
        Debug.Log($"[MonologueTrigger] monologueUI null? {monologueUI == null}");
        
        if (monologueData == null)
        {
            Debug.LogError("[MonologueTrigger] No monologue data assigned!");
            return;
        }
        
        if (monologueUI == null)
        {
            Debug.LogError("[MonologueTrigger] No MonologueUI found!");
            return;
        }
        
        Debug.Log("[MonologueTrigger] About to call DisplayMonologue...");
        
        // Display the monologue
        monologueUI.DisplayMonologue(monologueData);
        
        Debug.Log("[MonologueTrigger] DisplayMonologue called successfully!");
        
        // Mark as played
        if (triggerOnlyOnce && !string.IsNullOrEmpty(monologueID))
        {
            string key = "Monologue_" + monologueID;
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
            Debug.Log($"[MonologueTrigger] Marked monologue '{monologueID}' as played");
        }
    }
}