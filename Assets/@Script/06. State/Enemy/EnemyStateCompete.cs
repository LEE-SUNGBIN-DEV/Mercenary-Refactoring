using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCompete : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;

    public EnemyStateCompete(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_COMPETE;
    }

    public void Enter()
    {
        // Set Compete State
        enemy.IsInvincible = true;
        enemy.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.CompeteManager.CompetePower);
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE);
    }

    public void Update()
    {
        enemy.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.CompeteManager.CompetePower);
    }

    public void Exit()
    {
        enemy.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
