using UnityEngine.Events;

public class StatusPopup : UIPopup
{
    public enum TEXT
    {
        ClassText,
        LevelText,
        HPText,
        SPText,
        AttackPowerText,
        DefensivePowerText,
        AttackSpeedText,
        MoveSpeedText,
        CriticalChanceText,
        CriticalDamageText,

        StatPointText,
        StrengthText,
        VitalityText,
        DexterityText,
        LuckText
    }

    public enum BUTTON
    {
        StrengthButton,
        VitalityButton,
        DexterityButton,
        LuckButton
    }

    public override void Initialize(UnityAction<UIPopup> action = null)
    {
        base.Initialize(action);

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));
        GetButton((int)BUTTON.StrengthButton).onClick.AddListener(OnClickStrengthButton);
        GetButton((int)BUTTON.VitalityButton).onClick.AddListener(OnClickVitalityButton);
        GetButton((int)BUTTON.DexterityButton).onClick.AddListener(OnClickDexterityButton);
        GetButton((int)BUTTON.LuckButton).onClick.AddListener(OnClickLuckButton);

        Managers.DataManager.CurrentCharacter.CharacterData.StatData.OnChangeStatData -= RefreshStatData;
        Managers.DataManager.CurrentCharacter.CharacterData.StatData.OnChangeStatData += RefreshStatData;
        Managers.DataManager.CurrentCharacter.CharacterStatus.OnCharacterStatusChanged -= RefreshStatus;
        Managers.DataManager.CurrentCharacter.CharacterStatus.OnCharacterStatusChanged += RefreshStatus;
    }

    private void OnEnable()
    {
        RefreshStatData(Managers.DataManager.CurrentCharacter.CharacterData.StatData);
        RefreshStatus(Managers.DataManager.CurrentCharacter.CharacterStatus);
    }

    public void RefreshStatData(CharacterStatData statData)
    {
        GetText((int)TEXT.ClassText).text = statData.CharacterClass;
        GetText((int)TEXT.LevelText).text = statData.Level.ToString();

        GetText((int)TEXT.StatPointText).text = statData.StatPoint.ToString();
        GetText((int)TEXT.StrengthText).text = statData.Strength.ToString();
        GetText((int)TEXT.VitalityText).text = statData.Vitality.ToString();
        GetText((int)TEXT.DexterityText).text = statData.Dexterity.ToString();
        GetText((int)TEXT.LuckText).text = statData.Luck.ToString();
    }

    public void RefreshStatus(CharacterStatus status)
    {
        GetText((int)TEXT.AttackPowerText).text = status.AttackPower.ToString();
        GetText((int)TEXT.DefensivePowerText).text = status.DefensivePower.ToString();

        GetText((int)TEXT.HPText).text = status.CurrentHitPoint.ToString("F1") + "/" + status.MaxHitPoint.ToString();
        GetText((int)TEXT.SPText).text = status.CurrentStamina.ToString("F1") + "/" + status.MaxStamina.ToString();

        GetText((int)TEXT.AttackSpeedText).text = status.AttackSpeed.ToString();
        GetText((int)TEXT.MoveSpeedText).text = status.MoveSpeed.ToString();
        GetText((int)TEXT.CriticalChanceText).text = status.CriticalChance.ToString();
        GetText((int)TEXT.CriticalDamageText).text = status.CriticalDamage.ToString();
    }

    #region Button Event Function
    public void OnClickStrengthButton()
    {
        if (Managers.DataManager.CurrentCharacter.CharacterData.StatData.StatPoint > 0)
        {
            --Managers.DataManager.CurrentCharacter.CharacterData.StatData.StatPoint;
            ++Managers.DataManager.CurrentCharacter.CharacterData.StatData.Strength;
        }
    }
    public void OnClickVitalityButton()
    {
        if (Managers.DataManager.CurrentCharacter.CharacterData.StatData.StatPoint > 0)
        {
            --Managers.DataManager.CurrentCharacter.CharacterData.StatData.StatPoint;
            ++Managers.DataManager.CurrentCharacter.CharacterData.StatData.Vitality;
        }
    }

    public void OnClickDexterityButton()
    {
        if (Managers.DataManager.CurrentCharacter.CharacterData.StatData.StatPoint > 0)
        {
            --Managers.DataManager.CurrentCharacter.CharacterData.StatData.StatPoint;
            ++Managers.DataManager.CurrentCharacter.CharacterData.StatData.Dexterity;
        }
    }

    public void OnClickLuckButton()
    {
        if (Managers.DataManager.CurrentCharacter.CharacterData.StatData.StatPoint > 0)
        {
            --Managers.DataManager.CurrentCharacter.CharacterData.StatData.StatPoint;
            ++Managers.DataManager.CurrentCharacter.CharacterData.StatData.Luck;
        }
    }
    #endregion
}
