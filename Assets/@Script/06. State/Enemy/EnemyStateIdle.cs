using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private float idleTime;
    private float patrolInterval;
    private int idleAnimationNameHash;

    public EnemyStateIdle(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_IDLE;
        idleAnimationNameHash = Constants.ANIMATION_NAME_HASH_IDLE;
    }

    public void Enter()
    {
        idleTime = 0f;
        patrolInterval = Random.Range(Constants.TIME_ENEMY_MIN_PATROL, Constants.TIME_ENEMY_MAX_PATROL);
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.velocity = Vector3.zero;
        enemy.Animator.CrossFade(idleAnimationNameHash, 0.2f);
    }

    public void Update()
    {
        // -> Chase
        if (enemy.IsTargetInDetectionDistance() && enemy.IsTargetInSight())
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WAIT, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        // 일정 시간마다 패트롤 상태로 전환
        // -> Patrol
        idleTime += Time.deltaTime;
        if(idleTime >= patrolInterval)
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_PATROL, STATE_SWITCH_BY.WEIGHT);
            return;
        }
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
