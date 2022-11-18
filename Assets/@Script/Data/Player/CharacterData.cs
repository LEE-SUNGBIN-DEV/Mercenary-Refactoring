using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterData
{
    [SerializeField] private CharacterStatData statData;
    [SerializeField] private CharacterLocationData locationData;
    [SerializeField] private CharacterItemData itemData;
    [SerializeField] private CharacterQuestData questData;
    
    public CharacterData(CHARACTER_CLASS selectedClass)
    {
        statData = new CharacterStatData(selectedClass);
        locationData = new CharacterLocationData();
        itemData = new CharacterItemData();
        questData = new CharacterQuestData();
    }

    public void GetQuestReward(Quest quest)
    {
        itemData.Money += quest.RewardMoney;
        statData.CurrentExperience += quest.RewardExperience;
    }

    #region Property
    public CharacterStatData StatData { get { return statData; } set { statData = value; } }
    public CharacterLocationData LocationData { get { return locationData; } set { locationData = value; } }
    public CharacterItemData ItemData { get { return itemData; } set { itemData = value; } }
    public CharacterQuestData QuestData { get { return questData; } set { questData = value; } }
    #endregion
}
