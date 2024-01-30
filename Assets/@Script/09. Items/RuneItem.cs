using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RuneItem : BaseItem, IEquipableItem
{
    public RuneItem(string itemID) : base(itemID)
    {
    }

    public void Equip(CharacterStatusData statusData)
    {
        if (randomOptions.IsNullOrEmpty())
            return;

        for (int i = 0; i < randomOptions.Length; i++)
        {
            randomOptions[i].ApplyToStatus(statusData);
        }
    }

    public void UnEquip(CharacterStatusData statusData)
    {
        if (randomOptions.IsNullOrEmpty())
            return;

        for (int i = 0; i < randomOptions.Length; i++)
        {
            randomOptions[i].ReleaseFromStatus(statusData);
        }
    }
}
