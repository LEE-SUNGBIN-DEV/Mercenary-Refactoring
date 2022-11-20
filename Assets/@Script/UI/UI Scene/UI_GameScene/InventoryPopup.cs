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

    private Character character;
    [SerializeField] private InventorySlot[] inventorySlots;

    public void Initialize(Character targetCharacter)
    {
        character = targetCharacter;
        BindText(typeof(TEXT));

        inventorySlots = GetComponentsInChildren<InventorySlot>(true);
        for(int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i].Initialize(i);
        }

        character.CharacterData.InventoryData.OnChangeInventoryData -= LoadInventory;
        character.CharacterData.InventoryData.OnChangeInventoryData += LoadInventory;
        LoadInventory(character.CharacterData.InventoryData);
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

    public void AddItemToInventory<T>(T item) where T : BaseItem
    {
        character.CharacterData.InventoryData.AddItemToInventory(item);
    }

    public void RemoveItem(int slotIndex, int itemCount = 1)
    {
        character.CharacterData.InventoryData.RemoveItemFromInventory(slotIndex, itemCount);
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
