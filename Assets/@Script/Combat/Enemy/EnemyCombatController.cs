using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : BaseCombatController
{
    protected Enemy owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHitProcess(other);
        }
    }

    public void PlayerHitProcess(Collider target)
    {
        Character character = target.GetComponent<Character>();
        if (character != null)
        {
            Functions.EnemyDamageProcess(owner, character, DamageRatio);

            switch (CombatType)
            {
                case COMBAT_TYPE.DefaultAttack:
                    {
                        character.SwitchCharacterState(CHARACTER_STATE.Hit);
                        break;
                    }

                case COMBAT_TYPE.SmashAttack:
                    {
                        character.SwitchCharacterState(CHARACTER_STATE.HeavyHit);
                        break;
                    }

                case COMBAT_TYPE.StunAttack:
                    {
                        character.SwitchCharacterState(CHARACTER_STATE.Stun);
                        break;
                    }
            }
        }
    }

    #region Property
    public Enemy Owner { get { return owner; } set { owner = value; } }
    #endregion
}
