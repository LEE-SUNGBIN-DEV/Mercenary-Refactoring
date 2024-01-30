using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoledStateSpawn : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInfo animationClipInformation;

    private Coroutine spawnCoroutine;
    [SerializeField] private ParticleController roarVFX;

    public MoledStateSpawn(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_SPAWN;
        animationClipInformation = enemy.AnimationClipTable[Constants.ANIMATION_NAME_SPAWN];

        // VFX
        roarVFX = Functions.FindChild<ParticleController>(enemy.gameObject, "VFX_Roar", true);
        roarVFX.Initialize(PARTICLE_MODE.AUTO_DISABLE, 5f);
        roarVFX.gameObject.SetActive(false);
    }

    public void Enter()
    {
        enemy.Animator.Play(animationClipInformation.nameHash);
        enemy.HitState = HIT_STATE.INVINCIBLE;

        spawnCoroutine = enemy.StartCoroutine(CoSpawn());
    }

    public void Update()
    {
        if (enemy.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.ENEMY_IDLE, 1.0f))
        {
            return;
        }
    }

    public void Exit()
    {
        if(spawnCoroutine != null)
            enemy.StopCoroutine(spawnCoroutine);

        enemy.HitState = HIT_STATE.HITTABLE;
    }

    public IEnumerator CoSpawn()
    {
        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 30));
        enemy.TryPlaySFXFromStringArray(enemy.SpawnAudioClipNames);
        roarVFX.gameObject.SetActive(true);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 107));

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, animationClipInformation.maxFrame));
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
