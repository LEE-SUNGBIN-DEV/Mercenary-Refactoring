using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterQuestData
{
    public event UnityAction<CharacterQuestData> OnChangeQuestData;

    [Header("Quest")]
    [SerializeField] private uint mainQuestPrograss;
    [SerializeField] private List<QuestData> questSaveList;
    
    public CharacterQuestData()
    {
        CreateQuestData();
    }

    public void CreateQuestData()
    {
        mainQuestPrograss = Constants.CHARACTER_DATA_MAIN_QUEST_PROGRESS;
        questSaveList = new List<QuestData>();
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
    public List<QuestData> QuestSaveList
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
