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

    private CharacterData characterData;
    [SerializeField] private WeaponSlot weaponSlot;
    [SerializeField] private HelmetSlot helmetSlot;
    [SerializeField] private ArmorSlot armorSlot;
    [SerializeField] private BootsSlot bootsSlot;

    public void Initialize(CharacterData _characterData)
    {
        characterData = _characterData;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));
        GetButton((int)BUTTON.StrengthButton).onClick.AddListener(OnClickStrengthButton);
        GetButton((int)BUTTON.VitalityButton).onClick.AddListener(OnClickVitalityButton);
        GetButton((int)BUTTON.DexterityButton).onClick.AddListener(OnClickDexterityButton);
        GetButton((int)BUTTON.LuckButton).onClick.AddListener(OnClickLuckButton);

        weaponSlot = GetComponentInChildren<WeaponSlot>(true);
        helmetSlot = GetComponentInChildren<HelmetSlot>(true);
        armorSlot = GetComponentInChildren<ArmorSlot>(true);
        bootsSlot = GetComponentInChildren<BootsSlot>(true);

        weaponSlot.Initialize(characterData.StatusData);
        helmetSlot.Initialize(characterData.StatusData);
        armorSlot.Initialize(characterData.StatusData);
        bootsSlot.Initialize(characterData.StatusData);

        characterData.StatData.OnChangeStatData -= RefreshStatData;
        characterData.StatData.OnChangeStatData += RefreshStatData;
        characterData.StatusData.OnCharacterStatusChanged -= RefreshStatus;
        characterData.StatusData.OnCharacterStatusChanged += RefreshStatus;
        characterData.EquipmentSlotData.OnChangeEquipmentSlotData -= LoadEquipmentSlot;
        characterData.EquipmentSlotData.OnChangeEquipmentSlotData += LoadEquipmentSlot;

        RefreshStatData(characterData.StatData);
        RefreshStatus(characterData.StatusData);
        LoadEquipmentSlot(characterData.EquipmentSlotData);
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

    public void RefreshStatus(StatusData status)
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

    public void LoadEquipmentSlot(EquipmentSlotData equipmentSlotData)
    {
        weaponSlot.LoadSlot(equipmentSlotData.WeaponSlotItem);
        helmetSlot.LoadSlot(equipmentSlotData.HelmetSlotItem);
        armorSlot.LoadSlot(equipmentSlotData.ArmorSlotItem);
        bootsSlot.LoadSlot(equipmentSlotData.BootsSlotItem);
    }

    #region Button Event Function
    public void OnClickStrengthButton()
    {
        if (characterData.StatData.StatPoint > 0)
        {
            --characterData.StatData.StatPoint;
            ++characterData.StatData.Strength;
        }
    }
    public void OnClickVitalityButton()
    {
        if (characterData.StatData.StatPoint > 0)
        {
            --characterData.StatData.StatPoint;
            ++characterData.StatData.Vitality;
        }
    }

    public void OnClickDexterityButton()
    {
        if (characterData.StatData.StatPoint > 0)
        {
            --characterData.StatData.StatPoint;
            ++characterData.StatData.Dexterity;
        }
    }

    public void OnClickLuckButton()
    {
        if (characterData.StatData.StatPoint > 0)
        {
            --characterData.StatData.StatPoint;
            ++characterData.StatData.Luck;
        }
    }
    #endregion

    public WeaponSlot WeaponSlot { get { return weaponSlot; } set { weaponSlot = value; } }
    public HelmetSlot HelmetSlot { get { return helmetSlot; } set { helmetSlot = value; } }
    public ArmorSlot ArmorSlot { get { return armorSlot; } set { armorSlot = value; } }
    public BootsSlot BootsSlot { get { return bootsSlot; } set { bootsSlot = value; } }
}
