using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HalberdItem : BaseItem, IUniqueEquipment
{
    public HalberdItem(string itemID) : base(itemID)
    {
    }
    public void Equip(CharacterStatusData statusData)
    {
        if (fixedOptions.IsNullOrEmpty())
            return;

        for (int i = 0; i < fixedOptions.Length; i++)
        {
            fixedOptions[i].ApplyToStatus(statusData);
        }
    }

    public void UnEquip(CharacterStatusData statusData)
    {
        if (fixedOptions.IsNullOrEmpty())
            return;

        for (int i = 0; i < fixedOptions.Length; i++)
        {
            fixedOptions[i].ReleaseFromStatus(statusData);
        }
    }
}
