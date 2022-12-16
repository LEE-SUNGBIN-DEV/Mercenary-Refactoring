public enum CHARACTER_STATE
{
    // Common Character
    Move = 1,
    Attack = 2,
    Defense = 3,
    Skill = 4,
    Roll = 5,

    Hit = 6,
    HeavyHit = 7,

    Stun = 8,

    Compete = 9,
    Die = 10,

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
    Hit = 5,
    HeavyHit = 6,
    Stun = 7,
    Compete = 8,
    Die = 9,

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