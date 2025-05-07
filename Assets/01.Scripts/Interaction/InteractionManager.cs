using UnityEngine;

interface IInteractable
{
    void Interact();
}

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private LayerMask interactableLayer;

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
    public void Interactive(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapCircle(pos, interactRange, interactableLayer);
        if (hit != null)
        {
            hit.GetComponent<IInteractable>()?.Interact();
        }
    }
}