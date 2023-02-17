using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateLightHit : IEnemyState
{
    private int stateWeight;

    public EnemyStateLightHit()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Light_Hit;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_LIGHT_HIT);
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
