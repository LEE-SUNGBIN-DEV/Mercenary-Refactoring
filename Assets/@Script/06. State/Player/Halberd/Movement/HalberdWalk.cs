using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdWalk : IActionState
{
    private PlayerCharacter character;
    private int stateWeight;
    private int animationNameHash;
    private float walkSpeed;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public HalberdWalk(PlayerCharacter character)
    {
        this.character = character;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_WALK;
        animationNameHash = Constants.ANIMATION_NAME_HASH_HALBERD_WALK;
    }

    public void Enter()
    {
        character.Animator.CrossFadeInFixedTime(animationNameHash, 0.1f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && character.TryEquipWeapon(WEAPON_TYPE.SWORD_SHIELD))
        {
            character.State.SetState(ACTION_STATE.PLAYER_SWORD_SHIELD_WALK, STATE_SWITCH_BY.FORCED);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.SetState(ACTION_STATE.PLAYER_ROLL, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_SKILL_COUNTER, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_ATTACK_LIGHT_01, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.State.SetState(ACTION_STATE.PLAYER_HALBERD_GUARD_IN, STATE_SWITCH_BY.WEIGHT);
            return;
        }

        switch (character.GetGroundState())
        {
            case ACTOR_GROUND_STATE.GROUND:
                character.CharacterData.StatusData.AutoRecoverStamina(Constants.PLAYER_STAMINA_WALK_AUTO_RECOVERY);

                moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

                verticalDirection.x = character.PlayerCamera.transform.forward.x;
                verticalDirection.z = character.PlayerCamera.transform.forward.z;

                horizontalDirection.x = character.PlayerCamera.transform.right.x;
                horizontalDirection.z = character.PlayerCamera.transform.right.z;

                moveDirection = (verticalDirection * moveInput.z + horizontalDirection * moveInput.x).normalized;

                if (moveDirection.sqrMagnitude > 0f)
                {
                    // Run
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        character.State.SetState(ACTION_STATE.PLAYER_HALBERD_RUN, STATE_SWITCH_BY.WEIGHT);
                    }
                    else
                    {
                        walkSpeed = character.Status.MoveSpeed;
                        // Look Direction
                        character.transform.rotation = Quaternion.Lerp(character.transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);
                        character.CharacterController.SimpleMove(walkSpeed * moveDirection);
                    }
                }
                // Idle
                else
                {
                    character.State.SetState(ACTION_STATE.PLAYER_HALBERD_IDLE, STATE_SWITCH_BY.FORCED);
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