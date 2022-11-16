using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponItem : EquipmentItem
{
    [Header("Weapon Item")]
    private float increasedAmount;

    public override void Equip(Character character)
    {
        base.Equip(character);
        character.CharacterStats.AttackPower += increasedAmount;
    }

    public override void Release(Character character)
    {
        base.Release(character);
        character.CharacterStats.AttackPower -= increasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
