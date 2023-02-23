using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDie : IActionState<BaseEnemy>
{
    private int stateWeight;

    public EnemyStateDie()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_DIE;
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
