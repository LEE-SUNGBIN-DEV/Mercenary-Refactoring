using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChaseWalk : IActionState<BaseEnemy>
{
    private int stateWeight;
    private int animationNameHash;
    private float runDistance;

    public EnemyStateChaseWalk()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_WALK;
        animationNameHash = Constants.ANIMATION_NAME_HASH_WALK;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.speed = enemy.Status.MoveSpeed;
        enemy.NavMeshAgent.isStopped = false;
        enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);
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

                // Walk (Current)
                enemy.NavMeshAgent.speed = enemy.Status.MoveSpeed;
                enemy.NavMeshAgent.isStopped = false;
                enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);
                return;
            }
            // -> Chase Stop
            else
            {
                enemy.State.SetState(ACTION_STATE.ENEMY_CHASE, STATE_SWITCH_BY.FORCED);
                return;
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

    public void Stop(BaseEnemy enemy)
    {

    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
