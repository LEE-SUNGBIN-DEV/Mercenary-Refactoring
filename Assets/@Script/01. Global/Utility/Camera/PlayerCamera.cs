using UnityEngine;

public enum CAMERA_MODE
{
    PLAYER,
    LOCK_ON,
    CORRECTION,
    INTERACTION,
    FiXED,
}

public class PlayerCamera : BaseCamera
{
    [SerializeField] private float cameraSpeed;           // 카메라 속도
    [SerializeField] private float sensitivity;           // 마우스 민감도
    [SerializeField] private float clampAngle;            // 시야각
    [SerializeField] private float smoothness;            // 
    
    [SerializeField] private float minDistance;           // 최소 거리
    [SerializeField] private float maxDistance;           // 최대 거리

    private CAMERA_MODE mode;

    // Player 
    private float mouseRotateX;         // 마우스 이동에 따른 X축 회전
    private float mouseRotateY;         // 마우스 이동에 따른 Y축 회전

    // Lock On
    private Transform lockOnTarget;

    // Correction 
    private Vector3 correctionDirection;
    private Quaternion correctionRotation;

    // Common
    private Vector3 cameraDirection;
    private float cameraDistance;

    public void ReleaseTarget(PlayerCharacter character)
    {
        targetTransform = null;
    }

    public void Initialize(PlayerCharacter character)
    {
        base.Initialize();
        Managers.GameManager.PlayerCamera = this;
        mode = CAMERA_MODE.PLAYER;

        // Player
        character.OnPlayerDie += ReleaseTarget;
        targetTransform = Functions.FindChild<Transform>(character.gameObject, "Camera_Target", true);

        // Lock On
        lockOnTarget = null;
    }

    private void Start()
    {
        transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
        targetCamera.transform.SetLocalPositionAndRotation(cameraPositionOffset, Quaternion.Euler(cameraRotationOffset));

        cameraDirection = targetCamera.transform.localPosition.normalized;
        cameraDistance = targetCamera.transform.localPosition.magnitude;
    }

    private void Update()
    {
        switch (mode)
        {
            case CAMERA_MODE.PLAYER:
                if (targetTransform == null)
                    return;

                mouseRotateX += -(Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime); // 카메라 X축 회전은 마우스 Y 좌표에 의해 결정됨
                mouseRotateX = Mathf.Clamp(mouseRotateX, -clampAngle, clampAngle);

                mouseRotateY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;    // 카메라 Y축 회전은 마우스 X 좌표에 의해 결정됨

                Quaternion finalRotate = Quaternion.Euler(mouseRotateX, mouseRotateY, 0);
                transform.rotation = finalRotate;
                break;

            case CAMERA_MODE.CORRECTION:
                transform.rotation = Quaternion.Slerp(transform.rotation, correctionRotation, Time.deltaTime * 6f);
                break;

            case CAMERA_MODE.INTERACTION:
                break;

            case CAMERA_MODE.FiXED:
                break;
        }
    }

    private void LateUpdate()
    {
        switch (mode)
        {
            case CAMERA_MODE.PLAYER:
                SetLastPosition();
                break;

            case CAMERA_MODE.CORRECTION:
                SetLastPosition();

                if (Vector3.Angle(transform.forward, correctionDirection) < 1f)
                {
                    mode = CAMERA_MODE.PLAYER;
                    mouseRotateX = transform.rotation.eulerAngles.x;
                    mouseRotateY = transform.rotation.eulerAngles.y;

                    if (mouseRotateX > 180f)
                        mouseRotateX -= 360;
                }
                break;

            case CAMERA_MODE.INTERACTION:
                SetLastPosition();
                break;

            case CAMERA_MODE.FiXED:
                break;
        }
    }

    public void SetLastPosition()
    {
        if (targetTransform == null)
            return;

        // Last Object Position (Move To Target)
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, cameraSpeed * Time.deltaTime);

        Vector3 rayDirection = targetCamera.transform.TransformDirection(cameraDirection);
#if UNITY_EDITOR
        Debug.DrawRay(targetTransform.position, rayDirection * maxDistance, Color.red);
#endif
        // Shoot Ray from Camera to Target
        if (Physics.Raycast(targetTransform.position, rayDirection, out RaycastHit hitObject, maxDistance, 1 << Constants.LAYER_TERRAIN))
        {
            cameraDistance = Mathf.Clamp(hitObject.distance, minDistance, maxDistance);
            cameraDistance -= 0.1f;
        }
        else
            cameraDistance = maxDistance;

        // Last Camera Position
        targetCamera.transform.localPosition = Vector3.Lerp(targetCamera.transform.localPosition, cameraDirection * cameraDistance, Time.deltaTime * smoothness);
    }

    public void ActiveCorrectionMode(Vector3 targetPosition)
    {
        if (targetTransform == null)
            return;

        mode = CAMERA_MODE.CORRECTION;
        correctionDirection = targetPosition - targetTransform.position;
        correctionDirection.y = 0f;
        correctionDirection.Normalize();

        correctionRotation = Quaternion.LookRotation(correctionDirection);
        Vector3 eulerRotation = correctionRotation.eulerAngles;
        eulerRotation.z = 0f;
        correctionRotation = Quaternion.Euler(eulerRotation);
    }

    public void ActiveInteractionMode(bool isInteracting)
    {
        switch(isInteracting)
        {
            case true:
                mode = CAMERA_MODE.INTERACTION;
                break;

            case false:
                mode = CAMERA_MODE.PLAYER;
                break;
        }
    }

    public void ActiveFixedMode(bool isFixed)
    {
        switch (isFixed)
        {
            case true:
                mode = CAMERA_MODE.FiXED;
                break;

            case false:
                mode = CAMERA_MODE.PLAYER;
                break;
        }
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
