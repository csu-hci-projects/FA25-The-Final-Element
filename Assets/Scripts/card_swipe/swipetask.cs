using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using FMODUnity;

public class swipetask : MonoBehaviour
{
    [Header("Swipe Settings")]
    public List<swipepoint> swipe_points = new List<swipepoint>();
    public float _countdownMax = 0.5f;
    
    [Header("Exit Settings")]
    [SerializeField] private KeyCode exitKey = KeyCode.Q;
    [SerializeField] private string returnSceneName = "Island";
    [SerializeField] private string returnSpawnPointName = "CardSwipeExit";
    
    [Header("Success Settings")]
    [SerializeField] private string successSceneName = "NextScene";
    
    [Header("Audio")]
    [SerializeField] GameObject player;
    [SerializeField] EventReference WinEvent;
    [SerializeField] EventReference LooseEvent;
    private int currentSwipepointIndex = 0;
    private float countdown = 0;
    private bool taskCompleted = false;

    
    public void PlayWinSound()
    {
        RuntimeManager.PlayOneShotAttached(WinEvent, player);
    }

    public void PlayLooseSound()
    {
        RuntimeManager.PlayOneShotAttached(LooseEvent, player);
    }
    void Start()
    {
        // Unlock and show cursor for card dragging
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Make sure time is running
        Time.timeScale = 1f;
        
        Debug.Log("Card swipe game ready - cursor unlocked");
    }

    void Update()
    {
        // Check for exit key to return to spawn point
        if (Input.GetKeyDown(exitKey))
        {
            ReturnToSpawnPoint();
            return;
        }

        // Countdown timer logic
        countdown -= Time.deltaTime;
        if(currentSwipepointIndex != 0 && countdown <= 0)
        {
            currentSwipepointIndex = 0;
            Debug.Log("Error - Failed swipe sequence");
            PlayLooseSound();
        }
    }

    public void SwipePointTrigger(swipepoint SwipePoint)
    {
        if(taskCompleted) return;

        if(SwipePoint == swipe_points[currentSwipepointIndex])
        {
            currentSwipepointIndex++;
            countdown = _countdownMax;
        }
        
        if(currentSwipepointIndex >= swipe_points.Count)
        {
            currentSwipepointIndex = 0;
            taskCompleted = true;
            Debug.Log("Task finished! Loading next scene...");
            LoadSuccessScene();
        }
    }

    private void ReturnToSpawnPoint()
    {
        // Re-lock cursor before returning to Island
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        PlayerPrefs.SetString("SpawnPointName", returnSpawnPointName);
        PlayerPrefs.Save();
        
        Debug.Log($"Returning to {returnSceneName} at spawn point: {returnSpawnPointName}");
        SceneManager.LoadScene(returnSceneName);
    }

    private void LoadSuccessScene()
    {
        // Re-lock cursor before loading next scene
        PlayWinSound();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Debug.Log($"Loading success scene: {successSceneName}");
        SceneManager.LoadScene(successSceneName);
    }
}