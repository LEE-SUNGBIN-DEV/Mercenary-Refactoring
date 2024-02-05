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
        UI_START_BUTTON,
        UI_QUIT_BUTTON,
        UI_OPTION_BUTTON
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

        startButton = GetButton((int)BUTTON.UI_START_BUTTON);
        startButton.onClick.AddListener(OnClickStartGameButton);

        quitButton = GetButton((int)BUTTON.UI_QUIT_BUTTON);
        quitButton.onClick.AddListener(OnClickQuitButton);

        optionButton = GetButton((int)BUTTON.UI_OPTION_BUTTON);
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
        Managers.UIManager.UIInteractionPanelCanvas.OptionPanel.TogglePanel();
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
