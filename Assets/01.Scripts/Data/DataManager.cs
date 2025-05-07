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
        if(datas == null)
        {
            throw new System.Exception("DialogData 로드에 실패했습니다");
        }
        foreach(var data in datas)
        {
            List<DialogChoice> choices = new List<DialogChoice>();
            int cnt = 1;
            while(true)
            {
                if(!data.ContainsKey($"Choice{cnt}NextId") || !data.ContainsKey($"Choice{cnt}Text"))
                {
                    break;
                }
                if (ParseNullableInt(data[$"Choice{cnt}NextId"]) is null && data[$"Choice{cnt}Text"].ToString().Equals(""))
                {
                    break;
                }
                choices.Add(new DialogChoice(ParseNullableInt(data[$"Choice{cnt}NextId"]), data[$"Choice{cnt}Text"].ToString()));

                cnt++;
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
