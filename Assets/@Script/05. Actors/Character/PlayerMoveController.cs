using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(PlayerCharacter))]
public class PlayerMoveController : BaseMoveController
{
    private StateController stateController;

    private void Start()
    {
        if (TryGetComponent(out PlayerCharacter playerCharacter))
        {
            stateController = playerCharacter.State;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PushOut();
    }
    protected override void UpdatePosition()
    {
        switch (moveState)
        {
            case MOVE_STATE.SLIDING:
                stateController.SetState(ACTION_STATE.PLAYER_SLIDE, STATE_SWITCH_BY.WEIGHT);
                break;

            case MOVE_STATE.FALLING:
                stateController.SetState(ACTION_STATE.PLAYER_FALL, STATE_SWITCH_BY.WEIGHT);
                break;

            default:
                break;
        }
        base.UpdatePosition();
    }

    public void PushOut()
    {
        Collider[] colliders = Physics.OverlapCapsule(transform.position + new Vector3(0, capsuleRadius, 0), transform.position + new Vector3(0, capsuleHeight - capsuleRadius, 0), capsuleRadius, 1 << Constants.LAYER_ENEMY | 1 << Constants.LAYER_HITBOX | 1 << Constants.LAYER_NPC);
        if (!colliders.IsNullOrEmpty())
        {
            Vector3 finalDirection = Vector3.zero;
            float finalDistance = 0f;
            for (int i = 0; i < colliders.Length; ++i)
            {
                if (Physics.ComputePenetration(capsuleCollider, capsuleCollider.transform.position, capsuleCollider.transform.rotation, colliders[i], colliders[i].transform.position, colliders[i].transform.rotation, out Vector3 direction, out float distance))
                {
                    if(distance > finalDistance)
                    {
                        finalDistance = distance;
                        finalDirection = direction;
                    }
                }
            }
            actorRigidbody.position = actorRigidbody.position + (finalDirection * finalDistance);
        }
    }
}
