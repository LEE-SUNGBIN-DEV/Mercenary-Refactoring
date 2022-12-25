using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerWeapon : PlayerWeapon
{
    private Dictionary<BERSERKER_ATTACK_TYPE, CombatInformation> attackDictionary;

    public override void SetWeapon(BaseCharacter character)
    {
        base.SetWeapon(character);
        attackDictionary = new Dictionary<BERSERKER_ATTACK_TYPE, CombatInformation>()
        {
            // Combo Attack
            {BERSERKER_ATTACK_TYPE.ComboAttack1, new CombatInformation(HIT_TYPE.Light, 1f, ABNORMAL_TYPE.None, 0f, new Vector3(-0.507f, 1.039f, 2.009f), new Vector3(-11.366f, 173.507f, -147.071f)) },
            {BERSERKER_ATTACK_TYPE.ComboAttack2, new CombatInformation(HIT_TYPE.Light, 1.03f, ABNORMAL_TYPE.None, 0f, new Vector3(-0.652f, 0.923f, 1.222f), new Vector3(12.955f, -156.121f, -43.638f)) },
            {BERSERKER_ATTACK_TYPE.ComboAttack3, new CombatInformation(HIT_TYPE.Light, 1.06f, ABNORMAL_TYPE.None, 0f, new Vector3(0.204f, 1.864f, 0.687f), new Vector3(3.054f, -175.796f, 112.129f)) },
            {BERSERKER_ATTACK_TYPE.ComboAttack4, new CombatInformation(HIT_TYPE.Light, 1.09f, ABNORMAL_TYPE.None, 0f, new Vector3(-0.229f, 1.96f, 1.018f), new Vector3(25.3f, -171.152f, -80.728f)) },

            // Smash Attack
            {BERSERKER_ATTACK_TYPE.SmashAttack1, new CombatInformation(HIT_TYPE.Heavy, 1.5f, ABNORMAL_TYPE.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f, -194.508f)) },
            {BERSERKER_ATTACK_TYPE.SmashAttack2, new CombatInformation(HIT_TYPE.Heavy, 1.8f, ABNORMAL_TYPE.None, 0f, new Vector3(-0.147f, 1.110f, 1.349f), new Vector3(19.841f, 9.495f, -53.357f)) },
            {BERSERKER_ATTACK_TYPE.SmashAttack3_1, new CombatInformation(HIT_TYPE.Heavy, 1.5f, ABNORMAL_TYPE.None, 0f, new Vector3(0.312f, 1.192f, 0.016f), new Vector3(46.793f, -180f, 49.641f)) },
            {BERSERKER_ATTACK_TYPE.SmashAttack3_2, new CombatInformation(HIT_TYPE.Heavy, 1.5f, ABNORMAL_TYPE.None, 0f, new Vector3(0.3f, 1.504f, 0.903f), new Vector3(43.677f, 218.04f, 139.807f)) },
            {BERSERKER_ATTACK_TYPE.SmashAttack4_1, new CombatInformation(HIT_TYPE.Heavy, 1.5f, ABNORMAL_TYPE.None, 0f, new Vector3(-0.169f, 2.175f, 1.869f), new Vector3(107.975f, -111.439f, -215.095f)) },
            {BERSERKER_ATTACK_TYPE.SmashAttack4_2, new CombatInformation(HIT_TYPE.Heavy, 2.6f, ABNORMAL_TYPE.None, 0f, new Vector3(0.021f, 1.940f, 1.4f), new Vector3(128.932f, -3.225f, -281.598f)) },

            {BERSERKER_ATTACK_TYPE.ParryingAttack, new CombatInformation(HIT_TYPE.Heavy, 1.5f, ABNORMAL_TYPE.None, 0f, new Vector3(0f, 1.288f, 1.12f), new Vector3(183.757f, -24.78f ,-194.508f)) },
            {BERSERKER_ATTACK_TYPE.CounterAttack, new CombatInformation(HIT_TYPE.Light, 1.5f, ABNORMAL_TYPE.None, 0f, new Vector3(-0.433f, 1.757f, 3.043f), new Vector3(29.494f, -176.467f, -82.104f)) },
        };

        owner.ObjectPooler.RegisterObject(Constants.VFX_Berserker_Combo_Attack, 6);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Berserker_Smash_Attack, 6);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Berserker_Stinger_Attack, 2);
    }

    #region Called by Owner's Animation Event
    public void OnEnableAttack(BERSERKER_ATTACK_TYPE attackType)
    {
        SetCombatInformation(attackDictionary[attackType]);
        combatCollider.enabled = true;

        GameObject effectObject = null;
        switch (attackType)
        {
            case BERSERKER_ATTACK_TYPE.ComboAttack1:
            case BERSERKER_ATTACK_TYPE.ComboAttack2:
            case BERSERKER_ATTACK_TYPE.ComboAttack3:
            case BERSERKER_ATTACK_TYPE.ComboAttack4:
            case BERSERKER_ATTACK_TYPE.CounterAttack:
                effectObject = owner.ObjectPooler.RequestObject(Constants.VFX_Berserker_Combo_Attack);
                break;
            case BERSERKER_ATTACK_TYPE.SmashAttack1:
            case BERSERKER_ATTACK_TYPE.SmashAttack3_1:
            case BERSERKER_ATTACK_TYPE.SmashAttack3_2:
            case BERSERKER_ATTACK_TYPE.SmashAttack4_1:
            case BERSERKER_ATTACK_TYPE.SmashAttack4_2:
                effectObject = owner.ObjectPooler.RequestObject(Constants.VFX_Berserker_Smash_Attack);
                break;

            case BERSERKER_ATTACK_TYPE.SmashAttack2:
                effectObject = owner.ObjectPooler.RequestObject(Constants.VFX_Berserker_Stinger_Attack);
                break;

            default:
                break;
        }
        
        if (effectObject != null)
        {
            effectObject.transform.position = owner.transform.TransformPoint(attackDictionary[attackType].effectLocation.position);
            effectObject.transform.rotation = Quaternion.Euler(owner.transform.rotation.eulerAngles + attackDictionary[attackType].effectLocation.rotation);
        }
    }
    public virtual void OnDisableAttack()
    {
        combatCollider.enabled = false;
        hitDictionary.Clear();
    }
    #endregion
}
