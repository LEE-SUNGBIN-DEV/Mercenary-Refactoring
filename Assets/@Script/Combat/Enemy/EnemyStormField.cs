using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStormField : MonoBehaviour
{
    [SerializeField] private BaseEnemy owner;
    [SerializeField] private int amount;
    [SerializeField] private float interval;

    public void Initialize(BaseEnemy owner, int amount, float interval)
    {
        this.owner = owner;
        this.amount = amount;
        this.interval = interval;
        Managers.SceneManagerCS.CurrentScene.RegisterObject("Prefab_VFX_Enemy_Lightning_Strike", amount);
    }

    public IEnumerator GenerateLightningStrike()
    {
        WaitForSeconds waitTime = new WaitForSeconds(interval);

        for (int i = 0; i < amount; ++i)
        {
            Vector3 generateCoordinate = Functions.GetRandomCircleCoordinate(24f);

            if(Managers.SceneManagerCS.CurrentScene.RequestObject("Prefab_VFX_Enemy_Lightning_Strike").TryGetComponent(out EnemyLightningStrike lightningStrike))
            {
                lightningStrike.Owner = owner;
                lightningStrike.transform.position = generateCoordinate;
                lightningStrike.SetCombatController(HIT_TYPE.Heavy, CC_TYPE.Stun, 1.2f);
            }

            yield return waitTime;
        }
    }
}
