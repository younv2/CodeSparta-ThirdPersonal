using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] private Image npcImage;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Transform choiceRoot;        
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private SpriteAtlas npcAtlas;

    private Dictionary<string, Action> endActionMap = new();

    private DialogViewModel viewModel;
    private readonly List<GameObject> buttonPool = new();

    
    private void Awake()
    {
        endActionMap["StartMiniGame"] = () => SceneManager.LoadScene("TheStackScene");
    }
    private void OnEnable()
    {
        InputHandler.Instance.OnProgressDialogInput += Progress;
    }
    private void OnDisable()
    {
        InputHandler.Instance.OnProgressDialogInput -= Progress;
    }
    public void Bind(DialogViewModel vm)
    {
        viewModel = vm;
        viewModel.OnDialogUpdated += InitUI;
        viewModel.OnDialogEnded += HandleDialogEnd;
    }
    private void InitUI()
    {
        npcImage.sprite = npcAtlas.GetSprite(viewModel.ImagePath);
        npcName.text = viewModel.NpcName;
        dialogText.text = viewModel.DialogText;

        foreach (var btn in buttonPool) btn.SetActive(false);

        foreach (var choice in viewModel.Choices)
        {
            var go = GetButton();
            var txt = go.GetComponentInChildren<TextMeshProUGUI>();
            var btn = go.GetComponent<Button>();

            go.SetActive(true);
            txt.text = choice.Text;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => viewModel.Choose(choice));
        }
    }

    private GameObject GetButton()
    {
        foreach (var button in buttonPool)
            if (!button.activeSelf) return button;

        var newBtn = Instantiate(choiceButtonPrefab, choiceRoot);
        buttonPool.Add(newBtn);
        return newBtn;
    }

    public void Progress()
    {
        viewModel.Progress();
    }
    private void HandleDialogEnd(string actionId)
    {
        if (endActionMap.TryGetValue(actionId, out var action))
        {
            action.Invoke();
        }

        gameObject.SetActive(false);
    }
}