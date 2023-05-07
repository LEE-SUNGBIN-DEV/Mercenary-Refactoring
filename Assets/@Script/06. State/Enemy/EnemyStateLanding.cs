using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateLanding : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

    public EnemyStateLanding(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_LANDING;
        animationClipInformation = enemy.AnimationClipTable[Constants.ANIMATION_NAME_LANDING];
    }

    public void Enter()
    {
        enemy.Animator.Play(animationClipInformation.nameHash);
        enemy.Status.CurrentHP -= enemy.Status.MaxHP * enemy.MoveController.GetFallDamage();
    }

    public void Update()
    {
        // -> Idle
        if (enemy.State.SetStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.ENEMY_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
