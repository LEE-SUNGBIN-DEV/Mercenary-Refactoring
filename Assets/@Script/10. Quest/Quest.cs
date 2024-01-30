using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum QUEST_STATE
{
    NONE = 0,
    DISABLE = 1,    // 수락 불가능
    ENABLE = 2,     // 수락 가능
    PROGRESS = 3,   // 진행
    COMPLETE = 4   // 완료
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
    public event UnityAction<Quest> OnEnableQuest;
    public event UnityAction<Quest> OnAcceptQuest;
    public event UnityAction<Quest> OnProgressQuest;
    public event UnityAction<Quest> OnCompleteQuest;

    [Header("Quest Datas")]
    [SerializeField] private QuestData questData;
    [SerializeField] private QUEST_STATE questState;

    [Header("Tasks")]
    [SerializeField] private int currentTaskIndex;
    [SerializeField] private QuestTask[] questTasks;

    public void LoadFromSaveData(QuestSaveData saveData)
    {
        questData = Managers.DataManager.QuestTable[saveData.questID];
        questState = saveData.questState;
        currentTaskIndex = saveData.currentTaskIndex;
        questTasks = new QuestTask[questData.taskIDs.Length];
        for (int i = 0; i < questData.taskIDs.Length; ++i)
        {
            if (Managers.DataManager.TaskTable.TryGetValue(questData.taskIDs[i], out TaskData taskData))
            {
                switch (taskData.taskCategory)
                {
                    case TASK_CATEGORY.TALK:
                        questTasks[i] = new TalkTask(taskData);
                        break;

                    case TASK_CATEGORY.KILL:
                        questTasks[i] = new KillTask(taskData);
                        break;

                    case TASK_CATEGORY.ENTER_SCENE:
                        break; ;
                }
            }
        }
    }

    public void DisableQuest()
    {
        questState = QUEST_STATE.DISABLE;
    }    
    public void EnableQuest()
    {
        questState = QUEST_STATE.ENABLE;
        OnEnableQuest?.Invoke(this);
    }
    public void AcceptQuest()
    {
        questState = QUEST_STATE.PROGRESS;
        currentTaskIndex = 0;
        questTasks[currentTaskIndex].Initialize(this);
        questTasks[currentTaskIndex].StartTask();
        OnAcceptQuest?.Invoke(this);
    }
    public void ProgressQuest()
    {
        questState = QUEST_STATE.PROGRESS;
        ++currentTaskIndex;

        if (questTasks.Length == currentTaskIndex)
        {
            CompleteQuest();
        }
        else
        {
            questTasks[currentTaskIndex].Initialize(this);
            questTasks[currentTaskIndex].StartTask();
            OnProgressQuest?.Invoke(this);
        }
    }
    public void CompleteQuest()
    {
        questState = QUEST_STATE.COMPLETE;
        OnCompleteQuest?.Invoke(this);
    }


    #region Property
    public QuestData QuestData { get { return questData; } }
    public QUEST_STATE QuestState { get { return questState; } }
    public int CurrentTaskIndex { get { return currentTaskIndex; } }
    public QuestTask[] QuestTasks { get { return questTasks; } }
    #endregion
}


