using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InventoryPopup : UIPopup
{
    public enum TEXT
    {
        MoneyText
    }

    private InventoryData inventory;
    [SerializeField] private InventorySlot[] inventorySlots;

    public void Initialize(InventoryData inventoryData)
    {
        inventory = inventoryData;
        BindText(typeof(TEXT));

        inventorySlots = GetComponentsInChildren<InventorySlot>(true);
        for(int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i].Initialize(i);
        }

        inventory.OnChangeInventoryData -= LoadInventory;
        inventory.OnChangeInventoryData += LoadInventory;
        LoadInventory(inventory);
    }

    public void LoadInventory(InventoryData inventoryData)
    {
        GetText((int)TEXT.MoneyText).text = inventoryData.Money.ToString();

        for (int i=0; i< inventoryData.InventoryItems.Length; ++i)
        {
            inventorySlots[i].LoadSlot(inventoryData.InventoryItems[i]);
        }
    }

    public void ClearInventory()
    {
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i].ClearSlot();
        }
    }

    #region Property
    public InventorySlot[] InventorySlots
    {
        get { return inventorySlots; }
    }
    #endregion
}
