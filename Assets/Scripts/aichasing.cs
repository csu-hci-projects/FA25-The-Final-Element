using UnityEngine;
using UnityEngine.AI;

public class aichasing : MonoBehaviour
{
    public Transform player;          // Assign the player in Inspector
    public float stopDistance = 2f;   // Distance to building to stop
    public Animator animator;         // Assign AI's Animator

    private NavMeshAgent agent;
    private bool chasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true; // Start idle
        animator.SetBool("isChasing", false);
    }

    void Update()
    {
        if (chasing)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Stop chasing if player reaches the building
            if (distanceToPlayer <= stopDistance)
            {
                chasing = false;
                agent.isStopped = true;
                animator.SetBool("isChasing", false);
            }
            else
            {
                // Chase the player
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
        }
    }

    // Trigger to start chasing
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            chasing = true;
            animator.SetBool("isChasing", true); // Play running animation
        }
    }
}
