using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MATERIAL_TYPE
{
    Original = 0,
    Phase = 1,
    Dissolve = 2,
    Outline = 3,

    SIZE
}

public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    protected Animator animator;
    protected CharacterController characterController;
    protected StateController state;
    protected Dictionary<string, AnimationClipInformation> animationClipDictionary;
    [SerializeField] protected MoveController moveController;
    [SerializeField] protected SkinnedMeshRenderer[] skinnedMeshRenderers;
    [SerializeField] protected Material[] originalMaterials;
    [SerializeField] protected MaterialPropertyBlock propertyBlock;
    [SerializeField] protected ObjectPooler objectPooler = new ObjectPooler();
    
    [SerializeField] protected bool isInvincible;
    [SerializeField] protected bool isDie;

    protected virtual void Awake()
    {
        if(TryGetComponent(out animator))
        {
            animationClipDictionary = new Dictionary<string, AnimationClipInformation>();
            for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; ++i)
            {
                animationClipDictionary.Add(
                    animator.runtimeAnimatorController.animationClips[i].name,
                    new AnimationClipInformation(
                        animator.runtimeAnimatorController.animationClips[i].name,
                        animator.runtimeAnimatorController.animationClips[i].length,
                        animator.runtimeAnimatorController.animationClips[i].frameRate));
            }
        }

        if (TryGetComponent(out characterController))
        {
            moveController = new MoveController(characterController);
        }

        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);
        if (skinnedMeshRenderers != null && propertyBlock == null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        originalMaterials = new Material[skinnedMeshRenderers.Length];
        for (int i = 0; i < skinnedMeshRenderers.Length; ++i)
        {
            originalMaterials[i] = skinnedMeshRenderers[i].material;
        }

        state = new StateController(animator);
        objectPooler.Initialize(transform);
    }

    public void ChangeAllMaterials(MATERIAL_TYPE targetMaterial)
    {
        for (int i = 0; i < skinnedMeshRenderers.Length; ++i)
        {
            Material resultMaterial = Managers.ResourceManager.LoadResourceSync<Material>(originalMaterials[i].name.Replace(" (Instance)", "_") + targetMaterial.GetEnumName());
            skinnedMeshRenderers[i].material = resultMaterial;
        }
    }

    public void SetOriginalMaterials()
    {
        for (int i = 0; i < skinnedMeshRenderers.Length; ++i)
        {
            skinnedMeshRenderers[i].material = originalMaterials[i];
        }
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
        ChangeAllMaterials(MATERIAL_TYPE.Dissolve);
        float dissolveAmount = 0f;
        float dissolveSpeed = 0.3f;

        while(dissolveAmount <= 1f)
        {
            dissolveAmount += dissolveSpeed * Time.deltaTime;
            propertyBlock.SetFloat(Constants.SHADER_PROPERTY_HASH_DISSOLVE_AMOUNT, dissolveAmount);
            for (int i = 0; i < skinnedMeshRenderers.Length; ++i)
            {
                skinnedMeshRenderers[i].SetPropertyBlock(propertyBlock);
            }
            yield return null;
        }
    }

    #region Property
    public Animator Animator { get { return animator; } }
    public CharacterController CharacterController { get { return characterController; } }
    public StateController State { get { return state; } }
    public MoveController MoveController { get { return moveController; } }
    public Dictionary<string, AnimationClipInformation> AnimationClipDictionary { get { return animationClipDictionary; } }
    public SkinnedMeshRenderer[] MeshRenderers { get { return skinnedMeshRenderers; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    #endregion
}
