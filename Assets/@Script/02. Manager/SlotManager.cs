using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotManager
{
    private BaseSlot startSlot;
    private BaseSlot endSlot;
    private Image dragImage;

    public void Initialize(GameObject rootObject)
    {
        startSlot = null;
        endSlot = null;

        dragImage = Functions.FindChild<Image>(rootObject, "DragImage", true);
        dragImage.raycastTarget = false;
        DisableDragImage();
    }

    public void EnableDragImage(Sprite itemSprite)
    {
        dragImage.sprite = itemSprite;
        dragImage.color = Functions.SetColor(dragImage.color, 0.7f);
        dragImage.gameObject.SetActive(true);
    }
    public void DisableDragImage()
    {
        dragImage.sprite = null;
        dragImage.color = Functions.SetColor(dragImage.color, 0f);
        dragImage.gameObject.SetActive(false);
    }

    public void OnBeginDrag<T>(T slot) where T: BaseSlot
    {
        startSlot = slot;
        endSlot = null;

        if(startSlot.ItemImage.sprite != null)
        {
            EnableDragImage(startSlot.ItemImage.sprite);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        dragImage.rectTransform.position = eventData.position;
    }
    public void OnDrop<T>(T slot) where T : BaseSlot
    {
        endSlot = slot;
    }
    public void OnEndDrag()
    {
        if (startSlot == endSlot)
            return;

        if (endSlot == null)
        {
            DestoryItem();
        }

        if (startSlot is InventorySlot startInventorySlot)
            InteractInventory(startInventorySlot);

        else if (startSlot is QuickSlot startQuickSlot)
            InteractQuickSlot(startQuickSlot);

        else if (startSlot is EquipmentSlot<EquipmentItem>)
            InteractEquipmentSlot();

        startSlot = null;
        endSlot = null;
        DisableDragImage();
    }
    public void DestoryItem() { }
    public void InteractInventory(InventorySlot startInventorySlot)
    {
        if (endSlot is InventorySlot endInventorySlot)
        {
            Managers.DataManager.SelectCharacterData.InventoryData.SwapOrCombineItem(startInventorySlot, endInventorySlot);
        }
        else if (endSlot is QuickSlot endQuickSlot)
        {
            Managers.DataManager.SelectCharacterData.InventoryData.RegisterQuickSlot(endQuickSlot.SlotIndex, startInventorySlot.Item.ItemID);
        }
        else if (endSlot is WeaponSlot endWeaponSlot)
        {
            if (startInventorySlot.Item is WeaponItem weaponItem)
            {
                Managers.DataManager.SelectCharacterData.EquipmentSlotData.EquipWeapon(weaponItem);
                if(endWeaponSlot.Item != null)
                {
                    Managers.DataManager.SelectCharacterData.InventoryData.AddItemToSlotIndex(endWeaponSlot.Item, startInventorySlot.SlotIndex);
                    Managers.DataManager.SelectCharacterData.EquipmentSlotData.UnEquipItem();
                }
                else
                {
                    Managers.DataManager.SelectCharacterData.InventoryData.RemoveItemByIndex(startInventorySlot.SlotIndex);
                }
            }
        }
    }
    public void InteractQuickSlot(QuickSlot startInventorySlot)
    {
        if (endSlot is QuickSlot)
        {
            Managers.DataManager.SelectCharacterData.InventoryData.SwapQuickSlot(startSlot.SlotIndex, endSlot.SlotIndex);
        }
        else
        {
            Managers.DataManager.SelectCharacterData.InventoryData.ReleaseQuickSlot(startInventorySlot.SlotIndex);
        }
    }
    public void InteractEquipmentSlot()
    {
    }
    #region Property
    public BaseSlot StartSlot { get { return startSlot; } }
    public BaseSlot EndSlot { get { return endSlot; } }
    #endregion
}
