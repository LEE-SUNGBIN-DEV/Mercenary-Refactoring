using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HelmetItem : EquipmentItem
{
    [Header("Helmet Item")]
    private float increasedAmount;

    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is HelmetItem targetItem)
        {
            increasedAmount = targetItem.increasedAmount;
        }
    }

    public override void Equip(Character character)
    {
        base.Equip(character);
        character.Status.DefensivePower += increasedAmount;
    }

    public override void Disarm(Character character)
    {
        base.Disarm(character);
        character.Status.DefensivePower -= increasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
