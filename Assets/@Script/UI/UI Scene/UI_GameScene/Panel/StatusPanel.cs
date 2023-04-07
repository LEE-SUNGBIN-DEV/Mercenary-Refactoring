using UnityEngine;
using UnityEngine.Events;

public class StatusPanel : UIPanel
{
    public enum TEXT
    {
        Level_Amount_Text,
        HP_Amount_Text,
        SP_Amount_Text,
        Attack_Power_Amount_Text,
        Defensive_Power_Amount_Text,
        Attack_Speed_Amount_Text,
        Move_Speed_Amount_Text,
        Critical_Chance_Amount_Text,
        Critical_Damage_Amount_Text,

        Stat_Point_Amount_Text,
        Strength_Amount_Text,
        Vitality_Amount_Text,
        Dexterity_Amount_Text,
        Luck_Amount_Text
    }

    public enum BUTTON
    {
        Strength_Button,
        Vitality_Button,
        Dexterity_Button,
        Luck_Button
    }

    private CharacterData characterData;

    public void Initialize(CharacterData _characterData)
    {
        characterData = _characterData;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));
        GetButton((int)BUTTON.Strength_Button).onClick.AddListener(OnClickStrengthButton);
        GetButton((int)BUTTON.Vitality_Button).onClick.AddListener(OnClickVitalityButton);
        GetButton((int)BUTTON.Dexterity_Button).onClick.AddListener(OnClickDexterityButton);
        GetButton((int)BUTTON.Luck_Button).onClick.AddListener(OnClickLuckButton);

        characterData.StatusData.OnCharacterStatusChanged -= RefreshStatus;
        characterData.StatusData.OnCharacterStatusChanged += RefreshStatus;

        RefreshStatus(characterData.StatusData);
    }

    public void RefreshStatus(StatusData status)
    {
        GetText((int)TEXT.Level_Amount_Text).text = status.Level.ToString();

        GetText((int)TEXT.Stat_Point_Amount_Text).text = status.StatPoint.ToString();
        GetText((int)TEXT.Strength_Amount_Text).text = status.Strength.ToString();
        GetText((int)TEXT.Vitality_Amount_Text).text = status.Vitality.ToString();
        GetText((int)TEXT.Dexterity_Amount_Text).text = status.Dexterity.ToString();
        GetText((int)TEXT.Luck_Amount_Text).text = status.Luck.ToString();

        GetText((int)TEXT.Attack_Power_Amount_Text).text = status.AttackPower.ToString("F1");
        GetText((int)TEXT.Defensive_Power_Amount_Text).text = status.DefensivePower.ToString("F1");

        GetText((int)TEXT.HP_Amount_Text).text = status.CurrentHP.ToString("F1") + "/" + status.MaxHP.ToString();
        GetText((int)TEXT.SP_Amount_Text).text = status.CurrentSP.ToString("F1") + "/" + status.MaxSP.ToString();

        GetText((int)TEXT.Attack_Speed_Amount_Text).text = status.AttackSpeed.ToString("F1");
        GetText((int)TEXT.Move_Speed_Amount_Text).text = status.MoveSpeed.ToString("F1");
        GetText((int)TEXT.Critical_Chance_Amount_Text).text = status.CriticalChance.ToString("F1");
        GetText((int)TEXT.Critical_Damage_Amount_Text).text = status.CriticalDamage.ToString("F1");
    }

    #region Button Event Function
    public void OnClickStrengthButton()
    {
        if (characterData.StatusData.StatPoint > 0)
        {
            --characterData.StatusData.StatPoint;
            ++characterData.StatusData.Strength;
        }
    }
    public void OnClickVitalityButton()
    {
        if (characterData.StatusData.StatPoint > 0)
        {
            --characterData.StatusData.StatPoint;
            ++characterData.StatusData.Vitality;
        }
    }

    public void OnClickDexterityButton()
    {
        if (characterData.StatusData.StatPoint > 0)
        {
            --characterData.StatusData.StatPoint;
            ++characterData.StatusData.Dexterity;
        }
    }

    public void OnClickLuckButton()
    {
        if (characterData.StatusData.StatPoint > 0)
        {
            --characterData.StatusData.StatPoint;
            ++characterData.StatusData.Luck;
        }
    }
    #endregion
}
