using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

public enum TASK_CATEGORY
{
    TALK = 1,
    KILL = 2,
    ENTER_SCENE = 3,
}

[System.Serializable]
public class QuestTask
{
    public event UnityAction<QuestTask> OnStartTask;
    public event UnityAction<QuestTask> OnEndTask;

    [SerializeField] protected string taskID;
    [SerializeField] protected TASK_CATEGORY taskCategory;
    [SerializeField] protected string[] taskTooltips;
    [SerializeField] protected string[] targetIDs;
    [SerializeField] protected int[] targetAmounts;
    [SerializeField] protected int[] currentAmounts;

    public QuestTask(TaskData taskData)
    {
        this.taskID = taskData.taskID;
        this.taskCategory = taskData.taskCategory;
        this.taskTooltips = taskData.taskTooltips;
        this.targetIDs = taskData.targetIDs;
        this.targetAmounts = taskData.targetAmounts;

        currentAmounts = new int[taskData.targetAmounts.Length];
        for(int i=0; i<currentAmounts.Length; i++)
        {
            currentAmounts[i] = 0;
        }
    }

    public virtual void Initialize(Quest quest)
    {
    }

    public virtual void StartTask()
    {
        OnStartTask?.Invoke(this);
    }
    public virtual void EndTask()
    {
        OnEndTask?.Invoke(this);
    }

    #region Property
    public string[] TaskTooltips { get { return taskTooltips; } }
    public int[] TargetAmounts { get { return targetAmounts; } }
    public int[] CurrentAmounts { get { return currentAmounts; } }
    #endregion
}
