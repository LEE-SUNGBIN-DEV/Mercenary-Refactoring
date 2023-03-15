using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallController
{
    public enum FALL_STATE
    {
        GROUNDING, FALLING, LANDING
    }

    private Transform transform;
    private CharacterController characterController;
    private StateController state;

    private bool isGround;
    private RaycastHit groundHit;

    // Fall
    private FALL_STATE fallState;
    private float fallTime;
    private float fallRayDistance;

    // Slide
    private Vector3 slideDirection;
    private float slideSpeed;
    private float slopeLimit;

    public FallController(Transform transform, CharacterController characterController, StateController state)
    {
        this.transform = transform;
        this.characterController = characterController;
        this.state = state;

        isGround = true;

        // Fall
        fallState = FALL_STATE.GROUNDING;
        fallTime = 0f;
        fallRayDistance = 0.5f;

        // Slide
        slideDirection = Vector3.zero;
        slideSpeed = 3f;
        slopeLimit = characterController.slopeLimit;
    }

    public void Update()
    {
        if (IsIncline())
        {
            slideDirection = Vector3.ProjectOnPlane(Vector3.down, groundHit.normal);
            characterController.Move(slideDirection * slideSpeed);
        }
    }

    public void FallDamageProcess()
    {
        if (fallTime < 2f)
        {
            Debug.Log("Fall Damaged");
        }
    }

    public bool IsGround()
    {
        if (characterController.isGrounded)
            isGround = true;

        else
        {
            Ray fallRay = new Ray(transform.position, Vector3.down);
            isGround = Physics.Raycast(fallRay, fallRayDistance, LayerMask.GetMask("Terrain"));
        }

        switch(isGround)
        {
            case true:
                {
                    if(fallState == FALL_STATE.FALLING)
                    {
                        FallDamageProcess();
                        //state.SetState(ACTION_STATE.COMMON_FALL, STATE_SWITCH_BY.WEIGHT);
                        // To Landing
                    }

                    fallTime = 0f;
                    fallState = FALL_STATE.GROUNDING;
                    break;
                }
            case false:
                {
                    fallState = FALL_STATE.FALLING;
                    fallTime += Time.deltaTime;
                    break;
                }
        }

        return isGround;
    }

    private bool IsIncline()
    {
        if (IsGround())
        {
            if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 1f, LayerMask.GetMask("Terrain")))
            {
                float slopeAngle = Vector3.Angle(Vector3.up, groundHit.normal);

                if (slopeAngle > slopeLimit)
                    return true;
            }
        }

        return false;
    }
}
