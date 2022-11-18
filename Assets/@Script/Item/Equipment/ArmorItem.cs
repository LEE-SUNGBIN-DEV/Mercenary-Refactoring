using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmorItem : EquipmentItem
{
    [Header("Armor Item")]
    private float increasedAmount;

    public override void Equip(Character character)
    {
        base.Equip(character);
        character.CharacterStatus.DefensivePower += increasedAmount;
    }

    public override void Release(Character character)
    {
        base.Release(character);
        character.CharacterStatus.DefensivePower -= increasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
