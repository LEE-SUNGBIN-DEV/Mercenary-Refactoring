using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserPanel : UIPanel
{
    public enum IMAGE
    {
        HP_Bar,
        SP_Bar,
        Exp_Bar,

        Equip_Weapon_Image,
        Potion_Image
    }

    public enum BUTTON
    {
        Prefab_Option_Button,
        Prefab_Quest_Button,
        Prefab_Skill_Button,
        Prefab_Inventory_Button,
        Prefab_Status_Button
    }

    private CharacterData characterData;
    private QuickSlotPanel quickSlotPanel;
    private Image hpBar;
    private Image spBar;
    private Image expBar;
    private Image equipWeaponImage;
    private Image potionImage;

    public void Initialize(CharacterData characterData)
    {
        this.characterData = characterData;

        BindImage(typeof(IMAGE));
        hpBar = GetImage((int)IMAGE.HP_Bar);
        expBar = GetImage((int)IMAGE.Exp_Bar);
        spBar = GetImage((int)IMAGE.SP_Bar);
        equipWeaponImage = GetImage((int)IMAGE.Equip_Weapon_Image);
        potionImage = GetImage((int)IMAGE.Potion_Image);

        quickSlotPanel = GetComponentInChildren<QuickSlotPanel>(true);
        quickSlotPanel.Initialize(characterData.InventoryData);

        characterData.StatusData.OnCharacterStatusChanged += UpdateUserPanel;
        characterData.StatusData.OnCharacterWeaponChanged += UpdateWeaponImage;

        UpdateUserPanel(characterData.StatusData);
    }

    private void OnDestroy()
    {
        characterData.StatusData.OnCharacterStatusChanged -= UpdateUserPanel;
        characterData.StatusData.OnCharacterWeaponChanged -= UpdateWeaponImage;
    }

    public void UpdateWeaponImage(WEAPON_TYPE weaponType)
    {
        switch(weaponType)
        {
            case WEAPON_TYPE.None:
                equipWeaponImage.color = Functions.SetColor(Color.white, 0f);
                equipWeaponImage.sprite = null;
                break;
            case WEAPON_TYPE.HALBERD:
                equipWeaponImage.color = Functions.SetColor(Color.white, 1f);
                equipWeaponImage.sprite = Managers.ResourceManager.LoadResourceSync<Sprite>(Constants.Sprite_Thumbnail_Halberd);
                break;
            case WEAPON_TYPE.SWORD_SHIELD:
                equipWeaponImage.color = Functions.SetColor(Color.white, 1f);
                equipWeaponImage.sprite = Managers.ResourceManager.LoadResourceSync<Sprite>(Constants.Sprite_Thumbnail_Sword_Shield);
                break;
        }
    }

    public void UpdatePotionImage()
    {

    }

    public void UpdateUserPanel(PlayerStatusData status)
    {
        float ratio = status.CurrentHP / status.MaxHP;
        hpBar.fillAmount = ratio;

        ratio = status.CurrentExp / status.MaxExp;
        expBar.fillAmount = ratio;

        ratio = status.CurrentSP / status.MaxSP;
        spBar.fillAmount = ratio;
    }
}
