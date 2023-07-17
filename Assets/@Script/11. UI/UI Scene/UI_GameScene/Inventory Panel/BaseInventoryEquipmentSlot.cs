using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BaseInventoryEquipmentSlot : UIBase
{
    public enum IMAGE
    {
        Equipment_Image
    }
    protected Image equipmentImage;
    protected TextMeshProUGUI equipmentNameText;

    public virtual void Initialize(CharacterInventoryData inventoryData)
    {
        BindImage(typeof(IMAGE));

        equipmentImage = GetImage((int)IMAGE.Equipment_Image);
    }

    public abstract void LoadData(CharacterInventoryData inventoryData);
}
