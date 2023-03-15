using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSpawn : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private int animationNameHash;

    public EnemyStateSpawn(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_SPAWN;
        animationNameHash = Constants.ANIMATION_NAME_HASH_SPAWN;
    }

    public void Enter()
    {
        enemy.Animator.Play(animationNameHash);
        enemy.IsInvincible = true;
    }

    public void Update()
    {
        if (enemy.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.ENEMY_IDLE, 1.0f))
        {
            return;
        }
    }

    public void Exit()
    {
        enemy.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
