using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseMoveController : MonoBehaviour
{
    // Components
    protected Rigidbody actorRigidbody;
    protected StateController state;
    protected float capsuleRadius;
    protected float capsuleHeight;
    protected Vector3 capsuleRayPosition;

    [Header("Move Base")]
    [SerializeField] protected ACTOR_GROUND_STATE groundState;
    [SerializeField] protected Vector3 lastMoveDirection;

    [SerializeField] protected Vector3 inputMoveDirection;
    [SerializeField] protected float inputMoveSpeed;
    [SerializeField] protected Vector3 inputRotationDirection;
    [SerializeField] protected float inputRotationSpeed;

    [Header("Step Base")]
    [SerializeField] protected float stepSmooth;
    [SerializeField] protected float stepLimitHeight;

    [Header("Step Upper")]
    protected GameObject stepUpperObject;
    [SerializeField] protected float stepUpperZDistance;
    [SerializeField] protected Vector3 stepUpperHalfExtents;

    [Header("Step Lower")]
    protected GameObject stepLowerObject;
    [SerializeField] protected int stepLowerRayPrecision;
    [SerializeField] protected float stepLowerRayUnitScale;
    [SerializeField] protected float stepLowerShootDistance;

    [Header("Ground")]
    [SerializeField] protected float groundDetecingLimit;
    [SerializeField] protected float groundAngle;
    [SerializeField] protected Vector3 groundNormalVector;

    [Header("Floating")]
    [SerializeField] protected float floatingRayDistance;

    [Header("Sliding")]
    [SerializeField] protected float slidingLimitAngle;
    [SerializeField] protected float fallingSpeed;

    [SerializeField] protected float slidingTime;
    [SerializeField] protected float fallingTime;

    public void Initialize(BaseActor actor)
    {
        state = actor.State;
        actorRigidbody = actor.ActorRigidbody;
        float objectScale = transform.lossyScale.x;

        capsuleRadius = actor.CapsuleCollider.radius * objectScale;
        capsuleHeight = actor.CapsuleCollider.height * objectScale;
        capsuleRayPosition = new Vector3(0, capsuleRadius, 0);

        // Stepping
        stepSmooth = 0.05f;
        stepLimitHeight = capsuleHeight * 0.3f;

        stepUpperZDistance = (capsuleRadius + stepLimitHeight) * Mathf.Tan(Mathf.Deg2Rad * (90 - Constants.ANGLE_SLOPE_LIMIT));
        stepUpperHalfExtents = new Vector3(capsuleRadius, (capsuleHeight - stepLimitHeight) * 0.5f, stepUpperZDistance * 0.5f);
        stepUpperObject = Functions.FindObjectFromChild(gameObject, "Step_Upper_Object", true);
        if (stepUpperObject == null)
        {
            stepUpperObject = new GameObject("Step_Upper_Object");
            stepUpperObject.transform.SetParent(transform);
            stepUpperObject.transform.localPosition = new Vector3(0, stepLimitHeight + (capsuleHeight - stepLimitHeight) * 0.5f, stepUpperZDistance * 0.5f);
        }

        stepLowerRayPrecision = 3;
        stepLowerRayUnitScale = stepLimitHeight / (float)stepLowerRayPrecision;
        while (stepLowerRayUnitScale > 0.1f)
        {
            ++stepLowerRayPrecision;
            stepLowerRayUnitScale = stepLimitHeight / (float)stepLowerRayPrecision;
        }

        stepLowerShootDistance = capsuleRadius + capsuleRadius * 0.1f;
        stepLowerObject = Functions.FindObjectFromChild(gameObject, "Step_Lower_Object", true);
        if (stepLowerObject == null)
        {
            stepLowerObject = new GameObject("Step_Lower_Object");
            stepLowerObject.transform.SetParent(transform);
            stepLowerObject.transform.localPosition = Vector3.zero;
        }

        // Grounding
        groundDetecingLimit = capsuleRadius * 0.1f;

        // Floating
        floatingRayDistance = stepLimitHeight + stepSmooth;

        slidingLimitAngle = Constants.ANGLE_SLOPE_LIMIT;
    }

    public void SetMoveOnly(Vector3 moveDirection, float moveSpeed)
    {
        inputRotationDirection = Vector3.zero;

        inputMoveSpeed = moveSpeed;
        inputMoveDirection = moveDirection;
        inputMoveDirection.y = 0f;
        inputMoveDirection.Normalize();
    }

    public void SetRotationOnly(Vector3 roatationDirection, float rotationSpeed)
    {
        inputMoveDirection = Vector3.zero;

        inputRotationSpeed = rotationSpeed;
        inputRotationDirection = roatationDirection;
        inputRotationDirection.y = 0f;
        inputRotationDirection.Normalize();
    }

    public virtual void SetMovementAndRotation(Vector3 moveDirection, float moveSpeed, float rotationSpeed = 6f)
    {
        inputMoveSpeed = moveSpeed;
        inputMoveDirection = moveDirection;
        inputMoveDirection.y = 0f;
        inputMoveDirection.Normalize();

        inputRotationSpeed = rotationSpeed;
        inputRotationDirection = inputMoveDirection;
    }

    public void UpdatePosition()
    {
        // Rotate
        if (inputRotationDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(actorRigidbody.rotation, Quaternion.LookRotation(inputRotationDirection), inputRotationSpeed * Time.fixedDeltaTime);
        }

        // Move
        switch (groundState)
        {
            case ACTOR_GROUND_STATE.GROUNDING:
                actorRigidbody.velocity = inputMoveSpeed * lastMoveDirection;
                UpdateStep();
                break;

            case ACTOR_GROUND_STATE.FLOATING:
                actorRigidbody.velocity = inputMoveSpeed * lastMoveDirection;
                actorRigidbody.velocity += Vector3.down * fallingSpeed;
                UpdateStep();
                break;

            case ACTOR_GROUND_STATE.SLIDING:
            case ACTOR_GROUND_STATE.FALLING:

                actorRigidbody.velocity = fallingSpeed * lastMoveDirection;
                break;
        }
    }


    #region Step Functions
    public void UpdateStep()
    {
        if (inputMoveDirection == Vector3.zero || Physics.CheckBox(stepUpperObject.transform.position, stepUpperHalfExtents, Quaternion.identity, 1 << Constants.LAYER_TERRAIN))
            return;

        Vector3 rayDirection = transform.TransformDirection(Vector3.forward);

        for (int i = 0; i < stepLowerRayPrecision; ++i)
        {
            Debug.DrawRay(stepLowerObject.transform.position + new Vector3(0, stepLowerRayUnitScale * i, 0), rayDirection * stepLowerShootDistance);
            if (Physics.Raycast(stepLowerObject.transform.position + new Vector3(0, stepLowerRayUnitScale * i, 0), rayDirection, out RaycastHit forwardHit, stepLowerShootDistance, 1 << Constants.LAYER_TERRAIN))
            {
                float angle = Vector3.Angle(transform.up, forwardHit.normal);
                if (angle > Constants.ANGLE_SLOPE_LIMIT)
                {
                    actorRigidbody.velocity = new Vector3(actorRigidbody.velocity.x, 0f, actorRigidbody.velocity.z);
                    actorRigidbody.position += new Vector3(0, stepSmooth, 0);
                    return;
                }
            }
        }
    }
    #endregion

    #region Ground Functions
    public void CheckFloatingState()
    {
        Vector3 sphereCastPosition = transform.position + capsuleRayPosition;

        RaycastHit[] floatingHits = Physics.SphereCastAll(sphereCastPosition, capsuleRadius, Vector3.down, floatingRayDistance, 1 << Constants.LAYER_TERRAIN);
        if (floatingHits.Length > 0)
        {
            for (int i = 0; i < floatingHits.Length; ++i)
            {
                float angle = Vector3.Angle(transform.up, floatingHits[i].normal);
                if (groundAngle > angle)
                {
                    groundAngle = angle;
                    groundNormalVector = floatingHits[i].normal;
                }
            }

            if (groundAngle > Constants.ANGLE_SLOPE_LIMIT)
                groundState = ACTOR_GROUND_STATE.SLIDING;
            else
                groundState = ACTOR_GROUND_STATE.FLOATING;
        }
        else
        {
            groundState = ACTOR_GROUND_STATE.FALLING;
        }
    }
    public abstract void SetGroundStateAndValue(ACTOR_GROUND_STATE groundState);
    public ACTOR_GROUND_STATE UpdateGroundState()
    {
        // Initialize Ground Info
        groundAngle = 180;
        groundNormalVector = Vector3.zero;
        Vector3 sphereCastPosition = transform.position + capsuleRayPosition;

        // ------------ Grounding Check
        if (Physics.SphereCast(sphereCastPosition, capsuleRadius, Vector3.down, out RaycastHit groundHit, groundDetecingLimit, 1 << Constants.LAYER_TERRAIN))
        {
            float angle = Vector3.Angle(transform.up, groundHit.normal);
            groundAngle = angle;
            groundNormalVector = groundHit.normal;
            groundState = ACTOR_GROUND_STATE.GROUNDING;

            // ------------ Acceptable Down Check
            if (groundAngle > Constants.ANGLE_SLOPE_LIMIT)
                CheckFloatingState();
        }

        // ------------ Floating Check
        else
            CheckFloatingState();

        SetGroundStateAndValue(groundState);
        return groundState;
    }
    #endregion

    public virtual float GetFallDamage()
    {
        float slideDamageRatio = 0f;
        float fallDamageRatio = 0f;

        // Slide
        if (slidingTime <= 1.5f)
            slideDamageRatio = 0f;
        else
            slideDamageRatio = Mathf.Clamp01(0.2f * fallingTime);

        // Fall
        if (fallingTime <= 1f)
            fallDamageRatio = 0f;
        else
            fallDamageRatio = Mathf.Clamp01(0.4f * fallingTime);

        slidingTime = 0f;
        fallingTime = 0f;

        return Mathf.Clamp01(fallDamageRatio + slideDamageRatio);
    }

    #region Property
    public ACTOR_GROUND_STATE GroundState { get { return groundState; } }
    public float FallTime
    {
        get { return fallingTime; }
        set
        {
            fallingTime = value;

            if (fallingTime < 0f)
                fallingTime = 0f;
        }
    }
    public float SlideTime
    {
        get { return slidingTime; }
        set
        {
            slidingTime = value;

            if (slidingTime < 0f)
                slidingTime = 0f;
        }
    }
    #endregion
}
