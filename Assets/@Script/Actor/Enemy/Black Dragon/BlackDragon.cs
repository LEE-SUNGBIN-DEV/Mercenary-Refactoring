using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackDragon : BaseEnemy, IStaggerable, ICompetable
{
    public enum SKILL
    {
        RightClaw,
        LeftClaw,
        DoubleClaw,
        LandBreath,
        FireBall,
        FlyBreath,
        Storm,

        SIZE
    }

    [Header("Skills")]
    private BlackDragonRightClaw rightClaw;
    private BlackDragonLeftClaw leftClaw;
    private BlackDragonDoubleAttack doubleClaw;
    private BlackDragonLandBreath landBreath;
    private BlackDragonFireBall fireBall;
    private BlackDragonFlyBreath flyBreath;
    private BlackDragonStorm storm;

    [Header("State Machine")]
    [SerializeField] protected BlackDragonBehaviourTree behaviourTree;

    public override void Awake()
    {
        base.Awake();

        rightClaw = GetComponentInChildren<BlackDragonRightClaw>(true);
        leftClaw = GetComponentInChildren<BlackDragonLeftClaw>(true);
        doubleClaw = GetComponentInChildren<BlackDragonDoubleAttack>(true);
        landBreath = GetComponentInChildren<BlackDragonLandBreath>(true);
        fireBall = GetComponentInChildren<BlackDragonFireBall>(true);
        flyBreath = GetComponentInChildren<BlackDragonFlyBreath>(true);
        storm = GetComponentInChildren<BlackDragonStorm>(true);

        skillDictionary = new Dictionary<int, EnemySkill>()
        {
            {(int)SKILL.RightClaw, rightClaw },
            {(int)SKILL.LeftClaw, leftClaw },
            {(int)SKILL.DoubleClaw, doubleClaw },
            {(int)SKILL.LandBreath, landBreath },
            {(int)SKILL.FireBall, fireBall },
            {(int)SKILL.FlyBreath, flyBreath },
            {(int)SKILL.Storm, storm }
        };

        foreach(var skill in skillDictionary.Values)
            skill.Initialize(this);

        behaviourTree = new BlackDragonBehaviourTree(this);
        behaviourTree.Initialize();
    }

    private void Update()
    {
        behaviourTree.Update();
    }

    #region Override Function
    public override void OnLightHit()
    {
        state = ENEMY_STATE.Hit;
    }

    public override void OnHeavyHit()
    {
        state = ENEMY_STATE.HeavyHit;
    }

    public override void OnStun()
    {
        state = ENEMY_STATE.Stun;
    }
    public override void Die()
    {
        state = ENEMY_STATE.Die;

        StartCoroutine(WaitForDisapear(10f));
    }
    #endregion

    public void Stagger()
    {
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
        enemyData.CurrentHP -= (enemyData.MaxHP * 0.1f);
        IsCompete = false;
        Stagger();
    }

    #region Property
    public bool IsCompete { get; set; }
    public bool IsStagger { get; set; }
    #endregion
}
