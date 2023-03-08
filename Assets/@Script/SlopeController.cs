using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeController : MonoBehaviour
{
    private BaseActor actor;
    private float slopeLimit;
    private Vector3 slideDirection;
    RaycastHit slopeHit;

    private float slideSpeed;
    private float slideFriction;

    private void Start()
    {
        actor = GetComponent<BaseActor>();
        slopeLimit = actor.CharacterController.slopeLimit;
        slideDirection = Vector3.zero;

        slideSpeed = 10f;
        slideFriction = 0.2f;
    }

    private void Update()
    {
        // 경사면에 닿았을 경우
        if (IsIncline())
        {
            // 캐릭터를 경사면 방향으로 미끄러뜨림
            slideDirection = Vector3.ProjectOnPlane(Vector3.down, slopeHit.normal);
            actor.CharacterController.SimpleMove(slideDirection * slideSpeed);
        }

        /*
        // 마찰력
        slideSpeed -= slideFriction * slideDirection.magnitude * Time.deltaTime;
        if (slideSpeed < 0f)
        {
            slideSpeed = 0f;
            slideDirection = Vector3.zero;
        }
         */
    }

    private bool IsIncline()
    {
        if (actor.IsGround)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 10f, LayerMask.GetMask("Terrain"), QueryTriggerInteraction.Ignore))
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                Debug.Log(slopeAngle);

                if (slopeAngle > slopeLimit)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
