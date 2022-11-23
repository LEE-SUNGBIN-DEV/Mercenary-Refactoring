using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public abstract class EquipmentSlot<T> : BaseSlot where T : EquipmentItem
{
    protected StatusData status;
    protected T item;

    public virtual void Initialize(StatusData _status)
    {
        base.Initialize();
        status = _status;
    }

    public void LoadSlot(ItemData itemData)
    {
        ClearSlot();
        if (itemData != null)
        {
            item = Managers.DataManager.ItemTable[itemData.itemID] as T;
            if (item != null)
            {
                itemImage.sprite = item.ItemSprite;
                itemImage.color = Functions.SetColor(Color.white, 1f);
                EnableCountText(false);
            }
            EquipItem(item);
        }
    }
    public override void ClearSlot()
    {
        base.ClearSlot();
        item = null;
    }
    public void EquipItem(T item)
    {
        if(item is T equipItem)
        {
            equipItem.Equip(status);
        }
        else
        {
            Debug.Log("¿Â¬¯ ΩΩ∑‘¿Ã ¥Ÿ∏®¥œ¥Ÿ.");
        }
    }

    public void UnEquipItem(T item)
    {
        if (item is T equipItem)
        {
            equipItem.UnEquip(status);
            ClearSlot();
        }
    }

    #region Mouse Event Function
    public override void SlotRightClick(PointerEventData eventData)
    {
        UnEquipItem(item);
    }
    #endregion

    public T Item { get { return item; } }
}
