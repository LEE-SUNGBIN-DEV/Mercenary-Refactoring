using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponItem : EquipmentItem
{
    [Header("Weapon Item")]
    [SerializeField] private float increasedAmount;
    [SerializeField] private float finalIncereasedAmount;
    public override void Initialize(ItemData itemData)
    {
        base.Initialize(itemData);
        increasedAmount = (Managers.DataManager.ItemTable[itemData.itemID] as WeaponItem).increasedAmount;
    }
    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is WeaponItem targetItem)
        {
            increasedAmount = targetItem.increasedAmount;
        }
    }

    public override void Equip(PlayerStatusData status)
    {
        finalIncereasedAmount = increasedAmount;
        for(int i=0; i<grade; ++i)
        {
            finalIncereasedAmount *= 1.1f;
        }
        status.EquipAttackPower += finalIncereasedAmount;
    }

    public override void UnEquip(PlayerStatusData status)
    {
        status.EquipAttackPower -= finalIncereasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
