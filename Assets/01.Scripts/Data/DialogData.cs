using System.Collections.Generic;

public class DialogData
{
    private readonly int id;
    private readonly int npcId;
    private readonly string dialogText;
    private readonly int? nextId;
    private readonly List<DialogChoice> choiceList;

    public int Id { get => id; }
    public int NPCId { get => npcId; }
    public string DialogText { get => dialogText; }
    public int? NextId { get => nextId; }
    public IReadOnlyList<DialogChoice> ChoiceList {  get => choiceList; }

    public DialogData(int id, int npcId, string dialogText, int? nextId, List<DialogChoice> choiceList)
    {
        this.id = id;
        this.npcId = npcId;
        this.dialogText = dialogText;
        this.nextId = nextId;
        this.choiceList = choiceList;
    }
}
public class DialogChoice
{
    int? nextId;
    string text;
    
    public int? NextId { get => nextId; }
    public string Text { get => text; }
    
    public DialogChoice(int? choiceNextId, string choiceText)
    {
        this.nextId = choiceNextId;
        this.text = choiceText;
    }
}