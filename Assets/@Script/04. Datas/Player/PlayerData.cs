using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("Characters")]
    [SerializeField] private CharacterData[] characterDatas;
    [SerializeField] private int currentCharacterIndex;

    [Header("Option Data")]
    [SerializeField] private PlayerOptionData optionData;

    public void Initialize()
    {
        characterDatas = new CharacterData[Constants.MAX_CHARACTER_SLOT_NUMBER];
        currentCharacterIndex = 0;

        optionData = new PlayerOptionData();
        optionData.Initialize();
    }

    public CharacterData[] CharacterDatas { get { return characterDatas; } set { characterDatas = value; } }
    public int CurrentCharacterIndex { get { return currentCharacterIndex; } set { currentCharacterIndex = value; } }
    public PlayerOptionData OptionData { get { return optionData; } set { optionData = value; } }
}
