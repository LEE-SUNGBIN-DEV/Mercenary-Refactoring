using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverallStatusPanel : UIPanel
{
    public enum TEXT
    {
        Level_Amount_Text,
        HP_Amount_Text,
        SP_Amount_Text,
        Attack_Power_Amount_Text,
        Defense_Power_Amount_Text,
        Attack_Speed_Amount_Text,
        Move_Speed_Amount_Text,
        Critical_Chance_Amount_Text,
        Critical_Damage_Amount_Text,
        Fixed_Damage_Amount_Text,
        Defense_Penetration_Amount_Text,
        SP_Recovery_Amount_Text,
        SP_Cost_Reduction_Amount_Text,
        Damage_Reduction_Amount_Text
    }

    private CharacterStatusData statusData;

    private TextMeshProUGUI levelAmountText;
    private TextMeshProUGUI attackPowerAmountText;
    private TextMeshProUGUI defensePowerAmountText;
    private TextMeshProUGUI damageReductionAmountText;
    private TextMeshProUGUI hpAmountText;
    private TextMeshProUGUI spAmountText;
    private TextMeshProUGUI attackSpeedAmountText;
    private TextMeshProUGUI moveSpeedAmountText;
    private TextMeshProUGUI criticalChanceAmountText;
    private TextMeshProUGUI criticalDamageAmountText;
    private TextMeshProUGUI fixedDamageAmountText;
    private TextMeshProUGUI defensePenetrationTextAmount;
    private TextMeshProUGUI spRecoveryAmountText;
    private TextMeshProUGUI spCostReductionAmountText;

    public void Initialize(CharacterData characterData)
    {
        statusData = characterData.StatusData;

        BindText(typeof(TEXT));

        // Texts
        levelAmountText = GetText((int)TEXT.Level_Amount_Text);

        hpAmountText = GetText((int)TEXT.HP_Amount_Text);
        spAmountText = GetText((int)TEXT.SP_Amount_Text);
        attackPowerAmountText = GetText((int)TEXT.Attack_Power_Amount_Text);
        defensePowerAmountText = GetText((int)TEXT.Defense_Power_Amount_Text);
        damageReductionAmountText = GetText((int)TEXT.Damage_Reduction_Amount_Text);
        attackSpeedAmountText = GetText((int)TEXT.Attack_Speed_Amount_Text);
        moveSpeedAmountText = GetText((int)TEXT.Move_Speed_Amount_Text);
        criticalChanceAmountText = GetText((int)TEXT.Critical_Chance_Amount_Text);
        criticalDamageAmountText = GetText((int)TEXT.Critical_Damage_Amount_Text);
        fixedDamageAmountText = GetText((int)TEXT.Fixed_Damage_Amount_Text);
        defensePenetrationTextAmount = GetText((int)TEXT.Defense_Penetration_Amount_Text);
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
        levelAmountText.text = status.Level.ToString();

        hpAmountText.text = $"{status.CurrentHP.ToString("F1")}/{status.MaxHP.ToString("F1")}";
        spAmountText.text = $"{status.CurrentSP.ToString("F1")}/{status.MaxSP.ToString("F1")}";
        attackPowerAmountText.text = status.AttackPower.ToString("F1");
        defensePowerAmountText.text = status.DefensePower.ToString("F1");
        damageReductionAmountText.text = $"{status.DamageReduction.ToString("F1")}%";
        attackSpeedAmountText.text = $"{status.AttackSpeed.ToString("F1")}%";
        moveSpeedAmountText.text = $"{status.MoveSpeed.ToString("F1")}%";
        criticalChanceAmountText.text = $"{status.CriticalChance.ToString("F1")}%";
        criticalDamageAmountText.text = $"{status.CriticalDamage.ToString("F1")}%";
        fixedDamageAmountText.text = $"{status.FixedDamage.ToString("F1")}";
        defensePenetrationTextAmount.text = $"{status.DefensePenetration.ToString("F1")}%";
        spRecoveryAmountText.text = $"{status.SPRecovery.ToString("F1")}%";
        spCostReductionAmountText.text = $"{status.SPCostReduction.ToString("F1")}%";
    }
}
