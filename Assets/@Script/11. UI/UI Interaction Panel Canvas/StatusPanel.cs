using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatusPanel : UIPanel, IFocusPanel
{
    public enum TEXT
    {
        Level_Value_Text,
        HP_Value_Text,
        SP_Value_Text,

        Attack_Power_Value_Text,
        Weapon_Attack_Power_Value_Text,
        Fixed_Damage_Value_Text,
        Critical_Chance_Rate_Value_Text,
        Critical_Damage_Rate_Value_Text,
        Defense_Penetration_Rate_Value_Text,
        Light_Attack_Damage_Rate_Value_Text,
        Heavy_Attack_Damage_Rate_Value_Text,

        Defense_Power_Value_Text,
        Armor_Defense_Power_Value_Text,
        Damage_Reduction_Rate_Value_Text,

        Attack_Speed_Rate_Value_Text,
        Move_Speed_Rate_Value_Text,
        SP_Recovery_Rate_Value_Text,
        SP_Cost_Reduction_Rate_Value_Text,

        Ability_Point_Value_Text,
        Strength_Value_Text,
        Vitality_Value_Text,
        Dexterity_Value_Text,
        Will_Value_Text
    }
    public enum BUTTON
    {
        Strength_Button,
        Vitality_Button,
        Dexterity_Button,
        Will_Button,
        Ability_Initializing_Button
    }

    public event UnityAction<IFocusPanel> OnOpenFocusPanel;
    public event UnityAction<IFocusPanel> OnCloseFocusPanel;

    private bool isOpen;

    private TextMeshProUGUI levelValueText;
    private TextMeshProUGUI hpValueText;
    private TextMeshProUGUI spValueText;

    private TextMeshProUGUI attackPowerValueText;
    private TextMeshProUGUI weaponAttackPowerValueText;
    private TextMeshProUGUI fixedDamageValueText;
    private TextMeshProUGUI defensePenetrationRateValueAmount;
    private TextMeshProUGUI criticalChanceRateValueText;
    private TextMeshProUGUI criticalDamageRateValueText;
    private TextMeshProUGUI lightAttackDamageRateValueText;
    private TextMeshProUGUI heavyAttackDamageRateValueText;

    private TextMeshProUGUI defensePowerValueText;
    private TextMeshProUGUI armorDefensePowerValueText;
    private TextMeshProUGUI damageReductionRateValueText;

    private TextMeshProUGUI attackSpeedRateValueText;
    private TextMeshProUGUI moveSpeedRateValueText;
    private TextMeshProUGUI spRecoveryRateValueText;
    private TextMeshProUGUI spCostReductionRateValueText;

    private TextMeshProUGUI abilityPointAmountText;
    private TextMeshProUGUI strengthAmountText;
    private TextMeshProUGUI vitalityAmountText;
    private TextMeshProUGUI dexterityAmountText;
    private TextMeshProUGUI willAmountText;

    private Button strengthButton;
    private Button vitalityButton;
    private Button dexterityButton;
    private Button willButton;
    private Button abilityIntializingButton;

    private CharacterStatusData statusData;

    public void Initialize()
    {
        isOpen = false;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        // Texts
        levelValueText = GetText((int)TEXT.Level_Value_Text);
        hpValueText = GetText((int)TEXT.HP_Value_Text);
        spValueText = GetText((int)TEXT.SP_Value_Text);

        attackPowerValueText = GetText((int)TEXT.Attack_Power_Value_Text);
        weaponAttackPowerValueText = GetText((int)TEXT.Weapon_Attack_Power_Value_Text);
        fixedDamageValueText = GetText((int)TEXT.Fixed_Damage_Value_Text);
        defensePenetrationRateValueAmount = GetText((int)TEXT.Defense_Penetration_Rate_Value_Text);
        criticalChanceRateValueText = GetText((int)TEXT.Critical_Chance_Rate_Value_Text);
        criticalDamageRateValueText = GetText((int)TEXT.Critical_Damage_Rate_Value_Text);
        lightAttackDamageRateValueText = GetText((int)TEXT.Light_Attack_Damage_Rate_Value_Text);
        heavyAttackDamageRateValueText = GetText((int)TEXT.Heavy_Attack_Damage_Rate_Value_Text);

        defensePowerValueText = GetText((int)TEXT.Defense_Power_Value_Text);
        armorDefensePowerValueText = GetText((int)TEXT.Armor_Defense_Power_Value_Text);
        damageReductionRateValueText = GetText((int)TEXT.Damage_Reduction_Rate_Value_Text);

        attackSpeedRateValueText = GetText((int)TEXT.Attack_Speed_Rate_Value_Text);
        moveSpeedRateValueText = GetText((int)TEXT.Move_Speed_Rate_Value_Text);
        spRecoveryRateValueText = GetText((int)TEXT.SP_Recovery_Rate_Value_Text);
        spCostReductionRateValueText = GetText((int)TEXT.SP_Cost_Reduction_Rate_Value_Text);

        abilityPointAmountText = GetText((int)TEXT.Ability_Point_Value_Text);
        strengthAmountText = GetText((int)TEXT.Strength_Value_Text);
        vitalityAmountText = GetText((int)TEXT.Vitality_Value_Text);
        dexterityAmountText = GetText((int)TEXT.Dexterity_Value_Text);
        willAmountText = GetText((int)TEXT.Will_Value_Text);

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
    }

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
        if(statusData != null )
        {
            statusData.OnChangeStatusData -= UpdatePanelByStatusData;
            statusData.OnChangeStatusData += UpdatePanelByStatusData;
            UpdatePanelByStatusData(statusData);
        }
    }
    private void DisconnectData()
    {
        if (statusData != null)
        {
            statusData.OnChangeStatusData -= UpdatePanelByStatusData;
            statusData = null;
        }
    }
    #endregion
    public void UpdatePanelByStatusData(CharacterStatusData statusData)
    {
        levelValueText.text = statusData.Level.ToString();
        hpValueText.text = $"{Functions.GetStatusValueString(statusData.CurrentHP)}/{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_MAX_HP].GetFinalValue())}";
        spValueText.text = $"{Functions.GetStatusValueString(statusData.CurrentSP)}/{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_MAX_SP].GetFinalValue())}";

        attackPowerValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_ATTACK_POWER].GetFinalValue())}";
        weaponAttackPowerValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_WEAPON_ATTACK_POWER].GetFinalValue())}";
        fixedDamageValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_FIXED_DAMAGE].GetFinalValue())}";
        defensePenetrationRateValueAmount.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_DEFENSE_PENETRATION_RATE].GetFinalValue())}%";
        criticalChanceRateValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_CRITICAL_CHANCE_RATE].GetFinalValue())}%";
        criticalDamageRateValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_CRITICAL_DAMAGE_RATE].GetFinalValue())}%";
        lightAttackDamageRateValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_LIGHT_ATTACK_DAMAGE_RATE].GetFinalValue())}%";
        heavyAttackDamageRateValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_HEAVY_ATTACK_DAMAGE_RATE].GetFinalValue())}%";

        defensePowerValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_DEFENSE_POWER].GetFinalValue())}";
        armorDefensePowerValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_ARMOR_DEFENSE_POWER].GetFinalValue())}";
        damageReductionRateValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_DAMAGE_REDUCTION_RATE].GetFinalValue())}%";

        attackSpeedRateValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_ATTACK_SPEED].GetIncreaseRate())}%";
        moveSpeedRateValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_MOVE_SPEED].GetIncreaseRate())}%";
        spRecoveryRateValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_SP_RECOVERY_RATE].GetFinalValue())}%";
        spCostReductionRateValueText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_SP_COST_REDUCTION_RATE].GetFinalValue())}%";

        if (statusData.AbilityPoint < 1)
            ActiveAbilityButtons(false);
        else
            ActiveAbilityButtons(true);

        abilityPointAmountText.text = $"{statusData.AbilityPoint}";
        strengthAmountText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_STRENGTH].GetFinalValue())}";
        vitalityAmountText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_VITALITY].GetFinalValue())}";
        dexterityAmountText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_DEXTERITY].GetFinalValue())}";
        willAmountText.text = $"{Functions.GetStatusValueString(statusData.StatDict[STAT_TYPE.STAT_WILL].GetFinalValue())}";
    }

    private void ActiveAbilityButtons(bool isActive)
    {
        strengthButton.interactable = isActive;
        vitalityButton.interactable = isActive;
        dexterityButton.interactable = isActive;
        willButton.interactable = isActive;
    }
    #region Abillity Button Functions

    private void InitializeAbility()
    {
        Managers.DataManager.CurrentCharacterData.StatusData.InitializeAbility();
    }
    private void AddStrength()
    {
        CharacterStatusData statusData = Managers.DataManager.CurrentCharacterData.StatusData;
        if (statusData?.AbilityPoint > 0)
        {
            --statusData.AbilityPoint;
            ++statusData.StatDict[STAT_TYPE.STAT_STRENGTH].BaseValue;
        }
    }
    private void AddVitality()
    {
        CharacterStatusData statusData = Managers.DataManager.CurrentCharacterData.StatusData;
        if (statusData?.AbilityPoint > 0)
        {
            --statusData.AbilityPoint;
            ++statusData.StatDict[STAT_TYPE.STAT_VITALITY].BaseValue;
        }
    }
    private void AddDexterity()
    {
        CharacterStatusData statusData = Managers.DataManager.CurrentCharacterData.StatusData;
        if (statusData?.AbilityPoint > 0)
        {
            --statusData.AbilityPoint;
            ++statusData.StatDict[STAT_TYPE.STAT_DEXTERITY].BaseValue;
        }
    }
    private void AddWill()
    {
        CharacterStatusData statusData = Managers.DataManager.CurrentCharacterData.StatusData;
        if (statusData?.AbilityPoint > 0)
        {
            --statusData.AbilityPoint;
            ++statusData.StatDict[STAT_TYPE.STAT_WILL].BaseValue;
        }
    }
    #endregion

    public void TogglePanel()
    {
        if (isOpen)
            ClosePanel();
        else
            OpenPanel();
    }
    public void OpenPanel()
    {
        isOpen = true;
        OnOpenFocusPanel?.Invoke(this);
        Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.UI);
        Managers.UIManager.SetCursorMode(CURSOR_MODE.VISIBLE);
        FadeInPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE);
    }
    public void ClosePanel()
    {
        isOpen = false;
        OnCloseFocusPanel?.Invoke(this);
        Managers.InputManager.PopInputMode();
        Managers.UIManager.SetCursorMode(CURSOR_MODE.INVISIBLE);
        FadeOutPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE, () => gameObject.SetActive(false));
    }
}
