using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel; // Assign your Settings UI root (can be empty for now)

    [Header("Scene Loading")]
    [SerializeField] private string firstSceneName = "Game"; // Change to your first gameplay scene
    [SerializeField] private bool useFade = false;
    [SerializeField] private Animator fadeAnimator; // Optional: Animator with a Trigger "FadeOut"

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip clickSfx;

    private void Awake()
    {
        // Basic safety checks so nothing silently fails.
        if (!EventSystem.current)
        {
            var es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Debug.Log("[MainMenu] No EventSystem found; created one automatically.", es);
        }

        if (newGameButton)   newGameButton.onClick.AddListener(OnNewGame);
        if (settingsButton)  settingsButton.onClick.AddListener(OnOpenSettings);
        if (quitButton)      quitButton.onClick.AddListener(OnQuit);

        // Make sure settings panel starts hidden.
        if (settingsPanel) settingsPanel.SetActive(false);
    }

    private void PlayClick()
    {
        if (uiAudioSource && clickSfx) uiAudioSource.PlayOneShot(clickSfx);
    }

    public void OnNewGame()
    {
        PlayClick();
        DisableAllButtons(true);
        if (useFade && fadeAnimator)
        {
            StartCoroutine(LoadWithFade());
        }
        else
        {
            StartCoroutine(LoadSceneAsync(firstSceneName));
        }
    }

    public void OnOpenSettings()
    {
        PlayClick();
        if (settingsPanel)
        {
            settingsPanel.SetActive(true);
            // Move UI focus to first interactable in the settings panel (if any)
            var first = settingsPanel.GetComponentInChildren<Selectable>();
            if (first) first.Select();
        }
    }

    // Hook this to a "Back" button inside your Settings panel
    public void OnCloseSettings()
    {
        PlayClick();
        if (settingsPanel) settingsPanel.SetActive(false);
        if (newGameButton) newGameButton.Select();
    }

    public void OnQuit()
    {
        PlayClick();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    private IEnumerator LoadWithFade()
    {
        fadeAnimator.SetTrigger("FadeOut");
        // Wait for the fade animation to finish (expects an animation event or length)
        yield return new WaitForSeconds(0.5f); // adjust to your fade length
        yield return LoadSceneAsync(firstSceneName);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = true; // let it switch when done
        while (!op.isDone)
        {
            yield return null;
        }
    }

    private void DisableAllButtons(bool disabled)
    {
        if (newGameButton)  newGameButton.interactable = !disabled;
        if (settingsButton) settingsButton.interactable = !disabled;
        if (quitButton)     quitButton.interactable = !disabled;
    }
}
