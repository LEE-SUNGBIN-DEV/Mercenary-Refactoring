using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;
    private float idleTime;
    private float patrolInterval;

    public EnemyStateIdle(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_IDLE;
        animationClipInformation = enemy.AnimationClipTable[Constants.ANIMATION_NAME_IDLE];
    }

    public void Enter()
    {
        idleTime = 0f;
        patrolInterval = Random.Range(Constants.TIME_ENEMY_MIN_PATROL, Constants.TIME_ENEMY_MAX_PATROL);
        enemy.Animator.CrossFade(animationClipInformation.nameHash, 0.2f);
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
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
