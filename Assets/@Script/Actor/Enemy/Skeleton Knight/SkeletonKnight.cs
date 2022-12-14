using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SKELETON_KNIGHT_SKILL
{
    VERTICAL_SLASH,
    HORIZONTAL_SLASH,

    SIZE
}
public class SkeletonKnight : BaseEnemy, ICompetable
{
    private SkeletonKnightVerticalSlash verticalSlash;
    private SkeletonKnightHorizontalSlash horizontalSlash;

    public override void Awake()
    {
        base.Awake();

        verticalSlash = GetComponent<SkeletonKnightVerticalSlash>();
        horizontalSlash = GetComponent<SkeletonKnightHorizontalSlash>();

        skillDictionary = new Dictionary<int, EnemySkill>()
        {
            {0, verticalSlash},
            {1, horizontalSlash}
        };
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    private void Update()
    {
    }

    #region Override Function
    public virtual void OnLightHit()
    {
    }

    public virtual void OnHeavyHit()
    {
        Animator.SetTrigger("doHeavyHit");
    }

    public virtual void OnStun(float duration)
    {
        Animator.SetBool("isMove", false);
        Animator.SetBool("isStun", true);

        StartCoroutine(StunTime());
    }
    public override void OnDie()
    {
        // Die State
        IsDie = true;
        Animator.SetTrigger("doDie");

        StartCoroutine(WaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
    }
    #endregion

    public IEnumerator StunTime(float time = 4f)
    {
        yield return new WaitForSeconds(time);

        Animator.SetBool("isStun", false);
    }

    public void OnCompete()
    {
        if (IsCompete || IsDie)
            return;

        // Initialize Previous State
        verticalSlash.OffVerticalSlashCollider();

        // Compete State
        IsCompete = true;
        Animator.SetTrigger("doCompete");

        StartCoroutine(CompeteTime());
    }
    public IEnumerator CompeteTime()
    {
        yield return new WaitForSeconds(Constants.TIME_COMPETE);
        Animator.SetTrigger("doCompeteAttack");

        yield return new WaitForSeconds(Constants.TIME_COMPETE_ATTACK);
        enemyData.CurrentHP -= (enemyData.MaxHP * 0.1f);
        IsCompete = false;
        Stagger();
    }
    public void Stagger()
    {
        Animator.SetBool("isMove", false);
        Animator.SetBool("isStun", true);

        StartCoroutine(StunTime(Constants.TIME_STAGGER));
    }
    #region Animation Event Function
    public void OutCompete()
    {
        IsCompete = false;
    }
    #endregion

    #region Property
    public bool IsCompete { get; set; }
    #endregion
}
