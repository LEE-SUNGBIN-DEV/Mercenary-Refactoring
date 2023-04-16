using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdEquip : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private AnimationClipInformation animationClipInformation;

    public HalberdEquip(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_EQUIP;
        animationClipInformation = character.AnimationClipDictionary["Halberd_Equip"];
    }

    public void Enter()
    {
        character.Animator.CrossFadeInFixedTime(animationClipInformation.nameHash, 0.05f, (int)ANIMATOR_LAYER.UPPER);
        character.TryEquipWeapon(WEAPON_TYPE.HALBERD);
    }

    public void Update()
    {
        if (character.State.SetSubStateByAnimationTimeUpTo(animationClipInformation.nameHash, ACTION_STATE.COMMON_UPPER_EMPTY, 0.9f, (int)ANIMATOR_LAYER.UPPER))
        {
            return;
        }
    }

    public void Exit()
    {
        character.Animator.Play(Constants.ANIMATION_NAME_HASH_EMPTY, (int)ANIMATOR_LAYER.UPPER);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
