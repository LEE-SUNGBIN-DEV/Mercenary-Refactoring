using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSkillData
{
    public event UnityAction<CharacterSkillData> OnChangeSkillData;

    private Dictionary<int, int> skillLevelDictionary;

    public void CreateData()
    {
        skillLevelDictionary = new Dictionary<int, int>();
    }

    public void InitializeSkillData()
    {
        List<int> skillIDList = new List<int>(skillLevelDictionary.Keys);

        for (int i = 0; i < skillIDList.Count; ++i)
        {
            skillLevelDictionary[skillIDList[i]] = 0;
        }

        OnChangeSkillData?.Invoke(this);
    }

    public int GetSkillLevel(int skillID)
    {
        if(!skillLevelDictionary.ContainsKey(skillID))
            skillLevelDictionary.Add(skillID, 0);

        return skillLevelDictionary[skillID];
    }

    public void LevelUpSkill(int skillID)
    {
        if (!skillLevelDictionary.ContainsKey(skillID))
            skillLevelDictionary.Add(skillID, 0);

        skillLevelDictionary[skillID]++;

        OnChangeSkillData?.Invoke(this);
    }

    public Dictionary<int, int> SkillLevelDictionary { get { return skillLevelDictionary; } set { skillLevelDictionary = value; } }
}
