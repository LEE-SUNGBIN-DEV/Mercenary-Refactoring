using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMove : ICharacterState
{
    private int stateWeight;
    private bool isMove;
    private Vector3 verticalDirection;
    private Vector3 horizontalDirection;
    private Vector3 moveDirection;
    private float moveBlendTreeFloat;

    public CharacterStateMove()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Move;
        isMove = false;
    }

    public void Enter(BaseCharacter character)
    {
        isMove = false;
        verticalDirection = Vector3.zero;
        horizontalDirection = Vector3.zero;
        moveDirection = Vector3.zero;
        moveBlendTreeFloat = 0;

        return;
    }
    public void Update(BaseCharacter character)
    {
        verticalDirection.x = character.PlayerCamera.transform.forward.x;
        verticalDirection.z = character.PlayerCamera.transform.forward.z;

        horizontalDirection.x = character.PlayerCamera.transform.right.x;
        horizontalDirection.z = character.PlayerCamera.transform.right.z;

        moveDirection = (verticalDirection * character.PlayerInput.MoveInput.z + horizontalDirection * character.PlayerInput.MoveInput.x).normalized;

        isMove = (moveDirection.magnitude != 0f);

        // Move
        if (isMove)
        {
            // Run
            if (character.PlayerInput.IsLeftShiftKeyDown && character.StatusData.CurrentSP >= Constants.CHARACTER_STAMINA_CONSUMPTION_RUN)
            {
                character.StatusData.CurrentSP -= Constants.CHARACTER_STAMINA_CONSUMPTION_RUN * Time.deltaTime;
                character.CharacterController.Move((character.StatusData.MoveSpeed * 2) * Time.deltaTime * moveDirection);
                moveBlendTreeFloat = Mathf.Lerp(moveBlendTreeFloat, 2, 5f * Time.deltaTime);
            }

            // Walk
            else
            {
                character.CharacterController.Move(character.StatusData.MoveSpeed * Time.deltaTime * moveDirection);
                moveBlendTreeFloat = Mathf.Lerp(moveBlendTreeFloat, 1, 5f * Time.deltaTime);
            }

            // Character Look Direction
            character.transform.rotation = Quaternion.Lerp(character.transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);
        }

        // Stop
        else
        {
            moveBlendTreeFloat = Mathf.Lerp(moveBlendTreeFloat, 0, 5f * Time.deltaTime);
        }

        character.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_MOVE, moveBlendTreeFloat);
    }
    public void Exit(BaseCharacter character)
    {
        isMove = false;
        moveBlendTreeFloat = 0f;
        character.Animator.SetFloat(Constants.ANIMATOR_PARAMETERS_FLOAT_MOVE, moveBlendTreeFloat);
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}