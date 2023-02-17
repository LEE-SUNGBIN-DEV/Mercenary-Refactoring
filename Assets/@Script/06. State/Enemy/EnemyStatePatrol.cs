using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatePatrol : IEnemyState
{
    private int stateWeight;
    private int walkAnimationNameHash;
    private Vector3 destination;

    public EnemyStatePatrol()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Patrol;
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
        enemy.NavMeshAgent.speed = enemy.EnemyData.MoveSpeed;

        // Patrol -> Chase
        if (enemy.IsTargetInChaseDistance() && enemy.IsTargetInSight())
        {
            enemy.State.TryStateSwitchingByWeight(ENEMY_STATE.Chase);
            return;
        }

        // Patrol -> Idle
        if (enemy.NavMeshAgent.remainingDistance <= enemy.NavMeshAgent.stoppingDistance)
        {
            enemy.State.SetState(ENEMY_STATE.Idle);
        }
    }

    public void Exit(BaseEnemy enemy)
    {
    }


    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
