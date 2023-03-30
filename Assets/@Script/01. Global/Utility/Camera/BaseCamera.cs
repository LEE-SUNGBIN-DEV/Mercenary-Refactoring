using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
    [SerializeField] protected Camera targetCamera;
    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected Vector3 cameraPositionOffset;
    [SerializeField] protected Vector3 cameraRotationOffset;
    protected Vector3 originalPosition;
    protected IEnumerator shakeCoroutine;
    
    protected virtual void Awake()
    {
        if(targetCamera == null)
            targetCamera = GetComponentInChildren<Camera>();
    }

    public void SetCameraTransform(Transform targetTransform)
    {
        gameObject.SetTransform(targetTransform);
    }

    public void ShakeCamera(float shakeTime, float shakeIntensity = 0.05f)
    {
        originalPosition = targetCamera.transform.position;
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = CoShakeCamera(shakeTime, shakeIntensity);
        StartCoroutine(shakeCoroutine);
    }

    public IEnumerator CoShakeCamera(float shakeTime, float shakeIntensity)
    {
        float cumulativeTime = 0f;

        while(cumulativeTime < shakeTime)
        {
            cumulativeTime += Time.deltaTime;
            transform.position = (Random.insideUnitSphere * shakeIntensity) + originalPosition;
            yield return null;
        }
        targetCamera.transform.position = originalPosition;
    }

    #region Property
    public Camera TargetCamera { get { return targetCamera; } }
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 CameraOffset { get { return cameraPositionOffset; } }
    public Vector3 OriginalPosition { get { return originalPosition; } set { originalPosition = value; } }
    #endregion
}
