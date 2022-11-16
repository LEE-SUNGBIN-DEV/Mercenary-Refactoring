using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class QuickSlot : BaseSlot
{
    [Header("Quick Slot")]
    private bool isRegister;

    public override void Initialize()
    {
        base.Initialize();
        isRegister = false;
    }

    public void RegisterItem(BaseItem item)
    {
        PotionItem potionItem = item as PotionItem;
        if (potionItem != null)
        {
            AddItemToSlot(potionItem);
            isRegister = true;
        }
        else
        {
            Debug.Log("소비 아이템이 아닙니다.");
        }
    }
    public void ReleaseItem()
    {
        PotionItem potionItem = item as PotionItem;
        if (potionItem != null)
        {
            ClearSlot();
            isRegister = false;
        }
    }

    public override void Drop()
    {
        throw new System.NotImplementedException();
    }

    public override void EndDrag()
    {
        throw new System.NotImplementedException();
    }

    public override void ClickSlot(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public bool IsRegister { get { return isRegister; } }
}
