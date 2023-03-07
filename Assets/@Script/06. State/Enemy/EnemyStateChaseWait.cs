using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChaseWait : IActionState<BaseEnemy>
{
    private int stateWeight;
    private int animationNameHash;
    private float runDistance;

    public EnemyStateChaseWait()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_CHASE;
        animationNameHash = Constants.ANIMATION_NAME_HASH_IDLE;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.stoppingDistance = enemy.Status.StopDistance;
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.velocity = Vector3.zero;
        enemy.Animator.CrossFade(animationNameHash, 0.1f);

        runDistance = enemy.Status.ChaseDistance * 0.5f;
    }

    public void Update(BaseEnemy enemy)
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
                if (enemy.Animator.HasState(0, Constants.ANIMATION_NAME_HASH_RUN)
                    && enemy.TargetDistance > runDistance)
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_RUN, STATE_SWITCH_BY.WEIGHT);
                    return;
                }

                // -> Walk
                if (enemy.Animator.HasState(0, Constants.ANIMATION_NAME_HASH_WALK))
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_WALK, STATE_SWITCH_BY.WEIGHT);
                    return;
                }
            }
            // Stop (Current)
            else
            {
                enemy.NavMeshAgent.isStopped = true;
                enemy.NavMeshAgent.velocity = Vector3.zero;
            }
        }
        // -> Idle
        else
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
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
