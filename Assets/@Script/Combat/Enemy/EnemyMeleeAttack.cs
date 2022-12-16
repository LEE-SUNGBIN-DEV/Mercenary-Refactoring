using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyCombatController
{
    public void SetMeleeAttack(BaseEnemy owner)
    {
        this.owner = owner;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other != null)
            ExecuteAttackProcess(other);
    }
}
