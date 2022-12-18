using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateAttack : ICharacterState
{
    private int stateWeight;
    private bool isAttack;
    private Vector3 lookDirection;

    public CharacterStateAttack()
    {
        stateWeight = (int)CHARACTER_STATE_WEIGHT.Attack;
        isAttack = false;
    }

    public void Enter(BaseCharacter character)
    {
        isAttack = false;
    }

    public void Update(BaseCharacter character)
    {
        // Combo Attack
        if (character.PlayerInput.IsMouseLeftDown || character.PlayerInput.IsMouseLeftUp)
        {
            // 방향 전환
            lookDirection = character.PlayerCamera.transform.forward;
            lookDirection.y = 0f;
            character.transform.rotation = Quaternion.Lerp(character.transform.rotation, Quaternion.LookRotation(lookDirection), 10f * Time.deltaTime);

            isAttack = true;
            character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_COMBO_ATTACK, !character.PlayerInput.IsMouseLeftUp);
        }

        // Smash Attack
        if (isAttack)
        {
            if (character.PlayerInput.IsMouseRightDown || character.PlayerInput.IsMouseRightUp)
            {
                isAttack = true;
                character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_SMASH_ATTACK, !character.PlayerInput.IsMouseRightUp);
            }
        }
    }

    public void Exit(BaseCharacter character)
    {
        isAttack = false;
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_COMBO_ATTACK, false);
        character.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_SMASH_ATTACK, false);
    }

    #region Property
    public int StateWeight { get => stateWeight; }
    #endregion
}
