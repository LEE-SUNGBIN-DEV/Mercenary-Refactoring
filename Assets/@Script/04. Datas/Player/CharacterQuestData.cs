using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterQuestData
{
    public event UnityAction<CharacterQuestData> OnChangeQuestData;

    [Header("Quest")]
    [SerializeField] private uint mainQuestProgress;
    [SerializeField] private List<QuestSaveData> questSaveList;
    
    public void CreateData()
    {
        mainQuestProgress = Constants.CHARACTER_DATA_MAIN_QUEST_PROGRESS;
        questSaveList = new List<QuestSaveData>();
    }

    public void UpdateMainQuestPrograss(Quest quest)
    {
        if (quest.QuestCategory == QUEST_CATEGORY.MAIN)
        {
            MainQuestProgress = quest.QuestID;
        }
    }

    #region Property
    public uint MainQuestProgress
    {
        get { return mainQuestProgress; }
        set
        {
            mainQuestProgress = value;
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
