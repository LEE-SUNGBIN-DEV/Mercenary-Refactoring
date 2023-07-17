using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSpawn : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

    public EnemyStateSpawn(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_SPAWN;
        animationClipInformation = enemy.AnimationClipTable[Constants.ANIMATION_NAME_SPAWN];
    }

    public void Enter()
    {
        enemy.PlaySpawnSound();
        enemy.Animator.Play(animationClipInformation.nameHash);
        enemy.HitState = HIT_STATE.Invincible;
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
        enemy.HitState = HIT_STATE.Hittable;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
