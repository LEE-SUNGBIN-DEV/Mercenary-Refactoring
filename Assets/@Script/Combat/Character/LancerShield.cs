using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerShield : PlayerDefenseController
{
    private Dictionary<LANCER_DEFENSE_TYPE, CombatInformation> defenseDictionary;

    public override void SetShield(BaseCharacter character)
    {
        base.SetShield(character);
        defenseDictionary = new Dictionary<LANCER_DEFENSE_TYPE, CombatInformation>()
        {
            {LANCER_DEFENSE_TYPE.Defense, new CombatInformation(HIT_TYPE.Defense, 0f, ABNORMAL_TYPE.None, 0f, Vector3.zero, Vector3.zero) },
            {LANCER_DEFENSE_TYPE.Parrying, new CombatInformation(HIT_TYPE.Parrying, 0f, ABNORMAL_TYPE.None, 0f, Vector3.zero, Vector3.zero) },
        };
    }

    #region Called by Owner's Animation Event
    public void OnEnableDefense(LANCER_DEFENSE_TYPE attackType)
    {
        SetCombatInformation(defenseDictionary[attackType]);
        combatCollider.enabled = true;

        GameObject effectObject = null;
        switch (attackType)
        {
            case LANCER_DEFENSE_TYPE.Defense:
            case LANCER_DEFENSE_TYPE.Parrying:

            default:
                break;
        }

        if (effectObject != null)
        {
            effectObject.transform.SetPositionAndRotation(owner.transform.position + defenseDictionary[attackType].effectLocation.position,
                Quaternion.Euler(owner.transform.rotation.eulerAngles + defenseDictionary[attackType].effectLocation.rotation));
        }
    }
    public override void OnDisableDefense()
    {
        base.OnDisableDefense();
    }
    #endregion
}
