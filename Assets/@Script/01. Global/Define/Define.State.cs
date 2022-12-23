public enum CHARACTER_STATE
{
    // Common Character
    Move = 1,
    Attack = 2,
    Defense = 3,
    Skill = 4,
    Roll = 5,
    StandRoll = 6,

    LightHit = 512,
    HeavyHit = 513,

    Compete = 1024,
    Die = 2048,

    // Lancer
    // Berserker
}

public enum CHARACTER_STATE_WEIGHT
{
    // Common Character
    Move = 1,
    Attack = 2,
    Defense = 2,
    Counter = 3,
    Roll = 4,
    StandRoll = 4,

    LightHit = 512,
    HeavyHit = 513,

    Compete = 1024,
    Die = 2048,

    // Lancer
    // Berserker
}

public enum ENEMY_STATE
{
    Idle,
    Attack,
    Hit,
    HeavyHit,
    Stun,
    Compete,
    Spawn,
    Die
}

public enum INPUT_STATE
{
    Title,
    Selection,
    InGame,
    UI,
}