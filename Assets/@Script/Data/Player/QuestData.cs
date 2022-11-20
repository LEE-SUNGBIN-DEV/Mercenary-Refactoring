using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class QuestData
{
    public event UnityAction<QuestData> OnChangeQuestData;

    [Header("Quest")]
    [SerializeField] private uint mainQuestPrograss;
    [SerializeField] private List<QuestSaveData> questSaveList;
    
    public QuestData()
    {
        CreateQuestData();
    }

    public void CreateQuestData()
    {
        mainQuestPrograss = Constants.CHARACTER_DATA_MAIN_QUEST_PROGRESS;
        questSaveList = new List<QuestSaveData>();
    }

    public void UpdateMainQuestProcedure(Quest quest)
    {
        if (quest.questCategory == QUEST_CATEGORY.MAIN)
        {
            MainQuestPrograss = quest.QuestID;
        }
    }
    #region Property
    public uint MainQuestPrograss
    {
        get { return mainQuestPrograss; }
        set
        {
            mainQuestPrograss = value;
            OnChangeQuestData?.Invoke(this);
        }
    }
    public List<QuestSaveData> QuestSaveList
    {
        get { return questSaveList; }
        set
        {
            questSaveList = value;
            OnChangeQuestData?.Invoke(this);
        }
    }
    #endregion
}
