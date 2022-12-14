using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotPanel : UIPanel
{
    private InventoryData inventoryData;
    [SerializeField] private QuickSlot[] quickSlots;

    public void Initialize(InventoryData inventoryData)
    {
        this.inventoryData = inventoryData;
        quickSlots = GetComponentsInChildren<QuickSlot>(true);
        for(int i=0; i<quickSlots.Length; ++i)
        {
            quickSlots[i].Initialize(i);
        }

        inventoryData.OnChangeInventoryData -= LoadQuickSlot;
        inventoryData.OnChangeInventoryData += LoadQuickSlot;

        LoadQuickSlot(inventoryData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseQuickSlotItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseQuickSlotItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseQuickSlotItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseQuickSlotItem(3);
        }
    }

    public void LoadQuickSlot(InventoryData inventoryData)
    {
        for (int i = 0; i < inventoryData.QuickSlotItemIDs.Length; ++i)
        {
            quickSlots[i].LoadSlot(inventoryData);
        }
    }

    public void UseQuickSlotItem(int slotIndex)
    {
        quickSlots[slotIndex].UseItem(inventoryData);
    }

    public QuickSlot[] QuickSlots { get { return quickSlots; } set { quickSlots = value; } }
}
