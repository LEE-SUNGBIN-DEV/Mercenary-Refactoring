using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatePatrol : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private int walkAnimationNameHash;
    private Vector3 destination;

    public EnemyStatePatrol(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_PATROL;
        walkAnimationNameHash = Constants.ANIMATION_NAME_HASH_WALK;
    }

    public void Enter()
    {
        Vector3 patrolPoint = enemy.SpawnPosition + Random.onUnitSphere * Constants.ENEMY_PATROL_RANGE;
        
        if(NavMesh.SamplePosition(patrolPoint, out NavMeshHit navMeshHit, 1.0f, NavMesh.AllAreas))
            destination = navMeshHit.position;
        else
            destination = Vector3.zero;

        enemy.NavMeshAgent.isStopped = false;
        enemy.NavMeshAgent.SetDestination(destination);
        enemy.Animator.CrossFade(walkAnimationNameHash, 0.2f);
    }

    public void Update()
    {
        enemy.NavMeshAgent.speed = enemy.Status.MoveSpeed;

        // Patrol -> Chase
        if (enemy.IsTargetInDetectionDistance() && enemy.IsTargetInSight())
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WAIT, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        // Patrol -> Idle
        if (enemy.NavMeshAgent.remainingDistance <= enemy.NavMeshAgent.stoppingDistance)
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
        }
    }

    public void Exit()
    {
    }


    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
