using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateLanding : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private int animationNameHash;

    public EnemyStateLanding(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_LANDING;
        animationNameHash = Constants.ANIMATION_NAME_HASH_LANDING;
    }

    public void Enter()
    {
        enemy.Animator.Play(animationNameHash);
    }

    public void Update()
    {
        // !! When animation is over
        if (enemy.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.ENEMY_IDLE, 0.9f))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
