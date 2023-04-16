using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerLocationData
{
    public event UnityAction<PlayerLocationData> OnChangeLocationData;

    [Header("Location")]
    [SerializeField] private SCENE_LIST lastScene;
    [SerializeField] private Vector3 lastPosition;

    public void Initialize()
    {
        lastScene = SCENE_LIST.Fog_Canyon;
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
    #endregion
}
