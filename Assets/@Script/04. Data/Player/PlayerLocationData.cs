using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerLocationData
{
    public event UnityAction<PlayerLocationData> OnChangeLocationData;
    public event UnityAction<PlayerLocationData> OnChangeResonancePointData;

    [Header("Location")]
    [SerializeField] private SCENE_LIST lastScene;
    [SerializeField] private float lastXCoordinate;
    [SerializeField] private float lastYCoordinate;
    [SerializeField] private float lastZCoordinate;

    private Dictionary<SCENE_LIST, bool[]> resonancePointDictionary;

    public void Initialize()
    {
        lastScene = SCENE_LIST.Fog_Canyon;
        lastXCoordinate = 125.8251f;
        lastYCoordinate = 100.0769f;
        lastZCoordinate = 71.98943f;

        resonancePointDictionary = new Dictionary<SCENE_LIST, bool[]>();
        foreach (var gameSceneData in Managers.DataManager.GameSceneTable)
        {
            resonancePointDictionary.Add(gameSceneData.Value.scene, new bool[gameSceneData.Value.resonanceObjectDataList.Count]);
            for (int i = 0; i < resonancePointDictionary[gameSceneData.Value.scene].Length; ++i)
            {
                resonancePointDictionary[gameSceneData.Value.scene][i] = false;
            }
        }
    }

    public Vector3 GetLastPosition()
    {
        return new Vector3(lastXCoordinate, lastYCoordinate, lastZCoordinate);
    }

    public void SetLastPosition(float x, float y, float z)
    {
        SetLastPosition(new Vector3(x, y, z));
    }

    public void SetLastPosition(Vector3 lastPosition)
    {
        lastXCoordinate = lastPosition.x;
        lastYCoordinate = lastPosition.y;
        lastZCoordinate = lastPosition.z;
    }

    public void EnableResonancePoint(SCENE_LIST scene, int index)
    {
        if (resonancePointDictionary[scene][index] == false)
        {
            ResonancePointDictionary[scene][index] = true;
            OnChangeResonancePointData?.Invoke(this);
        }
    }

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

    public float LastXCoordinate { get { return lastXCoordinate; } set { lastXCoordinate = value; } }
    public float LastYCoordinate { get { return lastYCoordinate; } set { lastYCoordinate = value; } }
    public float LastZCoordinate { get { return lastZCoordinate; } set { lastZCoordinate = value; } }

    public Dictionary<SCENE_LIST, bool[]> ResonancePointDictionary
    {
        get { return resonancePointDictionary; }
        set { resonancePointDictionary = value; }
    }
    #endregion
}
