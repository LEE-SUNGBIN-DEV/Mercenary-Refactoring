using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BootsItem : EquipmentItem
{
    [Header("Boots Item")]
    private float increasedAmount;

    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is BootsItem targetItem)
        {
            increasedAmount = targetItem.increasedAmount;
        }
    }

    public override void Equip(StatusData _status)
    {
        base.Equip(_status);
        _status.DefensivePower += increasedAmount;
    }

    public override void UnEquip(StatusData _status)
    {
        base.UnEquip(_status);
        _status.DefensivePower -= increasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
