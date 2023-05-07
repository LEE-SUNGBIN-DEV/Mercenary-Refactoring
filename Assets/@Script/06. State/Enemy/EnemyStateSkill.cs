using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSkill : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private bool isDone;

    public EnemyStateSkill(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_SKILL;
        isDone = false;
    }

    public void Enter()
    {
        isDone = false;
        enemy.CurrentSkill.OnEndSkill += IsDone;
        enemy.CurrentSkill.EnableSkill();
    }

    public void Update()
    {
        if(isDone)
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WAIT, STATE_SWITCH_BY.FORCED);
            return;
        }
    }

    public void Exit()
    {
        enemy.CurrentSkill.OnEndSkill -= IsDone;
        enemy.CurrentSkill.DisableSkill();
    }

    public void IsDone(bool isSkillDone)
    {
        isDone = isSkillDone;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
