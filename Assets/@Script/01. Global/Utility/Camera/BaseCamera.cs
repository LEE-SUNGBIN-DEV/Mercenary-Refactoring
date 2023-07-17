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
    protected Quaternion originalRotation;
    protected Coroutine shakeCoroutine;
    
    public virtual void Initialize()
    {
        if (targetCamera == null)
            targetCamera = GetComponentInChildren<Camera>();
    }

    public void SetCameraPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        targetCamera.transform.position = position;
        targetCamera.transform.rotation = rotation;
    }
    public void SetCameraTransform(Transform targetTransform)
    {
        gameObject.SetTransform(targetTransform);
    }

    public void ShakeCamera(float shakeTime, float shakeIntensity = 0.05f)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(CoShakeCamera(shakeTime, shakeIntensity));
    }
    public void StopShakeCamera()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
    }

    public void ShakeCameraInplace(float shakeTime, float shakeIntensity = 0.05f)
    {
        originalPosition = targetCamera.transform.position;
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(CoShakeCameraInplace(shakeTime, shakeIntensity));
    }
    public void StopShakeCameraInplace()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        targetCamera.transform.position = originalPosition;
    }

    public IEnumerator CoShakeCamera(float shakeTime, float shakeIntensity)
    {
        float cumulativeTime = 0f;

        while (cumulativeTime < shakeTime)
        {
            cumulativeTime += Time.deltaTime;
            transform.position += (Random.insideUnitSphere * shakeIntensity);
            yield return null;
        }
    }

    public IEnumerator CoShakeCameraInplace(float shakeTime, float shakeIntensity)
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

    public Vector3 GetZeroYForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;

        return forward;
    }

    #region Property
    public Camera TargetCamera { get { return targetCamera; } }
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }
    public Vector3 CameraOffset { get { return cameraPositionOffset; } }
    public Vector3 OriginalPosition { get { return originalPosition; } set { originalPosition = value; } }
    public Quaternion OriginalRotation { get { return originalRotation; } set { originalRotation = value; } }
    #endregion
}
