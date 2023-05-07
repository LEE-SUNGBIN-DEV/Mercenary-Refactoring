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
        Managers.GameEventManager.OnPlayerKillEnemy -= Action;
        Managers.GameEventManager.OnPlayerKillEnemy += Action;
    }

    public override void EndTask()
    {
        Managers.GameEventManager.OnPlayerKillEnemy -= Action;
        ++OwnerQuest.TaskIndex;
        base.EndTask();
    }

    public void Action(BaseEnemy enemy)
    {
        if(enemy.Status.EnemyID == targetID)
        {
            ++SuccessAmount;
            Managers.UIManager.CommonSceneUI.RequestNotice(enemy.Status.EnemyName + " 처치: " + SuccessAmount + "/" + RequireAmount);
        }
    }
    #region Property
    public uint TargetID
    {
        get { return targetID; }
    }
    #endregion
}
