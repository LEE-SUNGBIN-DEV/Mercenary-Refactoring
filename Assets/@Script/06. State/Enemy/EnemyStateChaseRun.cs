using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChaseRun : IActionState<BaseEnemy>
{
    private int stateWeight;
    private int animationNameHash;
    private float runDistance;

    public EnemyStateChaseRun()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_RUN;
        animationNameHash = Constants.ANIMATION_NAME_HASH_RUN;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.speed = enemy.Status.MoveSpeed * 1.5f;
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
                // Run (Current)
                if (enemy.TargetDistance > runDistance)
                {
                    enemy.NavMeshAgent.speed = enemy.Status.MoveSpeed * 1.5f;
                    enemy.NavMeshAgent.isStopped = false;
                    enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);
                    return;
                }

                // -> Walk
                if (enemy.Animator.HasState(0, Constants.ANIMATION_NAME_HASH_WALK))
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_WALK, STATE_SWITCH_BY.FORCED);
                    return;
                }
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
