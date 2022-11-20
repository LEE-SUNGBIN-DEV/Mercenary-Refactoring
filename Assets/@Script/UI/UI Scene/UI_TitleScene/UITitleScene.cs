using UnityEngine;
using UnityEngine.Events;

public class UITitleScene : UIBaseScene
{
    enum TEXT
    {
        TitleText,
    }
    enum BUTTON
    {
        StartGameButton,
        QuitButton,
        OptionButton
    }

    public void Initialize()
    {
        if (isInitialized == true)
        {
            Debug.Log($"{this}: Already Initialized.");
            return;
        }
        isInitialized = true;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));
        GetButton((int)BUTTON.StartGameButton).onClick.AddListener(OnClickStartGameButton);
        GetButton((int)BUTTON.QuitButton).onClick.AddListener(OnClickQuitButton);
        GetButton((int)BUTTON.OptionButton).onClick.AddListener(OnClickOptionButton);
    }

    #region Event Function
    public void OnClickStartGameButton()
    {
        Managers.SceneManagerCS.LoadScene(SCENE_LIST.Selection);
    }
    public void OnClickQuitButton()
    {
        Managers.GameManager.SaveAndQuit();
    }
    public void OnClickOptionButton()
    {
        Managers.UIManager.CommonSceneUI.OpenPopup(Managers.UIManager.CommonSceneUI.OptionPopup);
    }
    #endregion
}
