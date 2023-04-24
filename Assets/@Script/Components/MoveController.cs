using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(CharacterController))]
public class MoveController
{
    private CharacterController characterController;
    [SerializeField] private ACTOR_GROUND_STATE groundState;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float fallTime;
    [SerializeField] private float slideTime;

    private float groundRayDistance;
    private float groundRayRadius;

    public MoveController(CharacterController characterController)
    {
        this.characterController = characterController;
        moveDirection = Vector3.zero;
        moveSpeed = 0f;
        fallTime = 0f;
        slideTime = 0f;

        groundRayDistance = characterController.height * 0.5f;
        groundRayRadius = characterController.radius;
    }

    public void SetMoveInformation(Vector3 direction, float speed)
    {
        moveDirection = direction;
        moveSpeed = speed;
    }

    public void Update()
    {
        if(moveDirection != Vector3.zero)
        {
            Vector3 rotateDirection = moveDirection;
            rotateDirection.y = 0f;

            characterController.transform.rotation = Quaternion.Lerp(characterController.transform.rotation, Quaternion.LookRotation(rotateDirection), 10f * Time.deltaTime);
            characterController.Move(moveSpeed * Time.deltaTime * moveDirection);
        }
    }

    public ACTOR_GROUND_STATE GetGroundState()
    {
        int precision = 12;
        float radianUnit = Mathf.Deg2Rad * 360 / (float)precision;

        for (int i = 0; i < precision; i++)
        {
            float positionX = groundRayRadius * Mathf.Cos(radianUnit * i);
            float positionZ = positionX * Mathf.Tan(radianUnit * i);
            Vector3 rayPosition = new Vector3(positionX, 0, positionZ);

            Debug.DrawRay(characterController.transform.position + rayPosition, Vector3.down * groundRayDistance, Color.red, 0.1f);
            if (Physics.Raycast(characterController.transform.position + rayPosition, Vector3.down, out RaycastHit groundHit, groundRayDistance, LayerMask.GetMask("Terrain")))
            {
                float slopeAngle = Vector3.Angle(Vector3.up, groundHit.normal);
                if (slopeAngle > characterController.slopeLimit)
                {
                    // Slope
                    moveSpeed = Constants.GRAVITY_DEFAULT * Mathf.Sin(Mathf.Deg2Rad * slopeAngle);
                    moveDirection = Vector3.ProjectOnPlane(Vector3.down, groundHit.normal).normalized;
                    groundState = ACTOR_GROUND_STATE.SLOPE;
                    return ACTOR_GROUND_STATE.SLOPE;
                }
                // Ground
                moveSpeed = 0f;
                moveDirection = Vector3.zero;
                groundState = ACTOR_GROUND_STATE.GROUND;
                return ACTOR_GROUND_STATE.GROUND;
            }
        }

        // Air
        moveSpeed = Constants.GRAVITY_DEFAULT;
        moveDirection = Vector3.down;
        groundState = ACTOR_GROUND_STATE.AIR;
        return ACTOR_GROUND_STATE.AIR;
    }

    public float GetFallDamage()
    {
        Debug.Log("Fall Time: " + fallTime + " Slide Time: " + slideTime);

        float fallDamageRatio = 0f;
        float slideDamageRatio = 0f;

        if(slideTime <= 1.5f)
        {
            slideDamageRatio = 0f;
        }
        else
        {
            slideDamageRatio = Mathf.Clamp01(0.2f * fallTime);
        }

        if (fallTime <= 1f)
        {
            fallTime = 0f;
            fallDamageRatio = 0f;
        }
        else
        {
            fallTime = 0f;
            fallDamageRatio = Mathf.Clamp01(0.4f * fallTime);
        }

        return Mathf.Clamp01(fallDamageRatio + slideDamageRatio);
    }

    public ACTOR_GROUND_STATE GroundState { get { return groundState; } }
    public float FallTime
    {
        get { return fallTime; }
        set
        {
            fallTime = value;

            if (fallTime < 0f)
                fallTime = 0f;
        }
    }
    public float SlideTime
    {
        get { return slideTime; }
        set
        {
            slideTime = value;

            if (slideTime < 0f)
                slideTime = 0f;
        }
    }
}
