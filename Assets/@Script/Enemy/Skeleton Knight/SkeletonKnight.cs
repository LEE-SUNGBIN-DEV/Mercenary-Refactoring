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
public class SkeletonKnight : Enemy, ICompetable
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
    public override void Hit()
    {
    }

    public override void HeavyHit()
    {
        if (IsStun || IsCompete || IsDie)
            return;

        IsHeavyHit = true;
        Animator.SetTrigger("doHeavyHit");
    }

    public override void Stun()
    {
        if (IsStun || IsCompete || IsDie)
            return;

        // Initialize Previous State
        IsHeavyHit = false;

        Animator.SetBool("isMove", false);

        // Stun State
        IsStun = true;
        Animator.SetBool("isStun", true);

        StartCoroutine(StunTime());
    }
    public override void Die()
    {
        InitializeAllState();

        // Die State
        IsDie = true;
        Animator.SetTrigger("doDie");

        StartCoroutine(WaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
    }
    public override void InitializeAllState()
    {
        // Initialize Previous State
        IsHit = false;
        IsHeavyHit = false;
        IsStun = false;
        IsCompete = false;

        Animator.SetBool("isMove", false);
        Animator.SetBool("isStun", false);
    }
    #endregion

    public IEnumerator StunTime(float time = 4f)
    {
        yield return new WaitForSeconds(time);

        IsStun = false;
        Animator.SetBool("isStun", false);
    }

    public void Compete()
    {
        if (IsCompete || IsDie)
            return;

        // Initialize Previous State
        verticalSlash.OffVerticalSlashCollider();

        InitializeAllState();

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
        if (IsStun || IsCompete || IsDie)
            return;

        // Initialize Previous State
        IsHeavyHit = false;

        Animator.SetBool("isMove", false);

        // Stun State
        IsStun = true;
        Animator.SetBool("isStun", true);

        StartCoroutine(StunTime(Constants.TIME_STAGGER));
    }
    #region Animation Event Function
    public void OutCompete()
    {
        IsHit = false;
        IsHeavyHit = false;
        IsStun = false;
        IsCompete = false;
    }
    #endregion

    #region Property
    public bool IsCompete { get; set; }
    #endregion
}
