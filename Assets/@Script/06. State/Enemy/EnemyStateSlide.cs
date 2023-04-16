using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSlide : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private int animationNameHash;

    public EnemyStateSlide(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_SLIDE;
        animationNameHash = Constants.ANIMATION_NAME_HASH_SLIDE;
    }

    public void Enter()
    {
        enemy.Animator.Play(animationNameHash);
    }

    public void Update()
    {
        switch (enemy.MoveController.GetGroundState())
        {
            case ACTOR_GROUND_STATE.GROUND:
                enemy.Status.CurrentHP -= enemy.Status.MaxHP * enemy.MoveController.GetFallDamage();
                enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
                break;

            case ACTOR_GROUND_STATE.SLOPE:
                enemy.MoveController.SlideTime += Time.deltaTime;
                break;

            case ACTOR_GROUND_STATE.AIR:
                enemy.State.SetState(ACTION_STATE.ENEMY_FALL, STATE_SWITCH_BY.WEIGHT);
                break;
        }
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
