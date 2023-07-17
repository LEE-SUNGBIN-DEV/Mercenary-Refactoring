using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryHalberdSlot : BaseInventoryEquipmentSlot
{
    public enum TEXT
    {
        Equipment_Name_Text,
        Attack_Power_Amount_Text,
        Attack_Speed_Amount_Text,
        Fixed_Damage_Amount_Text,
        Defense_Penetration_Amount_Text
    }

    private HalberdData halberdData;
    private TextMeshProUGUI attackPowerAmountText;
    private TextMeshProUGUI attackSpeedAmountText;
    private TextMeshProUGUI fixedDamageAmountText;
    private TextMeshProUGUI defensePenetrationAmountText;

    public override void Initialize(CharacterInventoryData inventoryData)
    {
        base.Initialize(inventoryData);
        BindText(typeof(TEXT));

        equipmentNameText = GetText((int)TEXT.Equipment_Name_Text);
        attackPowerAmountText = GetText((int)TEXT.Attack_Power_Amount_Text);
        attackSpeedAmountText = GetText((int)TEXT.Attack_Speed_Amount_Text);
        fixedDamageAmountText = GetText((int)TEXT.Fixed_Damage_Amount_Text);
        defensePenetrationAmountText = GetText((int)TEXT.Defense_Penetration_Amount_Text);
    }

    public override void LoadData(CharacterInventoryData inventoryData)
    {
        halberdData = Managers.DataManager.HalberdTable[inventoryData.HalberdID];
        equipmentImage.sprite = Managers.ResourceManager.LoadResourceSync<Sprite>($"Sprite_Thumbnail_Halberd");
        equipmentImage.color = Functions.SetColor(equipmentImage.color, 1f);

        equipmentNameText.text = halberdData.name;
        attackPowerAmountText.text = halberdData.attackPower.ToString();
        attackSpeedAmountText.text = halberdData.attackSpeed.ToString() + "%";
        fixedDamageAmountText.text = halberdData.fixedDamage.ToString();
        defensePenetrationAmountText.text = halberdData.defensePenetration.ToString() + "%";
    }
}
