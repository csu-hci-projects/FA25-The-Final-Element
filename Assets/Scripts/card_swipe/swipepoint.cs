using UnityEngine;

public class swipepoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private swipetask _swipetask;
    private void Awake()
    {
        _swipetask = GetComponentInParent<swipetask>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        _swipetask.SwipePointTrigger(this);
    }
}
