using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : GameScene
{
    protected override void Awake()
    {
        base.Awake();
        sceneType = SCENE_TYPE.Unknown;
    }
}
