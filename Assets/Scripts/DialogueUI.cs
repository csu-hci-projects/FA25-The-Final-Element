using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    public float displayTime = 3f;      // time AFTER typewriter finishes
    public float fadeSpeed = 2f;
    public float typeSpeed = 0.03f;     // typing delay per character

    private Coroutine dialogueRoutine;

    private void Awake()
    {
        Instance = this;
        if (dialogueText != null)
            dialogueText.raycastTarget = false;

        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;

        canvasGroup.alpha = 0f;
        dialogueText.text = "";
    }

    public void ShowMessage(string message)
    {
        // If another message is running, stop it
        if (dialogueRoutine != null)
            StopCoroutine(dialogueRoutine);

        dialogueRoutine = StartCoroutine(TypeAndDisplay(message));
    }

    private IEnumerator TypeAndDisplay(string message)
    {
        canvasGroup.alpha = 1f;
        dialogueText.text = "";

        // Typewriter effect
        foreach (char c in message)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        // WAIT after completing typewriter
        yield return new WaitForSecondsRealtime(displayTime);

        // Start fade-out
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }

        dialogueText.text = "";
    }
}
