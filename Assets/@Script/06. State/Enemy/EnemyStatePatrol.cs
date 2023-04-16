using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatePatrol : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private int animationNameHash;
    private Vector3 destination;

    public EnemyStatePatrol(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_PATROL;
        animationNameHash = Constants.ANIMATION_NAME_HASH_WALK;
    }

    public void Enter()
    {
        if (RandomPoint(enemy.SpawnPosition, out destination))
        {
            Debug.DrawRay(destination, Vector3.up, Color.blue, 3.0f);
            enemy.MoveTo(destination);
            enemy.Animator.CrossFade(animationNameHash, 0.2f);
        }
    }

    public void Update()
    {
        switch (enemy.MoveController.GetGroundState())
        {
            case ACTOR_GROUND_STATE.GROUND:

                // Patrol -> Chase
                if (enemy.IsTargetInDetectionDistance() && enemy.IsTargetInSight())
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WAIT, STATE_SWITCH_BY.WEIGHT);
                    return;
                }

                // Patrol -> Idle
                if (enemy.MoveDistance < enemy.Status.StopDistance)
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
                    return;
                }

                enemy.MoveTo(destination);
                return;

            case ACTOR_GROUND_STATE.SLOPE: // -> Slide
                enemy.State.SetState(ACTION_STATE.ENEMY_SLIDE, STATE_SWITCH_BY.WEIGHT);
                return;

            case ACTOR_GROUND_STATE.AIR: // -> Fall
                enemy.State.SetState(ACTION_STATE.ENEMY_FALL, STATE_SWITCH_BY.WEIGHT);
                return;
        }
    }

    public void Exit()
    {
    }

    private bool RandomPoint(Vector3 sourcePosition, out Vector3 result)
    {
        for (int i = 0; i < 10; ++i)
        {
            Vector3 randomPoint = sourcePosition + (Random.insideUnitSphere * Constants.ENEMY_PATROL_RANGE);
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = sourcePosition;
        return false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
