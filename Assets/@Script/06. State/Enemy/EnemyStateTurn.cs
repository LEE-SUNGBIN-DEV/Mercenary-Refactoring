using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateTurn : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInfo rightTurnAnimationClipInfo;
    private AnimationClipInfo leftTurnAnimationClipInfo;

    private float progress;
    private Quaternion startRotation;
    private Vector3 targetDirection;
    private bool isRight;

    public EnemyStateTurn(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_CHASE_TURN;
        rightTurnAnimationClipInfo = enemy.AnimationClipTable[Constants.ANIMATION_NAME_TURN_RIGHT];
        leftTurnAnimationClipInfo = enemy.AnimationClipTable[Constants.ANIMATION_NAME_TURN_LEFT];
    }

    public void Enter()
    {
        progress = 0f;
        startRotation = enemy.transform.rotation;
        targetDirection = enemy.TargetDirection;
        isRight = Vector3.Dot(enemy.transform.right, targetDirection) > 0;

        if(isRight)
            enemy.Animator.CrossFadeInFixedTime(rightTurnAnimationClipInfo.nameHash, 0.1f);
        else
            enemy.Animator.CrossFadeInFixedTime(leftTurnAnimationClipInfo.nameHash, 0.1f);

    }

    public void Update()
    {
        if (enemy.Animator.IsAnimationFrameUpTo(rightTurnAnimationClipInfo, rightTurnAnimationClipInfo.maxFrame)
            || enemy.Animator.IsAnimationFrameUpTo(leftTurnAnimationClipInfo, leftTurnAnimationClipInfo.maxFrame))
        {
            // -> Wait
            if (enemy.IsChaseCondition())
            {
                enemy.State.SetState(ACTION_STATE.ENEMY_CHASE_WAIT, STATE_SWITCH_BY.FORCED);
                return;
            }
            // -> Idle
            else
            {
                enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
                return;
            }
        }

        Mathf.Clamp01(progress += Time.deltaTime);
        enemy.transform.rotation = Quaternion.Slerp(startRotation, Quaternion.LookRotation(targetDirection), progress);
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
