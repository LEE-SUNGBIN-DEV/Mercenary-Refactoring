using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    [Header("Save Datas")]
    [SerializeField] private CharacterData[] characterDatas;
    [SerializeField] private PlayerOptionData optionData;

    [Header("Runtime")]
    [JsonIgnore] private CharacterData currentCharacterData;

    public void CreateData()
    {
        characterDatas = new CharacterData[Constants.MAX_CHARACTER_SLOT_NUMBER];
        currentCharacterData = null;

        optionData = new PlayerOptionData();
        optionData.CreateData();
    }

    public void LoadData()
    {
        if (characterDatas.IsNullOrEmpty())
            return;

        for (int i = 0; i < characterDatas.Length; ++i)
        {
            characterDatas[i]?.LoadData();
        }
    }
    public void UpdateData()
    {
        if (characterDatas.IsNullOrEmpty())
            return;

        for (int i = 0; i < characterDatas.Length; i++)
        {
            characterDatas[i]?.UpdateData();
        }
    }

    public void SaveData()
    {
        if (characterDatas.IsNullOrEmpty())
            return;

        for (int i = 0; i < characterDatas.Length; i++)
        {
            characterDatas[i]?.SaveData();
        }
    }
    public void CreateCharacterData(int index)
    {
        characterDatas[index] = new CharacterData();
        characterDatas[index].CreateData();
        characterDatas[index].LoadData();
        characterDatas[index].UpdateData();
    }
    public void RemoveCharacterData(int index)
    {
        characterDatas[index] = null;
    }
    public void SetCurrentCharacterData(int index)
    {
        currentCharacterData = characterDatas[index];
    }
    public void SetCurrentCharacterData(CharacterData characterData)
    {
        currentCharacterData = characterData;
    }

    public CharacterData[] CharacterDatas { get { return characterDatas; } set { characterDatas = value; } }
    public CharacterData CurrentCharacterData { get { return currentCharacterData; } }
    public PlayerOptionData OptionData { get { return optionData; } set { optionData = value; } }
}
