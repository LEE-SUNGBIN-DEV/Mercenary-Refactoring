using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHeavyHit : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

    public EnemyStateHeavyHit(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_HIT_HEAVY;
        animationClipInformation = enemy.AnimationClipTable[Constants.ANIMATION_NAME_HEAVY_HIT];
    }

    public void Enter()
    {
        enemy.Animator.Play(animationClipInformation.nameHash);
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
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
