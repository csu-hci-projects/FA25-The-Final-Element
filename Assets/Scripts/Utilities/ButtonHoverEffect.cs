using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Animation Settings")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float animationSpeed = 10f;
    
    [Header("Color Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(1f, 0.85f, 0.5f); // Yellowish tan
    
    private TextMeshProUGUI buttonText;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private Color targetColor;
    
    void Start()
    {
        // Get the TextMeshPro component from child
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        
        if (buttonText == null)
        {
            Debug.LogError("No TextMeshProUGUI found on button!");
            return;
        }
        
        originalScale = buttonText.transform.localScale;
        targetScale = originalScale;
        targetColor = normalColor;
        buttonText.color = normalColor;
    }
    
    void Update()
    {
        if (buttonText == null) return;
        
        // Smooth scale animation
        buttonText.transform.localScale = Vector3.Lerp(
            buttonText.transform.localScale,
            targetScale,
            Time.deltaTime * animationSpeed
        );
        
        // Smooth color animation
        buttonText.color = Color.Lerp(
            buttonText.color,
            targetColor,
            Time.deltaTime * animationSpeed
        );
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
        targetColor = hoverColor;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        targetColor = normalColor;
    }
}