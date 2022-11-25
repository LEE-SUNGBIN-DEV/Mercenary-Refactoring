using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmorItem : EquipmentItem
{
    [Header("Armor Item")]
    private float increasedAmount;

    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is ArmorItem targetItem)
        {
            increasedAmount = targetItem.increasedAmount;
        }
    }

    public override void Equip(StatusData _status)
    {
        _status.DefensivePower += increasedAmount;
    }

    public override void UnEquip(StatusData _status)
    {
        _status.DefensivePower -= increasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
