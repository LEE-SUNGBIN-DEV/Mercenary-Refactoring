using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillTask : QuestTask
{
    [SerializeField] private string targetName;

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
        if(enemy.EnemyName == targetName)
        {
            ++SuccessAmount;
            Managers.UIManager.RequestNotice(targetName + " 처치: " + SuccessAmount + "/" + RequireAmount);
        }
    }
    #region Property
    public string TargetName
    {
        get { return targetName; }
    }
    #endregion
}
