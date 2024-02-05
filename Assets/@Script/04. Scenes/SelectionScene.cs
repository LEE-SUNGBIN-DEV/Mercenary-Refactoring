using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScene : BaseScene
{
    public override void Initialize()
    {
        base.Initialize();

        sceneType = SCENE_TYPE.Selection;
        scene = SCENE_ID.Selection;

        Managers.UIManager.SetCursorMode(CURSOR_MODE.VISIBLE);
        Managers.InputManager.InitializeInputMode(CHARACTER_INPUT_MODE.UI);
        Managers.UIManager.UIScenePanelCanvas.OpenScenePanel(Managers.UIManager.UIScenePanelCanvas.SelectionScenePanel);
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}
