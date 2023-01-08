using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseController : BaseCombatController
{
    [Header("Player Defense")]
    protected BaseCharacter owner;

    public virtual void SetShield(BaseCharacter character)
    {
        owner = character;
        owner.ObjectPooler.RegisterObject(Constants.VFX_Player_Defense, 5);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Player_Parrying, 5);
    }

    public virtual void ExecuteDefenseProcess(EnemyCombatController enemyCombatController, Vector3 hitPoint)
    {
        GameObject effect = null;

        switch (combatType)
        {
            case HIT_TYPE.Defense:
                {
                    effect = owner.ObjectPooler.RequestObject(Constants.VFX_Player_Defense);
                    owner.TrySwitchState(CHARACTER_STATE.Defense_Breaked);
                    break;
                }

            case HIT_TYPE.Parrying:
                {
                    if (enemyCombatController is EnemyCompeteAttack competeController && Managers.CompeteManager.TryCompete(this, competeController))
                        break;

                    effect = owner.ObjectPooler.RequestObject(Constants.VFX_Player_Parrying);
                    owner.TrySwitchState(CHARACTER_STATE.Parrying);
                    break;
                }
        }

        if (effect != null)
            effect.transform.position = hitPoint;

        combatCollider.enabled = false;
    }

    public virtual void OnDisableDefense()
    {
        combatCollider.enabled = false;
        hitDictionary.Clear();
    }

    public BaseCharacter Owner { get { return owner; } }
}
