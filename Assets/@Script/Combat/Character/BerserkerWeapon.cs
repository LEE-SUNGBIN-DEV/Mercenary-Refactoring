using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerWeapon : PlayerCombatController
{
    public override void Initialize(BaseCharacter character)
    {
        base.Initialize(character);
        ratioDictionary = new Dictionary<PLAYER_ATTACK_TYPE, float>()
        {
            // Default
            {PLAYER_ATTACK_TYPE.PlayerCounterAttack, 1f },
            {PLAYER_ATTACK_TYPE.PlayerDefense, 1f },
            {PLAYER_ATTACK_TYPE.PlayerParrying, 1f },

            // Combo Attack
            {PLAYER_ATTACK_TYPE.PlayerComboAttack1, 1f },
            {PLAYER_ATTACK_TYPE.PlayerComboAttack2, 1.05f },
            {PLAYER_ATTACK_TYPE.PlayerComboAttack3, 1.1f },
            {PLAYER_ATTACK_TYPE.PlayerComboAttack4, 1.15f },

            // Smash Attack
            {PLAYER_ATTACK_TYPE.PlayerSmashAttack1, 1.5f },
            {PLAYER_ATTACK_TYPE.PlayerSmashAttack2, 1.8f },
            {PLAYER_ATTACK_TYPE.PlayerSmashAttack3, 2.16f },
            {PLAYER_ATTACK_TYPE.PlayerSmashAttack4, 2.6f },

            {PLAYER_ATTACK_TYPE.PlayerParryingAttack, 2f },
        };
    }
}
