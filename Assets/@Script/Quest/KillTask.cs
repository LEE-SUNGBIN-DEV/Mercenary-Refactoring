using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillTask : QuestTask
{
    [SerializeField] private uint targetID;

    public override void StartTask()
    {
        base.StartTask();
        Managers.EventManager.OnKillEnemy -= Action;
        Managers.EventManager.OnKillEnemy += Action;
    }

    public override void EndTask()
    {
        Managers.EventManager.OnKillEnemy -= Action;
        ++OwnerQuest.TaskIndex;
        base.EndTask();
    }

    public void Action(Character character, Enemy enemy)
    {
        if(enemy.EnemyData.EnemyID == targetID)
        {
            ++SuccessAmount;
            Managers.UIManager.RequestNotice(enemy.EnemyData.EnemyName + " 처치: " + SuccessAmount + "/" + RequireAmount);
        }
    }
    #region Property
    public uint TargetID
    {
        get { return targetID; }
    }
    #endregion
}
