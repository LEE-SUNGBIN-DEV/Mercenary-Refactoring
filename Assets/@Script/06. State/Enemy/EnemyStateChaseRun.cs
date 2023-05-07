using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChaseRun : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;
    private float runDistance;

    public EnemyStateChaseRun(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_CHASE_RUN;
        animationClipInformation = enemy.AnimationClipTable[Constants.ANIMATION_NAME_RUN];
    }

    public void Enter()
    {
        enemy.Animator.CrossFade(animationClipInformation.nameHash, 0.1f);
        runDistance = enemy.Status.ChaseDistance * Constants.ENEMY_RUN_DISTANCE;
    }

    public void Update()
    {
        if (enemy.IsTargetInChaseDistance())
        {
            // -> Skill
            if (enemy.IsReadyAnySkill())
            {
                enemy.State.SetState(ACTION_STATE.ENEMY_SKILL, STATE_SWITCH_BY.WEIGHT);
                return;
            }

            if (enemy.TargetDistance > enemy.Status.StopDistance)
            {
                // Run (Current)
                if (enemy.TargetDistance > runDistance)
                {
                    enemy.MoveTo(enemy.TargetTransform.position, 1.5f);
                    return;
                }

                // -> Walk
                if (enemy.AnimationClipTable.ContainsKey(Constants.ANIMATION_NAME_WALK))
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WALK, STATE_SWITCH_BY.FORCED);
                    return;
                }
            }
            // -> Wait
            else
            {
                enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WAIT, STATE_SWITCH_BY.FORCED);
                return;
            }
        }
        // -> Idle
        enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
