using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LocationData
{
    public event UnityAction<LocationData> OnChangeLocationData;

    [Header("Location")]
    [SerializeField] private SCENE_LIST lastViliage;

    public LocationData()
    {
        lastViliage = SCENE_LIST.Forestia;
    }

    #region Property
    public SCENE_LIST LastViliage
    {
        get { return lastViliage; }
        set
        {
            lastViliage = value;
            OnChangeLocationData?.Invoke(this);
        }
    }
    #endregion
}
