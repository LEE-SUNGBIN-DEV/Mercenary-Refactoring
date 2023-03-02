using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerShield : PlayerDefenseController
{
    public override void SetShield(BaseCharacter character)
    {
        base.SetShield(character);
        defenseDictionary = new Dictionary<COMBAT_TYPE, CombatInformation>()
        {
            {COMBAT_TYPE.DEFENSE, new CombatInformation(COMBAT_TYPE.DEFENSE, 0f) },
            {COMBAT_TYPE.PARRYING, new CombatInformation(COMBAT_TYPE.PARRYING, 0f) },
        };
    }

    #region Called by Owner's Animation Event
    public override void OnEnableDefense(COMBAT_TYPE defenseType)
    {
        SetCombatInformation(defenseDictionary[defenseType]);
        combatCollider.enabled = true;

        GameObject effectObject = null;
        switch (defenseType)
        {
            case COMBAT_TYPE.DEFENSE:
            case COMBAT_TYPE.PARRYING:

            default:
                break;
        }

        if (effectObject != null)
        {
            effectObject.transform.SetPositionAndRotation(owner.transform.position + defenseDictionary[defenseType].effectLocation.position,
                Quaternion.Euler(owner.transform.rotation.eulerAngles + defenseDictionary[defenseType].effectLocation.rotation));
        }
    }
    public override void OnDisableDefense()
    {
        base.OnDisableDefense();
    }
    #endregion
}
