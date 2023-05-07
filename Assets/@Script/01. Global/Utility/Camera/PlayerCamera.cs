using UnityEngine;

public class PlayerCamera : BaseCamera
{
    [SerializeField] private float cameraSpeed;           // 카메라 속도
    [SerializeField] private float sensitivity;           // 카메라 민감도
    [SerializeField] private float clampAngle;            // 시야각
    [SerializeField] private float smoothness;            // 
    
    [SerializeField] private float minDistance;           // 최소 거리
    [SerializeField] private float maxDistance;           // 최대 거리

    private float mouseRotateX;         // 마우스 이동에 따른 X축 회전
    private float mouseRotateY;         // 마우스 이동에 따른 Y축 회전

    private Vector3 finalDirection;
    private Vector3 cameraDirection;
    private float cameraDistance;

    private bool isInteracting;

    public void Initialize(PlayerCharacter character)
    {
        base.Initialize();
        Managers.GameManager.PlayerCamera = this;
        targetTransform = character.transform;
        isInteracting = false;

        transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
        mouseRotateX = transform.localRotation.eulerAngles.x;
        mouseRotateY = transform.localRotation.eulerAngles.y;

        targetCamera.transform.SetLocalPositionAndRotation(cameraPositionOffset, Quaternion.Euler(cameraRotationOffset));
        cameraDirection = targetCamera.transform.localPosition.normalized;
        cameraDistance = targetCamera.transform.localPosition.magnitude;
    }

    private void Start()
    {
        transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
        mouseRotateX = transform.localRotation.eulerAngles.x;
        mouseRotateY = transform.localRotation.eulerAngles.y;

        targetCamera.transform.SetLocalPositionAndRotation(cameraPositionOffset, Quaternion.Euler(cameraRotationOffset));
        cameraDirection = targetCamera.transform.localPosition.normalized;
        cameraDistance = targetCamera.transform.localPosition.magnitude;
    }

    private void Update()
    {
        if (targetTransform == null || isInteracting)
            return;

        mouseRotateX += -(Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime); // 카메라 X축 회전은 마우스 Y 좌표에 의해 결정됨
        mouseRotateY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;    // 카메라 Y축 회전은 마우스 X 좌표에 의해 결정됨

        mouseRotateX = Mathf.Clamp(mouseRotateX, -clampAngle, clampAngle);

        Quaternion mouseRotate = Quaternion.Euler(mouseRotateX, mouseRotateY, 0);
        transform.rotation = mouseRotate;
    }

    private void LateUpdate()
    {
        if (targetTransform == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, cameraSpeed * Time.deltaTime);
        finalDirection = targetCamera.transform.TransformPoint(cameraDirection * maxDistance);

        if (Physics.Linecast(targetCamera.transform.position, finalDirection, out RaycastHit hitObject))
        {
            cameraDistance = Mathf.Clamp(hitObject.distance, minDistance, maxDistance);
        }

        else
        {
            cameraDistance = maxDistance;
        }

        targetCamera.transform.localPosition = Vector3.Lerp(targetCamera.transform.localPosition, cameraDirection * cameraDistance, Time.deltaTime * smoothness);
    }

    public void SetInteraction(bool isInteracting)
    {
        this.isInteracting = isInteracting;
    }

    #region Property
    public float CameraSpeed { get { return cameraSpeed; } }
    public float Sensitivity { get { return sensitivity; } set { sensitivity = value; } }
    public float ClampAngle { get { return clampAngle; } }
    public float Smoothness { get { return smoothness; } }
    public float MinDistance { get { return minDistance; } }
    public float MaxDistance { get { return maxDistance; } }
    #endregion
}
