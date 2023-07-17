using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryArmorSlot : BaseInventoryEquipmentSlot
{
    public enum TEXT_ARMOR
    {
        Equipment_Name_Text,
        HP_Amount_Text,
        SP_Amount_Text,
        Defense_Power_Amount_Text,
        Damage_Reduction_Rate_Amount_Text,
        SP_Recovery_Amount_Text,
        SP_Cost_Reduction_Amount_Text
    }

    private ArmorData armorData;
    private TextMeshProUGUI hpAmountText;
    private TextMeshProUGUI spAmountText;
    private TextMeshProUGUI defensePowerAmountText;
    private TextMeshProUGUI damageReductionAmountText;
    private TextMeshProUGUI spRecoveryAmountText;
    private TextMeshProUGUI spCostReductionAmountText;

    public override void Initialize(CharacterInventoryData inventoryData)
    {
        base.Initialize(inventoryData);
        BindText(typeof(TEXT_ARMOR));

        equipmentNameText = GetText((int)TEXT_ARMOR.Equipment_Name_Text);
        hpAmountText = GetText((int)TEXT_ARMOR.HP_Amount_Text);
        spAmountText = GetText((int)TEXT_ARMOR.SP_Amount_Text);
        defensePowerAmountText = GetText((int)TEXT_ARMOR.Defense_Power_Amount_Text);
        damageReductionAmountText = GetText((int)TEXT_ARMOR.Damage_Reduction_Rate_Amount_Text);
        spRecoveryAmountText = GetText((int)TEXT_ARMOR.SP_Recovery_Amount_Text);
        spCostReductionAmountText = GetText((int)TEXT_ARMOR.SP_Cost_Reduction_Amount_Text);
    }

    public override void LoadData(CharacterInventoryData inventoryData)
    {
        armorData = Managers.DataManager.ArmorTable[inventoryData.ArmorID];
        equipmentImage.sprite = Managers.ResourceManager.LoadResourceSync<Sprite>($"Sprite_Thumbnail_Armor");
        equipmentImage.color = Functions.SetColor(equipmentImage.color, 1f);

        equipmentNameText.text = armorData.name;
        hpAmountText.text = armorData.hp.ToString("F0");
        spAmountText.text = armorData.sp.ToString("F0");
        defensePowerAmountText.text = armorData.defensePower.ToString("F0");
        damageReductionAmountText.text = armorData.damageReduction.ToString("F0") + "%";
        spRecoveryAmountText.text = armorData.spRecovery.ToString("F0") + "%";
        spCostReductionAmountText.text = armorData.spCostReduction.ToString("F0") + "%";
    }
}
