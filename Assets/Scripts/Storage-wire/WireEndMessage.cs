using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
public class WireEndMessage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public WireGameLogic Game;
    public GameObject welldonemessage;
    public GameObject returnmessage;
    
    public GameObject explosionUI;

    public GameObject fadeCanvas;   // black fade panel
    public GameObject blackPanel;    // Panel covering screen with CanvasGroup
    public float fadeTime = 1.5f; 
    [SerializeField] GameObject player;
    [SerializeField] EventReference ExplosionEvent;

    public void ExplosionSound()
    {
        RuntimeManager.PlayOneShotAttached(ExplosionEvent, player);
    }
    void Start()
    {
        welldonemessage.SetActive(false);
        returnmessage.SetActive(false);
        explosionUI.SetActive(false);
        fadeCanvas.SetActive(true);
        blackPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnReturnClicked()
    {
        // Game.ResetWires();
        welldonemessage.SetActive(false);
        returnmessage.SetActive(false);
        StartCoroutine(DoEndSequence());
        
    }

    IEnumerator DoEndSequence()
    {
        
        blackPanel.SetActive(true);

        // Get or add CanvasGroup for fading
        CanvasGroup cg = fadeCanvas.GetComponent<CanvasGroup>();
        if (cg == null) cg = fadeCanvas.AddComponent<CanvasGroup>();
        cg.alpha = 0;

        // Fade to black
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            cg.alpha = t / fadeTime;
            yield return null;
        }

        // Show explosion UI on top of black fade
        explosionUI.SetActive(true);
        ExplosionSound();
        explosionUI.transform.SetAsLastSibling();
    }
    
}
