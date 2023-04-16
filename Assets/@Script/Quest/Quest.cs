using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum QUEST_STATE
{
    INACTIVE = 0,   // 진행 불가능
    ACTIVE = 1,     // 진행 가능
    ACCEPT = 2,     // 진행 중
    COMPLETE = 3    // 완료
}

public enum QUEST_CATEGORY
{
    NONE = 0,
    MAIN = 1,
    SUB = 2
}

[System.Serializable]
public class Quest
{
    public event UnityAction<Quest> OnInactiveQuest;
    public event UnityAction<Quest> OnActiveQuest;
    public event UnityAction<Quest> OnAcceptQuest;
    public event UnityAction<Quest> OnCompleteQuest;

    [Header("Quest Infomations")]
    [SerializeField] private QUEST_CATEGORY questCategory;
    [SerializeField] private QUEST_STATE questState;
    [SerializeField] private uint questID;
    [SerializeField] private string questTitle;

    [Header("Conditions")]
    [SerializeField] private int levelCondition;
    [SerializeField] private uint mainQuestCondition;

    [Header("Tasks")]
    [SerializeField] private int taskIndex;
    [SerializeReference, SubclassSelector]
    private QuestTask[] questTasks;

    [Header("Rewards")]
    [SerializeField] private float rewardExperience;
    [SerializeField] private int rewardMoney;
    [SerializeField] private int[] rewardItemIDs;

    public void Initialize()
    {
        for(int i=0; i<questTasks.Length; ++i)
        {
            questTasks[i].Initialize(this);

            questTasks[i].OnStartTask -= Managers.QuestManager.AddDialogue;
            questTasks[i].OnStartTask += Managers.QuestManager.AddDialogue;

            questTasks[i].OnEndTask -= Managers.QuestManager.RemoveDialogue;
            questTasks[i].OnEndTask += Managers.QuestManager.RemoveDialogue;

            questTasks[i].OnEndTask -= Managers.QuestManager.RefreshAllNPCQuestList;
            questTasks[i].OnEndTask += Managers.QuestManager.RefreshAllNPCQuestList;
        }
    }

    public bool CanActive(CharacterData characterData)
    {
        if(characterData.StatusData.Level >= LevelCondition
            && characterData.QuestData.MainQuestProgress >= MainQuestCondition)
        {
            return true;
        }
        return false;
    }
    public void InactiveQuest()
    {
        questState = QUEST_STATE.INACTIVE;
        OnInactiveQuest(this);
    }
    
    public void ActiveQuest()
    {
        questState = QUEST_STATE.ACTIVE;
        questTasks[taskIndex].StartTask();
        OnActiveQuest(this);
    }

    public void AcceptQuest()
    {
        Managers.AudioManager.PlaySFX("Audio_Quest_Accept");
        questState = QUEST_STATE.ACCEPT;
        for (int i = 0; i < QuestTasks.Length; ++i)
        {
            QuestTasks[i].Initialize(this);
        }
        questTasks[taskIndex].StartTask();
        OnAcceptQuest(this);
    }
    
    public void CompleteQuest()
    {
        Managers.AudioManager.PlaySFX("Audio_Quest_Complete");
        questState = QUEST_STATE.COMPLETE;
        Reward();
        OnCompleteQuest(this);
    }

    public void Reward()
    {
        Managers.DataManager.CurrentCharacterData.GetQuestReward(this);
    }

    public QuestSaveData SaveQuest()
    {
        QuestSaveData saveData = new QuestSaveData(this);

        return saveData;
    }

    public void LoadQuest(QuestSaveData savedQuest)
    {
        if (questID == savedQuest.questID)
        {
            questState = savedQuest.questState;

            for (int i = 0; i < QuestTasks.Length; ++i)
            {
                QuestTasks[i].Initialize(this);
            }

            switch (questState)
            {
                case QUEST_STATE.ACTIVE:
                    {
                        taskIndex = savedQuest.taskIndex;
                        questTasks[savedQuest.taskIndex].StartTask();
                        questTasks[savedQuest.taskIndex].SuccessAmount = savedQuest.taskSuccessAmount;

                        OnActiveQuest(this);
                        break;
                    }
                case QUEST_STATE.ACCEPT:
                    {
                        taskIndex = savedQuest.taskIndex;
                        questTasks[savedQuest.taskIndex].StartTask();
                        questTasks[savedQuest.taskIndex].SuccessAmount = savedQuest.taskSuccessAmount;

                        OnActiveQuest(this);
                        OnAcceptQuest(this);
                        break;
                    }
                case QUEST_STATE.COMPLETE:
                    {
                        OnActiveQuest(this);
                        OnAcceptQuest(this);
                        OnCompleteQuest(this);
                        break;
                    }
            }
        }
    }

    #region Property
    public QUEST_CATEGORY QuestCategory { get { return questCategory; } }
    public QUEST_STATE QuestState { get { return questState; } }
    public uint QuestID { get { return questID; } }
    public string QuestTitle { get { return questTitle; } }
    public int LevelCondition { get { return levelCondition; } }
    public uint MainQuestCondition { get { return mainQuestCondition; } }
    public int TaskIndex
    {
        get { return taskIndex; }
        set
        {
            taskIndex = value;

            Managers.UIManager.NoticeQuestState(this);

            if (taskIndex == QuestTasks.Length && questState == QUEST_STATE.ACCEPT)
            {
                CompleteQuest();
            }

            if (taskIndex == 1)
            {
                AcceptQuest();
            }

            if(taskIndex < QuestTasks.Length)
            {
                QuestTasks[taskIndex].StartTask();
            }
        }
    }
    public QuestTask[] QuestTasks { get { return questTasks; } }
    public float RewardExperience { get { return rewardExperience; } }
    public int RewardMoney { get { return rewardMoney; } }
    public int[] RewardItemIDs { get { return rewardItemIDs; } }
    #endregion
}


