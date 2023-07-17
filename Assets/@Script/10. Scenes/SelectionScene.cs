using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScene : BaseScene
{
    private UISelectionScene selectionSceneUI;

    public override void Initialize()
    {
        base.Initialize();

        sceneType = SCENE_TYPE.Selection;
        scene = SCENE_LIST.Selection;

        selectionSceneUI = Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_UI_Scene_Selection).GetComponent<UISelectionScene>();
        selectionSceneUI.transform.SetParent(Managers.UIManager.RootObject.transform);
        selectionSceneUI.transform.SetAsFirstSibling();
        selectionSceneUI.Initialize();

        if (selectionSceneUI.gameObject.activeSelf == false)
        {
            selectionSceneUI.gameObject.SetActive(true);
        }

        Managers.GameManager.SetCursorMode(true);
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}
