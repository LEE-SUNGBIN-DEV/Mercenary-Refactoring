using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSpawn : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;

    public EnemyStateSpawn(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_SPAWN;
        animationClipInfo = enemy.AnimationClipTable[Constants.ANIMATION_NAME_SPAWN];
    }

    public void Enter()
    {
        enemy.TryPlaySFXFromStringArray(enemy.SpawnAudioClipNames);
        enemy.Animator.Play(animationClipInfo.nameHash);
        enemy.HitState = HIT_STATE.INVINCIBLE;
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
        enemy.HitState = HIT_STATE.HITTABLE;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
