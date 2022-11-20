using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HPPotion : ConsumptionItem
{
    [Header("HP Potion")]
    private float hpRecoveryAmount;

    public override void Consume(Character character)
    {
        Managers.AudioManager.PlaySFX("Potion Consume");
        character.Status.CurrentHitPoint += hpRecoveryAmount;
    }

    public float HPRecoveryAmount {  get { return hpRecoveryAmount; } }
}
