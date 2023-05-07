using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatePatrol : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;
    private Vector3 destination;

    public EnemyStatePatrol(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_PATROL;
        animationClipInformation = enemy.AnimationClipTable[Constants.ANIMATION_NAME_WALK];
    }

    public void Enter()
    {
        if (RandomPatrolPoint(enemy.SpawnPosition, out destination))
        {
            Debug.DrawRay(destination, Vector3.up, Color.blue, 5.0f);
            enemy.Animator.CrossFade(animationClipInformation.nameHash, 0.2f);
        }
        enemy.MoveTo(destination);
    }

    public void Update()
    {
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

        enemy.MoveTo(destination);
    }

    public void Exit()
    {
    }

    private bool RandomPatrolPoint(Vector3 spawnPosition, out Vector3 resultPosition)
    {
        for (int i = 0; i < 10; ++i)
        {
            Vector3 randomPoint = spawnPosition + (Random.insideUnitSphere * Constants.ENEMY_PATROL_RANGE);
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 0.5f, NavMesh.AllAreas))
            {
                resultPosition = hit.position;
                return true;
            }
        }
        resultPosition = spawnPosition;
        return false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
