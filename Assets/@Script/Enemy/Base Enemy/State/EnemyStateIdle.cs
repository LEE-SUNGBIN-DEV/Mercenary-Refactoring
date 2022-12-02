using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : IEnemyState
{
    private int stateWeight;

    public EnemyStateIdle()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.ATTACK;
    }

    public void Enter(Enemy enemy)
    {
        enemy.Animator.SetBool("isMove", false);
    }

    public void Update(Enemy enemy)
    {
    }

    public void Exit(Enemy enemy)
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
