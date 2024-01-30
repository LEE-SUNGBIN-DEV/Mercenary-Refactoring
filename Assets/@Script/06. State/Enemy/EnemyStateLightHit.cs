using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateLightHit : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;

    public EnemyStateLightHit(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_HIT_LIGHT;
        animationClipInfo = enemy.AnimationClipTable[Constants.ANIMATION_NAME_LIGHT_HIT];
    }

    public void Enter()
    {
        enemy.TryPlaySFXFromStringArray(enemy.LightHitAudioClipNames);
        enemy.Animator.Play(animationClipInfo.nameHash);
    }

    public void Update()
    {
        if (enemy.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.ENEMY_IDLE, 1.0f))
        {
            return;
        }
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
