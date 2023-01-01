using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateStagger : IEnemyState
{
    private int stateWeight;

    public EnemyStateStagger()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Stagger;
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
