using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDie : IEnemyState
{
    private int stateWeight;

    public EnemyStateDie()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Die;
    }

    public void Enter(BaseEnemy enemy)
    {
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
