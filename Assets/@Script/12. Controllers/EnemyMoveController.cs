using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMoveController : BaseMoveController
{
    public override void SetMovementAndRotation(Vector3 moveDirection, float moveSpeed, float rotationSpeed = Constants.ENEMY_DEFAULT_ROTATION_SPEED)
    {
        base.SetMovementAndRotation(moveDirection, moveSpeed, rotationSpeed);
    }

    public override void SetGroundStateAndValue(ACTOR_GROUND_STATE groundState)
    {
        switch (groundState)
        {
            case ACTOR_GROUND_STATE.GROUNDING:

                lastMoveDirection = Vector3.ProjectOnPlane(inputMoveDirection, groundNormalVector).normalized;
                fallingSpeed = 0f;
                return;

            case ACTOR_GROUND_STATE.FLOATING:

                lastMoveDirection = Vector3.ProjectOnPlane(inputMoveDirection, groundNormalVector).normalized;
                fallingSpeed += Constants.GRAVITY_DEFAULT * Time.deltaTime;

                return;

            case ACTOR_GROUND_STATE.SLIDING:

                lastMoveDirection = Vector3.ProjectOnPlane(Vector3.down, groundNormalVector).normalized;
                fallingSpeed += Constants.GRAVITY_DEFAULT * Time.deltaTime;
                state.SetState(ACTION_STATE.ENEMY_SLIDE, STATE_SWITCH_BY.WEIGHT);

                return;

            case ACTOR_GROUND_STATE.FALLING:

                lastMoveDirection = Vector3.down;
                fallingSpeed += Constants.GRAVITY_DEFAULT * Time.deltaTime;
                state.SetState(ACTION_STATE.ENEMY_FALL, STATE_SWITCH_BY.WEIGHT);

                return;
        }
    }
}
