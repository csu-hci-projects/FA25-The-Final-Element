using UnityEngine;
using UnityEngine.SceneManagement; // ADD THIS
using TMPro;
using Unity.VisualScripting;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxInteractionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    [SerializeField] private GameObject interactionPromptUI;
    [SerializeField] private TextMeshProUGUI interactionText;

    private IInteractable currentInteractable;
    private GameObject currentInteractableObject;

    // ==== NEW: pickup / hold settings ====
    [Header("Pickup / Hold Settings")]
    [SerializeField] private Transform handHold;   // empty child of camera where held object goes
    private Transform heldObject;
    // =====================================

    // ADD THESE METHODS
    void OnEnable()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Clear interaction prompt when scene loads
        ClearCurrentInteractable();
        Debug.Log($"[PlayerInteraction] Scene loaded: {scene.name} - Prompt cleared");
    }
    // END OF NEW METHODS

    private void Start()
    {
        // Auto-find camera if not assigned
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera == null)
            {
                Debug.LogError("No camera found on player!");
            }
            else
            {
                Debug.Log($"Camera found: {playerCamera.name}");
            }
        }

        // NEW: warn if handHold not set
        if (handHold == null)
        {
            Debug.LogWarning("[PlayerInteraction] HandHold is not assigned. Held items will not be positioned.");
        }
    }

    private void Update()
    {
        CheckForInteractable();

        // Handle interaction input
        if (Input.GetKeyDown(interactKey) && currentInteractable != null && currentInteractable.CanInteract())
        {
            currentInteractable.Interact(gameObject);
        }
    }

    private void CheckForInteractable()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Debug ray visualization
        Debug.DrawRay(ray.origin, ray.direction * maxInteractionDistance, Color.red);

        if (Physics.Raycast(ray, out hit, maxInteractionDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null && interactable.CanInteract())
            {
                float distance = Vector3.Distance(transform.position, hit.point);
                if (distance <= interactable.GetInteractionDistance())
                {
                    SetCurrentInteractable(interactable, hit.collider.gameObject);
                    return;
                }
            }
        }
        ClearCurrentInteractable();
    }

    private void SetCurrentInteractable(IInteractable interactable, GameObject obj)
    {
        if (currentInteractable != interactable)
        {
            currentInteractable = interactable;
            currentInteractableObject = obj;
            ShowInteractionPrompt(interactable.GetInteractionPrompt());
        }
    }

    private void ClearCurrentInteractable()
    {
        if(currentInteractable != null)
        {
            currentInteractable = null;
            currentInteractableObject = null;
            HideInteractionPrompt();
        }
    }

    private void ShowInteractionPrompt(string text)
    {
        if (interactionPromptUI != null)
        {
            interactionPromptUI.SetActive(true);
            if(interactionText != null)
            {
                interactionText.text = text;
            }
        }
    }

    private void HideInteractionPrompt()
    {
        if (interactionPromptUI != null)
        {
            interactionPromptUI.SetActive(false);
        }
    }

    // ====== NEW: functions used by the tracker to attach / drop ======

    public void AttachToHand(Transform obj)
{
    if (handHold == null)
    {
        Debug.LogWarning("[PlayerInteraction] AttachToHand called but handHold is null.");
        return;
    }

    // Drop whatever we're already holding (optional)
    if (heldObject != null)
    {
        DropHeldObject();
    }

    heldObject = obj;

    // Turn off physics if it has a Rigidbody
    if (heldObject.TryGetComponent<Rigidbody>(out var rb))
    {
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    // Parent to hand, but DO NOT touch rotation or scale
    heldObject.SetParent(handHold);

    // Put the pivot at the handHold position.
    // You can offset the handHold transform in the scene to place it nicely.
    heldObject.localPosition = Vector3.zero;

    // IMPORTANT:
    // - We DO NOT change heldObject.localRotation
    // - We DO NOT change heldObject.localScale
    // So whatever rotation/scale it had in the world is preserved.
}



    public void DropHeldObject()
    {
        if (heldObject == null) return;

        // Re-enable physics
        if (heldObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        heldObject.SetParent(null);
        heldObject = null;
    }

    // Optional helper if other scripts need to know
    public bool IsHoldingSomething()
    {
        return heldObject != null;
    }

    // ================================================================
}
