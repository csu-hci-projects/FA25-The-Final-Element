using UnityEngine;
using UnityEngine.EventSystems;

public class card_swipe : MonoBehaviour, IDragHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Canvas canvas;
    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }
}
