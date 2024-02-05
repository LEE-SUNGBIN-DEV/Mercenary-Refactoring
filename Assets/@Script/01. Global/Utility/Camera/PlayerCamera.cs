using UnityEngine;

public enum CAMERA_MODE
{
    FOLLOW_PLAYER,
    LOCK_ON,
    LOOK_ATTACKER,
    FiXED,
}

public class PlayerCamera : BaseCamera
{
    [SerializeField] private float cameraSpeed;           // ī�޶� �ӵ�
    [SerializeField] private float sensitivity;           // ���콺 �ΰ���
    [SerializeField] private float clampAngle;            // �þ߰�
    [SerializeField] private float smoothness;            // 
    
    [SerializeField] private float minDistance;           // �ּ� �Ÿ�
    [SerializeField] private float maxDistance;           // �ִ� �Ÿ�

    [SerializeField] private CAMERA_MODE mode;

    // Player 
    private float mouseRotateX;         // ���콺 �̵��� ���� X�� ȸ��
    private float mouseRotateY;         // ���콺 �̵��� ���� Y�� ȸ��

    // Lock On
    private Transform lockOnTarget;

    // Look Attacker 
    private Vector3 attackerDirection;
    private Quaternion attackerRotation;

    // Common
    private Vector3 cameraDirection;
    private float cameraDistance;

    #region Private
    private void OnEnable()
    {
        Managers.GameManager.ActivedCamera = this;
    }
    private void OnDisable()
    {
        
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
            case CAMERA_MODE.FOLLOW_PLAYER:
                if (targetTransform == null)
                    return;

                Vector2 mouseVector = Managers.InputManager.GetCharacterMouseVector();
                mouseRotateX += -(mouseVector.y * sensitivity * Time.deltaTime); // ī�޶� X�� ȸ���� ���콺 Y ��ǥ�� ���� ������
                mouseRotateX = Mathf.Clamp(mouseRotateX, -clampAngle, clampAngle);

                mouseRotateY += mouseVector.x * sensitivity * Time.deltaTime;    // ī�޶� Y�� ȸ���� ���콺 X ��ǥ�� ���� ������

                Quaternion finalRotate = Quaternion.Euler(mouseRotateX, mouseRotateY, 0);
                transform.rotation = finalRotate;
                break;

            case CAMERA_MODE.LOOK_ATTACKER:
                transform.rotation = Quaternion.Slerp(transform.rotation, attackerRotation, Time.deltaTime * 7f);
                break;

            case CAMERA_MODE.FiXED:
                break;
        }
    }

    private void LateUpdate()
    {
        switch (mode)
        {
            case CAMERA_MODE.FOLLOW_PLAYER:
                SetLastPosition();
                break;

            case CAMERA_MODE.LOOK_ATTACKER:
                SetLastPosition();

                if (Vector3.Angle(transform.forward, attackerDirection) < 1f)
                {
                    mode = CAMERA_MODE.FOLLOW_PLAYER;
                    mouseRotateX = transform.rotation.eulerAngles.x;
                    mouseRotateY = transform.rotation.eulerAngles.y;

                    if (mouseRotateX > 180f)
                        mouseRotateX -= 360;
                }
                break;

            case CAMERA_MODE.FiXED:
                break;
        }
    }
    #endregion

    public void Initialize(PlayerCharacter character)
    {
        base.Initialize();
        minDistance = targetCamera.nearClipPlane * 2 + Constants.CONTACT_OFFSET;
        mode = CAMERA_MODE.FOLLOW_PLAYER;

        // Player
        character.OnPlayerDie -= ReleaseTarget;
        character.OnPlayerDie += ReleaseTarget;
        targetTransform = Functions.FindChild<Transform>(character.gameObject, "Camera_Target", true);

        AddCustomPostProcess();
        // Lock On
        lockOnTarget = null;
    }
    public void AddCustomPostProcess()
    {
        Functions.GetOrAddComponent<ScreenShockWave>(targetCamera.gameObject).Initialize();
    }
    public void ReleaseTarget(PlayerCharacter character)
    {
        targetTransform = null;
    }

    public void SetLastPosition()
    {
        if (targetTransform == null)
            return;

        // Last Object Position (Move To Target)
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, cameraSpeed * Time.deltaTime);

        Vector3 castingDirection = targetCamera.transform.TransformDirection(cameraDirection);
#if UNITY_EDITOR
        Debug.DrawRay(targetTransform.position, castingDirection * maxDistance, Color.red);
#endif
        // Sphere Cast from Target to Camera
        if (Physics.SphereCast(targetTransform.position, minDistance, castingDirection, out RaycastHit terrainHit, maxDistance, 1 << Constants.LAYER_TERRAIN))
        {
            cameraDistance = Mathf.Clamp(terrainHit.distance, minDistance, maxDistance);
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

        mode = CAMERA_MODE.LOOK_ATTACKER;
        attackerDirection = targetPosition - targetTransform.position;
        attackerDirection.y = 0f;
        attackerDirection.Normalize();

        attackerRotation = Quaternion.LookRotation(attackerDirection);
        Vector3 eulerRotation = attackerRotation.eulerAngles;
        eulerRotation.z = 0f;
        attackerRotation = Quaternion.Euler(eulerRotation);
    }

    public void ActiveFixedMode(bool isFixed)
    {
        switch (isFixed)
        {
            case true:
                mode = CAMERA_MODE.FiXED;
                break;

            case false:
                mode = CAMERA_MODE.FOLLOW_PLAYER;
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
