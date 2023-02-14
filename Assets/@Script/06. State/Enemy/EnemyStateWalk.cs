using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateWalk : IEnemyState
{
    private int stateWeight;
    private int animationNameHash;

    public EnemyStateWalk()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Walk;
        animationNameHash = Constants.ANIMATION_NAME_WALK;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.isStopped = false;
        enemy.Animator.CrossFade(animationNameHash, 0.2f);
    }

    public void Update(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);

        if (enemy.TargetDistance <= enemy.EnemyData.ChaseRange)
        {
            enemy.NavMeshAgent.isStopped = false;
            enemy.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_MOVE, false);
            enemy.State.SetState(ENEMY_STATE.Idle);
        }
    }

    public void Exit(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.velocity = Vector3.zero;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
