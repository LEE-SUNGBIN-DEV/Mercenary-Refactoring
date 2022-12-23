using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : BaseCombatController
{
    [Header("Player Defense")]
    protected BaseCharacter owner;

    public virtual void SetShield(BaseCharacter character)
    {
        owner = character;
        owner.ObjectPooler.RegisterObject(Constants.VFX_Player_Defense, 5);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Player_Parrying, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
            ExecuteCombatProcess(other);
    }

    public virtual void ExecuteCombatProcess(Collider other)
    {
        Vector3 hitPoint = other.bounds.ClosestPoint(transform.position);
        DefenseProcess(other, hitPoint);
    }

    public void DefenseProcess(Collider other, Vector3 hitPoint)
    {
        if (other.GetComponent<EnemyMeleeAttack>() != null)
        {
            GameObject effect = null;
            switch (combatType)
            {
                case HIT_TYPE.Defense:
                    {
                        effect = owner.ObjectPooler.RequestObject(Constants.VFX_Player_Defense);
                        owner.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_BREAKED, true);
                        break;
                    }

                case HIT_TYPE.Parrying:
                    {
                        effect = owner.ObjectPooler.RequestObject(Constants.VFX_Player_Parrying);

                        owner.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_PARRYING, true);
                        owner.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_BREAKED, false);
                        break;
                    }
            }

            if (effect != null)
                effect.transform.position = hitPoint;

            combatCollider.enabled = false;
        }
    }

    public BaseCharacter Owner { get { return owner; } }
}
