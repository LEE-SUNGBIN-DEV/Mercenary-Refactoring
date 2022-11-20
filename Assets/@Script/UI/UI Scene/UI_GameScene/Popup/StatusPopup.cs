using UnityEngine;
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

    public enum SLOT
    {
        WeaponSlot,
        HelmetSlot,
        ArmorSlot,
        BootsSlot
    }

    private Character character;
    [SerializeField] private WeaponSlot weaponSlot;
    [SerializeField] private HelmetSlot helmetSlot;
    [SerializeField] private ArmorSlot armorSlot;
    [SerializeField] private BootsSlot bootsSlot;

    public void Initialize(Character targetCharacter)
    {
        character = targetCharacter;
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));
        GetButton((int)BUTTON.StrengthButton).onClick.AddListener(OnClickStrengthButton);
        GetButton((int)BUTTON.VitalityButton).onClick.AddListener(OnClickVitalityButton);
        GetButton((int)BUTTON.DexterityButton).onClick.AddListener(OnClickDexterityButton);
        GetButton((int)BUTTON.LuckButton).onClick.AddListener(OnClickLuckButton);

        weaponSlot = GetComponentInChildren<WeaponSlot>();
        helmetSlot = GetComponentInChildren<HelmetSlot>();
        armorSlot = GetComponentInChildren<ArmorSlot>();
        bootsSlot = GetComponentInChildren<BootsSlot>();

        weaponSlot.Initialize(character);
        helmetSlot.Initialize(character);
        armorSlot.Initialize(character);
        bootsSlot.Initialize(character);

        character.CharacterData.StatData.OnChangeStatData -= RefreshStatData;
        character.CharacterData.StatData.OnChangeStatData += RefreshStatData;
        character.Status.OnCharacterStatusChanged -= RefreshStatus;
        character.Status.OnCharacterStatusChanged += RefreshStatus;

        RefreshStatData(character.CharacterData.StatData);
        RefreshStatus(character.Status);
    }

    public void RefreshStatData(StatData statData)
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

    public void LoadFromCharacterData<T, U>(T loadItem, U targetSlot) where T : EquipmentItem where U : EquipmentSlot
    {
    }

    #region Button Event Function
    public void OnClickStrengthButton()
    {
        if (character.CharacterData.StatData.StatPoint > 0)
        {
            --character.CharacterData.StatData.StatPoint;
            ++character.CharacterData.StatData.Strength;
        }
    }
    public void OnClickVitalityButton()
    {
        if (character.CharacterData.StatData.StatPoint > 0)
        {
            --character.CharacterData.StatData.StatPoint;
            ++character.CharacterData.StatData.Vitality;
        }
    }

    public void OnClickDexterityButton()
    {
        if (character.CharacterData.StatData.StatPoint > 0)
        {
            --character.CharacterData.StatData.StatPoint;
            ++character.CharacterData.StatData.Dexterity;
        }
    }

    public void OnClickLuckButton()
    {
        if (character.CharacterData.StatData.StatPoint > 0)
        {
            --character.CharacterData.StatData.StatPoint;
            ++character.CharacterData.StatData.Luck;
        }
    }
    #endregion

    public WeaponSlot WeaponSlot { get { return weaponSlot; } set { weaponSlot = value; } }
    public HelmetSlot HelmetSlot { get { return helmetSlot; } set { helmetSlot = value; } }
    public ArmorSlot ArmorSlot { get { return armorSlot; } set { armorSlot = value; } }
    public BootsSlot BootsSlot { get { return bootsSlot; } set { bootsSlot = value; } }
}
