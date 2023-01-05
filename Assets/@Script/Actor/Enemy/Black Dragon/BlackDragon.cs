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

        state = new EnemyStateController(this);
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    private void Update()
    {
        BaseEnemyBehaviour();
        state.Update();
    }

    public override void OnBirth()
    {
        base.OnBirth();
        state.TrySwitchState(ENEMY_STATE.Birth);
    }

    public override void OnDie()
    {
        base.OnDie();
        StartCoroutine(WaitForDisapear(10f));
    }
       
    public void OnStagger() { state.TrySwitchState(ENEMY_STATE.Stagger); }
    public void OnCompete() { state.TrySwitchState(ENEMY_STATE.Compete); }
}
