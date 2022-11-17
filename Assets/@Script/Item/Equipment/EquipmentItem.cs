using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class EquipmentItem : BaseItem
{
    [Header("Equipment Item")]
    protected int grade;

    public virtual void Equip(Character character)
    {
        Managers.AudioManager.PlaySFX("Audio_Equipment_Mount");
    }
    public virtual void Release(Character character)
    {
        Managers.AudioManager.PlaySFX("Audio_Equipment_Dismount");
    }

    public int Grade { get { return grade; } set { grade = value; } }
}
