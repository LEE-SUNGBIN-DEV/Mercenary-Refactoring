using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HelmetItem : EquipmentItem
{
    [Header("Helmet Item")]
    private float increasedAmount;

    public override void Equip(CharacterStatus status)
    {
        base.Equip(status);
        status.DefensivePower += increasedAmount;
    }

    public override void Release(CharacterStatus status)
    {
        base.Release(status);
        status.DefensivePower -= increasedAmount;
    }

    public float IncreasedAmount { get { return increasedAmount; } set { increasedAmount = value; } }
}
