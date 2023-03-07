using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseController : BaseCombatController
{
    [Header("Player Defense Controller")]
    protected BaseCharacter character;
    protected Dictionary<COMBAT_TYPE, CombatInfo> defenseDictionary;

    public virtual void SetShield(BaseCharacter character)
    {
        this.character = character;
        this.character.ObjectPooler.RegisterObject(Constants.VFX_Player_Defense, 5);
        this.character.ObjectPooler.RegisterObject(Constants.VFX_Player_Parrying, 5);
    }

    public virtual void ExecuteDefenseProcess(EnemyCombatController enemyCombatController, Vector3 hitPoint)
    {
        GameObject effect = null;

        switch (combatType)
        {
            case COMBAT_TYPE.DEFENSE:
                {
                    effect = character.ObjectPooler.RequestObject(Constants.VFX_Player_Defense);
                    character.State.SetState(ACTION_STATE.PLAYER_DEFENSE_BREAK, STATE_SWITCH_BY.WEIGHT);
                    break;
                }

            case COMBAT_TYPE.PARRYING:
                {
                    if (enemyCombatController is EnemyCompeteAttack competeController && Managers.CompeteManager.TryCompete(this, competeController))
                        break;

                    effect = character.ObjectPooler.RequestObject(Constants.VFX_Player_Parrying);
                    character.State.SetState(ACTION_STATE.PLAYER_PARRYING, STATE_SWITCH_BY.WEIGHT);
                    break;
                }
        }

        if (effect != null)
            effect.transform.position = hitPoint;

        combatCollider.enabled = false;
    }

    public virtual void OnEnableDefense(COMBAT_TYPE defenseType)
    {
        Debug.Log("Virtual Function Called");
        combatCollider.enabled = true;
    }

    public virtual void OnDisableDefense()
    {
        combatCollider.enabled = false;
        hitDictionary.Clear();
    }

    public BaseCharacter Character { get { return character; } }
}
