using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class BaseEnemy : BaseActor
{
    [Header("Targets")]
    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected Vector3 targetDirection;
    [SerializeField] protected float targetDistance;
    protected Vector3 heightOffset;

    public void UpdateTarget()
    {
        if (targetTransform != null)
        {
            targetDistance = Vector3.Distance(targetTransform.position, transform.position);
            targetDirection = targetTransform.position - transform.position;
            targetDirection.y = 0f;
        }
    }

    public void LookTarget(float rotationSpeed = 6f)
    {
        if (targetTransform != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotationSpeed * Time.deltaTime);
        }
    }

    public bool IsTargetDetected()
    {
        if (IsTargetInChaseDistance())
            return true;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, status.DetectionDistance, LayerMask.GetMask("Player"));
        // In Distance
        for (int i = 0; i < targetsInViewRadius.Length; ++i)
        {
            targetTransform = targetsInViewRadius[i].transform;
            UpdateTarget();
            Vector3 rayPosition = new Vector3(transform.position.x, characterController.height, transform.position.z);
            // Blocked By Terrain
            Debug.DrawRay(rayPosition, targetDirection.normalized * status.DetectionDistance, Color.red);
            if (Physics.Raycast(rayPosition, targetDirection, out RaycastHit rayHit, status.DetectionDistance)
                && rayHit.transform.gameObject.layer == Constants.LAYER_TERRAIN)
            {
                targetTransform = null;
                return false;
            }

            // In Detection Angle & Height
            if (Vector3.Angle(transform.forward, targetDirection) < Constants.ENEMY_DETECTION_ANGLE * 0.5f
                && Mathf.Abs(transform.position.y - targetTransform.position.y) < Constants.ENEMY_DETECTION_HEIGHT                )
            {
                return true;
            }
        }

        targetTransform = null;
        return false;
    }

    public bool IsTargetInStopDistance()
    {
        if (targetTransform != null && targetDistance < status.StopDistance)
            return true;

        return false;
    }

    public bool IsTargetInChaseDistance()
    {
        if (targetTransform != null && targetDistance < status.ChaseDistance
            && Mathf.Abs(transform.position.y - targetTransform.position.y) < Constants.ENEMY_DETECTION_HEIGHT)
        {
            return true;
        }

        targetTransform = null;
        return false;
    }

    #region Property
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 TargetDirection { get { return targetDirection; } }
    public float TargetDistance { get { return targetDistance; } }
    #endregion
}
