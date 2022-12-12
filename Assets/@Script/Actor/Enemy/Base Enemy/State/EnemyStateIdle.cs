using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : IEnemyState
{
    private int stateWeight;

    public EnemyStateIdle()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Attack;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.Animator.SetBool("isMove", false);
    }

    public void Update(BaseEnemy enemy)
    {
    }

    public void Exit(BaseEnemy enemy)
    {
        enemy.Animator.SetBool("isMove", false);
    }

    #region Property
    public int StateWeight
    {
        get => stateWeight;
    }
    #endregion
}
