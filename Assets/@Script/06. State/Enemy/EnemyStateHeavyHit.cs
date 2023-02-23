using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHeavyHit : IActionState<BaseEnemy>
{
    private int stateWeight;

    public EnemyStateHeavyHit()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_HIT_HEAVY;
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
