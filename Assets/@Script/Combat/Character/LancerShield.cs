using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerShield : PlayerCombatController
{
    public override void Initialize(BaseCharacter character)
    {
        base.Initialize(character);
        ratioDictionary = new Dictionary<PLAYER_ATTACK_TYPE, float>()
        {
            // Default
            {PLAYER_ATTACK_TYPE.PlayerDefense, 1f },
            {PLAYER_ATTACK_TYPE.PlayerParrying, 1f },

            // Smash Attack
            {PLAYER_ATTACK_TYPE.PlayerParryingAttack, 1f },
        };
    }
}
