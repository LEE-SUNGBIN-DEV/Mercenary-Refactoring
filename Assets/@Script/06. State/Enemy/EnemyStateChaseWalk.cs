using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChaseWalk : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;
    private float runDistance;

    public EnemyStateChaseWalk(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_CHASE_WALK;
        animationClipInformation = enemy.AnimationClipTable[Constants.ANIMATION_NAME_WALK];
    }

    public void Enter()
    {
        enemy.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.2f);
        runDistance = enemy.Status.ChaseDistance * Constants.ENEMY_RUN_DISTANCE;
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

            // -> Wait
            if (enemy.IsTargetInDistance(enemy.Status.StopDistance))
            {
                enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WAIT, STATE_SWITCH_BY.FORCED);
                return;

            }
            else
            {
                // -> Run
                if (enemy.State.HasState(ACTION_STATE.ENEMY_CHASE_RUN) && enemy.TargetDistance > runDistance)
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_RUN, STATE_SWITCH_BY.FORCED);
                    return;
                }

                // Walk (Current)
                enemy.TryMoveTo(enemy.TargetTransform.position);
                return;
            }
        }
        // -> Idle
        enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
    }

    public void Exit()
    {
        enemy.MoveController.SetMovementAndRotation(Vector3.zero, 0f);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
