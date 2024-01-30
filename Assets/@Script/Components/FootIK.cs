using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    private Animator animator;
    private LayerMask terrainLayer;
    [Range(0, 1)][SerializeField] private float groundDetectingDistance;
    [Range(0, 1)][SerializeField] private float offset;

    private void Awake()
    {
        TryGetComponent(out animator);
        terrainLayer = LayerMask.GetMask("Terrain");
        groundDetectingDistance = 0.4f;
        offset = 0f;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);

            Vector3 leftFootPosition = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
            Vector3 rightFootPosition = animator.GetIKPosition(AvatarIKGoal.RightFoot);

            RaycastHit hit;

            // Left Foot
            Ray ray = new Ray(leftFootPosition + Vector3.up, Vector3.down);
            Debug.DrawRay(leftFootPosition + Vector3.up, Vector3.down * (1 + groundDetectingDistance), Color.green, 0.1f);
            if (Physics.Raycast(ray, out hit, 1 + groundDetectingDistance, terrainLayer))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += (groundDetectingDistance + offset);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
            }

            // Right Foot
            ray = new Ray(rightFootPosition + Vector3.up, Vector3.down);
            Debug.DrawRay(rightFootPosition + Vector3.up, Vector3.down * (1 + groundDetectingDistance), Color.green, 0.1f);

            if (Physics.Raycast(ray, out hit, 1 + groundDetectingDistance, terrainLayer))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += (groundDetectingDistance + offset);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
            }
        }
    }
}
