using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChaseWait : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;

    private float runDistance;

    public EnemyStateChaseWait(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_CHASE_WAIT;
        animationClipInfo = enemy.AnimationClipTable[Constants.ANIMATION_NAME_CHASE_WAIT];

        runDistance = enemy.Status.ChaseDistance * Constants.ENEMY_RUN_DISTANCE;
    }

    public void Enter()
    {
        enemy.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.1f);
    }

    public void Update()
    {
        if (enemy.IsChaseCondition())
        {
            // -> Skill
            if (enemy.IsReadyAnySkill())
            {
                enemy.State.SetState(ACTION_STATE.ENEMY_SKILL, STATE_SWITCH_BY.WEIGHT);
                return;
            }

            // -> Turn
            if (enemy.State.HasState(ACTION_STATE.ENEMY_CHASE_TURN) && !enemy.IsTargetInAngle(Constants.ENEMY_DETECTION_ANGLE * 0.5f))
            {
                enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_TURN, STATE_SWITCH_BY.WEIGHT);
                return;
            }

            if (enemy.TargetDistance > enemy.Status.StopDistance)
            {
                // -> Run
                if (enemy.State.HasState(ACTION_STATE.ENEMY_CHASE_RUN) && enemy.TargetDistance > runDistance)
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_RUN, STATE_SWITCH_BY.WEIGHT);
                    return;
                }

                // -> Walk
                if (enemy.State.HasState(ACTION_STATE.ENEMY_CHASE_WALK))
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WALK, STATE_SWITCH_BY.WEIGHT);
                    return;
                }
            }

            // Stop (Current)
            else
            {
                enemy.MoveController.SetMove(enemy.TargetDirection, 0f);
            }
        }
        // -> Idle
        else
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
            return;
        }
    }

    public void Exit()
    {
        enemy.MoveController.SetMove(Vector3.zero, 0f);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
