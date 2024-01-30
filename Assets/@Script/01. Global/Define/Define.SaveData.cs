using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSaveData
{
    public string itemID;
    public int itemCount;

    public string[] randomStatIDs;
    public VALUE_TYPE[] randomStatValueTypes;
    public float[] randomStatValues;

    public void SaveItemData<T>(T item) where T : BaseItem
    {
        if(item != null)
        {
            itemID = item.GetItemID();
            itemCount = item.ItemCount;

            if (!item.RandomOptions.IsNullOrEmpty())
            {
                randomStatIDs = new string[item.RandomOptions.Length];
                randomStatValueTypes = new VALUE_TYPE[item.RandomOptions.Length];
                randomStatValues = new float[item.RandomOptions.Length];

                for (int i = 0; i < item.RandomOptions.Length; i++)
                {
                    randomStatIDs[i] = item.RandomOptions[i].StatOptionID;
                    randomStatValueTypes[i] = item.RandomOptions[i].valueType;
                    randomStatValues[i] = item.RandomOptions[i].value;
                }
            }
        }
    }
}

[System.Serializable]
public class QuestSaveData
{
    public string questID;
    public QUEST_STATE questState;

    public int currentTaskIndex;
    public int[] currentAmounts;

    public QuestSaveData SaveQuestData(Quest quest)
    {
        questState = quest.QuestState;
        questID = quest.QuestData.questID;
        currentTaskIndex = quest.CurrentTaskIndex;
        currentAmounts = quest.QuestTasks[currentTaskIndex].CurrentAmounts;

        return this;
    }
    public QuestSaveData InitializeByQuestTable(QuestData questData)
    {
        questID = questData.questID;
        questState = QUEST_STATE.NONE;
        currentTaskIndex = 0;
        for (int i = 0; i < questData.taskIDs.Length; ++i)
        {
            currentAmounts[i] = 0;
        }
        return this;
    }
}
