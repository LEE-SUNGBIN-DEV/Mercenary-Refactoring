using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : IActionState<BaseEnemy>
{
    private int stateWeight;
    private float idleTime;
    private float patrolInterval;
    private int idleAnimationNameHash;

    public EnemyStateIdle()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_IDLE;
        idleAnimationNameHash = Constants.ANIMATION_NAME_HASH_IDLE;
    }

    public void Enter(BaseEnemy enemy)
    {
        idleTime = 0f;
        patrolInterval = Random.Range(Constants.TIME_ENEMY_MIN_PATROL, Constants.TIME_ENEMY_MAX_PATROL);
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.velocity = Vector3.zero;
        enemy.Animator.CrossFade(idleAnimationNameHash, 0.2f);
    }

    public void Update(BaseEnemy enemy)
    {
        // 추적 가능한 상태라면
        // Idle -> Chase
        if (enemy.IsTargetInChaseDistance() && enemy.IsTargetInSight())
        {
            enemy.State.TryStateSwitchingByWeight(ACTION_STATE.ENEMY_CHASE);
            return;
        }

        // 일정 시간마다 패트롤 상태로 전환
        // Idle -> Patrol
        idleTime += Time.deltaTime;
        if(idleTime >= patrolInterval)
        {
            enemy.State.TryStateSwitchingByWeight(ACTION_STATE.ENEMY_PATROL);
            return;
        }
    }

    public void Exit(BaseEnemy enemy)
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
