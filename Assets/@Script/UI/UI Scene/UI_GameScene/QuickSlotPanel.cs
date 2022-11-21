using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotPanel : UIPanel
{
    [SerializeField] private QuickSlot[] quickSlots;

    public void Initialize(Character character)
    {
        quickSlots = GetComponentsInChildren<QuickSlot>();
        character.CharacterData.InventoryData.OnChangeInventoryData -= LoadQuickSlot;
        character.CharacterData.InventoryData.OnChangeInventoryData += LoadQuickSlot;

        LoadQuickSlot(character.CharacterData.InventoryData);
    }

    private void Update()
    {
        if (Managers.SceneManagerCS.CurrentScene.SceneType == SCENE_TYPE.DUNGEON)
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
    }

    public void LoadQuickSlot(InventoryData inventoryData)
    {
        for (int i = 0; i < inventoryData.QuickSlotItemIDs.Length; ++i)
        {
            int itemCount = 0;
            for (int j = 0; i < inventoryData.InventoryItems.Length; ++j)
            {
                if (inventoryData.QuickSlotItemIDs[i] == inventoryData.InventoryItems[j].itemID)
                {
                    itemCount += inventoryData.InventoryItems[i].itemCount;
                }
            }

            if (inventoryData.QuickSlotItemIDs[i] != Constants.NULL_INT)
            {
                CountItem targetItem = Managers.DataManager.ItemTable[inventoryData.QuickSlotItemIDs[i]] as CountItem;
                targetItem.ItemCount = itemCount;
                quickSlots[i].SetSlotByItem(targetItem);
            }
        }
    }

    public void UseQuickSlotItem(int slotIndex)
    {
        if (quickSlots[slotIndex].Item != null)
        {
            quickSlots[slotIndex].UseItem();
        }
    }

    public QuickSlot[] QuickSlots { get { return quickSlots; } set { quickSlots = value; } }
}
