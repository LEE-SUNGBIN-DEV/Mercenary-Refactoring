using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public abstract class EquipmentSlot<T> : BaseSlot where T : EquipmentItem, new()
{
    protected StatusData status;
    [SerializeField] protected T item;

    public virtual void Initialize(StatusData _status)
    {
        base.Initialize();
        status = _status;
    }

    public void LoadSlot(ItemData itemData)
    {
        UnEquipItem();
        if (itemData != null)
        {
            item = new T();
            item.Initialize(itemData);
            itemGrade = item.Grade;
            itemImage.sprite = item.ItemSprite;
            itemImage.color = Functions.SetColor(Color.white, 1f);
            EnableCountText(false);
            EnableGradeText(true);

            EquipItem(item);
        }
    }
    public override void ClearSlot()
    {
        base.ClearSlot();
        item = null;
    }
    public void EquipItem(T requestItem)
    {
        if(requestItem is T equipItem)
        {
            equipItem.Equip(status);
        }
    }

    public void UnEquipItem()
    {
        if (item != null)
        {
            item.UnEquip(status);
            ClearSlot();
        }
    }

    #region Mouse Event Function
    public override void SlotRightClick(PointerEventData eventData)
    {
    }
    #endregion

    public T Item { get { return item; } }
}
