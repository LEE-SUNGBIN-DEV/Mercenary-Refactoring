using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterScene : BaseScene
{
    private UISelectCharacterScene selectCharacterSceneUI;

    public override void Initialize()
    {
        base.Initialize();

        sceneType = SCENE_TYPE.Selection;
        scene = SCENE_LIST.Selection;

        selectCharacterSceneUI = Managers.ResourceManager.InstantiatePrefabSync("Prefab_UI_Select_Character_Scene").GetComponent<UISelectCharacterScene>();
        selectCharacterSceneUI.transform.SetParent(Managers.UIManager.RootObject.transform);
        selectCharacterSceneUI.transform.SetAsFirstSibling();
        selectCharacterSceneUI.Initialize();
        if (selectCharacterSceneUI.gameObject.activeSelf == false)
        {
            selectCharacterSceneUI.gameObject.SetActive(true);
        }
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}
