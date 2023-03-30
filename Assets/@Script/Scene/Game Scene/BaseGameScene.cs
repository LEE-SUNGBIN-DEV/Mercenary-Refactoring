#define EDITOR_TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameScene : BaseScene
{
    [SerializeField] protected UIGameScene gameSceneUI;
    [SerializeField] protected PlayerCharacter character;
    [SerializeField] protected List<NPC> npcList = new List<NPC>();
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

#if EDITOR_TEST
        character.CharacterData.EquipmentSlotData.Initialize();
#endif
        gameSceneUI = Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_UI_Game_Scene).GetComponent<UIGameScene>();
        gameSceneUI.transform.SetParent(Managers.UIManager.RootObject.transform);
        gameSceneUI.transform.SetAsFirstSibling();
        gameSceneUI.Initialize(character.CharacterData);

        if (gameSceneUI.gameObject.activeSelf == false)
        {
            gameSceneUI.gameObject.SetActive(true);
        }

        RegisterObject(Constants.Prefab_Floating_Damage_Text, 16);
    }

    public override void ExitScene()
    {
        base.ExitScene();
        Managers.NPCManager.NPCDictionary.Clear();
        character = null;
    }

    public Vector3 SpawnPosition { get { return spawnPosition; } }
    public PlayerCharacter Character { get { return character; } }
}
