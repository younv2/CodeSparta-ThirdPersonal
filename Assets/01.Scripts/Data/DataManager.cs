using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<DataManager>();

                if (instance == null)
                {
                    GameObject go = new GameObject("DataManager");
                    instance = go.AddComponent<DataManager>();
                }

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }
    private Dictionary<int, DialogData> dialogData = new Dictionary<int, DialogData>();
    private Dictionary<int, NPCData> npcData = new Dictionary<int, NPCData>();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        LoadAllData();
    }

    private void LoadAllData()
    {
        LoadDialogData();
        LoadNPCData();
    }

    private void LoadDialogData()
    {
        var datas = CsvReader.Read(DataPath.DialogTableCSV);
        var choicedatas = CsvReader.Read(DataPath.DialogChoiceTableCSV);
        if (datas == null || choicedatas == null)
        {
            throw new System.Exception("DialogData 로드에 실패했습니다");
        }
        foreach (var data in datas)
        {
            List<DialogChoice> choices = new List<DialogChoice>();
            foreach(var choice in choicedatas.FindAll(x => x["DialogId"].ToString().Equals(data["DialogId"].ToString())))
            {
                choices.Add(new DialogChoice(ParseNullableInt(choice[$"NextId"]), choice[$"ChoiceText"].ToString()));
            }

            DialogData temp = new DialogData(
            (int)data["DialogId"],
            (int)data["NPCId"],
            data["DialogText"].ToString(),
            ParseNullableInt(data["NextId"]),
            choices,
            data["EndActionKey"].ToString()
            );
            dialogData.Add((int)data["DialogId"], temp);
        }
    }
    private void LoadNPCData()
    {
        var datas = CsvReader.Read(DataPath.NPCTableCSV);

        if (datas == null)
        {
            throw new System.Exception("NPCData 로드에 실패했습니다");
        }
        foreach (var data in datas)
        {
            NPCData temp = new NPCData(
            (int)data["NPCId"],
            data["Name"].ToString(),
            data["ImagePath"].ToString()
            );

            npcData.Add((int)data["NPCId"], temp);
        }
    }
    private int? ParseNullableInt(object value)
    {
        if (value == null) return null;
        if (int.TryParse(value.ToString(), out int result))
            return result;
        return null;
    }
    public DialogData GetDialogData(int dialogId)
    {
        return dialogData[dialogId];
    }

    public NPCData GetNpcData(int npcId)
    {
        return npcData[npcId];
    }
}
