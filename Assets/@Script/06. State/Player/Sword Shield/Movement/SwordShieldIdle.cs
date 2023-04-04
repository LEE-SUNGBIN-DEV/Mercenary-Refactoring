using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldIdle : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;
    private Vector3 moveInput;

    public SwordShieldIdle(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_IDLE;
        animationNameHash = Constants.ANIMATION_NAME_HASH_SWORD_SHIELD_IDLE;
        moveInput = Vector3.zero;
    }

    public void Enter()
    {
        moveInput = Vector3.zero;
        character.Animator.CrossFadeInFixedTime(animationNameHash, 0.1f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && character.TryEquipWeapon(WEAPON_TYPE.HALBERD))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_IDLE, STATE_SWITCH_BY.FORCED);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_ATTACK_LIGHT_01, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_GUARD_IN, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        switch (character.GetGroundState())
        {
            case ACTOR_GROUND_STATE.GROUND:
                character.CharacterData.StatusData.AutoRecoverStamina(Constants.PLAYER_STAMINA_IDLE_AUTO_RECOVERY);
                moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

                if (moveInput.sqrMagnitude > 0)
                {
                    // -> Run
                    if (Input.GetKey(KeyCode.LeftShift))
                        character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_RUN, STATE_SWITCH_BY.WEIGHT);

                    // -> Walk
                    else
                        character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_WALK, STATE_SWITCH_BY.WEIGHT);
                }
                return;

            case ACTOR_GROUND_STATE.SLOPE:
                return;

            case ACTOR_GROUND_STATE.AIR: // -> Fall
                character.State.SetState(ACTION_STATE.PLAYER_FALL, STATE_SWITCH_BY.WEIGHT);
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
