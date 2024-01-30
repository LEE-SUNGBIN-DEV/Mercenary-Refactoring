using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TitleScenePanel : UIScenePanel
{
    enum TEXT
    {
        Title_Text,
    }
    enum BUTTON
    {
        Prefab_Start_Button,
        Prefab_Quit_Button,
        Prefab_Option_Button
    }

    private Button startButton;
    private Button quitButton;
    private Button optionButton;

    #region Private
    protected override void Awake()
    {
        base.Awake();

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        startButton = GetButton((int)BUTTON.Prefab_Start_Button);
        startButton.onClick.AddListener(OnClickStartGameButton);

        quitButton = GetButton((int)BUTTON.Prefab_Quit_Button);
        quitButton.onClick.AddListener(OnClickQuitButton);

        optionButton = GetButton((int)BUTTON.Prefab_Option_Button);
        optionButton.onClick.AddListener(OnClickOptionButton);
    }
    #endregion

    public void OnClickStartGameButton()
    {
        startButton.interactable = false;
        quitButton.interactable = false;
        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Start);
        Managers.SceneManagerEX.LoadSceneFade(SCENE_ID.Selection);
    }
    public void OnClickQuitButton()
    {
        startButton.interactable = false;
        quitButton.interactable = false;
        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Quit);
        Managers.GameManager.SaveAndQuit();
    }
    public void OnClickOptionButton()
    {
        Managers.AudioManager.PlaySFX(Constants.Audio_Button_Click_Normal);
        Managers.UIManager.UIFocusPanelCanvas.SwitchFocusPanel(Managers.UIManager.UIFocusPanelCanvas.OptionPanel);
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
