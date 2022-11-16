using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HelmetItem : EquipmentItem
{
    [Header("Helmet Item")]
    private float increasedAmount;

    public override void Equip(Character character)
    {
        base.Equip(character);
        character.CharacterStats.DefensivePower += increasedAmount;
    }

    public override void Release(Character character)
    {
        base.Release(character);
        character.CharacterStats.DefensivePower -= increasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
