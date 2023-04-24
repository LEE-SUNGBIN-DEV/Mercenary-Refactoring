using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public static readonly float TIME_ENEMY_MIN_PATROL = 2f;
    public static readonly float TIME_ENEMY_MAX_PATROL = 4f;

    public static readonly float TIME_COMPETE = 5f;
    public static readonly float TIME_COMPETE_COOLDOWN = 60f;

    public static readonly float TIME_COMPETE_ATTACK = 1f;
    public static readonly float TIME_STAGGER = 6f;

    public static readonly float TIME_NORMAL_MONSTER_DISAPEAR = 3f;
    public static readonly float TIME_NAMED_MONSTER_DISAPEAR = 5f;
    public static readonly float TIME_BOSS_MONSTER_DISAPEAR = 10f;

    #region UI
    public static readonly float TIME_CLIENT_NOTICE = 1.6f;
    public static readonly float TIME_GLOBAL_NOTICE = 2.0f;

    public const float TIME_UI_PANEL_DEFAULT_FADE = 0.3f;
    public const float TIME_UI_SCENE_DEFAULT_FADE = 1f;
    #endregion

    public static readonly float TIME_CHARACTER_STAND_UP = 2f;
}
