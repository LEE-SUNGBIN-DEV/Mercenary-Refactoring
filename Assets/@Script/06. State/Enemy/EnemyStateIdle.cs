using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : IEnemyState
{
    private int stateWeight;
    private float idleTime;
    private float patrolInterval;
    private int idleAnimationNameHash;

    public EnemyStateIdle()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Idle;
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
        // ���� ������ ���¶��
        // Idle -> Chase
        if (enemy.IsTargetInChaseDistance() && enemy.IsTargetInSight())
        {
            enemy.State.TryStateSwitchingByWeight(ENEMY_STATE.Chase);
            return;
        }

        // ���� �ð����� ��Ʈ�� ���·� ��ȯ
        // Idle -> Patrol
        idleTime += Time.deltaTime;
        if(idleTime >= patrolInterval)
        {
            enemy.State.TryStateSwitchingByWeight(ENEMY_STATE.Patrol);
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
