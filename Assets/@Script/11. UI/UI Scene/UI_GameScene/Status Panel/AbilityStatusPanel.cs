using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityStatusPanel : UIPanel
{
    public enum TEXT
    {
        Ability_Point_Amount_Text,
        Strength_Amount_Text,
        Vitality_Amount_Text,
        Dexterity_Amount_Text,
        Will_Amount_Text
    }

    public enum BUTTON
    {
        Strength_Button,
        Vitality_Button,
        Dexterity_Button,
        Will_Button,
        Ability_Initializing_Button
    }

    private CharacterStatusData statusData;

    private Button strengthButton;
    private Button vitalityButton;
    private Button dexterityButton;
    private Button willButton;
    private Button abilityIntializingButton;

    private TextMeshProUGUI abilityPointAmountText;
    private TextMeshProUGUI strengthAmountText;
    private TextMeshProUGUI vitalityAmountText;
    private TextMeshProUGUI dexterityAmountText;
    private TextMeshProUGUI willAmountText;

    public void Initialize(CharacterData characterData)
    {
        statusData = characterData.StatusData;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        // Texts
        abilityPointAmountText = GetText((int)TEXT.Ability_Point_Amount_Text);
        strengthAmountText = GetText((int)TEXT.Strength_Amount_Text);
        vitalityAmountText = GetText((int)TEXT.Vitality_Amount_Text);
        dexterityAmountText = GetText((int)TEXT.Dexterity_Amount_Text);
        willAmountText = GetText((int)TEXT.Will_Amount_Text);

        // Buttons
        strengthButton = GetButton((int)BUTTON.Strength_Button);
        strengthButton.onClick.AddListener(AddStrength);

        vitalityButton = GetButton((int)BUTTON.Vitality_Button);
        vitalityButton.onClick.AddListener(AddVitality);

        dexterityButton = GetButton((int)BUTTON.Dexterity_Button);
        dexterityButton.onClick.AddListener(AddDexterity);

        willButton = GetButton((int)BUTTON.Will_Button);
        willButton.onClick.AddListener(AddWill);

        abilityIntializingButton = GetButton((int)BUTTON.Ability_Initializing_Button);
        abilityIntializingButton.onClick.AddListener(InitializeAbility);

        characterData.StatusData.OnChangeStatusData += RefreshPanel;
        RefreshPanel(statusData);
    }

    private void OnDestroy()
    {
        if (statusData != null)
            statusData.OnChangeStatusData -= RefreshPanel;
    }

    public void RefreshPanel(CharacterStatusData status)
    {
        if (status.AbilityPoint < 1)
            ActiveAbilityButtons(false);
        else
            ActiveAbilityButtons(true);

        abilityPointAmountText.text = $"{status.AbilityPoint}";
        strengthAmountText.text = $"{(status.Strength * status.GetPercentage(status.SkillAllStatsRatio)).ToString("F0")}";
        vitalityAmountText.text = $"{(status.Vitality * status.GetPercentage(status.SkillAllStatsRatio)).ToString("F0")}";
        dexterityAmountText.text = $"{(status.Dexterity * status.GetPercentage(status.SkillAllStatsRatio)).ToString("F0")}";
        willAmountText.text = $"{(status.Will * status.GetPercentage(status.SkillAllStatsRatio)).ToString("F0")}";
    }

    public void ActiveAbilityButtons(bool isActive)
    {
        strengthButton.interactable = isActive;
        vitalityButton.interactable = isActive;
        dexterityButton.interactable = isActive;
        willButton.interactable = isActive;
    }

    public void InitializeAbility()
    {
        statusData.InitializeAbility();
    }

    public void AddStrength()
    {
        if (statusData.AbilityPoint > 0)
        {
            --statusData.AbilityPoint;
            ++statusData.Strength;
        }
    }
    public void AddVitality()
    {
        if (statusData.AbilityPoint > 0)
        {
            --statusData.AbilityPoint;
            ++statusData.Vitality;
        }
    }

    public void AddDexterity()
    {
        if (statusData.AbilityPoint > 0)
        {
            --statusData.AbilityPoint;
            ++statusData.Dexterity;
        }
    }

    public void AddWill()
    {
        if (statusData.AbilityPoint > 0)
        {
            --statusData.AbilityPoint;
            ++statusData.Will;
        }
    }
}
