using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerShield : PlayerDefenseController
{
    public override void SetShield(BaseCharacter character)
    {
        base.SetShield(character);
        defenseDictionary = new Dictionary<DEFENSE_TYPE, CombatInformation>()
        {
            {DEFENSE_TYPE.Defense, new CombatInformation(COMBAT_TYPE.Defense, 0f, BUFF.None, 0f) },
            {DEFENSE_TYPE.Parrying, new CombatInformation(COMBAT_TYPE.Parrying, 0f, BUFF.None, 0f) },
        };
    }

    #region Called by Owner's Animation Event
    public override void OnEnableDefense(DEFENSE_TYPE defenseType)
    {
        SetCombatInformation(defenseDictionary[defenseType]);
        combatCollider.enabled = true;

        GameObject effectObject = null;
        switch (defenseType)
        {
            case DEFENSE_TYPE.Defense:
            case DEFENSE_TYPE.Parrying:

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
