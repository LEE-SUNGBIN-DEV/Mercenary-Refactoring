using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryPanel : UIPanel
{
    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>();

        Managers.DataManager.CurrentCharacter.CharacterData.OnChangeCharacterData += (CharacterData playerData) =>
        {
            MoneyText.text = playerData.Money.ToString();
        };
    }

    public void AddItemToInventory<T>(T item, int itemCount = 1) where T : BaseItem
    {
        // �ߺ� ������ ó��
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            if (inventorySlots[i].Item != null
                && item.ItemID == inventorySlots[i].Item.ItemID
                && item is not EquipmentItem)
            {
                inventorySlots[i].AddItemToSlot(item, inventorySlots[i].ItemCount + itemCount);
                return;
            }
        }

        // �� ���� ã��
        int slotIndex = FindEmptySlot();

        if (slotIndex != -1)
        {
            inventorySlots[slotIndex].AddItemToSlot(item, itemCount);
            return;
        }

        else
        {
            Managers.UIManager.RequestNotice("�κ��丮�� ���� á���ϴ�.");
            return;
        }

    }

    public void RemoveItem(BaseItem item, int itemCount = 1)
    {
        // �ߺ� ������ ó��
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            if (item.ItemID == inventorySlots[i].Item.ItemID)
            {
                inventorySlots[i].RemoveItemFromSlot(itemCount);
                return;
            }
        }

        Managers.UIManager.RequestNotice("�������� �������� �ʽ��ϴ�.");
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

    #region Save & Load
    public void LoadPlayerInventory(CharacterData characterData)
    {
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            if (characterData.ItemDataList[i].Item != null)
            {
                inventorySlots[i].AddItemToSlot(characterData.ItemDataList[i].Item, characterData.ItemDataList[i].ItemCount);
            }

            else
            {
                inventorySlots[i].ClearSlot();
            }
        }

        MoneyText.text = characterData.Money.ToString();
    }
    #endregion

    #region Property
    public InventorySlot[] InventorySlots
    {
        get { return inventorySlots; }
        set { inventorySlots = value; }
    }
    public TextMeshProUGUI MoneyText
    {
        get { return moneyText; }
        set { moneyText = value; }
    }
    #endregion
}
