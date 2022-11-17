using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class QuickSlot : BaseSlot
{

    public override void Initialize()
    {
        base.Initialize();
    }

    #region Register & Release Function
    public void RegisterItem<T>(T consumptionItem) where T : ConsumptionItem
    {
        // 빈 슬롯일 경우에 최초 등록
        if (item == null)
        {
            AddItemToSlot(consumptionItem);
        }
        // 이후 숫자만 증가
        else
        {
            item.ItemCount += consumptionItem.ItemCount;
        }
    }
    public void RegisterQuickSlot<T>(T consumptionItem) where T : ConsumptionItem
    {
        if (consumptionItem != null)
        {
            // 인벤토리를 순회하며 같은 종류의 아이템을 탐색후 등록
            for(int i=0; i< inventoryDatas.Length; ++i)
            {
                if (inventoryDatas[i].ItemID == consumptionItem.ItemID)
                {
                    RegisterItem(consumptionItem);
                }
            }
        }
    }
    public void ReleaseQuickSlot()
    {
        ClearSlot();
    }
    #endregion

    public void ConsumeItem()
    {
        if(item != null)
        {
            for (int i = 0; i < inventoryDatas.Length; ++i)
            {
                if (inventoryDatas[i].ItemID == item.ItemID)
                {
                    ConsumptionItem consumptionItem = inventoryDatas[i] as ConsumptionItem;
                    if(consumptionItem != null)
                    {
                        consumptionItem.Consume(Managers.DataManager.CurrentCharacter);
                        --item.ItemCount;

                        --inventoryDatas[i].ItemCount;
                        if(inventoryDatas[i].ItemCount <= 0)
                        {
                            inventoryDatas[i] = null;
                        }

                        return;
                    }
                }
            }
        }
    }

    #region Mouse Event Function
    public void DropQuickSlot<T>() where T : ConsumptionItem
    {
        T potionItem = Managers.SlotManager.DragSlot.Item as T;
        if (potionItem != null)
        {
            ReleaseQuickSlot();
            RegisterQuickSlot(Managers.SlotManager.DragSlot.Item as T);
        }
    }
    public void EndDragQuickSlot<T>() where T : ConsumptionItem
    {
        T potionItem = Managers.SlotManager.DragSlot.Item as T;
        if (potionItem != null)
        {
            ReleaseQuickSlot();
            RegisterQuickSlot(Managers.SlotManager.TargetSlot.Item as T);
        }
        else if (Managers.SlotManager.TargetSlot == null)
        {
            ReleaseQuickSlot();
        }
    }

    public override void Drop()
    {
        DropQuickSlot<ConsumptionItem>();
    }

    public override void EndDrag()
    {
        EndDragQuickSlot<ConsumptionItem>();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
        ConsumeItem();
    }
    #endregion
}
