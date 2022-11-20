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
    #region Event
    public event UnityAction<Quest> OnInactiveQuest;
    public event UnityAction<Quest> OnActiveQuest;
    public event UnityAction<Quest> OnAcceptQuest;
    public event UnityAction<Quest> OnCompleteQuest;
    #endregion

    [Header("Quest Infomations")]
    public QUEST_CATEGORY questCategory;
    public QUEST_STATE questState;
    [SerializeField] private uint questID;
    [SerializeField] private string questTitle;

    [Header("Conditions")]
    [SerializeField] private int levelCondition;
    [SerializeField] private uint questCondition;

    [Header("Tasks")]
    [SerializeField] private int taskIndex;
    [SerializeReference, SubclassSelector]
    private QuestTask[] questTasks;

    [Header("Rewards")]
    [SerializeField] private float rewardExperience;
    [SerializeField] private int rewardMoney;
    [SerializeField] private string[] rewardItems;

    public void Initialize()
    {
        for(int i=0; i<questTasks.Length; ++i)
        {
            questTasks[i].Initialize(this);

            questTasks[i].OnStartTask -= Managers.QuestManager.AddDialogue;
            questTasks[i].OnStartTask += Managers.QuestManager.AddDialogue;

            questTasks[i].OnEndTask -= Managers.QuestManager.RemoveDialogue;
            questTasks[i].OnEndTask += Managers.QuestManager.RemoveDialogue;

            questTasks[i].OnEndTask -= Managers.QuestManager.RequestNPCQuestList;
            questTasks[i].OnEndTask += Managers.QuestManager.RequestNPCQuestList;
        }
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
        Managers.AudioManager.PlaySFX("Quest Accept");
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
        Managers.AudioManager.PlaySFX("Quest Complete");
        questState = QUEST_STATE.COMPLETE;
        Reward();
        OnCompleteQuest(this);
    }

    public void Reward()
    {
        Managers.DataManager.SelectCharacterData.GetQuestReward(this);
    }

    public QuestSaveData SaveQuest()
    {
        if (questState == QUEST_STATE.COMPLETE)
        {
            QuestSaveData questData = new QuestSaveData
            {
                questState = questState,
                questID = questID,
                taskIndex = 0,
                taskSuccessAmount = 0
            };
            return questData;
        }
        else
        {
            QuestSaveData questData = new QuestSaveData
            {
                questState = questState,
                questID = questID,
                taskIndex = taskIndex,
                taskSuccessAmount = questTasks[taskIndex].SuccessAmount
            };
            return questData;
        }
    }
    public void LoadQuest(QuestSaveData questData)
    {
        if (QuestID == questData.questID)
        {
            questState = questData.questState;

            for (int i = 0; i < QuestTasks.Length; ++i)
            {
                QuestTasks[i].Initialize(this);
            }

            switch (questState)
            {
                case QUEST_STATE.ACTIVE:
                    {
                        taskIndex = questData.taskIndex;
                        questTasks[questData.taskIndex].StartTask();
                        questTasks[questData.taskIndex].SuccessAmount = questData.taskSuccessAmount;

                        OnActiveQuest(this);
                        break;
                    }
                case QUEST_STATE.ACCEPT:
                    {
                        taskIndex = questData.taskIndex;
                        questTasks[questData.taskIndex].StartTask();
                        questTasks[questData.taskIndex].SuccessAmount = questData.taskSuccessAmount;

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
    public uint QuestID { get { return questID; } }
    public string QuestTitle { get { return questTitle; } }
    public int LevelCondition { get { return levelCondition; } }
    public uint QuestCondition { get { return questCondition; } }
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
    public string[] RewardItems { get { return rewardItems; } }
    #endregion
}


