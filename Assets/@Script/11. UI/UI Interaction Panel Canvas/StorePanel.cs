using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StorePanel : UIPanel
{
    [SerializeField] private List<BaseItem> sellList;
    [SerializeField] private StoreSlot[] storeSlots;

    protected override void Awake()
    {
        base.Awake();
        storeSlots = GetComponentsInChildren<StoreSlot>();

        for (int i = 0; i < sellList.Count; ++i)
        {
            storeSlots[i].Initialize(sellList[i]);
        }
    }

    public void ShowItem()
    {

    }

    public void OpenPanel()
    {
    }

    public void ClosePanel()
    {
    }
}
