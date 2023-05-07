using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMoveController
{
    private CharacterController characterController;
    private StateController state;

    [SerializeField] private ACTOR_GROUND_STATE groundState;

    [SerializeField] private Vector3 inputMoveDirection;
    [SerializeField] private float inputMoveSpeed;
    [SerializeField] private Vector3 inputRotationDirection;
    [SerializeField] private float inputRotationSpeed;

    [SerializeField] private Vector3 slideDirection;
    [SerializeField] private float slideSpeed;

    private float groundRayDistance;
    private float groundRayRadius;
    private Vector3 groundRayPosition;

    private float slopeLimitAngle;
    [SerializeField] private float slideTime;
    [SerializeField] private float fallTime;

    public EnemyMoveController(BaseActor actor)
    {
        state = actor.State;
        characterController = actor.CharacterController;
        if (characterController != null)
        {
            characterController.slopeLimit = Constants.ANGLE_SLOPE_LIMIT;
            slopeLimitAngle = characterController.slopeLimit;

            groundRayDistance = characterController.height * 0.5f;
            groundRayRadius = characterController.radius + 0.01f;
            groundRayPosition = new Vector3(0, groundRayRadius, 0);
        }

        inputMoveDirection = Vector3.zero;
        inputMoveSpeed = 0f;

        slideDirection = Vector3.zero;
        slideSpeed = 0f;

        fallTime = 0f;
        slideTime = 0f;
    }

    public void SetMovement(Vector3 moveDirection, float moveSpeed, float rotationSpeed = 6f)
    {
        inputMoveDirection = moveDirection;
        inputMoveDirection.y = 0f;
        inputMoveDirection = inputMoveDirection.normalized;
        inputMoveSpeed = moveSpeed;

        inputRotationDirection = inputMoveDirection;
        inputRotationSpeed = rotationSpeed;
    }

    public bool CastGroundRay(Vector3 rayPosition, float rayDistance, int layerMask, ref float minAngle, ref Vector3 minNormalVector, ref bool isGround)
    {
        if (Physics.Raycast(rayPosition, Vector3.down, out RaycastHit centerGroundHit, rayDistance, layerMask))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, centerGroundHit.normal);
            if (slopeAngle < minAngle)
            {
                minAngle = slopeAngle;
                minNormalVector = centerGroundHit.normal;
            }
            isGround = true;
            return true;
        }
        return false;
    }

    public ACTOR_GROUND_STATE UpdateGroundState()
    {
        // Initialize Previous Movement
        inputMoveDirection = Vector3.zero;
        inputMoveSpeed = 0f;

        slideDirection = Vector3.zero;
        slideSpeed = 0f;

        // Initialize Ground Ray Info
        bool isGround = false;
        float minAngle = 90f;
        Vector3 minNormalVector = Vector3.zero;

        // Center Graound Ray
        //Debug.DrawRay(characterController.transform.position + groundRayPosition, Vector3.down * groundRayDistance, Color.red, 0.5f);
        CastGroundRay(characterController.transform.position + groundRayPosition, groundRayDistance, LayerMask.GetMask("Terrain"), ref minAngle, ref minNormalVector, ref isGround);

        // Edges Ground Ray
        int precision = 16;
        float radianUnit = Mathf.Deg2Rad * 360 / (float)precision;

        for (int i = 0; i < precision; i++)
        {
            float positionX = groundRayRadius * Mathf.Cos(radianUnit * i);
            float positionY = groundRayRadius;
            float positionZ = positionX * Mathf.Tan(radianUnit * i);
            Vector3 rayPosition = new Vector3(positionX, positionY, positionZ);

            //Debug.DrawRay(characterController.transform.position + rayPosition, Vector3.down * groundRayDistance, Color.red, 0.5f);
            CastGroundRay(characterController.transform.position + rayPosition, groundRayDistance, LayerMask.GetMask("Terrain"), ref minAngle, ref minNormalVector, ref isGround);
        }

        return groundState = SetGroundStateAndValue(isGround, minAngle, minNormalVector);
    }

    public ACTOR_GROUND_STATE SetGroundStateAndValue(bool isGround, float minAngle, Vector3 minNormalVector)
    {
        switch (isGround)
        {
            case true:
                if (minAngle > slopeLimitAngle)
                {
                    // Slope
                    slideDirection = Vector3.ProjectOnPlane(Vector3.down, minNormalVector).normalized;
                    slideSpeed = Constants.GRAVITY_DEFAULT * Mathf.Sin(Mathf.Deg2Rad * minAngle);
                    state.SetState(ACTION_STATE.ENEMY_SLIDE, STATE_SWITCH_BY.WEIGHT);
                    return ACTOR_GROUND_STATE.SLOPE;
                }

                // Ground
                slideDirection = Vector3.down;
                slideSpeed = Constants.GRAVITY_DEFAULT * Mathf.Sin(Mathf.Deg2Rad * minAngle);
                return ACTOR_GROUND_STATE.GROUND;

            case false:
                // Air
                slideDirection = Vector3.down;
                slideSpeed = Constants.GRAVITY_DEFAULT;
                state.SetState(ACTION_STATE.ENEMY_FALL, STATE_SWITCH_BY.WEIGHT);
                return ACTOR_GROUND_STATE.AIR;
        }
    }

    public void UpdatePosition()
    {
        // Input Move
        if (inputMoveDirection != Vector3.zero)
        {
            // Rotate
            characterController.transform.rotation = Quaternion.Lerp(characterController.transform.rotation, Quaternion.LookRotation(inputRotationDirection), inputRotationSpeed * Time.deltaTime);

            // Move
            characterController.Move(inputMoveSpeed * Time.deltaTime * inputMoveDirection);
        }

        // Sliding Move
        characterController.Move(slideSpeed * Time.deltaTime * slideDirection);
    }

    public float GetFallDamage()
    {
        float slideDamageRatio = 0f;
        float fallDamageRatio = 0f;

        // Slide
        if (slideTime <= 1.5f)
            slideDamageRatio = 0f;
        else
            slideDamageRatio = Mathf.Clamp01(0.2f * fallTime);

        // Fall
        if (fallTime <= 1f)
            fallDamageRatio = 0f;
        else
            fallDamageRatio = Mathf.Clamp01(0.4f * fallTime);

        slideTime = 0f;
        fallTime = 0f;

        return Mathf.Clamp01(fallDamageRatio + slideDamageRatio);
    }

    #region Property
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
    #endregion
}
