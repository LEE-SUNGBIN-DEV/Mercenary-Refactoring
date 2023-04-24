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

    private void OnDestroy()
    {
        characterData.StatusData.OnCharacterStatusChanged -= UpdateUserPanel;
    }

    public void Initialize(CharacterData characterData)
    {
        this.characterData = characterData;

        BindImage(typeof(IMAGE));

        quickSlotPanel = GetComponentInChildren<QuickSlotPanel>(true);
        quickSlotPanel.Initialize(characterData.InventoryData);

        characterData.StatusData.OnCharacterStatusChanged -= UpdateUserPanel;
        characterData.StatusData.OnCharacterStatusChanged += UpdateUserPanel;

        UpdateUserPanel(characterData.StatusData);
    }

    public void UpdateUserPanel(PlayerStatusData status)
    {
        float ratio = status.CurrentHP / status.MaxHP;
        GetImage((int)IMAGE.HP_Bar).fillAmount = ratio;

        ratio = status.CurrentExp / status.MaxExp;
        GetImage((int)IMAGE.Exp_Bar).fillAmount = ratio;

        ratio = status.CurrentSP / status.MaxSP;
        GetImage((int)IMAGE.SP_Bar).fillAmount = ratio;
    }
}
