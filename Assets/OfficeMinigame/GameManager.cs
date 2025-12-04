using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Orb orb;
    public TextMeshProUGUI scoreText;

    public GameObject playButton;
    public GameObject gameOver;
    public GameObject verificationUI;

    public bool minigameActive = false;

    private int score;
    private bool reached17 = false;   // <--- Only tracks whether score hit 17

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
        reached17 = false;   // RESET FLAG

        scoreText.text = "0";

        playButton.SetActive(false);
        gameOver.SetActive(false);
        verificationUI.SetActive(false);

        Time.timeScale = 1f;
        orb.enabled = true;

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

        // HARD FAIL at 50
        if (score >= 50)
        {
            GameOver();
            return;
        }

        // Player has hit exactly 17
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

        // SUCCESS CONDITION → CRASH + EXACTLY 17 REACHED EARLIER
        if (reached17 && score == 17)
        {
            playButton.SetActive(false);
            gameOver.SetActive(false);

            verificationUI.SetActive(true); // SHOW "Verification Confirmed"

            // Lock minigame so escape/start no longer work
            minigameActive = false;
            return;
        }

        // NORMAL GAME OVER
        gameOver.SetActive(true);
        playButton.SetActive(true);
    }
}
