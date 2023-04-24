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

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    protected Animator animator;
    protected CharacterController characterController;
    protected StateController state;
    protected Dictionary<string, AnimationClipInformation> animationClipTable;
    [SerializeField] protected MoveController moveController;
    [SerializeField] protected MaterialController materialController;
    [SerializeField] protected SkinnedMeshRenderer[] skinnedMeshRenderers;
    [SerializeField] protected ObjectPooler objectPooler = new ObjectPooler();
    
    [SerializeField] protected bool isInvincible;
    [SerializeField] protected bool isDie;

    protected virtual void Awake()
    {
        if(TryGetComponent(out animator))
        {
            animationClipTable = new Dictionary<string, AnimationClipInformation>();
            for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; ++i)
            {
                animationClipTable.Add(
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
        if (skinnedMeshRenderers != null)
        {
            materialController = new MaterialController();
            materialController.Initialize(skinnedMeshRenderers);
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

        StartCoroutine(CoDissolve());
    }

    public IEnumerator CoDissolve()
    {
        materialController.ChangeMaterials(MATERIAL_TYPE.Dissolve);
        float dissolveAmount = 0f;
        float dissolveSpeed = 0.3f;

        while (dissolveAmount <= 1f)
        {
            dissolveAmount += dissolveSpeed * Time.deltaTime;
            materialController.PropertyBlock.SetFloat(Constants.SHADER_PROPERTY_HASH_DISSOLVE_AMOUNT, dissolveAmount);
            materialController.SetPropertyBlock();
            yield return null;
        }
    }

    #region Property
    public Animator Animator { get { return animator; } }
    public CharacterController CharacterController { get { return characterController; } }
    public StateController State { get { return state; } }
    public MoveController MoveController { get { return moveController; } }
    public Dictionary<string, AnimationClipInformation> AnimationClipTable { get { return animationClipTable; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    #endregion
}
