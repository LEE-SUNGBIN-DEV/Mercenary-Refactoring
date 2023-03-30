using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChaseWait : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private int animationNameHash;
    private float runDistance;

    public EnemyStateChaseWait(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_CHASE_WAIT;
        animationNameHash = Constants.ANIMATION_NAME_HASH_HALBERD_IDLE;
    }

    public void Enter()
    {
        enemy.Animator.CrossFade(animationNameHash, 0.1f);

        runDistance = enemy.Status.ChaseDistance * 0.5f;
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
                // -> Run
                if (enemy.Animator.HasState(0, Constants.ANIMATION_NAME_HASH_HALBERD_RUN)
                    && enemy.TargetDistance > runDistance)
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_RUN, STATE_SWITCH_BY.WEIGHT);
                    return;
                }

                // -> Walk
                if (enemy.Animator.HasState(0, Constants.ANIMATION_NAME_HASH_HALBERD_WALK))
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WALK, STATE_SWITCH_BY.WEIGHT);
                    return;
                }
            }
            // Stop (Current)
            else
            {
                enemy.LookTarget();
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
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
