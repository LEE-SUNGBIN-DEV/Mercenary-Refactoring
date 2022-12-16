public enum HIT_TYPE
{
    // Attack
    Light,
    Heavy,

    // Defense
    Defense,
    Parrying,
}
public enum HIT_STATE
{
    Idle = 1,
    LightHit = 2,
    HeavyHit = 4,
    Invincible = 1024,
    Die = 2048
}

public enum CROWD_CONTROL_TYPE
{
    None = 0,
    Stun = 1,
}
public enum CROWD_CONTROL_STATE
{
    None = 0,
    Stun = 1,
}

public enum PLAYER_ATTACK_TYPE
{
    PlayerComboAttack1,
    PlayerComboAttack2,
    PlayerComboAttack3,
    PlayerComboAttack4,

    PlayerSmashAttack1,
    PlayerSmashAttack2,
    PlayerSmashAttack3,
    PlayerSmashAttack4,

    PlayerDefense,
    PlayerParrying,
    PlayerParryingAttack,
    PlayerCounterAttack,
}