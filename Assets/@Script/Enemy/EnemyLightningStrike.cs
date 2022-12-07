using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLightningStrike : EnemyCombatController
{
    private void OnEnable()
    {
        StartCoroutine(OnLightning());
    }

    private IEnumerator OnLightning()
    {
        yield return new WaitForSeconds(1.0f);
        combatCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        combatCollider.enabled = false;
    }
}
