using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSkillData : ICharacterData
{
    public event UnityAction<CharacterSkillData> OnChangeSkillData;
    public event UnityAction<CharacterSkillData> OnInitializeSkillData;
    public event UnityAction<BaseSkill> OnReleaseSkill;
    public event UnityAction<BaseSkill> OnApplySkill;

    [Header("Save Datas")]
    [JsonProperty] private int skillPoint;
    [JsonProperty] private HashSet<string> unlockedSkillHashSet;
    [JsonProperty] private Dictionary<string, string> nodeSkillDict;

    [Header("Runtime Datas")]
    [JsonIgnore] private Dictionary<string, BaseSkill> passiveSkillDict;

    public void CreateData()
    {
        InitializeSkillData();
    }
    public void LoadData()
    {
        InitializePassiveSkills();
    }
    public void UpdateData(CharacterData characterData)
    {
        characterData.StatusData.OnLevelUp -= GetLevelUpSkillPoint;
        characterData.StatusData.OnLevelUp += GetLevelUpSkillPoint;
    }
    public void SaveData()
    {

    }
    public void InitializeSkillData()
    {
        skillPoint = 0;
        if(unlockedSkillHashSet == null)
            unlockedSkillHashSet = new HashSet<string>();

        if(nodeSkillDict == null)
            nodeSkillDict = new Dictionary<string, string>();

        unlockedSkillHashSet.Clear();
        nodeSkillDict.Clear();

        foreach (SkillData skillData in Managers.DataManager.SkillTable.Values)
        {
            if (skillData.currentLevel == 0)
            {
                unlockedSkillHashSet.Add(skillData.skillID);
                nodeSkillDict.Add(skillData.nodeID, skillData.skillID);
            }
        }

        InitializePassiveSkills();
        OnInitializeSkillData?.Invoke(this);
        OnChangeSkillData?.Invoke(this);
    }
    public void InitializePassiveSkills()
    {
        if (passiveSkillDict != null)
        {
            foreach (BaseSkill passiveSkill in passiveSkillDict.Values)
            {
                OnReleaseSkill?.Invoke(passiveSkill);
            }
            passiveSkillDict.Clear();
        }
        else
        {
            passiveSkillDict = new Dictionary<string, BaseSkill>();
        }

        foreach (string skillID in nodeSkillDict.Values)
        {
            BaseSkill newSkill = new BaseSkill(skillID);
            passiveSkillDict.Add(skillID, newSkill);
            OnApplySkill?.Invoke(newSkill);
        }

        OnChangeSkillData?.Invoke(this);
    }

    public void GetLevelUpSkillPoint(CharacterStatusData statusData)
    {
        skillPoint += Constants.CHARACTER_STATUS_LEVEL_UP_SKILL_POINT;
        OnChangeSkillData?.Invoke(this);
    }

    public void LevelUpBySkillID(string skillID)
    {
        if(skillPoint > 0)
        {
            SkillData currentSkillData = Managers.DataManager.SkillTable[skillID];
            SkillData nextSkillData = currentSkillData.GetNextSkillData();

            if (currentSkillData.nodeID == nextSkillData.nodeID)
            {
                --skillPoint;
                OnReleaseSkill?.Invoke(passiveSkillDict[currentSkillData.skillID]);
                passiveSkillDict.Remove(currentSkillData.skillID);

                BaseSkill newSkill = new BaseSkill(nextSkillData.skillID);
                passiveSkillDict.Add(nextSkillData.skillID, newSkill);
                OnApplySkill?.Invoke(passiveSkillDict[nextSkillData.skillID]);

                unlockedSkillHashSet.Add(nextSkillData.skillID);
                nodeSkillDict[nextSkillData.nodeID] = nextSkillData.skillID;
            }
            OnChangeSkillData?.Invoke(this);
        }
    }
    public void LevelUpByNodeID(string nodeID)
    {
        LevelUpBySkillID(nodeSkillDict[nodeID]);
    }

    public bool IsLock(string skillID)
    {
        return !unlockedSkillHashSet.Contains(skillID);
    }

    public SkillData GetSkillDataFromNodeID(string nodeID)
    {
        return Managers.DataManager.SkillTable[nodeSkillDict[nodeID]];
    }

    public int SkillPoint { get { return skillPoint; } set { skillPoint = value; } }
    public HashSet<string> UnlockedSkillHashSet { get { return unlockedSkillHashSet; } }
    public Dictionary<string, string> NodeSkillDict { get { return nodeSkillDict; } }
    public Dictionary<string, BaseSkill> PassiveSkillDict { get { return passiveSkillDict; } }
}
