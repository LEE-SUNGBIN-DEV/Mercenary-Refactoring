using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBreath : EnemyRayAttack
{
    public override void GenerateMuzzleEffect(Transform muzzle)
    {
        base.GenerateMuzzleEffect(muzzle);
        GameObject requestObject = owner.ObjectPooler.RequestObject(Constants.VFX_Enemy_Breath);
        requestObject.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
    }
    public override void CollideWithTerrain(RaycastHit hitData)
    {
        base.CollideWithTerrain(hitData);
        GameObject requestObject = owner.ObjectPooler.RequestObject(Constants.VFX_Enemy_Flame_Area);
        requestObject.transform.SetPositionAndRotation(hitData.point + new Vector3(0, 0.36f, 0), Quaternion.Euler(Vector3.zero));
    }
    public override void CollideWithPlayer(RaycastHit hitData)
    {
        base.CollideWithPlayer(hitData);
        ExecuteAttackProcess(hitData.collider);
    }
}
