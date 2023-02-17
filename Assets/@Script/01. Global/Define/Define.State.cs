public enum CHARACTER_STATE
{
    // Common Character
    Idle = 0,
    Walk = 1,
    Run = 2,

    Light_Attack_01 = 16,
    Light_Attack_02 = 17,
    Light_Attack_03 = 18,
    Light_Attack_04 = 19,

    Heavy_Attack_01 = 32,
    Heavy_Attack_02 = 33,
    Heavy_Attack_03 = 34,
    Heavy_Attack_04 = 35,

    Defense = 64,
    Defense_Loop = 65,
    Defense_End = 66,
    Defense_Breaked = 67,
    Parrying = 68,
    Parrying_Attack = 69,

    Skill = 128,

    Roll = 256,

    Light_Hit = 512,
    Heavy_Hit = 513,
    Heavy_Hit_Loop = 514,
    Stand_Up = 515,
    Stand_Roll = 516,

    Compete = 1024,
    Spawn = 2048,
    Die = 4096,

    // Lancer
    // Berserker
}

public enum CHARACTER_STATE_WEIGHT
{
    // Common Character
    Idle = 0,
    Walk = 1,
    Run = 2,

    Light_Attack_01 = 8,
    Light_Attack_02 = 8,
    Light_Attack_03 = 8,
    Light_Attack_04 = 8,

    Heavy_Attack_01 = 8,
    Heavy_Attack_02 = 8,
    Heavy_Attack_03 = 8,
    Heavy_Attack_04 = 8,

    Defense = 8,
    Defense_Loop = 8,
    Defense_End = 8,
    Defense_Break = 8,
    Parrying = 8,
    Parrying_Attack = 8,

    Counter = 32,

    Roll = 64,

    LightHit = 512,
    HeavyHit = 513,
    HeavyHitLoop = 514,
    StandUp = 515,
    StandRoll = 516,

    Compete = 1024,
    Spawn = 2048,
    Die = 4096,

    // Lancer
    // Berserker
}

public enum ENEMY_STATE
{
    Idle,
    Patrol,
    Chase,

    Skill,

    Light_Hit,
    Heavy_Hit,
    Stagger,

    Compete,
    Compete_Defeat,

    Spawn,
    Die
}

public enum ENEMY_STATE_WEIGHT
{
    Idle = 0,
    Patrol = 1,
    Chase = 2,

    Skill = 8,

    Light_Hit = 512,
    Heavy_Hit = 513,
    Stagger = 515,

    Compete = 1024,
    Compete_Defeat = 1025,

    Spawn = 2048,
    Die = 4096,

    // Lancer
    // Berserker
}

public enum INPUT_STATE
{
    Title,
    Selection,
    InGame,
    UI,
}