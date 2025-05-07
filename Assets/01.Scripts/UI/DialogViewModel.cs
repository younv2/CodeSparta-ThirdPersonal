using System;
using System.Collections.Generic;

public class DialogViewModel
{
    public string NpcName => currentNpc.Name;
    public string ImagePath => currentNpc.ImagePath;
    public string DialogText => currentDialog.DialogText;
    public IReadOnlyList<DialogChoice> Choices => currentDialog.ChoiceList;

    public event Action OnDialogUpdated; 
    public event Action<string> OnDialogEnded; 

    private DialogData currentDialog;
    private NPCData currentNpc;

    public void SetDialog(int dialogId)
    {
        currentDialog = DataManager.Instance.GetDialogData(dialogId);
        currentNpc = DataManager.Instance.GetNpcData(currentDialog.NPCId);

        OnDialogUpdated?.Invoke();
    }

    public void Choose(DialogChoice choice)
    {
        if (choice.NextId.HasValue)
        {
            SetDialog(choice.NextId.Value);
        }
        else
        {
            OnDialogEnded?.Invoke(currentDialog.EndActionKey);
        }
    }
    public void Progress()
    {
        if (currentDialog.ChoiceList.Count > 0)
            return; // 선택지가 있을 땐 버튼 클릭으로만 처리

        if (currentDialog.NextId.HasValue)
            SetDialog(currentDialog.NextId.Value);
        else
            OnDialogEnded?.Invoke(currentDialog.EndActionKey);
    }
}