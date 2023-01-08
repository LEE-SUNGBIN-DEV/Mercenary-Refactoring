using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSkill : IEnemyState
{
    private int stateWeight;
    private float lookTime;
    private float cumulativeTime;

    public EnemyStateSkill()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Skill;
        lookTime = 0.5f;
        cumulativeTime = 0f;
    }

    public void Enter(BaseEnemy enemy)
    {
        cumulativeTime = 0f;
        enemy.SelectSkill.ActiveSkill();
    }

    public void Update(BaseEnemy enemy)
    {
        if(cumulativeTime < lookTime)
        {
            cumulativeTime += Time.deltaTime;
            enemy.LookTarget();
        }
    }

    public void Exit(BaseEnemy enemy)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
