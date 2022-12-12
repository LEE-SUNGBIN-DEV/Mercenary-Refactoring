using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePanel : UIPanel
{
    [SerializeField] private List<BaseItem> sellList;
    [SerializeField] private StoreSlot[] storeSlots;

    public void Initialize(BaseCharacter chracter)
    {
        storeSlots = GetComponentsInChildren<StoreSlot>();

        for (int i = 0; i < sellList.Count; ++i)
        {
            storeSlots[i].Initialize(sellList[i]);
        }
    }

    public void ShowItem()
    {

    }
}
