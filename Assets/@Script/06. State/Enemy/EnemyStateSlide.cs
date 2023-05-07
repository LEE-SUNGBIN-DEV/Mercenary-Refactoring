using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSlide : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

    public EnemyStateSlide(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_SLIDE;
        animationClipInformation = enemy.AnimationClipTable[Constants.ANIMATION_NAME_SLIDE];
    }

    public void Enter()
    {
        enemy.Animator.Play(animationClipInformation.nameHash);
    }

    public void Update()
    {
        switch (enemy.MoveController.GroundState)
        {
            case ACTOR_GROUND_STATE.GROUND:
                enemy.State.SetState(ACTION_STATE.ENEMY_LANDING, STATE_SWITCH_BY.FORCED);
                return;

            case ACTOR_GROUND_STATE.SLOPE:
                enemy.MoveController.SlideTime += Time.deltaTime;
                return;

            case ACTOR_GROUND_STATE.AIR:
                enemy.State.SetState(ACTION_STATE.ENEMY_FALL, STATE_SWITCH_BY.WEIGHT);
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
