using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BLACK_DRAGON_SKILL
{
    RIGHT_CLAW,
    LEFT_CLAW,
    DOUBLE_ATTACK,
    LAND_BREATH,
    FIRE_BALL,
    FLY_BREATH,
    FLY_LIGHTNING,

    SIZE
}

public class BlackDragon : Enemy, IStaggerable, ICompetable
{
    private BlackDragonRightClaw rightClaw;
    private BlackDragonLeftClaw leftClaw;
    private BlackDragonDoubleAttack doubleAttack;
    private BlackDragonLandBreath landBreath;
    private BlackDragonFireBall fireBall;
    private BlackDragonFlyBreath flyBreath;
    private BlackDragonLightning flyLightning;
    private Dictionary<int, EnemySkill> skillDictionary;

    public override void Awake()
    {
        base.Awake();

        rightClaw = GetComponent<BlackDragonRightClaw>();
        leftClaw = GetComponent<BlackDragonLeftClaw>();
        doubleAttack = GetComponent<BlackDragonDoubleAttack>();
        landBreath = GetComponent<BlackDragonLandBreath>();
        fireBall = GetComponent<BlackDragonFireBall>();
        flyBreath = GetComponent<BlackDragonFlyBreath>();
        flyLightning = GetComponent<BlackDragonLightning>();

        skillDictionary = new Dictionary<int, EnemySkill>();
        skillDictionary.Add(0, rightClaw);
        skillDictionary.Add(1, leftClaw);
        skillDictionary.Add(2, doubleAttack);
        skillDictionary.Add(3, landBreath);
        skillDictionary.Add(4, fireBall);
        skillDictionary.Add(5, flyBreath);
        skillDictionary.Add(6, flyLightning);
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
        if (Target == null || IsAttack || IsStun || IsStagger || IsCompete || IsSpawn || IsDie)
            return;

        FreezeVelocity();
        Move();
        Attack();
    }

    #region Override Function
    public override void Attack()
    {
        int randomNumber = Random.Range(0, (int)BLACK_DRAGON_SKILL.SIZE);

        if(skillDictionary[randomNumber].CheckCondition(DistanceFromTarget))
        {
            skillDictionary[randomNumber].ActiveSkill();
        }
    }

    public override void Hit()
    {
    }

    public override void HeavyHit()
    {
    }

    public override void Stun()
    {
        if (IsStun || IsStagger || IsCompete || IsDie)
            return;

        // Initialize Previous State
        IsMove = false;
        IsAttack = false;

        Animator.SetBool("isMove", false);

        // Stun State
        IsStun = true;
        Animator.SetTrigger("doStun");
    }
    public override void Die()
    {
        InitializeAllState();

        IsDie = true;
        Animator.SetTrigger("doDie");

        StartCoroutine(WaitForDisapear(10f));
    }
    public override void InitializeAllState()
    {
        IsMove = false;
        IsAttack = false;
        IsHit = false;
        IsHeavyHit = false;
        IsStun = false;
        IsStagger = false;
        IsCompete = false;
        IsDie = false;

        Animator.SetBool("isMove", false);
        Animator.SetBool("isDown", false);
    }
    #endregion

    public void Stagger()
    {
        if (IsStagger || IsCompete || IsDie)
            return;

        // Initialize Previous State
        IsMove = false;
        IsAttack = false;
        IsStun = false;

        Animator.SetBool("isMove", false);

        // Down State
        IsStagger = true;
        Animator.SetBool("isDown", true);

        StartCoroutine(StaggerTime());
    }
    private IEnumerator StaggerTime()
    {
        yield return new WaitForSeconds(Constants.TIME_STAGGER);

        IsStagger = false;
        Animator.SetBool("isDown", false);
    }
    public void Compete()
    {
        if (IsCompete || IsDie)
            return;

        // Initialize Previous State
        rightClaw.OffRightClaw();

        IsMove = false;
        IsAttack = false;
        IsStun = false;
        IsStagger = false;

        Animator.SetBool("isMove", false);

        // Compete State
        IsCompete = true;
        Animator.SetTrigger("doCompete");

        StartCoroutine(CompeteTime());
    }
    
    private IEnumerator CompeteTime()
    {
        yield return new WaitForSeconds(Constants.TIME_COMPETE);
        Animator.SetTrigger("doCompeteAttack");

        yield return new WaitForSeconds(Constants.TIME_COMPETE_ATTACK);
        CurrentHitPoint -= (MaxHitPoint * 0.1f);
        IsCompete = false;
        Stagger();
    }

    #region Animation Event Function
    public void SpawnMoveFront()
    {
        NavMeshAgent.speed = 7.0f;
        NavMeshAgent.isStopped = false;
        NavMeshAgent.SetDestination(Target.position);
    }

    public void SpawnMoveStop()
    {
        NavMeshAgent.speed = MoveSpeed;
        NavMeshAgent.isStopped = true;
    }
    public void OutCompete()
    {
        IsMove = false;
        IsAttack = false;
        IsHit = false;
        IsHeavyHit = false;
        IsStun = false;
        IsCompete = false;
    }
    #endregion

    #region Property
    public bool IsCompete { get; set; }
    public bool IsStagger { get; set; }
    #endregion
}
