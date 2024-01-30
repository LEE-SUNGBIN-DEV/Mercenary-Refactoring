using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class BaseEnemy : BaseActor
{
    [Header("Targets")]
    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected Vector3 targetDirection;
    [SerializeField] protected float targetDistance;
    protected LayerMask playerLayer = 1 << Constants.LAYER_PLAYER;

    public void UpdateTarget()
    {
        if (targetTransform != null)
        {
            targetDistance = Vector3.Distance(targetTransform.position, transform.position);
            targetDirection = Functions.GetXZAxisDirection(transform.position, targetTransform.position);
        }
    }

    public void LookTarget(float rotationSpeed = Constants.ENEMY_SKILL_DEFAULT_ROTATION_SPEED)
    {
        if (targetTransform != null)
        {
            Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        }
    }

    public void ReleaseTarget(PlayerCharacter player = null)
    {
        targetTransform = null;
    }

    public bool IsTargetDetected()
    {
        if (IsChaseCondition())
            return true;

        Collider[] collidersInDistance = Physics.OverlapSphere(transform.position, status.DetectionDistance, playerLayer);

        // In Distance
        for (int i = 0; i < collidersInDistance.Length; ++i)
        {
            if (collidersInDistance[i].TryGetComponent(out PlayerCharacter player))
            {
                targetTransform = player.transform;
                UpdateTarget();

                Vector3 rayPosition = transform.position + Vector3.up;

                // Blocked By Terrain
                if (Physics.Raycast(rayPosition, targetDirection, out RaycastHit rayHit, status.DetectionDistance)
                    && rayHit.transform.gameObject.layer == Constants.LAYER_TERRAIN)
                {
                    ReleaseTarget();
                    return false;
                }

                // In Detection Angle & Height
                if (IsTargetInAngle(Constants.ENEMY_DETECTION_ANGLE * 0.5f) && IsTargetInHeight(Constants.ENEMY_DETECTION_HEIGHT))
                {
                    player.OnPlayerDie += ReleaseTarget;
                    return true;
                }
            }
        }

        ReleaseTarget();
        return false;
    }

    public bool IsTargetInAngle(float angle)
    {
        return Vector3.Angle(transform.forward, targetDirection) < angle;
    }

    public bool IsTargetInHeight(float height)
    {
        return Mathf.Abs(transform.position.y - targetTransform.position.y) < height;
    }

    public bool IsTargetInDistance(float distance)
    {
        if (targetTransform != null && targetDistance < distance)
            return true;

        return false;
    }

    public bool IsChaseCondition()
    {
        if (IsTargetInDistance(status.ChaseDistance) && IsTargetInHeight(Constants.ENEMY_DETECTION_HEIGHT))
            return true;

        targetTransform = null;
        return false;
    }

    #region Property
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 TargetDirection { get { return targetDirection; } }
    public float TargetDistance { get { return targetDistance; } }
    #endregion
}
