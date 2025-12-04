using UnityEngine;

public class Orb : MonoBehaviour
{
    private Vector3 direction;
    public float gravity = -9.8f;
    public float strength = 5f;

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    void Update()
    {
        // Press SPACE to flap
        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction = Vector3.up * strength;
        }

        // Apply gravity
        direction.y += gravity * Time.deltaTime;

        // Move player
        transform.position += direction * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.tag == "Obstacle") {
            FindObjectOfType<GameManager>().GameOver();    
        } else if (other.gameObject.tag == "Scoring") {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
    }
}
