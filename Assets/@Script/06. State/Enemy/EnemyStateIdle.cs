using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private float idleTime;
    private float patrolInterval;
    private int animationNameHash;

    public EnemyStateIdle(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_IDLE;
        animationNameHash = Constants.ANIMATION_NAME_HASH_IDLE;
    }

    public void Enter()
    {
        idleTime = 0f;
        patrolInterval = Random.Range(Constants.TIME_ENEMY_MIN_PATROL, Constants.TIME_ENEMY_MAX_PATROL);
        enemy.Animator.CrossFade(animationNameHash, 0.2f);
    }

    public void Update()
    {
        switch (enemy.MoveController.GetGroundState())
        {
            case ACTOR_GROUND_STATE.GROUND:
                // -> Chase Wait
                if (enemy.IsTargetInDetectionDistance() && enemy.IsTargetInSight())
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
                return;

            case ACTOR_GROUND_STATE.SLOPE: // -> Slide
                enemy.State.SetState(ACTION_STATE.ENEMY_SLIDE, STATE_SWITCH_BY.WEIGHT);
                return;

            case ACTOR_GROUND_STATE.AIR: // -> Fall
                enemy.State.SetState(ACTION_STATE.ENEMY_FALL, STATE_SWITCH_BY.WEIGHT);
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
