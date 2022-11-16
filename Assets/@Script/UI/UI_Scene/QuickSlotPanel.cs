using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotPanel : UIPanel
{
    [SerializeField] private QuickSlot[] quickSlots;

    public override void Initialize()
    {
        quickSlots = GetComponentsInChildren<QuickSlot>();
        Managers.DataManager.CurrentCharacter.CharacterData.OnLoadCharacterData -= LoadPlayerQuickSlots;
        Managers.DataManager.CurrentCharacter.CharacterData.OnLoadCharacterData += LoadPlayerQuickSlots;
        Managers.DataManager.CurrentCharacter.CharacterData.OnSaveCharacterData -= SavePlayerQuickSlots;
        Managers.DataManager.CurrentCharacter.CharacterData.OnSaveCharacterData += SavePlayerQuickSlots;
    }

    private void Update()
    {
        if (Managers.GameSceneManager.CurrentScene.SceneType == SCENE_TYPE.DUNGEON)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UseQuickSlot(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                UseQuickSlot(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                UseQuickSlot(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                UseQuickSlot(3);
            }
        }
    }

    public void UseQuickSlot(int slotIndex)
    {
        if (quickSlots[slotIndex].Item != null)
        {
            PotionItem consumableItem = quickSlots[slotIndex].Item.GetComponent<PotionItem>();

            consumableItem.Consume(Managers.DataManager.CurrentCharacter);
            quickSlots[slotIndex].RemoveItemFromSlot();
        }
    }

    #region Save & Load
    public void LoadPlayerQuickSlots(CharacterData characterData)
    {
        for (int i = 0; i < quickSlots.Length; ++i)
        {
            if (characterData.QuickSlots != null)
            {
                quickSlots[i].SetSlot(characterData.QuickSlots[i]);
            }
            else
            {
                quickSlots[i].ClearSlot();
            }
        }
    }

    public void SavePlayerQuickSlots(CharacterData characterData)
    {
        for (int i = 0; i < quickSlots.Length; ++i)
        {
            if (quickSlots[i].Item != null)
            {
                characterData.QuickSlots[i] = quickSlots[i];
            }
            else
            {
                characterData.QuickSlots[i] = null;
            }
        }
    }
    #endregion

    public QuickSlot[] QuickSlots { get { return quickSlots; } set { quickSlots = value; } }
}
