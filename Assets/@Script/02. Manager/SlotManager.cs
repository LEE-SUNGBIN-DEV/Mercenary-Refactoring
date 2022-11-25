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
        dragImage.color = Functions.SetColor(dragImage.color, 0.75f);
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
            EnableDragImage(startSlot.ItemImage.sprite);
    }
    public void OnDrag(PointerEventData eventData)
    {
        dragImage.rectTransform.position = eventData.position;
    }
    public void OnDrop<T>(T slot) where T : BaseSlot
    {
        endSlot = slot;
        DisableDragImage();
    }
    public void OnEndDrag()
    {
        if (startSlot == endSlot)
            return;

        startSlot = null;
        endSlot = null;
    }

    #region Property
    public BaseSlot StartSlot { get { return startSlot; } }
    public BaseSlot EndSlot { get { return endSlot; } }
    public InventoryData InventoryData { get { return Managers.DataManager?.SelectCharacterData?.InventoryData; } }
    public EquipmentSlotData EquipmentSlotData { get { return Managers.DataManager?.SelectCharacterData?.EquipmentSlotData; } }
    #endregion

    /*
    private UnityAction<BaseSlot, BaseSlot>[,] slotActionTable;
    public void Initialize(GameObject rootObject)
    {
        slotActionTable = new UnityAction<BaseSlot, BaseSlot>[(int)SLOT_TYPE.SIZE, (int)SLOT_TYPE.SIZE]
        {
         //             Inven,          Quick,              Weapon,             Helmet,             Armor,              Boots      
         /*Inven  { SwapInventory,    RegisterQuickSlot,  EquipWeapon,        EquipHelmet,        EquipArmor,         EquipBoots},
         /*Quick  { ReleaseQuickSlot, SwapQuickSlot,      ReleaseQuickSlot,   ReleaseQuickSlot,   ReleaseQuickSlot,   ReleaseQuickSlot},
         /*Weapon { UnEquipWeapon,    null,               null,               null,               null,               null},
         /*Helmet { UnEquipHelmet,    null,               null,               null,               null,               null},
         /*Armor  { UnEquipArmor,     null,               null,               null,               null,               null},
         /*Boots  { UnEquipBoots,     null,               null,               null,               null,               null}
    
        };
    }
    
    // Inventory Action
    public void SwapInventory(BaseSlot startSlot, BaseSlot endSlot)
    {
        InventoryData.SwapOrCombineSlotItem(startSlot as InventorySlot, endSlot as InventorySlot);
    }
    public void RegisterQuickSlot(BaseSlot startSlot, BaseSlot endSlot)
    {
        InventoryData.RegisterQuickSlot(endSlot.SlotIndex, (startSlot as InventorySlot).Item.ItemID);
    }
    public void EquipWeapon(BaseSlot startSlot, BaseSlot endSlot)
    {
        if (EndSlot is WeaponSlot && (startSlot as WeaponSlot).Item is WeaponItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipWeaponData(InventoryData.InventoryItems[startSlot.SlotIndex]), startSlot.SlotIndex);
    }
    public void EquipHelmet(BaseSlot startSlot, BaseSlot endSlot)
    {
        if (EndSlot is WeaponSlot && (startSlot as HelmetSlot).Item is HelmetItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipHelmetData(InventoryData.InventoryItems[startSlot.SlotIndex]), startSlot.SlotIndex);
    }
    public void EquipArmor(BaseSlot startSlot, BaseSlot endSlot)
    {
        if (EndSlot is WeaponSlot && (startSlot as ArmorSlot).Item is ArmorItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipArmorData(InventoryData.InventoryItems[startSlot.SlotIndex]), startSlot.SlotIndex);
    }
    public void EquipBoots(BaseSlot startSlot, BaseSlot endSlot)
    {
        if (EndSlot is WeaponSlot && (startSlot as BootsSlot).Item is BootsItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipWeaponData(InventoryData.InventoryItems[startSlot.SlotIndex]), startSlot.SlotIndex);
    }

    // Quick Slot Action
    public void ReleaseQuickSlot(BaseSlot startSlot, BaseSlot endSlot)
    {
        InventoryData.ReleaseQuickSlot(startSlot.SlotIndex);
    }
    public void SwapQuickSlot(BaseSlot startSlot, BaseSlot endSlot)
    {
        InventoryData.SwapQuickSlot(startSlot.SlotIndex, EndSlot.SlotIndex);
    }

    // Weapon Slot Action
    public void UnEquipWeapon(BaseSlot startSlot, BaseSlot endSlot)
    {
        if ((endSlot as InventorySlot).Item is WeaponItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipWeaponData(InventoryData.InventoryItems[EndSlot.SlotIndex]), EndSlot.SlotIndex);

        else if ((endSlot as InventorySlot).Item == null)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.UnEquipWeaponData(), EndSlot.SlotIndex);
    }
    // Helmet Slot Action
    public void UnEquipHelmet(BaseSlot startSlot, BaseSlot endSlot)
    {
        if ((endSlot as InventorySlot).Item is HelmetItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipHelmetData(InventoryData.InventoryItems[EndSlot.SlotIndex]), EndSlot.SlotIndex);

        else if ((endSlot as InventorySlot).Item == null)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.UnEquipHelmetData(), EndSlot.SlotIndex);
    }
    // Armor Slot Action
    public void UnEquipArmor(BaseSlot startSlot, BaseSlot endSlot)
    {
        if ((endSlot as InventorySlot).Item is ArmorItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipArmorData(InventoryData.InventoryItems[EndSlot.SlotIndex]), EndSlot.SlotIndex);

        else if ((endSlot as InventorySlot).Item == null)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.UnEquipArmorData(), EndSlot.SlotIndex);
    }
    // Boots Slot Action
    public void UnEquipBoots(BaseSlot startSlot, BaseSlot endSlot)
    {
        if ((endSlot as InventorySlot).Item is BootsItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipBootsData(InventoryData.InventoryItems[EndSlot.SlotIndex]), EndSlot.SlotIndex);

        else if ((endSlot as InventorySlot).Item == null)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.UnEquipBootsData(), EndSlot.SlotIndex);
    }
    */
}
