using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMove : IEnemyState
{
    private int stateWeight;

    public EnemyStateMove()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Move;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.isStopped = false;
        enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_MOVE, true);
    }

    public void Update(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);

        if (enemy.TargetDistance <= enemy.EnemyData.MinChaseRange)
        {
            enemy.NavMeshAgent.isStopped = false;
            enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_MOVE, false);
            enemy.State.SwitchState(ENEMY_STATE.Idle);
        }
    }

    public void Exit(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.velocity = Vector3.zero;
        enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_MOVE, false);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
