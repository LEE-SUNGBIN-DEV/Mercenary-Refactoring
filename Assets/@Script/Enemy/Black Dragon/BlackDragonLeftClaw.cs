using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonLeftClaw : EnemySkill
{
    [SerializeField] private GameObject attackEffect;

    private void Awake()
    {
        IsRotate = false;
        IsReady = true;
        Owner = GetComponent<BlackDragon>();
    }

    public override void ActiveSkill()
    {
        StartCoroutine(WaitForRotate());
        Owner.Animator.SetTrigger("doLeftClawAttack");
        StartCoroutine(SkillCooldown());
    }

    #region Animation Event Function
    public void OnLeftClaw()
    {
        if (AttackEffect != null)
            AttackEffect.SetActive(true);
    }

    public void OffLeftClaw()
    {
        if (AttackEffect != null)
            AttackEffect.SetActive(false);
    }
    #endregion

    #region Property
    public GameObject AttackEffect
    {
        get { return attackEffect; }
        private set { attackEffect = value; }
    }
    #endregion
}
