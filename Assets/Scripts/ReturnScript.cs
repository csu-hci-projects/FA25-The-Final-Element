using UnityEngine;

public class ReturnScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public WireEndMessage endMessageManager;

    void OnMouseUp()
    {
        if (endMessageManager != null)
        {
            endMessageManager.OnReturnClicked();
        }
    }
}
