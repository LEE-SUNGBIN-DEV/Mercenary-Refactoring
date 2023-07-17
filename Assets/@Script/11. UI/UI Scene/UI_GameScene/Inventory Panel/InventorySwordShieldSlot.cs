using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySwordShieldSlot : BaseInventoryEquipmentSlot
{
    public enum TEXT
    {
        Equipment_Name_Text,
        Attack_Power_Amount_Text,
        Attack_Speed_Amount_Text,
        Fixed_Damage_Amount_Text,
        Defense_Power_Amount_Text,
    }

    private SwordShieldData swordShieldData;
    private TextMeshProUGUI attackPowerAmountText;
    private TextMeshProUGUI attackSpeedAmountText;
    private TextMeshProUGUI fixedDamageAmountText;
    private TextMeshProUGUI defensePowerAmountText;

    public override void Initialize(CharacterInventoryData inventoryData)
    {
        base.Initialize(inventoryData);
        BindText(typeof(TEXT));

        equipmentNameText = GetText((int)TEXT.Equipment_Name_Text);
        attackPowerAmountText = GetText((int)TEXT.Attack_Power_Amount_Text);
        attackSpeedAmountText = GetText((int)TEXT.Attack_Speed_Amount_Text);
        fixedDamageAmountText = GetText((int)TEXT.Fixed_Damage_Amount_Text);
        defensePowerAmountText = GetText((int)TEXT.Defense_Power_Amount_Text);
    }

    public override void LoadData(CharacterInventoryData inventoryData)
    {
        swordShieldData = Managers.DataManager.SwordShieldTable[inventoryData.SwordShieldID];
        equipmentImage.sprite = Managers.ResourceManager.LoadResourceSync<Sprite>($"Sprite_Thumbnail_Sword_Shield");
        equipmentImage.color = Functions.SetColor(equipmentImage.color, 1f);

        equipmentNameText.text = swordShieldData.name;
        attackPowerAmountText.text = swordShieldData.attackPower.ToString();
        attackSpeedAmountText.text = swordShieldData.attackSpeed.ToString() + "%";
        fixedDamageAmountText.text = swordShieldData.fixedDamage.ToString();
        defensePowerAmountText.text = swordShieldData.defensePower.ToString();
    }
}
