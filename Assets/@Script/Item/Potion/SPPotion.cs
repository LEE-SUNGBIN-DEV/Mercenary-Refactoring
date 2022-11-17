using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SPPotion : ConsumptionItem
{
    [Header("SP Potion")]
    private float spRecoveryAmount;

    public override void Consume(Character character)
    {
        Managers.AudioManager.PlaySFX("Potion Consume");
        character.CharacterStats.CurrentStamina += spRecoveryAmount;
    }

    public float SPRecoveryAmount { get { return spRecoveryAmount; } }
}
