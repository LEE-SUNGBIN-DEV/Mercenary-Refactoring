using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ACTOR_GROUND_STATE
{
    GROUND,
    SLOPE,
    AIR
}


public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    protected Animator animator;
    protected CharacterController characterController;
    protected StateController state;
    protected Dictionary<string, Material> materialDictionary;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;
    [SerializeField] protected MaterialContainer[] materialContainers;
    [SerializeField] protected ObjectPooler objectPooler = new ObjectPooler();
    
    [SerializeField] protected bool isInvincible;
    [SerializeField] protected bool isDie;

    protected float groundRayHeight;
    protected float groundRayRadius;

    protected virtual void Awake()
    {
        TryGetComponent(out animator);
        if(TryGetComponent(out characterController))
        {
            groundRayHeight = characterController.height * 0.5f;
            groundRayRadius = characterController.radius;
        }

        state = new StateController(animator);
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>(true);
        objectPooler.Initialize(transform);

        if (materialContainers != null)
        {
            materialDictionary = new Dictionary<string, Material>();
            for (int i=0; i<materialContainers.Length; ++i)
            {
                materialDictionary.Add(materialContainers[i].key, materialContainers[i].value);
            }
        }
    }

    public void SetMaterial(string key)
    {
        if(materialDictionary.ContainsKey(key))
        {
            meshRenderer.material = materialDictionary[key];
        }
    }

    public void FallDamageProcess(float fallTime)
    {
        if (fallTime >= 2f)
        {
            Debug.Log("Fall Damaged: " + fallTime);
        }
    }

    public ACTOR_GROUND_STATE GetGroundState()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit groundHit, groundRayHeight, LayerMask.GetMask("Terrain"))
            || Physics.Raycast(transform.position, -transform.up + new Vector3(groundRayRadius, 0, 0), out groundHit, groundRayHeight, LayerMask.GetMask("Terrain"))
            || Physics.Raycast(transform.position, -transform.up + new Vector3(-groundRayRadius, 0, 0), out groundHit, groundRayHeight, LayerMask.GetMask("Terrain"))
            || Physics.Raycast(transform.position, -transform.up + new Vector3(0, 0, groundRayRadius), out groundHit, groundRayHeight, LayerMask.GetMask("Terrain"))
            || Physics.Raycast(transform.position, -transform.up + new Vector3(0, 0, -groundRayRadius), out groundHit, groundRayHeight, LayerMask.GetMask("Terrain")))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, groundHit.normal);
            if (slopeAngle > characterController.slopeLimit)
            {
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, groundHit.normal).normalized;
                characterController.SimpleMove(slideDirection * 10f);

                return ACTOR_GROUND_STATE.SLOPE;
            }

            return ACTOR_GROUND_STATE.GROUND;
        }
        return ACTOR_GROUND_STATE.AIR;
    }

    #region Property
    public CharacterController CharacterController { get { return characterController; } }
    public StateController State { get { return state; } }
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    #endregion
}
