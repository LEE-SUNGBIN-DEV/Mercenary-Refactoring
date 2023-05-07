using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSlot
{
    public int slotIndex;
    public Vector3 characterPoint;
    public TextMeshProUGUI slotText;
    public Button slotButton;

    public CharacterSlot()
    {
        slotText = null;
        slotButton = null;
    }
}

public class UISelectionScene : UIBaseScene
{
    public enum BUTTON
    {
        // !! Slot button must come before the any other button. (For Initializing)
        Slot_Button_01,
        Slot_Button_02,
        Slot_Button_03,

        // Buttons
        Prefab_Start_Button,
        Prefab_Quit_Button,
        Prefab_Option_Button,
        Character_Remove_Button
    }
    public enum TEXT
    {
        // !! Slot text must come before the any other text. (For Initializing)
        Slot_Text_01,
        Slot_Text_02,
        Slot_Text_03,

        // Other
    }

    private CharacterData[] characterDatas;
    private CharacterSlot[] characterSlots;
    private CharacterSlot selectSlot;

    private Button startButton;
    private Button characterRemoveButton;
    private Button quitButton;
    private Button optionButton;

    public override void Initialize()
    {
        base.Initialize();
        Managers.UIManager.SelectionSceneUI = this;

        BindButton(typeof(BUTTON));
        BindText(typeof(TEXT));

        startButton = GetButton((int)BUTTON.Prefab_Start_Button);
        characterRemoveButton = GetButton((int)BUTTON.Character_Remove_Button);
        quitButton = GetButton((int)BUTTON.Prefab_Quit_Button);
        optionButton = GetButton((int)BUTTON.Prefab_Option_Button);

        startButton.onClick.AddListener(() => { OnClickStartGameButton(selectSlot.slotIndex); });
        characterRemoveButton.onClick.AddListener(() => { OnClickRemoveCharacter(selectSlot.slotIndex); });
        quitButton.onClick.AddListener(OnClickQuitGameButton);
        optionButton.onClick.AddListener(OnClickOptionButton);

        characterSlots = new CharacterSlot[Constants.MAX_CHARACTER_SLOT_NUMBER];
        characterDatas = Managers.DataManager.PlayerData.CharacterDatas;
        selectSlot = null;

        for (int i = 0; i < characterSlots.Length; ++i)
        {
            characterSlots[i] = new CharacterSlot
            {
                slotIndex = i,
                characterPoint = Constants.SELECTION_CHARACTER_POINT[i],
                slotButton = GetButton(i),
                slotText = GetText(i)
            };
        }

        Refresh();
    }

    public void Refresh()
    {
        selectSlot = null;

        startButton.interactable = false;
        characterRemoveButton.interactable = false;

        for (int i=0; i<characterSlots.Length; ++i)
        {
            characterSlots[i].slotButton.onClick.RemoveAllListeners();

            int index = i;
            // Exist Data
            if (characterDatas[i]?.StatusData != null)
            {
                characterSlots[i].slotText.text = "Lv. " + characterDatas[i].StatusData.Level;
                characterSlots[i].slotButton.onClick.AddListener(() => { OnClickCharacterSlot(index); });
            }

            // Don't Exist Data
            else
            {
                characterSlots[i].slotText.text = "Create";
                characterSlots[i].slotButton.onClick.AddListener(() => { OnClickCreateCharacter(index); });
            }

            characterSlots[i].slotButton.interactable = true;
        }
    }

    #region Event Function
    // Selection Scene Panel
    public void OnClickCharacterSlot(int slotIndex)
    {
        selectSlot = characterSlots[slotIndex];

        GetButton((int)BUTTON.Prefab_Start_Button).interactable = true;
        GetButton((int)BUTTON.Character_Remove_Button).interactable = true;
    }

    public void OnClickCreateCharacter(int slotIndex)
    {
        selectSlot = characterSlots[slotIndex];

        Managers.DataManager.PlayerData.CharacterDatas[selectSlot.slotIndex] = new CharacterData();
        Managers.DataManager.PlayerData.CharacterDatas[selectSlot.slotIndex].Initialize();
        Managers.DataManager.SavePlayerData();

        startButton.interactable = false;
        characterRemoveButton.interactable = false;

        Refresh();
    }

    public void OnClickRemoveCharacter(int slotIndex)
    {
        characterDatas[slotIndex] = null;
        Managers.DataManager.SavePlayerData();

        Refresh();
    }

    public void OnClickStartGameButton(int slotIndex)
    {
        startButton.interactable = false;
        characterRemoveButton.interactable = false;

        for (int i = 0; i < characterSlots.Length; ++i)
        {
            characterSlots[i].slotButton.interactable = false;
        }

        Managers.DataManager.SetCurrentCharacter(slotIndex);
        Managers.SceneManagerCS.LoadSceneAsync(Managers.DataManager.CurrentCharacterData.LocationData.LastScene);
    }

    public void OnClickQuitGameButton()
    {
        Managers.GameManager.SaveAndQuit();
    }
    public void OnClickOptionButton()
    {
        Managers.UIManager.TogglePanel(Managers.UIManager.CommonSceneUI.OptionPanel);
    }
    #endregion
}