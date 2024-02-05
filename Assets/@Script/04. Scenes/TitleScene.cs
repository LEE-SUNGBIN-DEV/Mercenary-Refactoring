using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    public override void Initialize()
    {
        base.Initialize();
        sceneType = SCENE_TYPE.Title;
        scene = SCENE_ID.Title;

        Managers.UIManager.SetCursorMode(CURSOR_MODE.VISIBLE);
        Managers.UIManager.UIScenePanelCanvas.OpenScenePanel(Managers.UIManager.UIScenePanelCanvas.TitleScenePanel);
        Managers.InputManager.InitializeInputMode(CHARACTER_INPUT_MODE.UI);
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}
