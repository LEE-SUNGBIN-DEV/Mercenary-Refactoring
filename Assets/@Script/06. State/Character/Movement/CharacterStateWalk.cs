using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateWalk : ICharacterState
{
    private int stateWeight;
    private int animationNameHash;
    private float walkSpeed;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public CharacterStateWalk()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Walk;
        animationNameHash = Constants.ANIMATION_NAME_HASH_WALK;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.CrossFade(animationNameHash, 0.2f);
    }

    public void Update(BaseCharacter character)
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Roll);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.StatusData.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Skill);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Light_Attack_01);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Defense);
            return;
        }

        // Move
        if (character.IsGround)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = 0;
            moveInput.z = Input.GetAxisRaw("Vertical");

            verticalDirection.x = character.PlayerCamera.transform.forward.x;
            verticalDirection.z = character.PlayerCamera.transform.forward.z;

            horizontalDirection.x = character.PlayerCamera.transform.right.x;
            horizontalDirection.z = character.PlayerCamera.transform.right.z;

            moveDirection = (verticalDirection * moveInput.z + horizontalDirection * moveInput.x).normalized;

            if (moveDirection.magnitude > 0f)
            {
                // Run
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    character.State.TryStateSwitchingByWeight(CHARACTER_STATE.Run);
                    return;
                }
                else
                {
                    character.CharacterData.StatusData.AutoRecoverStamina(Constants.PLAYER_STAMINA_WALK_AUTO_RECOVERY);
                    walkSpeed = character.StatusData.MoveSpeed;
                    // Look Direction
                    character.transform.rotation = Quaternion.Lerp(character.transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);
                    character.CharacterController.SimpleMove(walkSpeed * moveDirection);
                    return;
                }
            }
            // Idle
            else
            {
                character.State.SetState(CHARACTER_STATE.Idle);
                return;
            }
        }
        // Fall
        else
        {
            return;
        }
    }

    public void Exit(BaseCharacter character)
    {
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}