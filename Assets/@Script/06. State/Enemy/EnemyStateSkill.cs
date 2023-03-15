using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSkill : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private bool isDone;
    private float rotationTime;
    private float timer;

    public EnemyStateSkill(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_SKILL;
        isDone = false;
        rotationTime = 0.5f;
        timer = 0f;
    }

    public void Enter()
    {
        isDone = false;
        timer = 0f;
        enemy.CurrentSkill.OnEndSkill -= IsDone;
        enemy.CurrentSkill.OnEndSkill += IsDone;
        enemy.CurrentSkill.ActiveSkill();
    }

    public void Update()
    {
        if(timer < rotationTime)
        {
            timer += Time.deltaTime;
            enemy.LookTarget();
        }

        if(isDone)
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WAIT, STATE_SWITCH_BY.FORCED);
            return;
        }
    }

    public void Exit()
    {
        enemy.CurrentSkill.OnEndSkill -= IsDone;
        enemy.CurrentSkill.StopSkillCoroutine();
    }

    public void IsDone(bool isSkillDone)
    {
        isDone = isSkillDone;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
