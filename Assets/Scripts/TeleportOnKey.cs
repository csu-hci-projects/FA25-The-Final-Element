using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TeleportOnKey : MonoBehaviour
{
    public string sceneToLoad = "Lab1";
    public GameObject pressEText;

    private bool playerInZone = false;

    void Start()
    {
        // Hide the text at start
        if (pressEText != null)
            pressEText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            if (pressEText != null)
                pressEText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            if (pressEText != null)
                pressEText.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
