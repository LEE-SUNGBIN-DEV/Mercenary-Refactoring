using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerWeapon : PlayerCombatController
{
    public override void Initialize(Character character)
    {
        base.Initialize(character);
        ratioDictionary = new Dictionary<COMBAT_TYPE, float>()
        {
            // Default
            {COMBAT_TYPE.PlayerCounterAttack, 1f },

            // Combo Attack
            {COMBAT_TYPE.PlayerComboAttack1, 1f },
            {COMBAT_TYPE.PlayerComboAttack2, 1.05f },
            {COMBAT_TYPE.PlayerComboAttack3, 1.1f },
            {COMBAT_TYPE.PlayerComboAttack4, 1.15f },

            // Smash Attack
            {COMBAT_TYPE.PlayerSmashAttack1, 1.5f },
            {COMBAT_TYPE.PlayerSmashAttack2, 1.8f },
            {COMBAT_TYPE.PlayerSmashAttack3, 2.16f },
            {COMBAT_TYPE.PlayerSmashAttack4, 1.5f },

            {COMBAT_TYPE.PlayerParryingAttack, 2f },
        };
    }
}
