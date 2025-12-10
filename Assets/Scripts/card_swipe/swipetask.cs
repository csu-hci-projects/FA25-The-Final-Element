using UnityEngine;
using System.Collections.Generic;
public class swipetask : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<swipepoint> swipe_points = new List<swipepoint>();
    public float _countdownMax = 0.5f;
    private int currentSwipepointIndex = 0;
    private float countdown = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countdown-= Time.deltaTime;
        if(currentSwipepointIndex != 0 && countdown <= 0)
        {
            currentSwipepointIndex = 0;
            Debug.Log("Error");
        }
    }
    public void SwipePointTrigger(swipepoint SwipePoint)
    {
        if(SwipePoint == swipe_points[currentSwipepointIndex])
        {
            currentSwipepointIndex++;
            countdown = _countdownMax;
        }
        if(currentSwipepointIndex >= swipe_points.Count)
        {
            currentSwipepointIndex = 0;
            Debug.Log("finished");
        }
    }
}
