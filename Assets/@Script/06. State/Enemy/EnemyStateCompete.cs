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
        enemy.HitState = HIT_STATE.Invincible;
        enemy.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.SpecialCombatManager.CompetePower);
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_COMPETE);
    }

    public void Update()
    {
        enemy.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_COMPETE, Managers.SpecialCombatManager.CompetePower);
    }

    public void Exit()
    {
        enemy.HitState = HIT_STATE.Hittable;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
