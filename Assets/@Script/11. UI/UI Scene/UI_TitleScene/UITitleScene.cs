using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITitleScene : UIBaseScene
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

    public override void Initialize()
    {
        base.Initialize();

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        startButton = GetButton((int)BUTTON.Prefab_Start_Button);
        startButton.onClick.AddListener(OnClickStartGameButton);

        quitButton = GetButton((int)BUTTON.Prefab_Quit_Button);
        quitButton.onClick.AddListener(OnClickQuitButton);

        optionButton = GetButton((int)BUTTON.Prefab_Option_Button);
        optionButton.onClick.AddListener(OnClickOptionButton);
    }

    public void OnClickStartGameButton()
    {
        startButton.interactable = false;
        quitButton.interactable = false;
        Managers.SceneManagerCS.LoadSceneFade(SCENE_LIST.Selection);
    }
    public void OnClickQuitButton()
    {
        startButton.interactable = false;
        quitButton.interactable = false;
        Managers.GameManager.SaveAndQuit();
    }
    public void OnClickOptionButton()
    {
        Managers.UIManager.OpenPanel(Managers.UIManager.CommonSceneUI.OptionPanel);
    }
}
