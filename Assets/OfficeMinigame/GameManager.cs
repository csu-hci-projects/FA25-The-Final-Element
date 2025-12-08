using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using FMODUnity;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Orb orb;
    public TextMeshProUGUI scoreText;

    public GameObject playButton;
    public GameObject gameOver;
    public GameObject verificationUI;

    public bool minigameActive = false;

    private int score;
    private bool reached17 = false;

    [Header("Audio")]
    [SerializeField] EventReference LooseEvent;
    [SerializeField] EventReference WinEvent;
    [SerializeField] GameObject player;

    [Header("Success Teleport")]
    public float teleportDelay = 2f;   // delay before teleporting
    private string nextSceneName = "LabRoom";   // <-- hardcoded since you confirmed name

    public void PlayLooseSound()
    {
        RuntimeManager.PlayOneShotAttached(LooseEvent, player);
    }

    public void PlayWinSound()
    {
        RuntimeManager.PlayOneShotAttached(WinEvent, player);
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (minigameActive)
            Pause();
    }

    public void ActivateMinigame()
    {
        minigameActive = true;
        Pause();
    }

    public void Play()
    {
        if (!minigameActive)
            return;

        score = 0;
        reached17 = false;
        scoreText.text = "0";

        playButton.SetActive(false);
        gameOver.SetActive(false);
        verificationUI.SetActive(false);

        Time.timeScale = 1f;
        orb.enabled = true;

        // Remove old pipes
        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
            Destroy(pipes[i].gameObject);
    }

    public void Pause()
    {
        if (!minigameActive)
            return;

        Time.timeScale = 0f;
        orb.enabled = false;
    }

    public void IncreaseScore()
    {
        if (!minigameActive)
            return;

        score++;
        scoreText.text = score.ToString();

        // Hard fail
        if (score >= 50)
        {
            GameOver();
            return;
        }

        // Mark that we reached 17
        if (score == 17)
        {
            reached17 = true;
        }
    }

    public void GameOver()
    {
        if (!minigameActive)
            return;

        Pause();

        // SUCCESS = crash with score 17 AFTER having reached 17
        if (reached17 && score == 17)
        {
            playButton.SetActive(false);
            gameOver.SetActive(false);
            verificationUI.SetActive(true);

            minigameActive = false;   // lock out escape and restart
            PlayWinSound();

            StartCoroutine(TeleportAfterDelay());
            return;
        }

        // NORMAL FAILURE
        gameOver.SetActive(true);
        playButton.SetActive(true);
        PlayLooseSound();
    }

    private IEnumerator TeleportAfterDelay()
    {
        yield return new WaitForSecondsRealtime(teleportDelay);
        SceneManager.LoadScene(nextSceneName);
    }
}
