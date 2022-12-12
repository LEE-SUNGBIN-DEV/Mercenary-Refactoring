using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBreath : EnemyCombatController
{
    public override void Initialize(BaseEnemy owner)
    {
        if (isInitialized == false)
        {
            base.Initialize(owner);
            Managers.SceneManagerCS.CurrentScene.RegisterObject("Prefab_Effect_Enemy_Breath", 20);
            Managers.SceneManagerCS.CurrentScene.RegisterObject("Prefab_Effect_Enemy_Flame_Area", 20);
            isInitialized = true;
        }
    }

    public override void GenerateMuzzleEffect(Transform muzzle)
    {
        base.GenerateMuzzleEffect(muzzle);
        GameObject requestObject = Managers.SceneManagerCS.CurrentScene.RequestObject("Prefab_Effect_Enemy_Breath");
        requestObject.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
    }
    public override void CollideWithTerrain(RaycastHit hitData)
    {
        base.CollideWithTerrain(hitData);
        GameObject requestObject = Managers.SceneManagerCS.CurrentScene.RequestObject("Prefab_Effect_Enemy_Flame_Area");
        requestObject.transform.SetPositionAndRotation(hitData.point + new Vector3(0, 0.36f, 0), Quaternion.Euler(rotationOffset));
    }
    public override void CollideWithPlayer(RaycastHit hitData)
    {
        base.CollideWithPlayer(hitData);
        ExecuteCombatProcess(hitData.collider);
    }
}
