using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSlot
{
    public int slotIndex;
    public TextMeshProUGUI slotText;
    public Button slotButton;

    public CharacterSlot()
    {
        slotText = null;
        slotButton = null;
    }
}

public class SelectionScenePanel : UIScenePanel
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

    private CharacterSlot[] characterSlots;
    private CharacterSlot selectedCharacterSlot;

    private Button startButton;
    private Button characterRemoveButton;
    private Button quitButton;
    private Button optionButton;

    #region Private
    protected override void Awake()
    {
        base.Awake();

        BindButton(typeof(BUTTON));
        BindText(typeof(TEXT));

        startButton = GetButton((int)BUTTON.Prefab_Start_Button);
        characterRemoveButton = GetButton((int)BUTTON.Character_Remove_Button);
        quitButton = GetButton((int)BUTTON.Prefab_Quit_Button);
        optionButton = GetButton((int)BUTTON.Prefab_Option_Button);

        startButton.onClick.AddListener( () => { OnClickStartGameButton(selectedCharacterSlot.slotIndex); });
        characterRemoveButton.onClick.AddListener( () => { OnClickRemoveCharacter(selectedCharacterSlot.slotIndex); });
        quitButton.onClick.AddListener(OnClickQuitGameButton);
        optionButton.onClick.AddListener(OnClickOptionButton);

        characterSlots = new CharacterSlot[Constants.MAX_CHARACTER_SLOT_NUMBER];
        selectedCharacterSlot = null;

        for (int i = 0; i < characterSlots.Length; ++i)
        {
            characterSlots[i] = new CharacterSlot
            {
                slotIndex = i,
                slotButton = GetButton(i),
                slotText = GetText(i)
            };
        }

        UpdatePanel();
    }

    private void OnClickCharacterSlot(int slotIndex)
    {
        selectedCharacterSlot = characterSlots[slotIndex];

        startButton.interactable = true;
        characterRemoveButton.interactable = true;
        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Normal);
    }
    private void OnClickCreateCharacter(int slotIndex)
    {
        selectedCharacterSlot = characterSlots[slotIndex];

        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Normal);
        Managers.DataManager.PlayerData.CreateCharacterData(slotIndex);
        Managers.DataManager.SavePlayerData();

        startButton.interactable = false;
        characterRemoveButton.interactable = false;

        UpdatePanel();
    }
    private void OnClickRemoveCharacter(int slotIndex)
    {
        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Normal);
        Managers.DataManager.PlayerData.RemoveCharacterData(slotIndex);
        Managers.DataManager.SavePlayerData();

        UpdatePanel();
    }
    private void OnClickStartGameButton(int slotIndex)
    {
        startButton.interactable = false;
        characterRemoveButton.interactable = false;

        for (int i = 0; i < characterSlots.Length; ++i)
        {
            characterSlots[i].slotButton.interactable = false;
        }

        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Start);
        Managers.DataManager.PlayerData.SetCurrentCharacterData(slotIndex);
        Managers.SceneManagerEX.LoadSceneAsync(Managers.DataManager.CurrentCharacterData.LocationData.LastScene);
    }
    private void OnClickQuitGameButton()
    {
        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Quit);
        Managers.GameManager.SaveAndQuit();
    }
    private void OnClickOptionButton()
    {
        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Normal);
        Managers.UIManager.UIFocusPanelCanvas.SwitchFocusPanel(Managers.UIManager.UIFocusPanelCanvas.OptionPanel);
    }
    #endregion

    public void UpdatePanel()
    {
        PlayerSaveData playerData = Managers.DataManager.PlayerData;
        if (playerData != null)
        {
            selectedCharacterSlot = null;

            startButton.interactable = false;
            characterRemoveButton.interactable = false;

            for (int i = 0; i < characterSlots.Length; ++i)
            {
                characterSlots[i].slotButton.onClick.RemoveAllListeners();

                int index = i;
                // Exist Data
                if ((playerData.CharacterDatas[i]?.StatusData) != null)
                {
                    characterSlots[i].slotText.text =
                        "Lv. " + playerData.CharacterDatas[i].StatusData.Level + "\n"
                        + Managers.DataManager.GameSceneTable[playerData.CharacterDatas[i].LocationData.LastScene].sceneName;
                    characterSlots[i].slotButton.onClick.AddListener(() => { OnClickCharacterSlot(index); });
                }

                // Don't Exist Data
                else
                {
                    characterSlots[i].slotText.text = "»ý¼º";
                    characterSlots[i].slotButton.onClick.AddListener(() => { OnClickCreateCharacter(index); });
                }

                characterSlots[i].slotButton.interactable = true;
            }
        }
    }

    /// <summary> Please Use Function in Scene Panel Canvas </summary>
    public override void OpenScenePanel()
    {
        gameObject.SetActive(true);
    }

    /// <summary> Please Use Function in Scene Panel Canvas </summary>
    public override void CloseScenePanel()
    {
        gameObject.SetActive(false);
    }
}