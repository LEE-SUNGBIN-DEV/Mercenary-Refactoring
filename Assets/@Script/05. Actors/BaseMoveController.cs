using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MOVE_STATE
{
    STEP_UP,
    GROUNDING,
    FLOATING,
    SLIDING,
    FALLING
}

[System.Serializable]
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public abstract class BaseMoveController : MonoBehaviour
{
    // Components
    protected Rigidbody actorRigidbody;
    protected CapsuleCollider capsuleCollider;
    [SerializeField] protected float capsuleRadius;
    [SerializeField] protected float capsuleHeight;
    [SerializeField] protected float contactRadius;

    [Header("Move Base")]
    [SerializeField] protected MOVE_STATE moveState;
    [SerializeField] protected Vector3 inputMoveDirection;
    [SerializeField] protected float inputMoveSpeed;
    [SerializeField] protected Vector3 inputRotationDirection;
    [SerializeField] protected float inputRotationSpeed;

    [SerializeField] protected Vector3 finalMoveDirection;
    [SerializeField] protected float finalMoveSpeed;

    [Header("Step")]
    [SerializeField] protected float stepAngle;
    [SerializeField] protected float stepLimitHeight;
    [SerializeField] protected float stepHeight;
    [SerializeField] protected Vector3 stepPoint;

    [Header("Ground")]
    [SerializeField] protected float groundDetecingLimit;
    [SerializeField] protected float groundDistance;
    [SerializeField] protected float groundAngle;
    [SerializeField] protected Vector3 groundNormalVector;
    [SerializeField] protected Vector3 groundPoint;

    [Header("Floating")]
    [SerializeField] protected float floatingDetectionLimit;

    [Header("Sliding/Falling")]
    [SerializeField] protected float slidingLimitAngle;
    [SerializeField] protected float gravitySpeed;

    [SerializeField] protected float slidingTime;
    [SerializeField] protected float fallingTime;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, capsuleRadius, 0), capsuleRadius);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, capsuleHeight - capsuleRadius, 0), capsuleRadius);
    }
    protected virtual void FixedUpdate()
    {
        UpdateMoveState();
        UpdatePosition();
    }
    protected virtual void Awake()
    {
        TryGetComponent(out actorRigidbody);
        TryGetComponent(out capsuleCollider);
        float objectScale = transform.lossyScale.x;

        capsuleRadius = capsuleCollider.radius * objectScale;
        capsuleHeight = capsuleCollider.height * objectScale;
        contactRadius = capsuleRadius - Constants.CONTACT_OFFSET;

        // Stepping
        stepLimitHeight = capsuleRadius * 1.5f;
        stepAngle = 0f;
        stepHeight = 0f;
        stepPoint = Vector3.zero;

        // Grounding
        groundDetecingLimit = capsuleRadius * 0.1f;

        // Floating
        floatingDetectionLimit = stepLimitHeight + Constants.CONTACT_OFFSET;

        // Sliding-Falling
        slidingLimitAngle = Constants.ANGLE_SLOPE_LIMIT;
        slidingTime = 0f;
        fallingTime = 0f;
    }

    public virtual void SetMove(Vector3 moveDirection, float moveSpeed, float rotationSpeed = 6f)
    {
        inputMoveSpeed = moveSpeed;
        inputMoveDirection = Functions.GetXZAxisDirection(moveDirection);

        inputRotationSpeed = rotationSpeed;
        inputRotationDirection = inputMoveDirection;
    }
    protected bool ValidateGroundHits(ref RaycastHit[] groundHits)
    {
        bool isValid = false;
        for (int i = 0; i < groundHits.Length; i++)
        {
            if (groundHits[i].distance < groundDistance)
            {
                groundDistance = groundHits[i].distance;
                groundNormalVector = groundHits[i].normal;
                groundAngle = Vector3.Angle(transform.up, groundHits[i].normal);
                groundPoint = groundHits[i].point;
                isValid = true;
            }
        }

        // Overlapped
        if (isValid && groundDistance == 0 && groundPoint == Vector3.zero)
        {
#if UNITY_EDITOR
            Debug.Log("Overlapped");
#endif
        }

        return isValid;
    }
    protected void UpdateMoveState()
    {
        // Initialize Ground Info
        groundDistance = float.PositiveInfinity;
        groundNormalVector = Vector3.zero;
        groundAngle = 0;

        // Try to Find Ground
        Vector3 sphereCastPosition = transform.position + new Vector3(0, capsuleRadius, 0);
        RaycastHit[] groundHits = Physics.SphereCastAll(sphereCastPosition, contactRadius, -transform.up, floatingDetectionLimit, 1 << Constants.LAYER_TERRAIN, QueryTriggerInteraction.Ignore);

        // Find: (1) Grounding (2) Floating (3) Sliding
        if (!groundHits.IsNullOrEmpty() && ValidateGroundHits(ref groundHits))
        {
            // Check Step
            if (TryStepUp())
                moveState = MOVE_STATE.STEP_UP;
            else
            {
                // Check Sliding Condition
                if (groundAngle > Constants.ANGLE_SLOPE_LIMIT)
                {
                    // Second Check For Step Down
                    if (Physics.Raycast(transform.position + new Vector3(0, Constants.CONTACT_OFFSET, 0), -transform.up, floatingDetectionLimit, 1 << Constants.LAYER_TERRAIN, QueryTriggerInteraction.Ignore))
                        SetGroundStateByDistance();
                    else
                        moveState = MOVE_STATE.SLIDING;
                }
                else
                    SetGroundStateByDistance();
            }
        }
        // Not Find: Falling
        else
        {
            moveState = MOVE_STATE.FALLING;            
        }
    }
    protected void SetGroundStateByDistance()
    {   
        // Check Grounding-Floating Condition
        if (groundDistance <= groundDetecingLimit)
        {
            moveState = MOVE_STATE.GROUNDING;
        }
        else
            moveState = MOVE_STATE.FLOATING;
    }
    protected virtual void UpdatePosition()
    {
        // Rotate
        if (inputRotationDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputRotationDirection), inputRotationSpeed * Time.fixedDeltaTime);
        }

        // Move
        switch (moveState)
        {
            case MOVE_STATE.STEP_UP:
                finalMoveDirection = Vector3.ProjectOnPlane(inputMoveDirection, groundNormalVector).normalized;
                finalMoveSpeed = inputMoveSpeed;
                gravitySpeed = 0f;

                actorRigidbody.position += transform.up * stepHeight;
                break;

            case MOVE_STATE.GROUNDING:
                finalMoveDirection = Vector3.ProjectOnPlane(inputMoveDirection, groundNormalVector).normalized;
                finalMoveSpeed = inputMoveSpeed;
                gravitySpeed = 0f;
                break;

            case MOVE_STATE.FLOATING:
                finalMoveDirection = Vector3.ProjectOnPlane(inputMoveDirection, groundNormalVector).normalized;
                finalMoveSpeed = inputMoveSpeed;
                gravitySpeed = Mathf.Min(gravitySpeed + Constants.GRAVITY_SPEED * Time.fixedDeltaTime, Constants.MAX_GRAVITY_SPEED);
                break;

            case MOVE_STATE.SLIDING:
                finalMoveDirection = new Vector3(inputMoveDirection.x, -1, inputMoveDirection.z);
                finalMoveDirection = Vector3.ProjectOnPlane(finalMoveDirection, groundNormalVector).normalized;
                finalMoveSpeed = inputMoveSpeed;
                gravitySpeed = Mathf.Min(gravitySpeed + Constants.GRAVITY_SPEED * Time.fixedDeltaTime, Constants.MAX_GRAVITY_SPEED);
                break;

            case MOVE_STATE.FALLING:
                finalMoveDirection = inputMoveDirection;
                finalMoveSpeed = inputMoveSpeed;
                gravitySpeed = Mathf.Min(gravitySpeed + Constants.GRAVITY_SPEED * Time.fixedDeltaTime, Constants.MAX_GRAVITY_SPEED);
                break;
        }
        actorRigidbody.velocity = finalMoveSpeed * finalMoveDirection;
        actorRigidbody.velocity += gravitySpeed * -transform.up;
    }
    protected bool TryStepUp()
    {
        if (inputMoveDirection == Vector3.zero)
            return false;

        stepHeight = 0f;
        stepAngle = 0f;
        stepPoint = transform.position;

        // Check Forward
        Vector3 capsulePoint1 = transform.position + new Vector3(0, capsuleRadius, 0);
        Vector3 capsulePoint2 = transform.position + new Vector3(0, capsuleHeight - capsuleRadius, 0);
        RaycastHit[] stepHits = Physics.CapsuleCastAll(capsulePoint1, capsulePoint2, contactRadius, transform.forward, 0.1f, 1 << Constants.LAYER_TERRAIN, QueryTriggerInteraction.Ignore);

        // Exist Obstacles
        if (!stepHits.IsNullOrEmpty())
        {
            // Find Step Point
            for (int i = 0; i < stepHits.Length; i++)
            {
                stepHeight = stepHits[i].point.y - transform.position.y;
                if (stepHeight.IsBetween(Constants.CONTACT_OFFSET, stepLimitHeight))
                {
                    float angle = Vector3.Angle(transform.up, stepHits[i].normal);
                    if (angle > Constants.ANGLE_SLOPE_LIMIT && angle > stepAngle)
                    {
                        stepAngle = angle;
                        stepPoint = stepHits[i].point;
                    }
                }
                else
                    stepHeight = 0;
            }
            // Validate Step up Conditions
            if (stepAngle > Constants.ANGLE_SLOPE_LIMIT && stepHeight.IsBetween(Constants.CONTACT_OFFSET, stepLimitHeight))
            {
                Vector3 stepCapsulePoint1 = stepPoint + new Vector3(0, capsuleRadius + stepHeight, 0);
                Vector3 stepCapsulePoint2 = stepPoint + new Vector3(0, capsuleHeight - capsuleRadius + stepHeight, 0);
                if (!Physics.CheckCapsule(stepCapsulePoint1, stepCapsulePoint2, contactRadius, 1 << Constants.LAYER_TERRAIN, QueryTriggerInteraction.Ignore))
                {
                    return true;
                }                    
            }
        }
        return false;
    }

    public virtual float GetFallDamageRate()
    {
        float slideDamageRate = 0f;
        float fallDamageRate = 0f;

        // Slide
        if (slidingTime <= 2f)
            slideDamageRate = 0f;
        else
            slideDamageRate = Mathf.Clamp01(0.1f * fallingTime);

        // Fall
        if (fallingTime <= 1.5f)
            fallDamageRate = 0f;
        else
            fallDamageRate = Mathf.Clamp01(0.3f * fallingTime);

        slidingTime = 0f;
        fallingTime = 0f;

        return Mathf.Clamp01(fallDamageRate + slideDamageRate);
    }

    #region Property
    public MOVE_STATE MoveState { get { return moveState; } }
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
