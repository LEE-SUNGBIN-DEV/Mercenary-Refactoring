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
    [SerializeField] protected SkinnedMeshRenderer[] meshRenderers;
    [SerializeField] protected MaterialPropertyBlock propertyBlock;
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
            groundRayRadius = characterController.radius + 0.1f;
        }

        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);
        if (meshRenderers != null && propertyBlock == null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        state = new StateController(animator);
        objectPooler.Initialize(transform);
    }

    public IEnumerator CoWaitForDisapear(float time)
    {
        float disapearTime = 0f;

        while (disapearTime <= time)
        {
            disapearTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(CoStartDissolve());
    }

    public IEnumerator CoStartDissolve()
    {
        float dissolveAmount = 0f;
        float dissolveSpeed = 0.3f;

        while(dissolveAmount <= 1f)
        {
            dissolveAmount += dissolveSpeed * Time.deltaTime;
            propertyBlock.SetFloat("_DissolveAmount", dissolveAmount);
            for (int i = 0; i < meshRenderers.Length; ++i)
            {
                meshRenderers[i].SetPropertyBlock(propertyBlock);
            }
            yield return null;
        }
    }

    public float FallDamageProcess(float fallTime)
    {
        Debug.Log("Fall Time: " + fallTime);

        if (fallTime <= 1f)
        {
            return 0f;
        }

        else
        {
            return Mathf.Clamp01(0.4f * fallTime);
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
    public SkinnedMeshRenderer[] MeshRenderers { get { return meshRenderers; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    #endregion
}
