using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDrink : IActionState
{
    private int stateWeight;

    private AnimationClipInfo animationClipInfo;
    private PlayerCharacter character;
    private Coroutine drinkCoroutine;

    public PlayerStateDrink(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_DRINK;
        animationClipInfo = character.AnimationClipTable["Player_Drink"];
    }

    public void Enter()
    {
        character.UniqueEquipmentController.ShowResponseWater();
        drinkCoroutine = character.StartCoroutine(CoDrink());
        character.Animator.CrossFadeInFixedTime(animationClipInfo.nameHash, 0.05f, (int)ANIMATOR_LAYER.UPPER);
    }

    public void Update()
    {
        // -> Upper Empty
        if (character.State.SetSubStateByAnimationTimeUpTo(animationClipInfo.nameHash, ACTION_STATE.COMMON_UPPER_EMPTY, 0.9f, (int)ANIMATOR_LAYER.UPPER))
            return;
    }

    public void Exit()
    {
        if (drinkCoroutine != null)
            character.StopCoroutine(drinkCoroutine);

        character.UniqueEquipmentController.HideResponseWater();
    }

    private IEnumerator CoDrink()
    {
        yield return new WaitUntil(() => character.Animator.IsAnimationFrameUpTo(animationClipInfo, 26, (int)ANIMATOR_LAYER.UPPER) && !character.Animator.IsInTransition((int)ANIMATOR_LAYER.UPPER));
        character.InventoryData.ConsumeResponseWater(character.StatusData);
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
