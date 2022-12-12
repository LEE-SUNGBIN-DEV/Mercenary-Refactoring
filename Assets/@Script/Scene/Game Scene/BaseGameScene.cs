using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameScene : BaseScene
{
    protected UIGameScene gameSceneUI;
    protected BaseCharacter character;
    protected List<NPC> npcList = new List<NPC>();
    [SerializeField] protected Vector3 spawnPosition;

    public override void Initialize()
    {
        base.Initialize();

        // 캐릭터 생성
        if (Managers.DataManager.SelectCharacterData != null)
        {
            character = Functions.CreateCharacterWithCamera(spawnPosition);
        }

        // NPC 등록
        for(int i=0; i<npcList.Count; ++i)
        {
            Managers.NPCManager.NPCDictionary.Add(npcList[i].NpcID, npcList[i]);
            npcList[i].Initialize();
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
        Managers.NPCManager.NPCDictionary.Clear();
        character = null;
    }

    public Vector3 SpawnPosition { get { return spawnPosition; } }
    public BaseCharacter Character { get { return character; } }
}
