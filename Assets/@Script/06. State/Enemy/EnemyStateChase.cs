using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase : IActionState<BaseEnemy>
{
    private enum MOVEMENT
    {
        None,
        Stop,
        Walk,
        Run
    }

    private int stateWeight;
    private MOVEMENT currentMovement;
    private int idleAnimationNameHash;
    private int walkAnimationNameHash;
    private int runAnimationNameHash;
    private float walkChaseRange;
    private float runChaseRange;

    public EnemyStateChase()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_CHASE;
        currentMovement = MOVEMENT.None;
        idleAnimationNameHash = Constants.ANIMATION_NAME_HASH_IDLE;
        walkAnimationNameHash = Constants.ANIMATION_NAME_HASH_WALK;
        runAnimationNameHash = Constants.ANIMATION_NAME_HASH_RUN;
    }

    public void Enter(BaseEnemy enemy)
    {
        currentMovement = MOVEMENT.None;

        enemy.NavMeshAgent.isStopped = false;
        enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);
        enemy.Animator.CrossFade(walkAnimationNameHash, 0.2f);

        runChaseRange = enemy.Status.ChaseDistance;
        walkChaseRange = runChaseRange * 0.5f;
    }

    public void Update(BaseEnemy enemy)
    {
        enemy.NavMeshAgent.speed = enemy.Status.MoveSpeed;

        // ����� �� �ִ� ��ų�� �ִٸ�
        // Chase -> Skill
        if(enemy.IsReadyAnySkill())
        {
            enemy.State.TryStateSwitchingByWeight(ACTION_STATE.ENEMY_SKILL);

            return;
        }

        // ���� ���� �ȿ� �ִٸ� ����ؼ� ����
        if(enemy.IsTargetInChaseDistance())
        {
            enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);

            // Run Animation�� �����Ѵٸ� Run Animation�� CrossFade�� ���
            if (currentMovement != MOVEMENT.Run
                && enemy.Animator.HasState(0, Constants.ANIMATION_NAME_HASH_RUN)
                && enemy.TargetDistance <= runChaseRange
                && enemy.TargetDistance > walkChaseRange)
            {
                currentMovement = MOVEMENT.Run;
                enemy.NavMeshAgent.speed = enemy.Status.MoveSpeed * 1.5f;
                enemy.NavMeshAgent.isStopped = false;
                enemy.Animator.CrossFade(runAnimationNameHash, 0.2f);
            }

            // Arrived
            else if (currentMovement != MOVEMENT.Stop
                && enemy.NavMeshAgent.remainingDistance <= enemy.NavMeshAgent.stoppingDistance)
            {
                currentMovement = MOVEMENT.Stop;
                enemy.NavMeshAgent.isStopped = true;
                enemy.NavMeshAgent.velocity = Vector3.zero;
                enemy.State.SetState(ACTION_STATE.ENEMY_IDLE);
                enemy.Animator.CrossFade(idleAnimationNameHash, 0.2f);
            }

            // Run Animation�� �������� �ʴ´ٸ� Walk Animation�� Cross Fade�� ���
            else
            {
                if(currentMovement != MOVEMENT.Walk)
                {
                    currentMovement = MOVEMENT.Walk;
                    enemy.NavMeshAgent.isStopped = false;
                    enemy.Animator.CrossFade(walkAnimationNameHash, 0.2f);
                }
            }
            return;
        }

        // ���� ���� ���̶�� Patrol ���·� ��ȯ
        else
        {
            currentMovement = MOVEMENT.None;
            enemy.State.SetState(ACTION_STATE.ENEMY_PATROL);
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
