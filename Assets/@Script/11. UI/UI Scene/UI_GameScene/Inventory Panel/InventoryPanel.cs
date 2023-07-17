using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InventoryPanel : UIPanel
{
    public enum TEXT
    {
        Resonance_Stone_Amount_Text
    }

    private CharacterInventoryData inventoryData;
    [SerializeField] private InventorySlot[] inventorySlots;

    [SerializeField] private InventoryResonanceSlot resonanceWaterSlot;
    [SerializeField] private InventoryHalberdSlot halberdSlot;
    [SerializeField] private InventorySwordShieldSlot swordShieldSlot;
    [SerializeField] private InventoryArmorSlot armorSlot;

    public void Initialize(CharacterData characterData)
    {
        inventoryData = characterData.InventoryData;

        BindText(typeof(TEXT));

        // Inventory
        inventorySlots = GetComponentsInChildren<InventorySlot>(true);
        for(int i = 0; i < inventorySlots.Length; ++i)
            inventorySlots[i].Initialize(i);

        // Resonance Water Slot
        resonanceWaterSlot = GetComponentInChildren<InventoryResonanceSlot>(true);
        resonanceWaterSlot.Initialize(inventoryData);

        // Equipment Information Slots
        halberdSlot = GetComponentInChildren<InventoryHalberdSlot>(true);
        swordShieldSlot = GetComponentInChildren<InventorySwordShieldSlot>(true);
        armorSlot = GetComponentInChildren<InventoryArmorSlot>(true);

        halberdSlot.Initialize(inventoryData);
        swordShieldSlot.Initialize(inventoryData);
        armorSlot.Initialize(inventoryData);
    }

    private void OnEnable()
    {
        if(inventoryData != null)
        {
            inventoryData.OnChangeInventoryData += RefreshInventory;
            RefreshInventory(inventoryData);
        }
    }

    private void OnDisable()
    {
        if (inventoryData != null)
        {
            inventoryData.OnChangeInventoryData -= RefreshInventory;
        }
    }

    public void RefreshInventory(CharacterInventoryData inventoryData)
    {
        GetText((int)TEXT.Resonance_Stone_Amount_Text).text = inventoryData.ResonanceStone.ToString();

        for (int i=0; i< inventoryData.InventoryItems.Length; ++i)
        {
            inventorySlots[i].LoadSlot(inventoryData.InventoryItems[i]);
        }

        halberdSlot.LoadData(inventoryData);
        swordShieldSlot.LoadData(inventoryData);
        armorSlot.LoadData(inventoryData);
        resonanceWaterSlot.LoadData(inventoryData);
    }

    public void ClearInventory()
    {
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i].ClearSlot();
        }
    }

    #region Property
    public InventorySlot[] InventorySlots { get { return inventorySlots; } }

    public InventoryHalberdSlot HalberdSlot { get { return halberdSlot; } set { halberdSlot = value; } }
    public InventorySwordShieldSlot SwordShieldSlot { get { return swordShieldSlot; } set { swordShieldSlot = value; } }
    public InventoryArmorSlot ArmorSlot { get { return armorSlot; } set { armorSlot = value; } }
    #endregion
}
