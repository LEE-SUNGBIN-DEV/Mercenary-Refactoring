using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MATERIAL_TYPE
{
    Original = 0,
    Phase = 1,
    Dissolve = 2,
    Outline = 3,

    SIZE
}

public enum HIT_STATE
{
    Hittable,
    Guardable,
    Parryable,
    Invincible,
}

[RequireComponent(typeof(Animator))]
public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    protected CharacterController characterController;
    protected Animator animator;
    protected SkinnedMeshRenderer[] skinnedMeshRenderers;
    protected Dictionary<string, AnimationClipInformation> animationClipTable;
    protected SFXPlayer sfxPlayer;

    [Header("Controllers")]
    protected StateController state;
    [SerializeField] protected MaterialController materialController;
    [SerializeField] protected ObjectPooler objectPooler = new ();
    
    [SerializeField] protected HIT_STATE hitState;
    [SerializeField] protected bool isDie;

    protected virtual void Awake()
    {
        if (TryGetComponent(out characterController))
        {
            characterController.slopeLimit = Constants.ANGLE_SLOPE_LIMIT;
        }

        if (TryGetComponent(out animator))
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

        TryGetComponent(out sfxPlayer);

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

        StartCoroutine(materialController.CoDissolve(0f, 1f, 3f));
    }

    #region Property
    public CharacterController CharacterController { get { return characterController; } }
    public Animator Animator { get { return animator; } }
    public SFXPlayer SFXPlayer { get { return sfxPlayer; } }
    public StateController State { get { return state; } }
    public Dictionary<string, AnimationClipInformation> AnimationClipTable { get { return animationClipTable; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public HIT_STATE HitState { get { return hitState; } set { hitState = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    #endregion
}
