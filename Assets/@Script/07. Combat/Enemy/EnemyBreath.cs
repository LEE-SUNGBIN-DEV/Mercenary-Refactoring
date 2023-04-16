using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBreath : EnemyRayAttack
{
    public override void GenerateMuzzleEffect(Transform muzzle)
    {
        base.GenerateMuzzleEffect(muzzle);
        GameObject requestObject = enemy.ObjectPooler.RequestObject(Constants.VFX_Enemy_Breath);
        requestObject.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
    }
    public override void CollideWithTerrain(RaycastHit hitData)
    {
        base.CollideWithTerrain(hitData);
        GameObject requestObject = enemy.ObjectPooler.RequestObject(Constants.VFX_Enemy_Flame_Area);
        requestObject.transform.position = hitData.point;
    }
    public override void CollideWithPlayer(RaycastHit hitData)
    {
        base.CollideWithPlayer(hitData);
        ExecuteAttackProcess(hitData.collider);
    }
}
