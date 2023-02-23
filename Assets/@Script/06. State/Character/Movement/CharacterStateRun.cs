using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateRun : IActionState<BaseCharacter>
{
    private int stateWeight;
    private int animationNameHash;
    private float runSpeed;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public CharacterStateRun()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_RUN;
        animationNameHash = Constants.ANIMATION_NAME_HASH_RUN;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.CrossFade(animationNameHash, 0.2f);
    }

    public void Update(BaseCharacter character)
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_ROLL))
        {
            character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_ROLL);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.Status.CheckStamina(Constants.PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER))
        {
            character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_SKILL_COUNTER);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_ATTACK_LIGHT_01);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_DEFENSE_START);
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
                    character.CharacterData.StatusData.CurrentSP -= (Constants.PLAYER_STAMINA_CONSUMPTION_RUN * Time.deltaTime);
                    runSpeed = character.Status.MoveSpeed * 2;
                    // Look Direction
                    character.transform.rotation = Quaternion.Lerp(character.transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);
                    character.CharacterController.SimpleMove(runSpeed * moveDirection);
                }
                else
                {
                    character.State.SetState(ACTION_STATE.PLAYER_WALK);
                    return;
                }
            }
            // Idle
            else
            {
                character.State.SetState(ACTION_STATE.PLAYER_IDLE);
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
