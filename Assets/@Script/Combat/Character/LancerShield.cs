using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerShield : PlayerCombatController
{
    public override void Initialize(Character character)
    {
        base.Initialize(character);
        ratioDictionary = new Dictionary<COMBAT_TYPE, float>()
        {
            // Default
            {COMBAT_TYPE.PlayerDefense, 1f },
            {COMBAT_TYPE.PlayerParrying, 1f },

            // Smash Attack
            {COMBAT_TYPE.PlayerParryingAttack, 1f },
        };
    }
}
