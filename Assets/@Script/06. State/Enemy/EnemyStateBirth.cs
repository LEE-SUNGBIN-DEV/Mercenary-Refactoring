using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBirth : IEnemyState
{
    private int stateWeight;

    public EnemyStateBirth()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Birth;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.Animator.SetTrigger(Constants.ANIMATOR_PARAMETERS_TRIGGER_BIRTH);
    }

    public void Update(BaseEnemy enemy)
    {
        if (enemy.Animator.GetNextAnimatorStateInfo(0).IsName(Constants.ANIMATOR_STATE_NAME_IDLE))
        {
            enemy.State.SwitchState(ENEMY_STATE.Idle);
        }
    }

    public void Exit(BaseEnemy enemy)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
