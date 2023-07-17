using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInformation animationClipInfo;
    private float idleTime;
    private float patrolInterval;

    public EnemyStateIdle(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_IDLE;
        animationClipInfo = enemy.AnimationClipTable[Constants.ANIMATION_NAME_IDLE];
    }

    public void Enter()
    {
        idleTime = 0f;
        patrolInterval = Random.Range(Constants.TIME_ENEMY_MIN_PATROL, Constants.TIME_ENEMY_MAX_PATROL);
        enemy.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.2f);
    }

    public void Update()
    {
        // -> Chase Wait
        if (enemy.IsTargetDetected())
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WAIT, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        // -> Patrol
        idleTime += Time.deltaTime;
        if (idleTime >= patrolInterval)
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_PATROL, STATE_SWITCH_BY.WEIGHT);
            return;
        }
    }

    public void Exit()
    {
        enemy.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
