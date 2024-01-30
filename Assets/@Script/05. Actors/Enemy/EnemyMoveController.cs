using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(BaseEnemy))]
public class EnemyMoveController : BaseMoveController
{
    private StateController stateController;

    private void Start()
    {
        if (TryGetComponent(out BaseEnemy baseEnemy))
        {
            stateController = baseEnemy.State;
        }
    }

    protected override void UpdatePosition()
    {
        base.UpdatePosition();
        switch (moveState)
        {
            case MOVE_STATE.SLIDING:
                stateController.SetState(ACTION_STATE.ENEMY_SLIDE, STATE_SWITCH_BY.WEIGHT);
                break;

            case MOVE_STATE.FALLING:
                stateController.SetState(ACTION_STATE.ENEMY_FALL, STATE_SWITCH_BY.WEIGHT);
                break;

            default:
                break;
        }
    }
}
