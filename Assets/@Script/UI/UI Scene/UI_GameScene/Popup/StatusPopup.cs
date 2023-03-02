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

        characterData.StatusData.OnCharacterStatusChanged -= RefreshStatus;
        characterData.StatusData.OnCharacterStatusChanged += RefreshStatus;
        characterData.EquipmentSlotData.OnChangeEquipmentSlotData -= LoadEquipmentSlot;
        characterData.EquipmentSlotData.OnChangeEquipmentSlotData += LoadEquipmentSlot;

        RefreshStatus(characterData.StatusData);
        LoadEquipmentSlot(characterData.EquipmentSlotData);
    }

    public void RefreshStatus(StatusData status)
    {
        GetText((int)TEXT.LevelText).text = status.Level.ToString();

        GetText((int)TEXT.StatPointText).text = status.StatPoint.ToString();
        GetText((int)TEXT.StrengthText).text = status.Strength.ToString();
        GetText((int)TEXT.VitalityText).text = status.Vitality.ToString();
        GetText((int)TEXT.DexterityText).text = status.Dexterity.ToString();
        GetText((int)TEXT.LuckText).text = status.Luck.ToString();

        GetText((int)TEXT.AttackPowerText).text = status.AttackPower.ToString("F1");
        GetText((int)TEXT.DefensivePowerText).text = status.DefensivePower.ToString("F1");

        GetText((int)TEXT.HPText).text = status.CurrentHP.ToString("F1") + "/" + status.MaxHP.ToString();
        GetText((int)TEXT.SPText).text = status.CurrentSP.ToString("F1") + "/" + status.MaxSP.ToString();

        GetText((int)TEXT.AttackSpeedText).text = status.AttackSpeed.ToString("F1");
        GetText((int)TEXT.MoveSpeedText).text = status.MoveSpeed.ToString("F1");
        GetText((int)TEXT.CriticalChanceText).text = status.CriticalChance.ToString("F1");
        GetText((int)TEXT.CriticalDamageText).text = status.CriticalDamage.ToString("F1");
    }

    public void LoadEquipmentSlot(EquipmentSlotData equipmentSlotData)
    {
        weaponSlot.LoadSlot(equipmentSlotData.WeaponSlotItemData);
        helmetSlot.LoadSlot(equipmentSlotData.HelmetSlotItemData);
        armorSlot.LoadSlot(equipmentSlotData.ArmorSlotItemData);
        bootsSlot.LoadSlot(equipmentSlotData.BootsSlotItemData);
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

    public WeaponSlot WeaponSlot { get { return weaponSlot; } set { weaponSlot = value; } }
    public HelmetSlot HelmetSlot { get { return helmetSlot; } set { helmetSlot = value; } }
    public ArmorSlot ArmorSlot { get { return armorSlot; } set { armorSlot = value; } }
    public BootsSlot BootsSlot { get { return bootsSlot; } set { bootsSlot = value; } }
}
