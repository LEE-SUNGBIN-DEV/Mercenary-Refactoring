using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSceneData
{
    [SerializeField] private Dictionary<SCENE_LIST, bool> sceneClearDictionary;
    [SerializeField] private Dictionary<int, bool> resonanceGateDictionary;
    [SerializeField] private Dictionary<int, bool> treasureBoxGetDictionary;

    public void CreateData()
    {
        sceneClearDictionary = new Dictionary<SCENE_LIST, bool>();
        resonanceGateDictionary = new Dictionary<int, bool>();
        treasureBoxGetDictionary = new Dictionary<int, bool>();
    }

    public void ModifySceneClearInformation(SCENE_LIST scene, bool isClear)
    {
        if (sceneClearDictionary.ContainsKey(scene))
            sceneClearDictionary[scene] = isClear;
        else
            sceneClearDictionary.Add(scene, isClear);
    }

    public bool IsClearedScene(SCENE_LIST scene)
    {
        if (!sceneClearDictionary.ContainsKey(scene))
            ModifySceneClearInformation(scene, false);

        return sceneClearDictionary[scene];
    }

    public void ModifyResonanceGateInformation(int resonanceGateID, bool isOpen)
    {
        if (resonanceGateDictionary.ContainsKey(resonanceGateID))
            resonanceGateDictionary[resonanceGateID] = isOpen;
        else
            resonanceGateDictionary.Add(resonanceGateID, isOpen);
    }

    public bool IsEnabledResonanceGate(int resonanceGateID)
    {
        if (!resonanceGateDictionary.ContainsKey(resonanceGateID))
            ModifyResonanceGateInformation(resonanceGateID, false);

        return resonanceGateDictionary[resonanceGateID];
    }

    public void ModifyTreasureBoxInformation(int treasureBoxID, bool isGet)
    {
        if (treasureBoxGetDictionary.ContainsKey(treasureBoxID))
            treasureBoxGetDictionary[treasureBoxID] = isGet;
        else
            treasureBoxGetDictionary.Add(treasureBoxID, isGet);
    }

    public bool IsGetTreasureBox(int treasureBoxID)
    {
        if (!treasureBoxGetDictionary.ContainsKey(treasureBoxID))
            ModifyTreasureBoxInformation(treasureBoxID, false);

        return treasureBoxGetDictionary[treasureBoxID];
    }

    public Dictionary<SCENE_LIST, bool> SceneClearDictionary { get { return sceneClearDictionary; } set { sceneClearDictionary = value; } }
    public Dictionary<int, bool> ResonanceGateDictionary { get { return resonanceGateDictionary; } set { resonanceGateDictionary = value; } }
    public Dictionary<int, bool> TreasureBoxGetDictionary { get { return treasureBoxGetDictionary; } set { treasureBoxGetDictionary = value; } }
}
