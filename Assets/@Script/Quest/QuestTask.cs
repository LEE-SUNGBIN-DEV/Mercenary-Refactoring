using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

[System.Serializable]
public abstract class QuestTask
{
    public event UnityAction<QuestTask> OnStartTask;
    public event UnityAction<QuestTask> OnEndTask;

    [JsonIgnore][SerializeField] private Quest ownerQuest;
    [TextArea]
    [SerializeField] private string taskTooltip;

    [SerializeField] private int requireAmount;
    [SerializeField] private int successAmount;

    public virtual void Initialize(Quest quest)
    {
        ownerQuest = quest;
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
    public Quest OwnerQuest { get { return ownerQuest; } }
    public string TaskTooltip { get { return taskTooltip; } }
    public int RequireAmount { get { return requireAmount; } }

    public int SuccessAmount
    {
        get { return successAmount; }
        set
        {
            successAmount = value;

            if (successAmount >= RequireAmount)
            {
                successAmount = RequireAmount;
                EndTask();
            }
        }
    }
    #endregion
}
