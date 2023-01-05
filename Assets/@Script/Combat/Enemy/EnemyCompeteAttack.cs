using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCompeteAttack : EnemyCombatController
{
    [Header("Enemy Compete Attack")]
    private Transform competePoint;
    private Transform cameraPoint;

    public void SetCompeteAttack(BaseEnemy owner)
    {
        this.owner = owner;
        competePoint = Functions.FindChild<Transform>(owner.gameObject, "@Compete_Point", true);
        cameraPoint = Functions.FindChild<Transform>(owner.gameObject, "@Camera_Point", true);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other != null)
            ExecuteAttackProcess(other);
    }

    #region Property
    public Transform CompetePoint { get { return competePoint; } }
    public Transform CameraPoint { get { return cameraPoint; } }
    #endregion
}
