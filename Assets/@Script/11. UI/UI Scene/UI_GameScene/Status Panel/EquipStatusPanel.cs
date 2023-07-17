using TMPro;

public class EquipStatusPanel : UIPanel
{
    public enum TEXT
    {
        // Weapon
        Weapon_Name_Text,
        Attack_Power_Amount_Text,
        Attack_Speed_Amount_Text,
        Fixed_Damage_Amount_Text,
        Weapon_Defense_Power_Amount_Text,
        Weapon_Defense_Penetration_Amount_Text,

        // Armor
        HP_Amount_Text,
        SP_Amount_Text,
        Defense_Power_Amount_Text,
        Damage_Reduction_Rate_Amount_Text,
        SP_Recovery_Amount_Text,
        SP_Cost_Reduction_Amount_Text
    }

    private CharacterStatusData statusData;

    // Weapon
    private TextMeshProUGUI weaponNameText;
    private TextMeshProUGUI attackPowerAmountText;
    private TextMeshProUGUI attackSpeedAmountText;
    private TextMeshProUGUI fixedDamageAmountText;
    private TextMeshProUGUI weaponDefensePowerAmountText;
    private TextMeshProUGUI weaponDefensePenetrationTextAmount;

    // Armor
    private TextMeshProUGUI hpAmountText;
    private TextMeshProUGUI spAmountText;
    private TextMeshProUGUI defensePowerAmountText;
    private TextMeshProUGUI damageReductionAmountText;
    private TextMeshProUGUI spRecoveryAmountText;
    private TextMeshProUGUI spCostReductionAmountText;

    public void Initialize(CharacterData characterData)
    {
        statusData = characterData.StatusData;
        BindText(typeof(TEXT));

        // Weapon
        weaponNameText = GetText((int)TEXT.Weapon_Name_Text);
        attackPowerAmountText = GetText((int)TEXT.Attack_Power_Amount_Text);
        attackSpeedAmountText = GetText((int)TEXT.Attack_Speed_Amount_Text);
        fixedDamageAmountText = GetText((int)TEXT.Fixed_Damage_Amount_Text);
        weaponDefensePowerAmountText = GetText((int)TEXT.Weapon_Defense_Power_Amount_Text);
        weaponDefensePenetrationTextAmount = GetText((int)TEXT.Weapon_Defense_Penetration_Amount_Text);

        // Armor
        hpAmountText = GetText((int)TEXT.HP_Amount_Text);
        spAmountText = GetText((int)TEXT.SP_Amount_Text);
        defensePowerAmountText = GetText((int)TEXT.Defense_Power_Amount_Text);
        damageReductionAmountText = GetText((int)TEXT.Damage_Reduction_Rate_Amount_Text);
        spRecoveryAmountText = GetText((int)TEXT.SP_Recovery_Amount_Text);
        spCostReductionAmountText = GetText((int)TEXT.SP_Cost_Reduction_Amount_Text);

        statusData.OnChangeStatusData += RefreshPanel;
        RefreshPanel(characterData.StatusData);
    }

    private void OnDestroy()
    {
        if(statusData != null)
            statusData.OnChangeStatusData -= RefreshPanel;
    }

    public void RefreshPanel(CharacterStatusData status)
    {
        // Weapon
        weaponNameText.text = status.WeaponNameText;
        attackPowerAmountText.text = status.WeaponAttackPower.ToString("F1");
        attackSpeedAmountText.text = $"{status.WeaponAttackSpeed.ToString("F1")}%";
        fixedDamageAmountText.text = status.WeaponFixedDamage.ToString("F1");
        weaponDefensePowerAmountText.text = status.WeaponDefensePower.ToString("F1");
        weaponDefensePenetrationTextAmount.text = $"{status.WeaponDefensePenetration.ToString("F1")}%";

        // Armor
        hpAmountText.text = status.ArmorMaxHP.ToString("F1");
        spAmountText.text = status.ArmorMaxSP.ToString("F1");
        defensePowerAmountText.text = status.ArmorDefensePower.ToString("F1");
        damageReductionAmountText.text = $"{status.ArmorDamageReduction.ToString("F1")}%";
        spRecoveryAmountText.text = $"{status.ArmorSPRecovery.ToString("F1")}%";
        spCostReductionAmountText.text = $"{status.ArmorSPCostReduction.ToString("F1")}%";
    }
}
