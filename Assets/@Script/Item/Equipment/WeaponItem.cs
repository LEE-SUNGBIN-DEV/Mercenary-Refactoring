using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponItem : EquipmentItem
{
    [Header("Weapon Item")]
    private float increasedAmount;
    private float finalIncereasedAmount;

    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is WeaponItem targetItem)
        {
            increasedAmount = targetItem.increasedAmount;
        }
    }

    public override void Equip(StatusData _status)
    {
        finalIncereasedAmount = increasedAmount;
        for(int i=0; i<grade; ++i)
        {
            finalIncereasedAmount *= 1.1f;
        }
        _status.EquipAttackPower += finalIncereasedAmount;
    }

    public override void UnEquip(StatusData _status)
    {
        _status.EquipAttackPower -= finalIncereasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
