using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public static readonly string TAG_ENEMY = "Enemy";
    public static readonly string TAG_PLAYER = "Player";

    public const int LAYER_DEFAULT = 0;
    public const int LAYER_TERRAIN = 6;
    public const int LAYER_PLAYER = 7;
    public const int LAYER_ENEMY = 8;
    public const int LAYER_DIE = 9;

    public const int LAYER_HITBOX = 12;
    public const int LAYER_RENDER_TEXTURE = 13;
    public const int LAYER_PREVENT_PUSH = 14;
}
