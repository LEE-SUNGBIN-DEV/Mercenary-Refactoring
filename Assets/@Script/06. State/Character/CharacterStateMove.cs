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
    }

    public void Update(BaseCharacter character)
    {
        verticalDirection.x = character.PlayerCamera.transform.forward.x;
        verticalDirection.z = character.PlayerCamera.transform.forward.z;

        horizontalDirection.x = character.PlayerCamera.transform.right.x;
        horizontalDirection.z = character.PlayerCamera.transform.right.z;

        moveDirection = (verticalDirection * Managers.InputManager.MoveInput.z + horizontalDirection * Managers.InputManager.MoveInput.x).normalized;

        isMove = (moveDirection.magnitude != 0f);

        // Move
        if (isMove)
        {
            // Run
            if (Managers.InputManager.IsLeftShiftKeyDown && character.StatusData.CurrentSP >= Constants.CHARACTER_STAMINA_CONSUMPTION_RUN)
            {
                character.StatusData.CurrentSP -= Constants.CHARACTER_STAMINA_CONSUMPTION_RUN * Time.deltaTime;
                character.CharacterController.Move((character.StatusData.MoveSpeed * 2) * Time.deltaTime * moveDirection);
                moveBlendTreeFloat = Mathf.Lerp(moveBlendTreeFloat, 2, 3f * Time.deltaTime);
            }

            // Walk
            else
            {
                character.CharacterController.Move(character.StatusData.MoveSpeed * Time.deltaTime * moveDirection);
                moveBlendTreeFloat = Mathf.Lerp(moveBlendTreeFloat, 1, 3f * Time.deltaTime);
            }

            // Character Look Direction
            character.transform.rotation = Quaternion.Lerp(character.transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);
        }

        // Stop
        else
            moveBlendTreeFloat = Mathf.Lerp(moveBlendTreeFloat, 0, 3f * Time.deltaTime);

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