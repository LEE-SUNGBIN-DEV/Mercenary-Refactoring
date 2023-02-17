using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHeavyHit : IEnemyState
{
    private int stateWeight;

    public EnemyStateHeavyHit()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Heavy_Hit;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_HEAVY_HIT);
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
