using UnityEngine;

public class Pipes : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;   // how long before the pipe is destroyed

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
