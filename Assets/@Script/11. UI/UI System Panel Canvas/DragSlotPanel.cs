using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragSlotPanel : UIBase
{
    public enum IMAGE
    {
        Drag_Image
    }

    [SerializeField] private Image dragImage;

    private BaseItemSlot fromSlot;
    private BaseItemSlot toSlot;

    public void Initialize()
    {
        BindImage(typeof(IMAGE));
        dragImage = GetImage((int)IMAGE.Drag_Image);
        dragImage.raycastTarget = false;
        DisableDragImage();

        fromSlot = null;
        toSlot = null;
    }

    private void Update()
    {
        if (Managers.InputManager.UIMouseLeftButton.WasReleasedThisFrame() && toSlot == null)
        {
            DisableDragImage();
        }
    }

    public void EnableDragImage(Sprite itemSprite)
    {
        dragImage.sprite = itemSprite;
        dragImage.color = new Color32(255, 255, 255, 192);
        gameObject.SetActive(true);
    }
    public void DisableDragImage()
    {
        dragImage.sprite = null;
        dragImage.color = new Color32(255, 255, 255, 0);
        gameObject.SetActive(false);
    }

    public void BeginDrag<T>(T slot) where T: BaseItemSlot
    {
        fromSlot = slot;
        toSlot = null;

        if(fromSlot.ItemImage.sprite != null)
            EnableDragImage(fromSlot.ItemImage.sprite);
    }
    public void Drag(PointerEventData eventData)
    {
        dragImage.rectTransform.position = eventData.position;
    }
    public void Drop<T>(T slot) where T : BaseItemSlot
    {
        toSlot = slot;
        DisableDragImage();
    }
    public void EndDrag<T>(T slot) where T : BaseItemSlot
    {
        if (fromSlot == toSlot)
            return;

        fromSlot = null;
        toSlot = null;
    }

    #region Property
    public BaseItemSlot FromSlot { get { return fromSlot; } }
    public BaseItemSlot ToSlot { get { return toSlot; } }
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
