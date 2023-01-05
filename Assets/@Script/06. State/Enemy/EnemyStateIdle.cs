using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : IEnemyState
{
    private int stateWeight;

    public EnemyStateIdle()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Idle;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.velocity = Vector3.zero;
        enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_MOVE, false);
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
