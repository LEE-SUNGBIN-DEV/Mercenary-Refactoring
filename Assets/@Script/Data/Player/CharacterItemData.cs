using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class CharacterItemData
{
    public event UnityAction<CharacterItemData> OnChangeItemData;

    [Header("Item")]
    [SerializeField] private int money;
    [SerializeField] private List<ItemData> inventoryItemList;
    [SerializeField] private List<int> quickSlots;
    [SerializeField] private ItemData weaponSlotItem;
    [SerializeField] private ItemData helmetSlotItem;
    [SerializeField] private ItemData armorSlotItem;
    [SerializeField] private ItemData bootsSlotItem;

    public CharacterItemData()
    {
        CreateItemData();
    }

    public void CreateItemData()
    {
        money = Constants.CHARACTER_DATA_DEFAULT_MONEY;
        inventoryItemList = new List<ItemData>();
        quickSlots = new List<int>();
        weaponSlotItem = null;
        helmetSlotItem = null;
        armorSlotItem = null;
        bootsSlotItem = null;
    }



    #region Property
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            if (money < 0)
            {
                money = 0;
            }
            OnChangeItemData?.Invoke(this);
        }
    }
    public List<ItemData> InventoryItemList
    {
        get { return inventoryItemList; }
        set
        {
            inventoryItemList = value;
            OnChangeItemData?.Invoke(this);
        }
    }
    public List<int> QuickSlots
    {
        get { return quickSlots; }
        set
        {
            quickSlots = value;
            OnChangeItemData?.Invoke(this);
        }
    }
    public ItemData WeaponSlotItem
    {
        get { return weaponSlotItem; }
        set
        {
            weaponSlotItem = value;
            OnChangeItemData?.Invoke(this);
        }
    }
    public ItemData HelemetSlotItem
    {
        get { return helmetSlotItem; }
        set
        {
            helmetSlotItem = value;
            OnChangeItemData?.Invoke(this);
        }
    }
    public ItemData ArmorSlotItem
    {
        get { return armorSlotItem; }
        set
        {
            armorSlotItem = value;
            OnChangeItemData?.Invoke(this);
        }
    }
    public ItemData BootsSlotItem
    {
        get { return bootsSlotItem; }
        set
        {
            bootsSlotItem = value;
            OnChangeItemData?.Invoke(this);
        }
    }
    #endregion
}
