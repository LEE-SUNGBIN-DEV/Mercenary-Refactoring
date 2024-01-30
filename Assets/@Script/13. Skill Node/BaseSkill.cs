using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill
{
    [Header("Base Item")]
    [SerializeField] protected SkillData skillData;
    [SerializeField] protected StatOption[] fixedOptions;

    public BaseSkill(string skillID)
    {
        LoadFromSkillID(skillID);
    }

    public virtual void LoadFromSkillID(string skillID)
    {
        if (Managers.DataManager.SkillTable.TryGetValue(skillID, out skillData))
        {
            CreateFixedOptions();
        }
    }
    public void CreateFixedOptions()
    {
        if (skillData?.fixedOptionID != null && Managers.DataManager.FixedOptionTable.TryGetValue(skillData.fixedOptionID, out FixedOptionData fixedOptionData))
        {
            fixedOptions = fixedOptionData.CreateFixedOptions();
        }
    }
    public void ApplySkill(CharacterStatusData statusData)
    {
        if(!fixedOptions.IsNullOrEmpty())
        {
            for(int i=0; i<fixedOptions.Length; i++)
            {
                fixedOptions[i].ApplyToStatus(statusData);
            }
        }
    }
    public void ReleaseSkill(CharacterStatusData statusData)
    {
        if (!fixedOptions.IsNullOrEmpty())
        {
            for (int i = 0; i < fixedOptions.Length; i++)
            {
                fixedOptions[i].ReleaseFromStatus(statusData);
            }
        }
    }
    public bool IsExistItemData()
    {
        return (skillData != null);
    }

    #region Property
    public SkillData SkillData { get { return skillData; } }
    public StatOption[] FixedStatusOptions { get { return fixedOptions; } }
    #endregion
}
