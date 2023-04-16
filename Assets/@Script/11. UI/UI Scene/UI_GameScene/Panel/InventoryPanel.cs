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

    public enum SLOT
    {
        Prefab_Slot_Weapon,
        Prefab_Slot_Helmet,
        Prefab_Slot_Armor,
        Prefab_Slot_Boots
    }

    private PlayerInventoryData inventory;
    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private WeaponSlot weaponSlot;
    [SerializeField] private HelmetSlot helmetSlot;
    [SerializeField] private ArmorSlot armorSlot;
    [SerializeField] private BootsSlot bootsSlot;

    public void Initialize(CharacterData characterData)
    {
        BindText(typeof(TEXT));

        inventorySlots = GetComponentsInChildren<InventorySlot>(true);
        for(int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i].Initialize(i);
        }

        inventory = characterData.InventoryData;
        inventory.OnChangeInventoryData -= RefreshInventory;
        inventory.OnChangeInventoryData += RefreshInventory;
        RefreshInventory(inventory);

        weaponSlot = GetComponentInChildren<WeaponSlot>(true);
        helmetSlot = GetComponentInChildren<HelmetSlot>(true);
        armorSlot = GetComponentInChildren<ArmorSlot>(true);
        bootsSlot = GetComponentInChildren<BootsSlot>(true);

        weaponSlot.Initialize(characterData.StatusData);
        helmetSlot.Initialize(characterData.StatusData);
        armorSlot.Initialize(characterData.StatusData);
        bootsSlot.Initialize(characterData.StatusData);

        characterData.EquipmentSlotData.OnChangeEquipmentSlotData -= RefreshEquipmentSlot;
        characterData.EquipmentSlotData.OnChangeEquipmentSlotData += RefreshEquipmentSlot;

        RefreshEquipmentSlot(characterData.EquipmentSlotData);
    }

    public void RefreshInventory(PlayerInventoryData inventoryData)
    {
        GetText((int)TEXT.Resonance_Stone_Amount_Text).text = inventoryData.Money.ToString();

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

    public void RefreshEquipmentSlot(PlayerEquipmentSlotData equipmentSlotData)
    {
        weaponSlot.LoadSlot(equipmentSlotData.WeaponSlotItemData);
        helmetSlot.LoadSlot(equipmentSlotData.HelmetSlotItemData);
        armorSlot.LoadSlot(equipmentSlotData.ArmorSlotItemData);
        bootsSlot.LoadSlot(equipmentSlotData.BootsSlotItemData);
    }

    #region Property
    public InventorySlot[] InventorySlots { get { return inventorySlots; } }

    public WeaponSlot WeaponSlot { get { return weaponSlot; } set { weaponSlot = value; } }
    public HelmetSlot HelmetSlot { get { return helmetSlot; } set { helmetSlot = value; } }
    public ArmorSlot ArmorSlot { get { return armorSlot; } set { armorSlot = value; } }
    public BootsSlot BootsSlot { get { return bootsSlot; } set { bootsSlot = value; } }
    #endregion
}
