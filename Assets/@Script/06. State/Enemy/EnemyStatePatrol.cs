using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatePatrol : IActionState<BaseEnemy>
{
    private int stateWeight;
    private int walkAnimationNameHash;
    private Vector3 destination;

    public EnemyStatePatrol()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_PATROL;
        walkAnimationNameHash = Constants.ANIMATION_NAME_HASH_WALK;
    }

    public void Enter(BaseEnemy enemy)
    {
        Vector3 patrolPoint = enemy.SpawnPosition + Random.insideUnitSphere * Constants.ENEMY_PATROL_RANGE;
        
        if(NavMesh.SamplePosition(patrolPoint, out NavMeshHit navMeshHit, 1.0f, NavMesh.AllAreas))
            destination = navMeshHit.position;
        else
            destination = Vector3.zero;

        enemy.NavMeshAgent.isStopped = false;
        enemy.NavMeshAgent.SetDestination(destination);
        enemy.Animator.CrossFade(walkAnimationNameHash, 0.2f);
    }

    public void Update(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.speed = enemy.Status.MoveSpeed;

        // Patrol -> Chase
        if (enemy.IsTargetInChaseDistance() && enemy.IsTargetInSight())
        {
            enemy.State.TryStateSwitchingByWeight(ACTION_STATE.ENEMY_CHASE);
            return;
        }

        // Patrol -> Idle
        if (enemy.NavMeshAgent.remainingDistance <= enemy.NavMeshAgent.stoppingDistance)
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_IDLE);
        }
    }

    public void Exit(BaseEnemy enemy)
    {
    }


    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
