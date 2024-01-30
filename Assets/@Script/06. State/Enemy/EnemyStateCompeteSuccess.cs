using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCompeteSuccess : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;

    public EnemyStateCompeteSuccess(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_COMPETE_SUCCESS;
        animationClipInfo = enemy.AnimationClipTable["Compete_Success"];
    }

    public void Enter()
    {
        enemy.HitState = HIT_STATE.INVINCIBLE;
        enemy.Animator.Play(animationClipInfo.nameHash);
    }

    public void Update()
    {
        // -> Idle
        if (enemy.State.SetStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.ENEMY_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
        enemy.HitState = HIT_STATE.HITTABLE;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
