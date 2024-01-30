using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterSceneData : ICharacterData
{
    public event UnityAction<CharacterSceneData> OnChangeSceneData;

    [SerializeField] private Dictionary<string, bool> bossClearDictionary;
    [SerializeField] private Dictionary<string, bool> resonanceGateDictionary;
    [SerializeField] private Dictionary<string, bool> treasureBoxGetDictionary;

    public void CreateData()
    {
        bossClearDictionary = new Dictionary<string, bool>();
        resonanceGateDictionary = new Dictionary<string, bool>();
        treasureBoxGetDictionary = new Dictionary<string, bool>();
    }
    public void LoadData()
    {
    }

    public void UpdateData(CharacterData characterData)
    {

    }
    public void SaveData()
    {

    }

    public void ModifyBossClearInformation(string bossKey, bool isClear)
    {
        if (bossClearDictionary.ContainsKey(bossKey))
            bossClearDictionary[bossKey] = isClear;
        else
            bossClearDictionary.Add(bossKey, isClear);

        OnChangeSceneData?.Invoke(this);
    }

    public bool IsClearedBoss(string bossKey)
    {
        if (!bossClearDictionary.ContainsKey(bossKey))
            ModifyBossClearInformation(bossKey, false);

        return bossClearDictionary[bossKey];
    }

    public void ModifyResonanceGateInformation(string resonanceGateID, bool isOpen)
    {
        if (resonanceGateDictionary.ContainsKey(resonanceGateID))
            resonanceGateDictionary[resonanceGateID] = isOpen;
        else
            resonanceGateDictionary.Add(resonanceGateID, isOpen);

        OnChangeSceneData?.Invoke(this);
    }

    public bool IsEnabledResonanceGate(string resonanceGateID)
    {
        if (!resonanceGateDictionary.ContainsKey(resonanceGateID))
            ModifyResonanceGateInformation(resonanceGateID, false);

        return resonanceGateDictionary[resonanceGateID];
    }

    public void ModifyTreasureBoxInformation(string treasureBoxID, bool isGet)
    {
        if (treasureBoxGetDictionary.ContainsKey(treasureBoxID))
            treasureBoxGetDictionary[treasureBoxID] = isGet;
        else
            treasureBoxGetDictionary.Add(treasureBoxID, isGet);

        OnChangeSceneData?.Invoke(this);
    }

    public bool IsGetTreasureBox(string treasureBoxID)
    {
        if (!treasureBoxGetDictionary.ContainsKey(treasureBoxID))
            ModifyTreasureBoxInformation(treasureBoxID, false);

        return treasureBoxGetDictionary[treasureBoxID];
    }

    public Dictionary<string, bool> BossClearDictionary { get { return bossClearDictionary; } set { bossClearDictionary = value; } }
    public Dictionary<string, bool> ResonanceGateDictionary { get { return resonanceGateDictionary; } set { resonanceGateDictionary = value; } }
    public Dictionary<string, bool> TreasureBoxGetDictionary { get { return treasureBoxGetDictionary; } set { treasureBoxGetDictionary = value; } }
}
