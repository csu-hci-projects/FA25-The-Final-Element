using System.Collections;
using UnityEngine;
using TMPro;

public class MonologueUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject monologuePanel;
    [SerializeField] private TextMeshProUGUI monologueText;
    [SerializeField] private GameObject skipHint; // Optional "Press Space to Skip" text
    
    private Coroutine typingCoroutine;
    private bool isDisplaying = false;
    private bool canSkip = false;
    
    private void Start()
    {
        // Make sure panel is hidden at start
        if (monologuePanel != null)
        {
            monologuePanel.SetActive(false);
        }
        
        if (skipHint != null)
        {
            skipHint.SetActive(false);
        }
    }
    
    private void Update()
    {
        // Allow skipping if enabled
        if (isDisplaying && canSkip && Input.GetKeyDown(KeyCode.Space))
        {
            SkipMonologue();
        }
    }
    
public void DisplayMonologue(MonologueData monologue)
{
    Debug.Log("[MonologueUI] DisplayMonologue called!");
    
    if (monologue == null)
    {
        Debug.LogError("[MonologueUI] Monologue data is null!");
        return;
    }
    
    Debug.Log($"[MonologueUI] Monologue text: {monologue.monologueText}");
    Debug.Log($"[MonologueUI] monologuePanel null? {monologuePanel == null}");
    Debug.Log($"[MonologueUI] monologueText null? {monologueText == null}");
    
    // Stop any existing monologue
    if (typingCoroutine != null)
    {
        StopCoroutine(typingCoroutine);
    }
    
    typingCoroutine = StartCoroutine(DisplayMonologueCoroutine(monologue));
}
    
private IEnumerator DisplayMonologueCoroutine(MonologueData monologue)
{
    Debug.Log("[MonologueUI] Coroutine started!");
    isDisplaying = true;
    canSkip = monologue.canSkip;
    
    // Show panel
    if (monologuePanel != null)
    {
        Debug.Log("[MonologueUI] Activating panel...");
        monologuePanel.SetActive(true);
        Debug.Log($"[MonologueUI] Panel active? {monologuePanel.activeSelf}");
    }
    else
    {
        Debug.LogError("[MonologueUI] monologuePanel is NULL!");
    }
    
    // Show skip hint if allowed
    if (skipHint != null && monologue.canSkip)
    {
        skipHint.SetActive(true);
        Debug.Log("[MonologueUI] Skip hint activated");
    }
    
    // Clear text
    monologueText.text = "";
    Debug.Log("[MonologueUI] Text cleared, starting typing...");
    
    // Type out text
    if (monologue.typingSpeed > 0)
    {
        float delay = 1f / monologue.typingSpeed;
        foreach (char c in monologue.monologueText)
        {
            monologueText.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
    else
    {
        // Instant display
        monologueText.text = monologue.monologueText;
    }
    
    Debug.Log("[MonologueUI] Typing finished, waiting for display duration...");
    
    // Wait for display duration
    yield return new WaitForSeconds(monologue.displayDuration);
    
    // Hide monologue
    HideMonologue();
}
    
    private void SkipMonologue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        HideMonologue();
    }
    
    private void HideMonologue()
    {
        isDisplaying = false;
        canSkip = false;
        
        if (monologuePanel != null)
        {
            monologuePanel.SetActive(false);
        }
        
        if (skipHint != null)
        {
            skipHint.SetActive(false);
        }
        
        Debug.Log("[MonologueUI] Monologue finished");
    }
    
    public bool IsDisplaying()
    {
        return isDisplaying;
    }
}