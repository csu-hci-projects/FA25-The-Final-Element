using UnityEngine;

public class TrackerInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] private string promptText = "Pick up tracker";
    [SerializeField] private float interactionDistance = 3f;

    [Header("Tracking Target for glow")]
    [SerializeField] private Transform target;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private Color farEmissionColor = Color.black;
    [SerializeField] private Color closeEmissionColor = Color.cyan;
    [SerializeField] private float baseEmissionIntensity = 2f;

    private bool isHeld = false;
    private Renderer _renderer;
    private Material _matInstance;
    private Rigidbody _rb;

    private void Awake()
    {
        // Use Renderer from self or child
        _renderer = GetComponentInChildren<Renderer>();
        _rb = GetComponent<Rigidbody>();

        if (_renderer != null)
        {
            _matInstance = _renderer.material;   // unique material instance

            if (_matInstance.HasProperty("_EmissionColor"))
            {
                _matInstance.EnableKeyword("_EMISSION");
                _matInstance.SetColor("_EmissionColor", farEmissionColor * baseEmissionIntensity);
            }
            else
            {
                Debug.LogWarning("TrackerInteractable: Material has no _EmissionColor. Use URP/Lit with Emission enabled.");
            }
        }

        // Normal collider so raycast can hit it
        var col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = false;
    }

    private void Update()
    {
        if (target == null || _matInstance == null)
            return;

        float distance = Vector3.Distance(transform.position, target.position);
        UpdateGlowAndPulse(distance);
    }

    private void UpdateGlowAndPulse(float distance)
    {
        // Overall closeness (for color blend)
        float closeness = Mathf.InverseLerp(maxDistance, 0f, distance); // 0 = far, 1 = very close
        Color baseColor = Color.Lerp(farEmissionColor, closeEmissionColor, closeness);

        float intensity = baseEmissionIntensity;


        if (_matInstance.HasProperty("_EmissionColor"))
        {
            _matInstance.SetColor("_EmissionColor", baseColor * intensity);
        }
    }

    // ===== IInteractable implementation =====

    public bool CanInteract()
    {
        return !isHeld;
    }

    public float GetInteractionDistance()
    {
        return interactionDistance;
    }

    public string GetInteractionPrompt()
    {
        return promptText;
    }

    public void Interact(GameObject interactor)
    {
        var controller = interactor.GetComponent<PlayerInteractionController>();
        if (controller == null)
        {
            Debug.LogWarning("TrackerInteractable: Interactor has no PlayerInteractionController.");
            return;
        }

        if (!isHeld)
        {
            controller.AttachToHand(transform);
            isHeld = true;
        }
        else
        {
            controller.DropHeldObject();
            isHeld = false;
        }
    }
}
