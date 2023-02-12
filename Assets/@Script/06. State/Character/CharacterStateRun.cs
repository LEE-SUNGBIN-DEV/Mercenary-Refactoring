using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateRun : ICharacterState
{
    private int stateWeight;
    private float runSpeed;
    private Vector3 moveInput;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;

    public CharacterStateRun()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Run;
    }

    public void Enter(BaseCharacter character)
    {
        character.Animator.CrossFade(Constants.ANIMATION_NAME_RUN, 0.2f);
    }

    public void Update(BaseCharacter character)
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.StatusData.CheckStamina(Constants.CHARACTER_STAMINA_CONSUMPTION_ROLL))
        {
            character.TrySwitchState(CHARACTER_STATE.Roll);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && character.StatusData.CheckStamina(Constants.CHARACTER_STAMINA_CONSUMPTION_COUNTER))
        {
            character.TrySwitchState(CHARACTER_STATE.Skill);
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            character.TrySwitchState(CHARACTER_STATE.Combo_1);
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            character.TrySwitchState(CHARACTER_STATE.Defense);
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
                    character.CharacterData.StatusData.CurrentSP -= (Constants.CHARACTER_STAMINA_CONSUMPTION_RUN * 0.01f * Time.deltaTime);
                    runSpeed = character.StatusData.MoveSpeed * 2;
                    // Look Direction
                    character.transform.rotation = Quaternion.Lerp(character.transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);
                    character.CharacterController.SimpleMove(runSpeed * moveDirection);
                }
                else
                {
                    character.SetState(CHARACTER_STATE.Walk);
                    return;
                }
            }
            // Idle
            else
            {
                character.SetState(CHARACTER_STATE.Idle);
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
