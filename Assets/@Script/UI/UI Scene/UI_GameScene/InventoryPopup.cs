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

    private InventoryData inventoryData;
    [SerializeField] private InventorySlot[] inventorySlots;

    public void Initialize(Character targetCharacter)
    {
        inventoryData = targetCharacter.CharacterData.InventoryData;
        BindText(typeof(TEXT));

        inventorySlots = GetComponentsInChildren<InventorySlot>(true);
        for(int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i].Initialize(i);
        }

        inventoryData.OnChangeInventoryData -= LoadInventory;
        inventoryData.OnChangeInventoryData += LoadInventory;
        LoadInventory(inventoryData);
    }

    public void LoadInventory(InventoryData inventoryData)
    {
        GetText((int)TEXT.MoneyText).text = inventoryData.Money.ToString();

        for (int i=0; i< inventoryData.InventoryItems.Length; ++i)
        {
            inventorySlots[i].ClearSlot();
            inventorySlots[i].LoadSlot(inventoryData.InventoryItems[i]);
        }
    }

    public void AddItem<T>(T item) where T : BaseItem
    {
        inventoryData.AddItem(item);
    }

    public void RemoveItem(int slotIndex, int itemCount = 1)
    {
        inventoryData.RemoveItem(slotIndex, itemCount);
    }

    public void DestroyItem(int slotIndex)
    {
        inventoryData.DestroyItem(slotIndex);
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
