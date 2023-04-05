using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFogCanyon : BaseGameScene
{
    public override void Initialize()
    {
        base.Initialize();

        mapName = Constants.SCENE_NAME_FOG_CANYON;

        SpawnEnemy();
    }

    private void Start()
    {
        Managers.EnvironmentManager.SetWeather(EnvironmentManager.WEATHER_TYPE.Rain_Light);
    }
}
