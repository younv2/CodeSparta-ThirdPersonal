using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Dialog dialogView;
    private DialogViewModel dialogViewModel;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UIManager>();

                if (instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    instance = go.AddComponent<UIManager>();
                }
            }

            return instance;
        }
    }

    private void Start()
    {
        dialogViewModel = new DialogViewModel();
        dialogView.Bind(dialogViewModel);
    }

    public void ShowDialog(int dialogId)
    {
        dialogViewModel.SetDialog(dialogId);
        dialogView.gameObject.SetActive(true);
    }
}
