using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class EquipmentItem : BaseItem
{
    [Header("Equipment Item")]
    protected int grade;

    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is EquipmentItem targetItem)
        {
            grade = targetItem.grade;
        }
    }

    public virtual void Equip(StatusData _status)
    {
        Managers.AudioManager.PlaySFX("Audio_Equipment_Mount");
    }

    public virtual void UnEquip(StatusData _status)
    {
        Managers.AudioManager.PlaySFX("Audio_Equipment_Dismount");
    }

    public int Grade { get { return grade; } set { grade = value; } }
}
