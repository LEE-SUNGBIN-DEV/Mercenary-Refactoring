using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

[System.Serializable]
public class CharacterStatusData
{
    public event UnityAction<CharacterStatusData> OnChangeStatusData;

    [SerializeField] private int level;
    [SerializeField] private int skillPoint;
    [SerializeField] private int abilityPoint;

    [Header("Ability")]
    [SerializeField] private int strength;
    [SerializeField] private int vitality;
    [SerializeField] private int dexterity;
    [SerializeField] private int will;

    [Header("Final Status")]
    [SerializeField] private float currentHP;
    [SerializeField] private float currentSP;
    [SerializeField] private float currentExp;
    [SerializeField] private float maxHP;
    [SerializeField] private float maxSP;
    [SerializeField] private float maxExp;
    [SerializeField] private float attackPower;
    [SerializeField] private float defensePower;
    [SerializeField] private float damageReduction;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float fixedDamage;
    [SerializeField] private float defensePenetration;
    [SerializeField] private float spRecovery;
    [SerializeField] private float spCostReduction;
    [SerializeField] private int hitLevel;

    [Header("Equip Weapon Status")]
    [JsonIgnore] private string weaponNameText;
    [JsonIgnore] private float weaponAttackPower;
    [JsonIgnore] private float weaponAttackSpeed;
    [JsonIgnore] private float weaponFixedDamage;
    [JsonIgnore] private float weaponDefensePower;
    [JsonIgnore] private float weaponDefensePenetration;

    [Header("Equip Armor Status")]
    [JsonIgnore] private float armorMaxHP;
    [JsonIgnore] private float armorMaxSP;
    [JsonIgnore] private float armorDefensePower;
    [JsonIgnore] private float armorDamageReduction;
    [JsonIgnore] private float armorSPRecovery;
    [JsonIgnore] private float armorSPCostReduction;

    [Header("Skill Status")]
    [SerializeField] private float skillAllStatsRatio;

    [SerializeField] private float skillLightAttackDamageRatio;
    [SerializeField] private float skillHeavyAttackDamageRatio;

    [SerializeField] private float skillWeaponAttackPowerRatio;
    [SerializeField] private float skillArmorDefensePowerRatio;

    [SerializeField] private float skillMaxHPRatio;
    [SerializeField] private float skillMaxSPRatio;
    [SerializeField] private float skillAttackPowerRatio;
    [SerializeField] private float skillDefensePowerRatio;
    [SerializeField] private float skillDamageReductionRatio;
    [SerializeField] private float skillAttackSpeed;
    [SerializeField] private float skillMoveSpeed;
    [SerializeField] private float skillCriticalChance;
    [SerializeField] private float skillCriticalDamage;
    [SerializeField] private float skillFixedDamage;
    [SerializeField] private float skillDefensePenetration;
    [SerializeField] private float skillSPRecovery;
    [SerializeField] private float skillSPCostReduction;

    public void CreateData()
    {
        level = Constants.CHARACTER_DATA_DEFALUT_LEVEL;
        abilityPoint = 0;
        skillPoint = 0;

        maxExp = Managers.DataManager.LevelTable[level];
        currentExp = Constants.CHARACTER_DATA_DEFALUT_EXPERIENCE;

        strength = Constants.CHARACTER_DATA_DEFALUT_STRENGTH;
        vitality = Constants.CHARACTER_DATA_DEFALUT_VITALITY;
        dexterity = Constants.CHARACTER_DATA_DEFALUT_DEXTERITY;
        will = Constants.CHARACTER_DATA_DEFALUT_LUCK;

        UpdateStatus();
        currentHP = maxHP;
        currentSP = maxSP;
    }

    public void UpdateStatus()
    {
        maxExp = Managers.DataManager.LevelTable[level];

        maxHP = (Constants.CHARACTER_DATA_DEFALUT_MAX_HP + (vitality * 4.5f * GetPercentage(skillAllStatsRatio)) + armorMaxHP) * (GetPercentage(skillMaxHPRatio));
        maxSP = (Constants.CHARACTER_DATA_DEFALUT_MAX_SP + (vitality * 2.5f * GetPercentage(skillAllStatsRatio)) + armorMaxSP) * (GetPercentage(skillMaxSPRatio));

        attackPower = (strength * 1.9f * GetPercentage(skillAllStatsRatio) + weaponAttackPower * GetPercentage(skillWeaponAttackPowerRatio)) * GetPercentage(skillAttackPowerRatio);
        defensePower = (strength * 0.9f * GetPercentage(skillAllStatsRatio) + weaponDefensePower + (armorDefensePower * GetPercentage(skillArmorDefensePowerRatio))) * GetPercentage(skillDefensePowerRatio);
        damageReduction = (armorDamageReduction + skillDamageReductionRatio) * 0.01f;

        criticalChance = will * GetPercentage(skillAllStatsRatio) + skillCriticalChance;
        criticalDamage = Constants.PLAYER_STAT_CRITICAL_DAMAGE_DEFAULT + (will * GetPercentage(skillAllStatsRatio)) + skillCriticalDamage;

        attackSpeed = Constants.PLAYER_STAT_ATTACK_SPEED_DEFAULT + ((dexterity * GetPercentage(skillAllStatsRatio)) + weaponAttackSpeed + skillAttackSpeed) * 0.01f;
        moveSpeed = Constants.PLAYER_STAT_MOVE_SPEED_DEFAULT + (dexterity * 0.02f * GetPercentage(skillAllStatsRatio)) + (skillMoveSpeed * 0.01f);

        fixedDamage = weaponFixedDamage + skillFixedDamage;
        defensePenetration = weaponDefensePenetration + skillDefensePenetration;
        spRecovery = armorSPRecovery + skillSPRecovery;
        spCostReduction = armorSPCostReduction + skillSPCostReduction;

        OnChangeStatusData?.Invoke(this);
    }

    public float GetPercentage(float value)
    {
        return 1 + (value * 0.01f);
    }
    public void Respawn()
    {
        currentHP = maxHP;
        currentSP = maxSP;
    }

    #region Level Functions
    public void LevelUp()
    {
        currentExp -= maxExp;
        maxExp = Managers.DataManager.LevelTable[Level];
        ++level;
        skillPoint += Constants.CHARACTER_DATA_LEVEL_UP_SKILL_POINT;
        abilityPoint += Constants.CHARACTER_DATA_LEVEL_UP_ABILITY_POINT;

        OnChangeStatusData?.Invoke(this);
    }
    public float GetExpRatio()
    {
        return currentExp / maxExp;
    }
    public void InitializeSkillPoint()
    {
        skillPoint = (level -1) * Constants.CHARACTER_DATA_LEVEL_UP_SKILL_POINT;

        OnChangeStatusData?.Invoke(this);
    }
    public void InitializeAbility()
    {
        strength = Constants.CHARACTER_DATA_DEFALUT_STRENGTH;
        vitality = Constants.CHARACTER_DATA_DEFALUT_VITALITY;
        dexterity = Constants.CHARACTER_DATA_DEFALUT_DEXTERITY;
        will = Constants.CHARACTER_DATA_DEFALUT_LUCK;
        abilityPoint = (level - 1) * Constants.CHARACTER_DATA_LEVEL_UP_ABILITY_POINT;

        OnChangeStatusData?.Invoke(this);
    }
    #endregion

    #region HP & SP Functions
    public bool CheckStamina(float amount, CALCULATE_MODE mode = CALCULATE_MODE.Absolute)
    {
        amount *= (1 - spCostReduction * 0.01f);
        switch (mode)
        {
            case CALCULATE_MODE.Absolute:
                if (currentSP >= amount)
                    return true;
                break;

            case CALCULATE_MODE.Ratio:
                if (currentSP >= (maxSP * amount * 0.01f))
                    return true;
                break;
        }
        return false;
    }
    public void RecoverStamina(float amount, CALCULATE_MODE mode)
    {
        amount *= (1 + spRecovery * 0.01f);
        switch (mode)
        {
            case CALCULATE_MODE.Absolute:
                CurrentSP += amount;
                break;
            case CALCULATE_MODE.Ratio:
                CurrentSP += (maxSP * amount * 0.01f);
                break;
        }
    }

    public void RecoverStaminaPerSec(float amount, CALCULATE_MODE mode)
    {
        amount *= (1 + spRecovery * 0.01f);
        switch (mode)
        {
            case CALCULATE_MODE.Absolute:
                CurrentSP += amount * Time.deltaTime;
                break;
            case CALCULATE_MODE.Ratio:
                CurrentSP += (maxSP * amount * 0.01f * Time.deltaTime);
                break;
        }
    }

    public void ConsumeStamina(float amount, CALCULATE_MODE mode = CALCULATE_MODE.Absolute)
    {
        amount *= (1 - spCostReduction * 0.01f);
        switch (mode)
        {
            case CALCULATE_MODE.Absolute:
                CurrentSP -= amount;
                break;
            case CALCULATE_MODE.Ratio:
                CurrentSP -= (maxSP * amount * 0.01f);
                break;
        }
    }

    public void ConsumeStaminaPerSec(float amount, CALCULATE_MODE mode)
    {
        amount *= (1 - spCostReduction * 0.01f);
        switch (mode)
        {
            case CALCULATE_MODE.Absolute:
                CurrentSP -= amount * Time.deltaTime;
                break;
            case CALCULATE_MODE.Ratio:
                CurrentSP -= (maxSP * amount * 0.01f * Time.deltaTime);
                break;
        }
    }
    public float GetStaminaRatio()
    {
        return currentSP / maxSP;
    }

    public void RecoverHP(float amount, CALCULATE_MODE mode)
    {
        switch (mode)
        {
            case CALCULATE_MODE.Absolute:
                CurrentHP += amount;
                break;
            case CALCULATE_MODE.Ratio:
                CurrentHP += (maxHP * amount * 0.01f);
                break;
        }
    }
    public void ReduceHP(float amount, CALCULATE_MODE mode = CALCULATE_MODE.Absolute)
    {
        switch (mode)
        {
            case CALCULATE_MODE.Absolute:
                CurrentHP -= amount;
                break;
            case CALCULATE_MODE.Ratio:
                CurrentHP -= (maxHP * amount * 0.01f);
                break;
        }

    }
    public float GetHPRatio()
    {
        return currentHP / maxHP;
    }
    #endregion

    #region Equipment Status Functions
    public void EquipWeapon(CharacterInventoryData inventoryData, WEAPON_TYPE weaponType)
    {
        switch (weaponType)
        {
            case WEAPON_TYPE.HALBERD:
                HalberdData halberdData = Managers.DataManager.HalberdTable[inventoryData.HalberdID];
                weaponNameText = halberdData.name;
                weaponAttackPower = halberdData.attackPower;
                weaponAttackSpeed = halberdData.attackSpeed;
                weaponFixedDamage = halberdData.fixedDamage;
                weaponDefensePower = 0f;
                weaponDefensePenetration = halberdData.defensePenetration;
                break;

            case WEAPON_TYPE.SWORD_SHIELD:
                SwordShieldData swordShieldData = Managers.DataManager.SwordShieldTable[inventoryData.SwordShieldID];
                weaponNameText = swordShieldData.name;
                weaponAttackPower = swordShieldData.attackPower;
                weaponAttackSpeed = swordShieldData.attackSpeed;
                weaponFixedDamage = swordShieldData.fixedDamage;
                weaponDefensePower = swordShieldData.defensePower;
                weaponDefensePenetration = 0f;
                break;

            default:
                weaponNameText = "-";
                weaponAttackPower = 0f;
                weaponAttackSpeed = 0f;
                weaponFixedDamage = 0f;
                weaponDefensePower = 0f;
                weaponDefensePenetration = 0f;
                break;
        }

        UpdateStatus();
    }
    public void EquipArmor(CharacterInventoryData inventoryData)
    {
        ArmorData armorData = Managers.DataManager.ArmorTable[inventoryData.ArmorID];
        armorMaxHP = armorData.hp;
        armorMaxSP = armorData.sp;
        armorDefensePower = armorData.defensePower;
        armorDamageReduction = armorData.damageReduction;
        armorSPRecovery = armorData.spRecovery;
        armorSPCostReduction = armorData.spCostReduction;

        UpdateStatus();
    }
    #endregion

    #region Ability Property
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            if (level < 1)
                level = 1;

            OnChangeStatusData?.Invoke(this);
        }
    }
    public int AbilityPoint
    {
        get { return abilityPoint; }
        set
        {
            abilityPoint = value;
            OnChangeStatusData?.Invoke(this);
        }
    }
    public int SkillPoint
    {
        get { return skillPoint; }
        set
        {
            skillPoint = value;
            OnChangeStatusData?.Invoke(this);
        }
    }
    public int Strength
    {
        get { return strength; }
        set
        {
            strength = value;
            UpdateStatus();
        }
    }

    public int Vitality
    {
        get { return vitality; }
        set
        {
            vitality = value;
            UpdateStatus();
        }
    }

    public int Dexterity
    {
        get { return dexterity; }
        set
        {
            dexterity = value;
            UpdateStatus();
        }
    }
    public int Will
    {
        get { return will; }
        set
        {
            will = value;
            UpdateStatus();
        }
    }
    #endregion

    #region Status Property
    public float CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = value;

            if (currentHP > MaxHP)
                currentHP = MaxHP;

            if (currentHP < 0)
                currentHP = 0;

            OnChangeStatusData?.Invoke(this);
        }
    }
    public float CurrentExp
    {
        get { return currentExp; }
        set
        {
            currentExp = value;
            if (currentExp < 0)
                currentExp = 0;

            while (currentExp >= MaxExp)
                LevelUp();

            OnChangeStatusData?.Invoke(this);
        }
    }
    public float MaxHP
    {
        get { return maxHP; }
        set
        {
            maxHP = value;
            if (maxHP <= 0)
                maxHP = 1;

            OnChangeStatusData?.Invoke(this);
        }
    }
    public float MaxExp
    {
        get { return maxExp; }
        set
        {
            maxExp = value;
            if (maxExp < 0)
                maxExp = 0;

            OnChangeStatusData?.Invoke(this);
        }
    }
    public float MaxSP
    {
        get { return maxSP; }
        set
        {
            maxSP = value;
            if (maxSP <= 0)
                maxSP = 1;

            OnChangeStatusData?.Invoke(this);
        }
    }
    public float CurrentSP
    {
        get { return currentSP; }
        set
        {
            currentSP = value;
            if (currentSP > MaxSP)
                currentSP = MaxSP;

            if (currentSP < 0)
                currentSP = 0;

            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float AttackPower
    {
        get { return attackPower; }
        set
        {
            attackPower = value;
            if (attackPower < 0)
                attackPower = 0;

            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float DefensePower
    {
        get { return defensePower; }
        set
        {
            defensePower = value;
            if (defensePower < 0)
                defensePower = 0;

            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float DamageReduction
    {
        get { return damageReduction; }
        set
        {
            damageReduction = value;
            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float CriticalChance
    {
        get { return criticalChance; }
        set
        {
            criticalChance = value;
            if (criticalChance < Constants.PLAYER_STAT_CRITICAL_CHANCE_MIN)
                criticalChance = Constants.PLAYER_STAT_CRITICAL_CHANCE_MIN;

            if (criticalChance > Constants.PLAYER_STAT_CRITICAL_CHANCE_MAX)
                criticalChance = Constants.PLAYER_STAT_CRITICAL_CHANCE_MAX;

            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float CriticalDamage
    {
        get { return criticalDamage; }
        set
        {
            criticalDamage = value;
            if (criticalDamage < Constants.PLAYER_STAT_CRITICAL_DAMAGE_MIN)
                criticalDamage = Constants.PLAYER_STAT_CRITICAL_DAMAGE_MIN;

            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set
        {
            attackSpeed = value;

            if (attackSpeed < Constants.PLAYER_STAT_ATTACK_SPEED_MIN)
                attackSpeed = Constants.PLAYER_STAT_ATTACK_SPEED_MIN;

            if (attackSpeed > Constants.PLAYER_STAT_ATTACK_SPEED_MAX)
                attackSpeed = Constants.PLAYER_STAT_ATTACK_SPEED_MAX;

            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = value;

            if (moveSpeed < Constants.PLAYER_STAT_MOVE_SPEED_MIN)
                moveSpeed = Constants.PLAYER_STAT_MOVE_SPEED_MIN;

            if (moveSpeed > Constants.PLAYER_STAT_MOVE_SPEED_MAX)
                moveSpeed = Constants.PLAYER_STAT_MOVE_SPEED_MAX;

            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float FixedDamage
    {
        get { return fixedDamage; }
        set
        {
            fixedDamage = value;
            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float DefensePenetration
    {
        get { return defensePenetration; }
        set
        {
            defensePenetration = value;
            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float SPRecovery
    {
        get { return spRecovery; }
        set
        {
            spRecovery = value;
            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float SPCostReduction
    {
        get { return spCostReduction; }
        set
        {
            spCostReduction = value;
            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public int HitLevel
    {
        get { return hitLevel; }
        set
        {
            hitLevel = value;
            if (hitLevel < 0)
                hitLevel = 0;

            OnChangeStatusData?.Invoke(this);
        }
    }

    [JsonIgnore] public string WeaponNameText { get { return weaponNameText; } set { weaponNameText = value; UpdateStatus(); } }
    [JsonIgnore] public float WeaponAttackPower { get { return weaponAttackPower; } set { weaponAttackPower = value; UpdateStatus(); } }
    [JsonIgnore] public float WeaponAttackSpeed { get { return weaponAttackSpeed; } set { weaponAttackSpeed = value; UpdateStatus(); } }
    [JsonIgnore] public float WeaponFixedDamage { get { return weaponFixedDamage; } set { weaponFixedDamage = value; UpdateStatus(); } }
    [JsonIgnore] public float WeaponDefensePower { get { return weaponDefensePower; } set { weaponDefensePower = value; UpdateStatus(); } }
    [JsonIgnore] public float WeaponDefensePenetration { get { return weaponDefensePenetration; } set { weaponDefensePenetration = value; UpdateStatus(); } }

    [JsonIgnore] public float ArmorMaxHP { get { return armorMaxHP; } set { armorMaxHP = value; UpdateStatus(); } }
    [JsonIgnore] public float ArmorMaxSP { get { return armorMaxSP; } set { armorMaxSP = value; UpdateStatus(); } }
    [JsonIgnore] public float ArmorDefensePower { get { return armorDefensePower; } set { armorDefensePower = value; UpdateStatus(); } }
    [JsonIgnore] public float ArmorDamageReduction { get { return armorDamageReduction; } set { armorDamageReduction = value; UpdateStatus(); } }
    [JsonIgnore] public float ArmorSPRecovery { get { return armorSPRecovery; } set { armorSPRecovery = value; UpdateStatus(); } }
    [JsonIgnore] public float ArmorSPCostReduction { get { return armorSPCostReduction; } set { armorSPCostReduction = value; UpdateStatus(); } }


    [JsonIgnore] public float SkillAllStatsRatio { get { return skillAllStatsRatio; } set { skillAllStatsRatio = value; UpdateStatus(); } }

    [JsonIgnore] public float SkillLightAttackDamageRatio { get { return skillLightAttackDamageRatio; } set { skillLightAttackDamageRatio = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillHeavyAttackDamageRatio { get { return skillHeavyAttackDamageRatio; } set { skillHeavyAttackDamageRatio = value; UpdateStatus(); } }

    [JsonIgnore] public float SkillWeaponAttackPowerRatio { get { return skillWeaponAttackPowerRatio; } set { skillWeaponAttackPowerRatio = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillArmorDefensePowerRatio { get { return skillArmorDefensePowerRatio; } set { skillArmorDefensePowerRatio = value; UpdateStatus(); } }

    [JsonIgnore] public float SkillMaxHPRatio { get { return skillMaxHPRatio; } set { skillMaxHPRatio = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillMaxSPRatio { get { return skillMaxSPRatio; } set { skillMaxSPRatio = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillAttackPowerRatio { get { return skillAttackPowerRatio; } set { skillAttackPowerRatio = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillDefensePowerRatio { get { return skillDefensePowerRatio; } set { skillDefensePowerRatio = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillDamageReductionRatio { get { return skillDamageReductionRatio; } set { skillDamageReductionRatio = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillAttackSpeed { get { return skillAttackSpeed; } set { skillAttackSpeed = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillMoveSpeed { get { return skillMoveSpeed; } set { skillMoveSpeed = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillCriticalChance { get { return skillCriticalChance; } set { skillCriticalChance = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillCriticalDamage { get { return skillCriticalDamage; } set { skillCriticalDamage = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillFixedDamage { get { return skillFixedDamage; } set { skillFixedDamage = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillDefensePenetration { get { return skillDefensePenetration; } set { skillDefensePenetration = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillSPRecovery { get { return skillSPRecovery; } set { skillSPRecovery = value; UpdateStatus(); } }
    [JsonIgnore] public float SkillSPCostReduction { get { return skillSPCostReduction; } set { skillSPCostReduction = value; UpdateStatus(); } }
    #endregion
}
