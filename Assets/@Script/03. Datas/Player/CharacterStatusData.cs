using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using System;

[System.Serializable]
public class CharacterStatusData : ICharacterData
{
    public event UnityAction<CharacterStatusData> OnChangeStatusData;
    public event UnityAction<CharacterStatusData> OnLevelUp;
    public event UnityAction<float> OnGetRewardedExperience;

    // Save Data
    [Header("Save Datas")]
    [JsonProperty][SerializeField] private int level;
    [JsonProperty][SerializeField] private int abilityPoint;

    [JsonProperty][SerializeField] private float strengthSaveData;
    [JsonProperty][SerializeField] private float dexteritySaveData;
    [JsonProperty][SerializeField] private float vitalitySaveData;
    [JsonProperty][SerializeField] private float willSaveData;

    [JsonProperty][SerializeField] private float currentExp;
    [JsonProperty][SerializeField] private float currentHP;
    [JsonProperty][SerializeField] private float currentSP;

    [Header("Runetime Datas")]
    [JsonIgnore] private Dictionary<STAT_TYPE, BaseStat> statDict;
    [JsonIgnore][SerializeField] private float maxExp;
    [JsonIgnore][SerializeField] private int hitLevel;

    public void CreateData()
    {
        level = 1;
        abilityPoint = 0;
        strengthSaveData = GetStatDefaultValue(STAT_TYPE.STAT_STRENGTH);
        vitalitySaveData = GetStatDefaultValue(STAT_TYPE.STAT_VITALITY);
        dexteritySaveData = GetStatDefaultValue(STAT_TYPE.STAT_DEXTERITY);
        willSaveData = GetStatDefaultValue(STAT_TYPE.STAT_WILL);
        maxExp = Managers.DataManager.LevelTable[level].experience;
        currentExp = 0;

        currentHP = 0;
        currentSP = 0;
    }
    public void LoadData()
    {
        StatDict[STAT_TYPE.STAT_STRENGTH].BaseValue = strengthSaveData;
        StatDict[STAT_TYPE.STAT_VITALITY].BaseValue = vitalitySaveData;
        StatDict[STAT_TYPE.STAT_DEXTERITY].BaseValue = dexteritySaveData;
        StatDict[STAT_TYPE.STAT_WILL].BaseValue = willSaveData;
        maxExp = Managers.DataManager.LevelTable[level].experience;
    }
    public void UpdateData(CharacterData characterData)
    {
        if (characterData.InventoryData != null)
        {
            characterData.InventoryData.OnChangeInventoryData -= EquipWeapon;
            characterData.InventoryData.OnChangeInventoryData += EquipWeapon;
            EquipWeapon(characterData.InventoryData);

            characterData.InventoryData.OnChangeInventoryData -= EquipArmor;
            characterData.InventoryData.OnChangeInventoryData += EquipArmor;
            EquipArmor(characterData.InventoryData);

            characterData.InventoryData.OnEquipRune -= EquipRune;
            characterData.InventoryData.OnEquipRune += EquipRune;
            characterData.InventoryData.OnUnequipRune -= UnequipRune;
            characterData.InventoryData.OnUnequipRune += UnequipRune;
            for (int i = 0; i < characterData.InventoryData.RuneSlotItems.Length; i++)
            {
                if (characterData.InventoryData.RuneSlotItems[i] != null)
                {
                    UnequipRune(characterData.InventoryData.RuneSlotItems[i]);
                    EquipRune(characterData.InventoryData.RuneSlotItems[i]);
                }
            }
        }

        if (characterData.SkillData != null)
        {
            characterData.SkillData.OnInitializeSkillData -= UpdateSkillPoint;
            characterData.SkillData.OnInitializeSkillData += UpdateSkillPoint;

            characterData.SkillData.OnApplySkill -= ApplySkill;
            characterData.SkillData.OnApplySkill += ApplySkill;
            characterData.SkillData.OnReleaseSkill -= ReleaseSkill;
            characterData.SkillData.OnReleaseSkill += ReleaseSkill;

            foreach (BaseSkill baseSkill in characterData.SkillData.PassiveSkillDict.Values)
            {
                baseSkill.ReleaseSkill(this);
                baseSkill.ApplySkill(this);
            }
        }

        if(currentHP <= 0 )
        {
            currentHP = StatDict[STAT_TYPE.STAT_MAX_HP].GetFinalValue();
            currentSP = StatDict[STAT_TYPE.STAT_MAX_SP].GetFinalValue();
        }
        OnChangeStatusData?.Invoke(this);
    }
    public void SaveData()
    {
        strengthSaveData = StatDict[STAT_TYPE.STAT_STRENGTH].BaseValue;
        vitalitySaveData = StatDict[STAT_TYPE.STAT_VITALITY].BaseValue;
        dexteritySaveData = StatDict[STAT_TYPE.STAT_DEXTERITY].BaseValue;
        willSaveData = StatDict[STAT_TYPE.STAT_WILL].BaseValue;
    }
    public void CreateStatDictionary()
    {
        statDict = new Dictionary<STAT_TYPE, BaseStat>();
        foreach (STAT_TYPE optionType in Enum.GetValues(typeof(STAT_TYPE)))
        {
            if (Managers.DataManager.StatTable.TryGetValue(optionType.GetEnumName(), out StatData statusOptionData))
            {
                BaseStat newStat = new BaseStat(statusOptionData.defaultValue, statusOptionData.minValue, statusOptionData.maxValue);

                switch (optionType)
                {
                    case STAT_TYPE.STAT_STRENGTH:
                    case STAT_TYPE.STAT_VITALITY:
                    case STAT_TYPE.STAT_DEXTERITY:
                    case STAT_TYPE.STAT_WILL:
                    case STAT_TYPE.STAT_WEAPON_ATTACK_POWER:
                    case STAT_TYPE.STAT_ARMOR_DEFENSE_POWER:
                        newStat.OnStatModified -= UpdateBaseValues;
                        newStat.OnStatModified += UpdateBaseValues;
                        break;
                }
                statDict.Add(optionType, newStat);
            }
        }
        OnChangeStatusData?.Invoke(this);
    }
    public float GetStatDefaultValue(STAT_TYPE optionType)
    {
        return Managers.DataManager.StatTable[optionType.GetEnumName()].defaultValue;
    }

    public void UpdateBaseValues()
    {
        maxExp = Managers.DataManager.LevelTable[level].experience;
        StatDict[STAT_TYPE.STAT_ATTACK_POWER].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_ATTACK_POWER) + StatDict[STAT_TYPE.STAT_STRENGTH].GetFinalValue() * 1.4f + StatDict[STAT_TYPE.STAT_WEAPON_ATTACK_POWER].GetFinalValue();
        StatDict[STAT_TYPE.STAT_DEFENSE_POWER].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_DEFENSE_POWER) + StatDict[STAT_TYPE.STAT_STRENGTH].GetFinalValue() * 0.7f + StatDict[STAT_TYPE.STAT_ARMOR_DEFENSE_POWER].GetFinalValue();

        StatDict[STAT_TYPE.STAT_MAX_HP].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_MAX_HP) + StatDict[STAT_TYPE.STAT_VITALITY].GetFinalValue() * 4.2f;
        StatDict[STAT_TYPE.STAT_MAX_SP].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_MAX_SP) + StatDict[STAT_TYPE.STAT_VITALITY].GetFinalValue() * 2.4f;

        StatDict[STAT_TYPE.STAT_ATTACK_SPEED].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_ATTACK_SPEED) + StatDict[STAT_TYPE.STAT_DEXTERITY].GetFinalValue() * 0.005f;
        StatDict[STAT_TYPE.STAT_MOVE_SPEED].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_MOVE_SPEED) + StatDict[STAT_TYPE.STAT_DEXTERITY].GetFinalValue() * 0.01f;

        StatDict[STAT_TYPE.STAT_CRITICAL_CHANCE_RATE].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_CRITICAL_CHANCE_RATE) + (StatDict[STAT_TYPE.STAT_WILL].GetFinalValue() * 0.6f);
        StatDict[STAT_TYPE.STAT_CRITICAL_DAMAGE_RATE].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_CRITICAL_DAMAGE_RATE) + StatDict[STAT_TYPE.STAT_WILL].GetFinalValue();

        OnChangeStatusData?.Invoke(this);
    }

    #region Skill
    public void UpdateSkillPoint(CharacterSkillData characterSkillData)
    {
        if (characterSkillData != null)
        {
            characterSkillData.SkillPoint = (level - 1) * Constants.CHARACTER_STATUS_LEVEL_UP_SKILL_POINT;
        }
    }
    public void ApplySkill(BaseSkill baseSkill)
    {
        baseSkill.ApplySkill(this);
    }
    public void ReleaseSkill(BaseSkill baseSkill)
    {
        baseSkill.ReleaseSkill(this);
    }
    #endregion
    public float GetIncreasePercentage(float value)
    {
        return 1 + (value * 0.01f);
    }
    public float GetReductionPercentage(float value)
    {
        return 1 - (value * 0.01f);
    }
    public void Respawn()
    {
        currentHP = StatDict[STAT_TYPE.STAT_MAX_HP].GetFinalValue();
        currentSP = StatDict[STAT_TYPE.STAT_MAX_SP].GetFinalValue();
    }

    #region EXP/Level Functions
    public void RewardExperience(float amount)
    {
        if (amount == 0)
            return;

        CurrentExp += amount;
        OnGetRewardedExperience?.Invoke(amount);
    }
    public void LevelUp()
    {
#if UNITY_EDITOR
        Debug.Log($"[Notice]: Character Level Up");
#endif
        currentExp -= maxExp;
        maxExp = Managers.DataManager.LevelTable[Level].experience;
        ++level;
        abilityPoint += Constants.CHARACTER_STATUS_LEVEL_UP_ABILITY_POINT;

        OnLevelUp?.Invoke(this);
        OnChangeStatusData?.Invoke(this);
    }
    public float GetExpRatio()
    {
        return currentExp / maxExp;
    }
    #endregion

    #region Ability Functions
    public void InitializeAbility()
    {
        StatDict[STAT_TYPE.STAT_STRENGTH].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_STRENGTH);
        StatDict[STAT_TYPE.STAT_VITALITY].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_VITALITY);
        StatDict[STAT_TYPE.STAT_DEXTERITY].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_DEXTERITY);
        StatDict[STAT_TYPE.STAT_WILL].BaseValue = GetStatDefaultValue(STAT_TYPE.STAT_WILL);
        abilityPoint = (level - 1) * Constants.CHARACTER_STATUS_LEVEL_UP_ABILITY_POINT;
    }
    #endregion

    #region HP & SP Functions
    public bool CheckStamina(float amount, VALUE_TYPE mode = VALUE_TYPE.FIXED)
    {
        amount *= GetReductionPercentage(StatDict[STAT_TYPE.STAT_SP_COST_REDUCTION_RATE].GetFinalValue());
        switch (mode)
        {
            case VALUE_TYPE.FIXED:
                if (currentSP >= amount)
                    return true;
                break;

            case VALUE_TYPE.PERCENTAGE:
                if (currentSP >= (StatDict[STAT_TYPE.STAT_MAX_SP].GetFinalValue() * amount * 0.01f))
                    return true;
                break;
        }
        return false;
    }
    public void RecoverStamina(float amount, VALUE_TYPE mode)
    {
        amount *= GetIncreasePercentage(StatDict[STAT_TYPE.STAT_SP_RECOVERY_RATE].GetFinalValue());
        switch (mode)
        {
            case VALUE_TYPE.FIXED:
                CurrentSP += amount;
                break;
            case VALUE_TYPE.PERCENTAGE:
                CurrentSP += (StatDict[STAT_TYPE.STAT_MAX_SP].GetFinalValue() * amount * 0.01f);
                break;
        }
    }
    public void RecoverStaminaPerSec(float amount, VALUE_TYPE mode)
    {
        amount *= GetIncreasePercentage(StatDict[STAT_TYPE.STAT_SP_RECOVERY_RATE].GetFinalValue());
        switch (mode)
        {
            case VALUE_TYPE.FIXED:
                CurrentSP += amount * Time.deltaTime;
                break;
            case VALUE_TYPE.PERCENTAGE:
                CurrentSP += (StatDict[STAT_TYPE.STAT_MAX_SP].GetFinalValue() * amount * 0.01f * Time.deltaTime);
                break;
        }
    }
    public void ConsumeStamina(float amount, VALUE_TYPE mode = VALUE_TYPE.FIXED)
    {
        amount *= GetReductionPercentage(StatDict[STAT_TYPE.STAT_SP_COST_REDUCTION_RATE].GetFinalValue());
        switch (mode)
        {
            case VALUE_TYPE.FIXED:
                CurrentSP -= amount;
                break;
            case VALUE_TYPE.PERCENTAGE:
                CurrentSP -= (StatDict[STAT_TYPE.STAT_MAX_SP].GetFinalValue() * amount * 0.01f);
                break;
        }
    }
    public void ConsumeStaminaPerSec(float amount, VALUE_TYPE mode)
    {
        amount *= GetReductionPercentage(StatDict[STAT_TYPE.STAT_SP_COST_REDUCTION_RATE].GetFinalValue());
        switch (mode)
        {
            case VALUE_TYPE.FIXED:
                CurrentSP -= amount * Time.deltaTime;
                break;
            case VALUE_TYPE.PERCENTAGE:
                CurrentSP -= (StatDict[STAT_TYPE.STAT_MAX_SP].GetFinalValue() * amount * 0.01f * Time.deltaTime);
                break;
        }
    }
    public float GetSPRatio()
    {
        return currentSP / StatDict[STAT_TYPE.STAT_MAX_SP].GetFinalValue();
    }
    public void RecoverHP(float amount, VALUE_TYPE mode)
    {
        switch (mode)
        {
            case VALUE_TYPE.FIXED:
                CurrentHP += amount;
                break;
            case VALUE_TYPE.PERCENTAGE:
                CurrentHP += (StatDict[STAT_TYPE.STAT_MAX_HP].GetFinalValue() * amount * 0.01f);
                break;
        }
    }
    public void ReduceHP(float amount, VALUE_TYPE mode = VALUE_TYPE.FIXED)
    {
        switch (mode)
        {
            case VALUE_TYPE.FIXED:
                CurrentHP -= amount;
                break;
            case VALUE_TYPE.PERCENTAGE:
                CurrentHP -= (StatDict[STAT_TYPE.STAT_MAX_HP].GetFinalValue() * amount * 0.01f);
                break;
        }

    }
    public float GetHPRatio()
    {
        return currentHP / StatDict[STAT_TYPE.STAT_MAX_HP].GetFinalValue();
    }
    #endregion

    #region Equipment Status Functions
    public void EquipWeapon(CharacterInventoryData inventoryData)
    {
        inventoryData.SwordShieldSlotItem.UnEquip(this);
        inventoryData.HalberdSlotItem.UnEquip(this);
        switch (inventoryData.CurrentWeaponType)
        {
            case WEAPON_TYPE.HALBERD:
                inventoryData.HalberdSlotItem.Equip(this);
                break;
            case WEAPON_TYPE.SWORD_SHIELD:
                inventoryData.SwordShieldSlotItem.Equip(this);
                break;
        }
    }
    public void EquipArmor(CharacterInventoryData inventoryData)
    {
        inventoryData.ArmorSlotItem.UnEquip(this);
        inventoryData.ArmorSlotItem.Equip(this);
    }
    public void EquipRune(RuneItem runeItem)
    {
        runeItem?.Equip(this);

    }
    public void UnequipRune(RuneItem runeItem)
    {
        runeItem?.UnEquip(this);
    }
    #endregion

    #region Property
    [JsonIgnore]
    public int Level
    {
        get { return level; }
        set
        {
            level = Mathf.Clamp(value, Constants.CHARACTER_STATUS_LEVEL_MIN, Constants.CHARACTER_STATUS_LEVEL_MAX);
            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public int AbilityPoint
    {
        get { return abilityPoint; }
        set
        {
            abilityPoint = value;
            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = Mathf.Clamp(value, 0, StatDict[STAT_TYPE.STAT_MAX_HP].GetFinalValue());
            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float CurrentSP
    {
        get { return currentSP; }
        set
        {
            currentSP = Mathf.Clamp(value, 0, StatDict[STAT_TYPE.STAT_MAX_SP].GetFinalValue());
            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float MaxExp
    {
        get { return maxExp; }
        set
        {
            maxExp = value;
            if (maxExp < 1)
                maxExp = 1;

            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float CurrentExp
    {
        get { return currentExp; }
        set
        {
            currentExp = value;

            if (currentExp < 0)
                currentExp = 0;

            maxExp = Managers.DataManager.LevelTable[level].experience;
            while (currentExp >= MaxExp)
                LevelUp();

            OnChangeStatusData?.Invoke(this);
        }
    }
    [JsonIgnore] public int HitLevel { get { return hitLevel; } set { hitLevel = value; OnChangeStatusData?.Invoke(this); } }

    [JsonIgnore]
    public Dictionary<STAT_TYPE, BaseStat> StatDict
    {
        get
        {
            if (statDict == null)
            {
                CreateStatDictionary();
            }
            return statDict;
        }
    }
    [JsonIgnore] public float FinalAttackSpeed { get { return statDict[STAT_TYPE.STAT_ATTACK_SPEED].GetFinalValue(); } }
    #endregion
}
