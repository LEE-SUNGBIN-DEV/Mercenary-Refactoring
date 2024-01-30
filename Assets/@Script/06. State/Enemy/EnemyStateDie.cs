using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDie : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;

    public EnemyStateDie(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_DIE;
        animationClipInfo = enemy.AnimationClipTable[Constants.ANIMATION_NAME_DIE];
    }

    public void Enter()
    {
        enemy.TryPlaySFXFromStringArray(enemy.DieAudioClipNames);
        enemy.Animator.Play(animationClipInfo.nameHash);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
