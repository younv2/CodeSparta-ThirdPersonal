using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI;

    private void Start()
    {
        InteractionManager.Instance.OnInteractableFound += ShowUI;
        InteractionManager.Instance.OnInteractableLost += HideUI;
    }

    private void ShowUI(Transform target)
    {
        interactionUI.SetActive(true);
        interactionUI.transform.position = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 0.5f);
    }

    private void HideUI()
    {
        interactionUI.SetActive(false);

    }
}