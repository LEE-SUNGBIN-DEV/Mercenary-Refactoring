using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateUpperEmpty : IActionState
{
    private BaseActor actor;
    private int stateWeight;
    private int animationNameHash;
    private float layerWeight;
    private float decreaseSpeed;

    public CommonStateUpperEmpty(BaseActor actor)
    {
        this.actor = actor;
        stateWeight = (int)ACTION_STATE_WEIGHT.COMMON_UPPER_EMPTY;
        animationNameHash = Constants.ANIMATION_NAME_HASH_EMPTY;
        decreaseSpeed = 5f;
    }

    public void Enter()
    {
        layerWeight = 1f;
        actor.Animator.Play(animationNameHash, (int)ANIMATOR_LAYER.UPPER);
    }

    public void Update()
    {
        if(layerWeight > 0f)
        {
            layerWeight -= decreaseSpeed * Time.deltaTime;
            if (layerWeight < 0f)
                layerWeight = 0f;

            actor.Animator.SetLayerWeight((int)ANIMATOR_LAYER.UPPER, 0f);
        }
    }

    public void Exit()
    {
        actor.Animator.SetLayerWeight((int)ANIMATOR_LAYER.UPPER, 1f);
    }


    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
