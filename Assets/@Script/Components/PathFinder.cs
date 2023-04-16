using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinder : MonoBehaviour
{
    private NavMeshPath path;
    private Vector3[] pathPoints;
    private Vector3 moveDirection;
    private int currentPathIndex;
    private float stopDistance;

    public void Initialize(float stopDistance)
    {
        path = new NavMeshPath();
        this.stopDistance = stopDistance;
    }

    public Vector3 FindPath(Vector3 targetPosition, int areaMask)
    {
        if (NavMesh.CalculatePath(transform.position, targetPosition, areaMask, path))
        {
            pathPoints = path.corners;
            currentPathIndex = 0;
        }

        moveDirection = Vector3.zero;

        for (int i = 0; i < pathPoints.Length; ++i)
        {
            if (currentPathIndex < pathPoints.Length && (pathPoints[currentPathIndex] - transform.position).magnitude < stopDistance)
            {
                currentPathIndex++;
            }
            else
            {
                moveDirection = pathPoints[currentPathIndex] - transform.position;
                moveDirection.y = 0f;

                return Vector3.Normalize(moveDirection);
            }
        }

        return moveDirection;
    }
}
