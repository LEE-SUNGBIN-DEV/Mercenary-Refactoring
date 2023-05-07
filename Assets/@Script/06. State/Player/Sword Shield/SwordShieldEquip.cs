using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldEquip : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;

    private AnimationClipInformation animationClipInformation;

    public SwordShieldEquip(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_EQUIP;

        animationClipInformation = character.AnimationClipTable["Sword_Shield_Equip"];
    }

    public void Enter()
    {
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.05f, (int)ANIMATOR_LAYER.UPPER);
    }

    public void Update()
    {
        // -> Upper Empty
        if (character.State.SetSubStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.COMMON_UPPER_EMPTY, 0.9f, (int)ANIMATOR_LAYER.UPPER))
            return;
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
