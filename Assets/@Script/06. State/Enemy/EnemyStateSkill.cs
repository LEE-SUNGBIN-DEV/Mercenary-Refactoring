using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSkill : IActionState<BaseEnemy>
{
    private int stateWeight;
    private bool isDone;
    private float rotationTime;
    private float timer;

    public EnemyStateSkill()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_SKILL;
        isDone = false;
        rotationTime = 0.5f;
        timer = 0f;
    }

    public void Enter(BaseEnemy enemy)
    {
        isDone = false;
        timer = 0f;
        enemy.CurrentSkill.OnEndSkill += IsDone;
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
            enemy.State.SetState(ACTION_STATE.ENEMY_CHASE);
            return;
        }
    }

    public void Exit(BaseEnemy enemy)
    {
        enemy.CurrentSkill.OnEndSkill -= IsDone;
    }

    public void IsDone(bool isSkillDone)
    {
        isDone = isSkillDone;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
