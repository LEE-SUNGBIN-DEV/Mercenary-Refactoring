using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(NPC))]
public class NPCMoveController : BaseMoveController
{
    private StateController stateController;

    private void Start()
    {
        if (TryGetComponent(out NPC npc))
        {
            stateController = npc.State;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PushOut();
    }
    protected override void UpdatePosition()
    {
        base.UpdatePosition();
        switch (moveState)
        {
            case MOVE_STATE.SLIDING:
                break;

            case MOVE_STATE.FALLING:
                break;

            default:
                break;
        }
    }
    public void PushOut()
    {
        Collider[] colliders = Physics.OverlapCapsule(transform.position + new Vector3(0, capsuleRadius, 0), transform.position + new Vector3(0, capsuleHeight - capsuleRadius, 0), capsuleRadius, 1 << Constants.LAYER_ENEMY | 1 << Constants.LAYER_HITBOX);
        if (!colliders.IsNullOrEmpty())
        {
            Vector3 finalDirection = Vector3.zero;
            float finalDistance = 0f;
            for (int i = 0; i < colliders.Length; ++i)
            {
                if (Physics.ComputePenetration(capsuleCollider, capsuleCollider.transform.position, capsuleCollider.transform.rotation, colliders[i], colliders[i].transform.position, colliders[i].transform.rotation, out Vector3 direction, out float distance))
                {
                    if (distance > finalDistance)
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
