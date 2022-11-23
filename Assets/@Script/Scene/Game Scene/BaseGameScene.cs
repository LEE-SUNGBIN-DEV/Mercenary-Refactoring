using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameScene : BaseScene
{
    protected UIGameScene gameSceneUI;
    protected Character character;
    [SerializeField] protected Vector3 spawnPosition;

    public override void Initialize()
    {
        base.Initialize();

        if (Managers.DataManager.SelectCharacterData != null)
        {
            character = Functions.CreateCharacterWithCamera(spawnPosition);
        }

        gameSceneUI = Managers.ResourceManager.InstantiatePrefabSync("Prefab_UI_Game_Scene").GetComponent<UIGameScene>();
        gameSceneUI.transform.SetParent(Managers.UIManager.RootObject.transform);
        gameSceneUI.transform.SetAsFirstSibling();
        gameSceneUI.Initialize(character.CharacterData);

        if (gameSceneUI.gameObject.activeSelf == false)
        {
            gameSceneUI.gameObject.SetActive(true);
        }
    }

    public override void ExitScene()
    {
        base.ExitScene();
        character = null;
    }

    public Vector3 SpawnPosition { get { return spawnPosition; } }
    public Character Character { get { return character; } }
}
