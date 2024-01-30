using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum LOCATION_MODE
{
    SCENE_DEFAULT,
    SCENE_POSITION,
    SCENE_RESPONSE_POINT,
    SCENE_RESPONSE_GATE,
    SCENE_BOSS_ROOM
}

[System.Serializable]
public class CharacterLocationData : ICharacterData
{
    public event UnityAction<CharacterLocationData> OnChangeLocationData;
    public event UnityAction<CharacterLocationData> OnChangeResponsePointData;

    [Header("Save Datas")]
    [JsonProperty] [SerializeField] private SCENE_ID lastScene;
    [JsonProperty] [SerializeField] private LOCATION_MODE locationMode;

    [JsonProperty] [SerializeField] private string lastResponseCrystalID;
    [JsonProperty] [SerializeField] private string lastResponseGateID;
    [JsonProperty] [SerializeField] private string lastBossRoomID;

    [JsonProperty] [SerializeField] private float lastLocationX;
    [JsonProperty] [SerializeField] private float lastLocationY;
    [JsonProperty] [SerializeField] private float lastLocationZ;

    [JsonProperty] private HashSet<string> lockedPointHashSet;
    [JsonProperty] private HashSet<string> unlockedPointHashSet;

    public void CreateData()
    {
        lastScene = SCENE_ID.Fog_Canyon;
        SetLocationMode(LOCATION_MODE.SCENE_DEFAULT);
        lastResponseCrystalID = null;
        lastLocationX = 0f;
        lastLocationY = 0f;
        lastLocationZ = 0f;

        unlockedPointHashSet = new HashSet<string>();
        lockedPointHashSet = new HashSet<string>();

        foreach (var responseCrystal in Managers.DataManager.ResponseCrystalTable)
            lockedPointHashSet.Add(responseCrystal.Value.responseCrystalID);
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

    #region Get Last Location Info
    public Vector3 GetCharacterLastLocation(GameScene gameScene)
    {
        Vector3 characterPosition;

        switch (locationMode)
        {
            case LOCATION_MODE.SCENE_DEFAULT:
                characterPosition = gameScene.PlayerDefaultPosition;
                break;

            case LOCATION_MODE.SCENE_POSITION:
                characterPosition = new Vector3(lastLocationX, lastLocationY, lastLocationZ);
                break;

            case LOCATION_MODE.SCENE_RESPONSE_POINT:
                if(lastResponseCrystalID == null)
                    characterPosition = gameScene.PlayerDefaultPosition;
                else
                    characterPosition = gameScene.ResponseCrystalsDictionary[lastResponseCrystalID].WarpPointTransform.position;
                break;

            case LOCATION_MODE.SCENE_RESPONSE_GATE:
                if (lastResponseGateID == null)
                    characterPosition = gameScene.PlayerDefaultPosition;
                else
                    characterPosition = gameScene.ResponseGateDictionary[lastResponseGateID].WarpPointTransform.position;
                break;

            case LOCATION_MODE.SCENE_BOSS_ROOM:
                characterPosition = gameScene.BossRoomDictionary[lastBossRoomID].RespawnTransform.position;
                break;

            default:
                characterPosition = gameScene.PlayerDefaultPosition;
                break;
        }

        return characterPosition;
    }
    public SCENE_ID GetLastResponsePointScene(int resonanceID)
    {
        return (SCENE_ID)(resonanceID / 100);
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
    public void SetLastResponsePoint(string responseCrystalID)
    {
        locationMode = LOCATION_MODE.SCENE_RESPONSE_POINT;
        lastResponseCrystalID = responseCrystalID;
    }
    public void SetLastResponseGate(string responseGateID)
    {
        locationMode = LOCATION_MODE.SCENE_RESPONSE_GATE;
        lastResponseGateID = responseGateID;
    }
    public void SetLastBossRoom(string bossRoomID)
    {
        locationMode = LOCATION_MODE.SCENE_BOSS_ROOM;
        lastBossRoomID = bossRoomID;
    }
    #endregion

    #region Response Point
    public void EnableResponsePoint(string responseCrystalID, bool isEnable)
    {
        SetLastResponsePoint(responseCrystalID);
        if (lockedPointHashSet.Contains(responseCrystalID))
        {
            lockedPointHashSet.Remove(responseCrystalID);
            unlockedPointHashSet.Add(responseCrystalID);
        }
        OnChangeResponsePointData?.Invoke(this);
    }
    #endregion

    #region Property
    public SCENE_ID LastScene { get { return lastScene; } set { lastScene = value; OnChangeLocationData?.Invoke(this); } }
    public HashSet<string> LockedPointHashSet { get { return lockedPointHashSet; } }
    public HashSet<string> UnlockedPointHashSet { get { return unlockedPointHashSet; } }
    #endregion
}
