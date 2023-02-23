using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseController : BaseCombatController
{
    [Header("Player Defense Controller")]
    protected BaseCharacter owner;
    protected Dictionary<DEFENSE_TYPE, CombatInformation> defenseDictionary;

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
            case COMBAT_TYPE.Defense:
                {
                    effect = owner.ObjectPooler.RequestObject(Constants.VFX_Player_Defense);
                    owner.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_DEFENSE_BREAK);
                    break;
                }

            case COMBAT_TYPE.Parrying:
                {
                    if (enemyCombatController is EnemyCompeteAttack competeController && Managers.CompeteManager.TryCompete(this, competeController))
                        break;

                    effect = owner.ObjectPooler.RequestObject(Constants.VFX_Player_Parrying);
                    owner.State.TryStateSwitchingByWeight(ACTION_STATE.PLAYER_PARRYING);
                    break;
                }
        }

        if (effect != null)
            effect.transform.position = hitPoint;

        combatCollider.enabled = false;
    }

    public virtual void OnEnableDefense(DEFENSE_TYPE defenseType)
    {
        Debug.Log("Virtual Function Called");
        combatCollider.enabled = true;
    }

    public virtual void OnDisableDefense()
    {
        combatCollider.enabled = false;
        hitDictionary.Clear();
    }

    public BaseCharacter Owner { get { return owner; } }
}
