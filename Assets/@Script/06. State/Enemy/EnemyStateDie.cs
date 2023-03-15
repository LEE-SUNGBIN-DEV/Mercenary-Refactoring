using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDie : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;

    public EnemyStateDie(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_DIE;
    }

    public void Enter()
    {
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
