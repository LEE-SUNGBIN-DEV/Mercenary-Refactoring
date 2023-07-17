using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RuneItem : EquipmentItem
{
    [Header("Weapon Item")]
    [SerializeField] private float increasedAmount;
    [SerializeField] private float finalIncereasedAmount;
    public override void Initialize(ItemData itemData)
    {
        if (CheckItemTable(itemData))
        {
            base.Initialize(itemData);
            increasedAmount = (Managers.DataManager.ItemTable[itemData.itemID] as RuneItem).increasedAmount;
        }
    }
    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is RuneItem targetItem)
        {
            increasedAmount = targetItem.increasedAmount;
        }
    }

    public override void Equip(CharacterStatusData status)
    {
        finalIncereasedAmount = increasedAmount;
        for(int i=0; i<grade; ++i)
        {
            finalIncereasedAmount *= 1.1f;
        }
        status.WeaponAttackPower += finalIncereasedAmount;
    }

    public override void UnEquip(CharacterStatusData status)
    {
        status.WeaponAttackPower -= finalIncereasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
