using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonRightClaw : EnemySkill
{
    [SerializeField] private GameObject attackEffect;

    private void Awake()
    {
        isRotate = false;
        isReady = true;
        Owner = GetComponent<BlackDragon>();
    }

    public override void ActiveSkill()
    {
        StartCoroutine(WaitForRotate());
        Owner.Animator.SetTrigger("doRightClawAttack");
        StartCoroutine(SkillCooldown());
    }

    #region Animation Event Function
    public void OnRightClaw()
    {
        if (AttackEffect != null)
            AttackEffect.SetActive(true);
    }

    public void OffRightClaw()
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
