using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillTask : QuestTask
{
    public KillTask(TaskData taskData) : base(taskData)
    {
    }

    public override void StartTask()
    {
        base.StartTask();
        Managers.GameManager.OnPlayerKillEnemy -= Action;
        Managers.GameManager.OnPlayerKillEnemy += Action;
    }

    public override void EndTask()
    {
        Managers.GameManager.OnPlayerKillEnemy -= Action;
        base.EndTask();
    }

    public void Action(BaseEnemy enemy)
    {
        for(int i=0; i<targetIDs.Length; i++)
        {
            if (enemy.Status.EnemyID == targetIDs[i])
            {
                ++currentAmounts[i];
                Managers.UIManager.UISystemPanelCanvas.SystemMessagePanel.OpenPanel(enemy.Status.EnemyName + " 처치: " + CurrentAmounts + "/" + TargetAmounts);
            }
        }
    }
}
