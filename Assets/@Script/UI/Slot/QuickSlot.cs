using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class QuickSlot : BaseSlot
{
    [SerializeField] private IUsableItem item;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void LoadSlot(ItemData itemSaveData)
    {
        if (itemSaveData != null)
        {
            BaseItem loadItem = Managers.DataManager.ItemTable[itemSaveData.itemID];
            if(loadItem is IUsableItem usableItem)
            {
                item = usableItem;
                SetSlotByItem(loadItem);
            }
        }
    }

    public override void SetSlotByItem<T>(T targetItem)
    {
        base.SetSlotByItem(targetItem);
        if(targetItem is CountItem countItem)
        {
            if (countItem.ItemCount > 0)
            {
                itemImage.color = Color.white;
            }
            else
            {
                itemImage.color = Color.gray;
            }
        }
    }

    public void UseItem()
    {
    }

    #region Mouse Event Function
    public void DropQuickSlot<T>() where T : CountItem
    {
        
    }
    public void EndDragQuickSlot<T>() where T : CountItem
    {
        
    }

    public override void Drop()
    {
        DropQuickSlot<CountItem>();
    }

    public override void EndDrag()
    {
        EndDragQuickSlot<CountItem>();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
        UseItem();
    }
    #endregion

    public IUsableItem Item { get { return item; } }
}
