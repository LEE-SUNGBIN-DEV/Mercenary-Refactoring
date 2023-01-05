public enum CHARACTER_STATE
{
    // Common Character
    Move = 1,

    Combo_1 = 16,
    Combo_2 = 17,
    Combo_3 = 18,
    Combo_4 = 19,

    Smash_1 = 32,
    Smash_2 = 33,
    Smash_3 = 34,
    Smash_4 = 35,

    Defense = 64,
    Defense_Breaked = 65,
    Parrying = 66,
    Parrying_Attack = 67,

    Skill = 128,

    Roll = 256,
    StandRoll = 257,

    LightHit = 512,
    HeavyHit = 513,

    Compete = 1024,
    Spawn = 2048,
    Die = 4096,

    // Lancer
    // Berserker
}

public enum CHARACTER_STATE_WEIGHT
{
    // Common Character
    Move = 1,

    Attack = 8,
    Defense = 8,

    Defense_Breaked = 9,
    Parrying = 9,

    Counter = 32,

    Roll = 64,
    StandRoll = 64,

    LightHit = 512,
    HeavyHit = 513,

    Compete = 1024,
    Spawn = 2048,
    Die = 4096,

    // Lancer
    // Berserker
}

public enum ENEMY_STATE
{
    Idle,
    Move,
    Skill,

    LightHit,
    HeavyHit,
    Stagger,

    Compete,
    Birth,
    Die
}

public enum ENEMY_STATE_WEIGHT
{
    Idle = 0,
    Move = 1,

    Skill = 8,

    LightHit = 512,
    HeavyHit = 513,
    Stagger = 515,

    Compete = 1024,
    Birth = 2048,
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

public enum KEY_STATE
{
    None,
    Down,
    Click,
    Press,
}