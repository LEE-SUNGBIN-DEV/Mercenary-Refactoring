using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonLightning : EnemySkill
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
        Owner.Animator.SetTrigger("doLightning");
        StartCoroutine(SkillCooldown());
    }

    #region Animation Event Function
    public void OnLightning()
    {
        if (AttackEffect != null)
            AttackEffect.SetActive(true);
    }

    public void OffLightning()
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
