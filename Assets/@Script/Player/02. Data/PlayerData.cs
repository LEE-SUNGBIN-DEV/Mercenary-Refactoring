using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("Characters")]
    [SerializeField] private CharacterData[] characterDatas;
    [SerializeField] private int currentCharacterIndex;

    [Header("Option")]
    [SerializeField] private float bgmVolume;
    [SerializeField] private float sfxVolume;

    public PlayerData(bool isFirst)
    {
        if(isFirst)
        {
            characterDatas = new CharacterData[Constants.MAX_CHARACTER_SLOT_NUMBER];
            currentCharacterIndex = 0;

            bgmVolume = 0.5f;
            sfxVolume = 0.5f;
        }
    }

    public CharacterData[] CharacterDatas { get { return characterDatas; } set { characterDatas = value; } }
    public int SelectCharacterIndex { get { return currentCharacterIndex; } set { currentCharacterIndex = value; } }
    public float BgmVolume { get { return bgmVolume; } set { bgmVolume = value; } }
    public float SfxVolume { get { return sfxVolume; } set { sfxVolume = value; } }
}
