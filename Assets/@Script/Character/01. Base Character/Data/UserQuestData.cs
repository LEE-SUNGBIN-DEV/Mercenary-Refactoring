using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UserQuestData
{
    public event UnityAction<UserQuestData> OnChangeQuestData;

    [Header("Quest")]
    [SerializeField] private uint mainQuestPrograss;
    [SerializeField] private List<QuestSaveData> questSaveList;
    
    public void Initialize()
    {
        mainQuestPrograss = Constants.CHARACTER_DATA_MAIN_QUEST_PROGRESS;
        questSaveList = new List<QuestSaveData>();
    }

    public void UpdateMainQuestPrograss(Quest quest)
    {
        if (quest.QuestCategory == QUEST_CATEGORY.MAIN)
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
