using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionUIPrefab;

    private void Start()
    {
        InteractionManager.Instance.OnInteractableFound += ShowUI;
        InteractionManager.Instance.OnInteractableLost += HideUI;
    }

    private void ShowUI(Transform target)
    {
        interactionUIPrefab.SetActive(true);
        interactionUIPrefab.transform.position = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 0.5f);
    }

    private void HideUI()
    {
        interactionUIPrefab.SetActive(false);

    }
}