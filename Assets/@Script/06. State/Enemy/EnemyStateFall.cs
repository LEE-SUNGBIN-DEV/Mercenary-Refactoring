using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFall : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private int animationNameHash;

    private float fallTime;

    public EnemyStateFall(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_FALL;
        animationNameHash = Constants.ANIMATION_NAME_HASH_FALL;

        fallTime = 0f;
    }

    public void Enter()
    {
        enemy.Animator.Play(animationNameHash);
        fallTime = 0f;
    }

    public void Update()
    {
        switch (enemy.MoveController.GetGroundState())
        {
            case ACTOR_GROUND_STATE.GROUND:
                enemy.Status.CurrentHP -= enemy.Status.MaxHP * enemy.MoveController.GetFallDamage();
                enemy.State.SetState(ACTION_STATE.ENEMY_LANDING, STATE_SWITCH_BY.WEIGHT);
                break;

            case ACTOR_GROUND_STATE.SLOPE:
                enemy.MoveController.SlideTime += Time.deltaTime;
                break;
            case ACTOR_GROUND_STATE.AIR:
                enemy.MoveController.FallTime += Time.deltaTime;
                break;
        }

        if (fallTime > 2.5f)
            enemy.Status.CurrentHP = 0f;
    }

    public void Exit()
    {
        fallTime = 0f;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
