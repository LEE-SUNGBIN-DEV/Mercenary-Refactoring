using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : BaseSlot
{
    [SerializeField] private BaseItem item;

    public void Initialize(int i)
    {
        base.Initialize();
        slotIndex = i;
    }

    public void LoadSlot(ItemData itemData)
    {
        ClearSlot();
        if (itemData != null)
        {
            item = Managers.DataManager.ItemTable[itemData.itemID];
            itemCount = itemData.itemCount;

            if (item != null)
            {
                itemImage.sprite = item.ItemSprite;
                itemImage.color = Functions.SetColor(Color.white, 1f);
                if (item is CountItem)
                {
                    itemCount = itemData.itemCount;
                    EnableCountText(true);
                }
                else if (item is EquipmentItem)
                {
                    itemGrade = itemData.grade;
                    EnableGradeText(true);
                }
                else
                {
                    EnableGradeText(false);
                    EnableCountText(false);
                }
            }
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        item = null;
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
    }

    public BaseItem Item { get { return item; } }
}
