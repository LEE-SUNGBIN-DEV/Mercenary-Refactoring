using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateLightHit : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;

    public EnemyStateLightHit(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_HIT_LIGHT;
    }

    public void Enter()
    {
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_LIGHT_HIT);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
