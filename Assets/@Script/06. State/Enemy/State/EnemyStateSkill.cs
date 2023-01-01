using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSkill : IEnemyState
{
    private int stateWeight;

    public EnemyStateSkill()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Skill;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.SelectSkill.ActiveSkill();
    }

    public void Update(BaseEnemy enemy)
    {
    }

    public void Exit(BaseEnemy enemy)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
