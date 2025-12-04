using UnityEngine;
using UnityEngine.UI;

public class BackgroundDrift : MonoBehaviour
{
    [Header("Drift Settings")]
    [SerializeField] private float driftSpeed = 2f;
    [SerializeField] private float driftAmount = 20f;
    
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private float timeElapsed;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }
    
    void Update()
    {
        timeElapsed += Time.deltaTime * driftSpeed;
        
        // Create subtle horizontal and vertical drift using sine waves
        float xOffset = Mathf.Sin(timeElapsed * 0.5f) * driftAmount;
        float yOffset = Mathf.Cos(timeElapsed * 0.3f) * driftAmount * 0.5f;
        
        rectTransform.anchoredPosition = startPosition + new Vector2(xOffset, yOffset);
    }
}