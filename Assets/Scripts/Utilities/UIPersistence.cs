using UnityEngine;

public class UIPersistence : MonoBehaviour
{
    void Awake()
    {
        // Just make this object persist - no singleton check
        DontDestroyOnLoad(gameObject);
        Debug.Log("Persisting object: " + gameObject.name);
    }
}