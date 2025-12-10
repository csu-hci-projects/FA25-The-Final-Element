using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private GameObject settingsPanel;
    
    void Start()
    {
        // Make sure panel is hidden at start
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        
        // Load saved settings
        LoadSettings();
        
        // Add listener to toggle
        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }
    }
    
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }
    
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        
        // Save the setting
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
        
        Debug.Log("Fullscreen set to: " + isFullscreen);
    }
    
    private void LoadSettings()
    {
        // Load fullscreen setting (default to true)
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        
        // Apply the setting
        Screen.fullScreen = isFullscreen;
        
        // Update toggle UI
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = isFullscreen;
        }
    }
}