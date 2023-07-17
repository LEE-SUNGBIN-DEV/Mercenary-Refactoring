using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum LOCATION_MODE
{
    SCENE_DEFAULT,
    SCENE_POSITION,
    SCENE_RESONANCE_POINT
}

[System.Serializable]
public class CharacterLocationData
{
    public event UnityAction<CharacterLocationData> OnChangeLocationData;
    public event UnityAction<CharacterLocationData> OnChangeResonancePointData;

    [Header("Location")]
    [SerializeField] private SCENE_LIST lastScene;
    [SerializeField] private LOCATION_MODE locationMode;

    [SerializeField] private int lastResonanceID;

    [SerializeField] private float lastLocationX;
    [SerializeField] private float lastLocationY;
    [SerializeField] private float lastLocationZ;

    private Dictionary<int, bool> resonancePointDictionary;

    public void CreateData()
    {
        lastScene = SCENE_LIST.Fog_Canyon;
        SetLocationMode(LOCATION_MODE.SCENE_DEFAULT);
        lastResonanceID = 0;
        lastLocationX = 0f;
        lastLocationY = 0f;
        lastLocationZ = 0f;

        resonancePointDictionary = new Dictionary<int, bool>();

        foreach (var resonanceObjectData in Managers.DataManager.ResonanceCrystalTable)
            resonancePointDictionary.Add(resonanceObjectData.Value.id, false);
    }

    #region Get Last Location Info
    public Vector3 GetCharacterLastLocation(GameScene baseGameScene)
    {
        Vector3 characterPosition;

        switch (locationMode)
        {
            case LOCATION_MODE.SCENE_DEFAULT:
                characterPosition = baseGameScene.PlayerDefaultPosition;
                break;
            case LOCATION_MODE.SCENE_POSITION:
                characterPosition = new Vector3(lastLocationX, lastLocationY, lastLocationZ);
                break;
            case LOCATION_MODE.SCENE_RESONANCE_POINT:
                if(lastResonanceID == 0)
                    characterPosition = baseGameScene.PlayerDefaultPosition;
                else
                    characterPosition = baseGameScene.ResonanceCrystals[GetLastResonancePointIndex()].transform.position;
                break;

            default:
                characterPosition = baseGameScene.PlayerDefaultPosition;
                break;
        }

        return characterPosition;
    }
    public int GetLastResonancePointID()
    {
        return lastResonanceID;
    }
    public SCENE_LIST GetLastResonancePointScene(int resonanceID)
    {
        return (SCENE_LIST)(resonanceID / 100);
    }
    public int GetLastResonancePointIndex()
    {
        return lastResonanceID % 100;
    }
    #endregion

    #region Set Last Location Info
    public void SetLocationMode(LOCATION_MODE locationMode)
    {
        this.locationMode = locationMode;
    }
    public void SetLastPosition(Vector3 lastPosition)
    {
        lastLocationX = lastPosition.x;
        lastLocationY = lastPosition.y;
        lastLocationZ = lastPosition.z;
    }
    public void SetLastResonancePoint(int resonanceID)
    {
        locationMode = LOCATION_MODE.SCENE_RESONANCE_POINT;
        lastResonanceID = resonanceID;
    }
    #endregion

    #region Resonance Point
    public void EnableResonancePoint(int resonanceID, bool isEnable)
    {
        SetLastResonancePoint(resonanceID);
        if (!resonancePointDictionary.ContainsKey(resonanceID))
            resonancePointDictionary.Add(resonanceID, isEnable);

        if (resonancePointDictionary[resonanceID] == false)
            resonancePointDictionary[resonanceID] = true;

        OnChangeResonancePointData?.Invoke(this);
    }
    #endregion

    #region Property
    public SCENE_LIST LastScene
    {
        get { return lastScene; }
        set
        {
            lastScene = value;
            OnChangeLocationData?.Invoke(this);
        }
    }

    public float LastLocationX { get { return lastLocationX; } set { lastLocationX = value; } }
    public float LastLocationY { get { return lastLocationY; } set { lastLocationY = value; } }
    public float LastLocationZ { get { return lastLocationZ; } set { lastLocationZ = value; } }

    public Dictionary<int, bool> ResonancePointDictionary
    {
        get { return resonancePointDictionary; }
        set { resonancePointDictionary = value; }
    }
    #endregion
}
