using UnityEngine;
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

    // ADD THIS DEBUG LINE
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
}