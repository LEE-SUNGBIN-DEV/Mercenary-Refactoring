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
        // Set Compete State
        enemy.IsInvincible = true;
        enemy.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.CompeteManager.CompetePower);
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE);
    }

    public void Update(BaseEnemy enemy)
    {
        enemy.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.CompeteManager.CompetePower);
    }

    public void Exit(BaseEnemy enemy)
    {
        enemy.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
