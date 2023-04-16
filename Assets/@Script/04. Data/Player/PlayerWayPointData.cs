using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWayPointData
{
    public event UnityAction<PlayerWayPointData> OnChangeWayPointData;

    [Header("Location")]
    [SerializeField] private bool[][] isEnableWayPoint;

    public void Initialize()
    {
        for(int i=0; i < isEnableWayPoint.Length; ++i)
        {
            for(int j=0; j < isEnableWayPoint[i].Length; ++j)
            {
                isEnableWayPoint[i][j] = false;
            }
        }
    }

    #region Property
    public bool[][] IsEnableResonancePoint
    {
        get { return isEnableWayPoint; }
        set
        {
            isEnableWayPoint = value;
            OnChangeWayPointData?.Invoke(this);
        }
    }
    #endregion
}
