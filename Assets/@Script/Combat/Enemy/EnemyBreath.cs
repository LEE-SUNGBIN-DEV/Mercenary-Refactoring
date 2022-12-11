using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBreath : EnemyCombatController
{
    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
    }

    public override void CollideWithTerrain(RaycastHit hitData)
    {
        GameObject requestObject = owner.RequestObject("Enemy Flame Area");
        requestObject.transform.SetPositionAndRotation(hitData.point, Quaternion.Euler(rotationOffset));
    }
    public override void CollideWithPlayer(RaycastHit hitData)
    {
        ExecuteCombatProcess(hitData.collider);
    }
}
