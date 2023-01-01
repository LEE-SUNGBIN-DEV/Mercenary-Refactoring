using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCompete : IEnemyState
{
    private int stateWeight;

    public EnemyStateCompete()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Compete;
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
