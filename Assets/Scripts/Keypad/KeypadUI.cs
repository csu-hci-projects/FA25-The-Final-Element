using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class KeypadUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private GameObject keypadCanvas;
    [SerializeField] private Image ledDisplayPanel;
    
    [Header("Code Settings")]
    [SerializeField] private string correctCode = "1234";
    [SerializeField] private int maxDigits = 4;
    
    [Header("Visual Feedback Colors")]
    [SerializeField] private Color normalDisplayColor = new Color(0f, 0.78f, 1f); // Cyan
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color errorColor = Color.red;
    
    private string currentInput = "";
    private bool isProcessing = false;
    private FirstPersonController playerController;
    private KeypadInteraction connectedKeypad; // The keypad that opened this UI
    
    void Start()
    {
        UpdateDisplay();
        
        // Find player controller
        playerController = FindAnyObjectByType<FirstPersonController>();
        
        // Make sure keypad UI is hidden at start
        if (keypadCanvas != null)
        {
            keypadCanvas.SetActive(false);
        }
    }
    
    // Call this from each number button (0-9)
    public void OnNumberPressed(string number)
    {
        if (isProcessing) return;
        
        // Only add if we haven't reached max digits
        if (currentInput.Length < maxDigits)
        {
            currentInput += number;
            UpdateDisplay();
            
            // Auto-check when we reach max digits
            if (currentInput.Length == maxDigits)
            {
                StartCoroutine(CheckCode());
            }
        }
    }
    
    // Call this from the CLR button
    public void OnClearPressed()
    {
        if (isProcessing) return;
        
        currentInput = "";
        UpdateDisplay();
    }
    
    // Call this from the DEL button
    public void OnDeletePressed()
    {
        if (isProcessing) return;
        
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
        }
    }
    
    private void UpdateDisplay()
    {
        // Show entered digits, pad with dashes
        string display = currentInput.PadRight(maxDigits, '-');
        displayText.text = display;
        displayText.color = normalDisplayColor;
    }
    
    private IEnumerator CheckCode()
    {
        isProcessing = true;
        
        if (currentInput == correctCode)
        {
            // Success!
            yield return StartCoroutine(ShowSuccess());
            
            // Tell the keypad it was successful
            if (connectedKeypad != null)
            {
                connectedKeypad.OnCodeCorrect();
            }
            
            // Close the UI
            CloseKeypad();
        }
        else
        {
            // Wrong code
            yield return StartCoroutine(ShowError());
            currentInput = "";
            UpdateDisplay();
        }
        
        isProcessing = false;
    }
    
    private IEnumerator ShowSuccess()
    {
        // Flash green
        displayText.color = successColor;
        if (ledDisplayPanel != null)
        {
            ledDisplayPanel.color = successColor;
        }
        
        yield return new WaitForSecondsRealtime(0.5f);
        
        // Reset colors
        displayText.color = normalDisplayColor;
        if (ledDisplayPanel != null)
        {
            ledDisplayPanel.color = new Color(0.02f, 0.04f, 0.08f); // Dark blue
        }
    }
    
    private IEnumerator ShowError()
    {
        // Flash red multiple times
        for (int i = 0; i < 3; i++)
        {
            displayText.color = errorColor;
            if (ledDisplayPanel != null)
            {
                ledDisplayPanel.color = errorColor;
            }
            
            yield return new WaitForSecondsRealtime(0.15f);
            
            displayText.color = normalDisplayColor;
            if (ledDisplayPanel != null)
            {
                ledDisplayPanel.color = new Color(0.02f, 0.04f, 0.08f);
            }
            
            yield return new WaitForSecondsRealtime(0.15f);
        }
    }
    
    public void OpenKeypad(KeypadInteraction keypad)
    {
        connectedKeypad = keypad;
        
        if (keypadCanvas != null)
        {
            keypadCanvas.SetActive(true);
        }
        
        currentInput = "";
        UpdateDisplay();
        
        // Pause player movement
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Disable player controller
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }
    
    public void CloseKeypad()
    {
        if (keypadCanvas != null)
        {
            keypadCanvas.SetActive(false);
        }
        
        currentInput = "";
        
        // Resume player movement
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Re-enable player controller
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
    
    void Update()
    {
        // Allow ESC to close keypad
        if (Input.GetKeyDown(KeyCode.Escape) && keypadCanvas != null && keypadCanvas.activeSelf)
        {
            CloseKeypad();
        }
    }
}