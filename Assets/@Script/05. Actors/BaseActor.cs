using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MATERIAL_TYPE
{
    ORIGINAL = 0,
    PHASE = 1,
    DISSOLVE = 2,
    OUTLINE = 3,

    SIZE
}

public enum HIT_STATE
{
    HITTABLE,
    GUARDABLE,
    PARRYABLE,
    INVINCIBLE,
}

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    protected Rigidbody actorRigidbody;
    protected CapsuleCollider capsuleCollider;
    protected Animator animator;
    protected SkinnedMeshRenderer[] skinnedMeshRenderers;
    protected Dictionary<string, AnimationClipInfo> animationClipTable;
    protected SFXPlayer sfxPlayer;

    [Header("Controllers")]
    protected StateController state;
    [SerializeField] protected MaterialController materialController;
    [SerializeField] protected ObjectPooler objectPooler = new ();

    [Header("Audio Source Strings")]
    protected string[] spawnAudioClipNames;
    protected string[] attackAudioClipNames;
    protected string[] lightHitAudioClipNames;
    protected string[] heavyHitAudioClipNames;
    protected string[] dieAudioClipNames;
    protected string[] footstepAudioClipNames;
    protected string[] staggerAudioClipNames;

    [SerializeField] protected HIT_STATE hitState;
    [SerializeField] protected bool isDie;

    protected virtual void Awake()
    {
        TryGetComponent(out sfxPlayer);
        TryGetComponent(out actorRigidbody);
        TryGetComponent(out capsuleCollider);

        if (TryGetComponent(out animator))
        {
            animationClipTable = new Dictionary<string, AnimationClipInfo>();
            for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; ++i)
            {
                animationClipTable.Add(
                    animator.runtimeAnimatorController.animationClips[i].name,
                    new AnimationClipInfo(
                        animator.runtimeAnimatorController.animationClips[i].name,
                        animator.runtimeAnimatorController.animationClips[i].length,
                        animator.runtimeAnimatorController.animationClips[i].frameRate));
            }
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

    public IEnumerator CoWaitForDisapear(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(materialController.CoDissolve(0f, 1f, 5f));
    }

    public void TryPlaySFXFromStringArray(string[] sfxArray)
    {
        if (sfxArray != null && sfxArray.Length > 0)
        {
            int randomIndex = Random.Range(0, sfxArray.Length);
            sfxPlayer.PlaySFX(sfxArray[randomIndex]);
        }
    }
    public void PlayFootStep()
    {
        if(footstepAudioClipNames != null && footstepAudioClipNames.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepAudioClipNames.Length);
            sfxPlayer.PlaySFX(footstepAudioClipNames[randomIndex]);
        }
    }

    #region Property
    public Rigidbody ActorRigidbody { get { return actorRigidbody; } }
    public CapsuleCollider CapsuleCollider { get { return capsuleCollider; } }
    public Animator Animator { get { return animator; } }
    public SFXPlayer SFXPlayer { get { return sfxPlayer; } }
    public StateController State { get { return state; } }
    public Dictionary<string, AnimationClipInfo> AnimationClipTable { get { return animationClipTable; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public HIT_STATE HitState { get { return hitState; } set { hitState = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }

    // Resources
    public string[] SpawnAudioClipNames { get { return spawnAudioClipNames; } }
    public string[] AttackAudioClipNames { get { return attackAudioClipNames; } }
    public string[] LightHitAudioClipNames { get { return lightHitAudioClipNames; } }
    public string[] HeavyHitAudioClipNames { get { return heavyHitAudioClipNames; } }
    public string[] DieAudioClipNames { get { return dieAudioClipNames; } }
    public string[] FootstepAudioClipNames { get { return footstepAudioClipNames; } }
    public string[] StaggerAudioClipNames { get { return staggerAudioClipNames; } }
    #endregion
}
