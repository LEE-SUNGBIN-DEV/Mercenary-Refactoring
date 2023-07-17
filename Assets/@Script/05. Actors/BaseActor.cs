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
    protected Rigidbody actorRigidbody;
    protected CapsuleCollider capsuleCollider;
    protected Animator animator;
    protected SkinnedMeshRenderer[] skinnedMeshRenderers;
    protected Dictionary<string, AnimationClipInformation> animationClipTable;
    protected SFXPlayer sfxPlayer;

    [Header("Controllers")]
    protected StateController state;
    [SerializeField] protected MaterialController materialController;
    [SerializeField] protected ObjectPooler objectPooler = new ();

    [Header("Audio Souce Strings")]
    protected string[] spawnAudioClipNames;
    protected string[] attackAudioClipNames;
    protected string[] dieAudioClipNames;
    protected string[] footstepAudioClipNames;

    [SerializeField] protected HIT_STATE hitState;
    [SerializeField] protected bool isDie;

    protected void InitializeActor()
    {
        TryGetComponent(out sfxPlayer);
        TryGetComponent(out actorRigidbody);
        TryGetComponent(out capsuleCollider);

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

        StartCoroutine(materialController.CoDissolve(0f, 1f, 5f));
    }

    public void PlaySpawnSound()
    {
        if (spawnAudioClipNames.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnAudioClipNames.Length);
            sfxPlayer.PlaySFX(spawnAudioClipNames[randomIndex]);
        }
    }
    public void PlayAttackSound()
    {
        if (attackAudioClipNames.Length > 0)
        {
            int randomIndex = Random.Range(0, attackAudioClipNames.Length);
            sfxPlayer.PlaySFX(attackAudioClipNames[randomIndex]);
        }
    }
    public void PlayDieSound()
    {
        if (dieAudioClipNames.Length > 0)
        {
            int randomIndex = Random.Range(0, dieAudioClipNames.Length);
            sfxPlayer.PlaySFX(dieAudioClipNames[randomIndex]);
        }
    }
    public void PlayFootStep()
    {
        if(footstepAudioClipNames.Length > 0)
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
    public Dictionary<string, AnimationClipInformation> AnimationClipTable { get { return animationClipTable; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public HIT_STATE HitState { get { return hitState; } set { hitState = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    #endregion
}
