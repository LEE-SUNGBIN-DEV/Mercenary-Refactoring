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

    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private TextMeshProUGUI moneyText;

    public override void Initialize(UnityAction<UIPopup> action = null)
    {
        base.Initialize(action);

        BindText(typeof(TEXT));
        moneyText = GetText((int)TEXT.MoneyText);

        inventorySlots = GetComponentsInChildren<InventorySlot>(true);
        for(int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i].Initialize();
        }

        Managers.DataManager.CurrentCharacter.CharacterData.ItemData.OnChangeItemData -= RefreshInventory;
        Managers.DataManager.CurrentCharacter.CharacterData.ItemData.OnChangeItemData += RefreshInventory;
    }

    private void OnEnable()
    {
        RefreshInventory(Managers.DataManager.CurrentCharacter.CharacterData.ItemData);
    }

    public void RefreshInventory(CharacterItemData itemData)
    {
        Debug.Log(GetText((int)TEXT.MoneyText));
        Debug.Log(GetText((int)TEXT.MoneyText).text);

        GetText((int)TEXT.MoneyText).text = itemData.Money.ToString();

        ClearInventory();
        for (int i=0; i< itemData.InventoryItemList.Count; ++i)
        {
            LoadSlot(itemData.InventoryItemList[i]);
        }
    }

    public void LoadSlot(ItemData itemData)
    {
        switch (itemData.itemType)
        {
            case ITEM_TYPE.Normal:
                {
                    BaseItem item = Managers.DataManager.ItemTable[itemData.itemID];
                    inventorySlots[itemData.slotIndex].SetItemToSlot(item);

                    break;
                }
            case ITEM_TYPE.Equipment:
                {
                    EquipmentItem item = Managers.DataManager.ItemTable[itemData.itemID] as EquipmentItem;
                    item.Grade = itemData.grade;
                    inventorySlots[itemData.slotIndex].SetItemToSlot(item);

                    break;
                }
            case ITEM_TYPE.Consumption:
                {
                    ConsumptionItem item = Managers.DataManager.ItemTable[itemData.itemID] as ConsumptionItem;
                    inventorySlots[itemData.slotIndex].SetItemToSlot(item);

                    break;
                }
        }
    }

    public void AddItemToInventory<T>(T item) where T : BaseItem
    {
        List<ItemData> inventory = Managers.DataManager.CurrentCharacter.CharacterData.ItemData.InventoryItemList;

        // 중복 아이템 처리
        if (item.IsCountable)
        {
            for (int i = 0; i < inventory.Count; ++i)
            {
                if (item.ItemID == inventory[i].itemID)
                {
                    inventory[i].itemCount += item.ItemCount;
                    return;
                }
            }
        }
        // 빈 슬롯 찾기
        else
        {
            int slotIndex = FindEmptySlot();

            if (slotIndex != -1)
            {
                inventory.Add(new ItemData(item, slotIndex));
                return;
            }
            else
            {
                Managers.UIManager.RequestNotice("인벤토리가 가득 찼습니다.");
                return;
            }
        }
    }

    public void RemoveItem(BaseItem item, int itemCount = 1)
    {
        // 중복 아이템 처리
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            if (item.ItemID == inventorySlots[i].Item.ItemID)
            {
                inventorySlots[i].RemoveItemFromSlot(itemCount);
                return;
            }
        }

        Managers.UIManager.RequestNotice("아이템이 존재하지 않습니다.");
    }

    public int FindEmptySlot()
    {
        for(int i=0; i<inventorySlots.Length; ++i)
        {
            if(inventorySlots[i].Item == null)
            {
                return i;
            }
        }

        return -1;
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
        set { inventorySlots = value; }
    }
    #endregion
}
