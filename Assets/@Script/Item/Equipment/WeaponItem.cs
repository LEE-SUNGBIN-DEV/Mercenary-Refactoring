using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponItem : EquipmentItem
{
    [Header("Weapon Item")]
    private float increasedAmount;

    public override void Equip(CharacterStatus status)
    {
        base.Equip(status);
        status.AttackPower += increasedAmount;
    }

    public override void Release(CharacterStatus status)
    {
        base.Release(status);
        status.AttackPower -= increasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
