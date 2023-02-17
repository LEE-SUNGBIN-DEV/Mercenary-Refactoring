using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSkill : IEnemyState
{
    private int stateWeight;
    private bool isDone;
    private float rotationTime;
    private float timer;

    public EnemyStateSkill()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Skill;
        isDone = false;
        rotationTime = 0.5f;
        timer = 0f;
    }

    public void Enter(BaseEnemy enemy)
    {
        isDone = false;
        timer = 0f;
        enemy.CurrentSkill.OnSKillEnd += IsDone;
        enemy.CurrentSkill.ActiveSkill();
    }

    public void Update(BaseEnemy enemy)
    {
        if(timer < rotationTime)
        {
            timer += Time.deltaTime;
            enemy.LookTarget();
        }

        if(isDone)
        {
            enemy.State.SetState(ENEMY_STATE.Chase);
            return;
        }
    }

    public void Exit(BaseEnemy enemy)
    {
        enemy.CurrentSkill.OnSKillEnd -= IsDone;
    }

    public void IsDone(bool isSkillDone)
    {
        isDone = isSkillDone;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
