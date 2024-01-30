using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract partial class BaseEnemy : BaseActor
{
    [Header("Move")]
    protected EnemyMoveController moveController;
    protected NavMeshPath path;
    [SerializeField] protected Vector3 moveDirection;
    [SerializeField] protected float moveDistance;

    [SerializeField] protected bool isRepathable;
    [SerializeField] protected float repathInterval;
    [SerializeField] protected float repathDuration;

    [SerializeField] protected float avoidanceRadius;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }

    public void InitializeMovement()
    {
        if (TryGetComponent(out moveController))
        {
            path = new NavMeshPath();
            isRepathable = true;
            repathInterval = 0.4f;
            repathDuration = 0f;
            avoidanceRadius = capsuleCollider.radius * transform.lossyScale.x * 5f;
        }
    }

    public void UpdateMoveInterval()
    {
        repathDuration += Time.deltaTime;
        if (repathDuration > repathInterval)
        {
            repathDuration = 0f;
            isRepathable = true;
        }
    }

    public void TryMoveTo(Vector3 destination, float speedRatio = 1f)
    {
        if (!isRepathable)
            return;

        moveDistance = Vector3.Distance(destination, transform.position);
        moveDirection = (destination - transform.position);
        if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path))
        {
            // Path Move
            Vector3 pathDirection = (destination - transform.position).normalized;
            for (int i = 0; i < path.corners.Length; ++i)
            {
                pathDirection = path.corners[i] - transform.position;
                if (pathDirection.magnitude > status.StopDistance)
                {
                    pathDirection = pathDirection.normalized;
                    break;
                }
            }

            // Avoid Move
            Vector3 avoidanceMove = Vector3.zero;
            Collider[] nearbyAgentColliders = Physics.OverlapSphere(transform.position, avoidanceRadius, 1 << Constants.LAYER_ENEMY);
            for (int i = 0; i < nearbyAgentColliders.Length; ++i)
            {
                if (nearbyAgentColliders[i] != capsuleCollider)
                {
                    float lossyRadius = capsuleCollider.radius * transform.lossyScale.x;
                    float agentDistance = lossyRadius
                        / Mathf.Clamp(Vector3.Distance(transform.position, nearbyAgentColliders[i].transform.position), lossyRadius, avoidanceRadius);

                    avoidanceMove += agentDistance * (transform.position - nearbyAgentColliders[i].transform.position).normalized;
                }
            }

            // Move + Combine
            moveDirection = pathDirection + avoidanceMove;
#if UNITY_EDITOR
            // Draw Path
            for (int i = 0; i < path.corners.Length - 1; i++)
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.gray);
#endif
        }
        isRepathable = false;
        moveController.SetMove(moveDirection, status.MoveSpeed * speedRatio);
    }

    #region Property
    public EnemyMoveController MoveController { get { return moveController; } }
    public float MoveDistance { get { return moveDistance; } }
    #endregion
}
