using System;
using UnityEngine;

interface IInteractable
{
    void Interact();
}

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform target;
    private Collider2D currentNearbyTarget;


    public event Action<Transform> OnInteractableFound;
    public event Action OnInteractableLost;

    public static InteractionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    

    private void Update()
    {
        DetectNearbyInteractable();
    }

    private void DetectNearbyInteractable()
    {
        Collider2D hit = Physics2D.OverlapCircle(target.position, interactRange, interactableLayer);

        if (hit != null && hit != currentNearbyTarget)
        {
            currentNearbyTarget = hit;
            OnInteractableFound?.Invoke(hit.transform);
        }
        else if (hit == null && currentNearbyTarget != null)
        {
            currentNearbyTarget = null;
            OnInteractableLost?.Invoke();
        }
    }

    public void Interactive(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapCircle(pos, interactRange, interactableLayer);
        if (hit != null)
        {
            hit.GetComponent<IInteractable>()?.Interact();
        }
    }
}