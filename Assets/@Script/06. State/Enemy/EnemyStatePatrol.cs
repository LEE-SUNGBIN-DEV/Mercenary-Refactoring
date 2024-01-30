using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatePatrol : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;
    private Vector3 destination;

    public EnemyStatePatrol(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_PATROL;
        animationClipInfo = enemy.AnimationClipTable[Constants.ANIMATION_NAME_WALK];
    }

    public void Enter()
    {
        GetRandomPatrolPoint(enemy.SpawnPosition, out destination);
        enemy.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.1f);
        Debug.DrawRay(destination, Vector3.up, Color.blue, 5.0f);
    }

    public void Update()
    {
        enemy.TryMoveTo(destination);

        // -> Chase
        if (enemy.IsTargetDetected())
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WAIT, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        // -> Idle
        if (enemy.MoveDistance < enemy.Status.StopDistance)
        {
            enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
            return;
        }
    }

    public void Exit()
    {
        enemy.MoveController.SetMove(Vector3.zero, 0f);
    }

    private void GetRandomPatrolPoint(Vector3 spawnPosition, out Vector3 resultPosition)
    {
        for (int i = 0; i < 5; ++i)
        {
            Vector3 randomPoint = spawnPosition + (Random.onUnitSphere * Constants.ENEMY_PATROL_RANGE);
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 0.5f, NavMesh.AllAreas))
            {
                resultPosition = hit.position;
                return;
            }
        }
        resultPosition = spawnPosition;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
