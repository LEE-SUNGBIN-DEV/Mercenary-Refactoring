using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CharacterPanel : UIPanel
{
    public enum DYNAMIC_BAR
    {
        HP_Dynamic_Bar,
        SP_Dynamic_Bar,
        EXP_Dynamic_Bar
    }
    public enum IMAGE
    {
        Equip_Weapon_Image,

        Response_Water_Image_Frame,
        Response_Water_Image
    }
    public enum TEXT
    {
        Equip_Weapon_Name_Text,
        Response_Water_Name_Text
    }

    public enum BUTTON
    {
        Prefab_Option_Button,
        Prefab_Quest_Button,
        Prefab_Skill_Button,
        Prefab_Inventory_Button,
        Prefab_Status_Button
    }

    private CharacterStatusData statusData;
    private CharacterInventoryData inventoryData;

    // HP, SP Dynamic Bar
    private DynamicBar hpDynamicBar;
    private DynamicBar spDynamicBar;
    private DynamicBar expDynamicBar;

    // Weapon, Resonance Water Slot
    private TextMeshProUGUI equipWeaponNameText;
    private Image equipWeaponImage;

    private TextMeshProUGUI responseWaterNameText;
    private Image responseWaterImageFrame;
    private Image responseWaterImage;

    #region Private
    private void OnEnable()
    {
        ConnectData();
    }
    private void OnDisable()
    {
        DisconnectData();
    }
    private void ConnectData()
    {
        statusData = Managers.DataManager.CurrentCharacterData.StatusData;
        inventoryData = Managers.DataManager.CurrentCharacterData.InventoryData;
        if (inventoryData != null)
        {
            inventoryData.OnChangeInventoryData -= UpdatePanelByInventoryData;
            inventoryData.OnChangeInventoryData += UpdatePanelByInventoryData;
            UpdatePanelByInventoryData(inventoryData);
        }
    }
    private void DisconnectData()
    {
        if (statusData != null)
        {
            statusData = null;
        }

        if (inventoryData != null)
        {
            inventoryData.OnChangeInventoryData -= UpdatePanelByInventoryData;
            inventoryData = null;
        }
    }
    private void Update()
    {
        if(statusData != null)
        {
            hpDynamicBar.UpdateBar(statusData.GetHPRatio());
            spDynamicBar.UpdateBar(statusData.GetSPRatio());
            expDynamicBar.UpdateBar(statusData.GetExpRatio());
        }
    }
    #endregion

    public void Initialize()
    {
        base.Awake();

        // UI Binding
        BindImage(typeof(IMAGE));
        BindText(typeof(TEXT));
        BindObject<DynamicBar>(typeof(DYNAMIC_BAR));

        // Dynamic Bar
        hpDynamicBar = GetObject<DynamicBar>((int)DYNAMIC_BAR.HP_Dynamic_Bar);
        hpDynamicBar.Initialize();
        spDynamicBar = GetObject<DynamicBar>((int)DYNAMIC_BAR.SP_Dynamic_Bar);
        spDynamicBar.Initialize();
        expDynamicBar = GetObject<DynamicBar>((int)DYNAMIC_BAR.EXP_Dynamic_Bar);
        expDynamicBar.Initialize();

        // Equip Weapon, Resonance Water
        responseWaterNameText = GetText((int)TEXT.Response_Water_Name_Text);
        responseWaterImage = GetImage((int)IMAGE.Response_Water_Image);
        responseWaterImage.sprite = Managers.ResourceManager.LoadResourceSync<Sprite>(Constants.SPRITE_THUMBNAIL_RESPONSE_WATER);
        responseWaterImageFrame = GetImage((int)IMAGE.Response_Water_Image_Frame);
        responseWaterImageFrame.sprite = Managers.ResourceManager.LoadResourceSync<Sprite>(Constants.SPRITE_THUMBNAIL_RESPONSE_WATER_FRAME);

        equipWeaponNameText = GetText((int)TEXT.Equip_Weapon_Name_Text);
        equipWeaponImage = GetImage((int)IMAGE.Equip_Weapon_Image);
    }


    public void UpdatePanelByInventoryData(CharacterInventoryData inventoryData)
    {
        responseWaterNameText.text = $"{inventoryData.ResponseWaterSlotItem.GetItemName()} ({inventoryData.ResponseWaterRemainingCount}/{inventoryData.ResponseWaterSlotItem.MaxCount})";
        responseWaterImage.fillAmount = inventoryData.GetRemainingResponseWaterRatio();

        switch (inventoryData.CurrentWeaponType)
        {
            case WEAPON_TYPE.HALBERD:
                equipWeaponImage.sprite = inventoryData.HalberdSlotItem.GetItemSprite();
                equipWeaponNameText.text = inventoryData.HalberdSlotItem.GetItemName();
                break;
            case WEAPON_TYPE.SWORD_SHIELD:
                equipWeaponImage.sprite = inventoryData.SwordShieldSlotItem.GetItemSprite();
                equipWeaponNameText.text = inventoryData.SwordShieldSlotItem.GetItemName();
                break;
        }
        equipWeaponImage.color = new Color32(255, 255, 255, 255);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
