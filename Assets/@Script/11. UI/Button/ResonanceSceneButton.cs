using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ResonanceSceneButton : UIBase
{
    public enum BUTTON
    {
        Prefab_Button_Scene
    }

    public enum TEXT
    {
        Scene_Text
    }

    public event UnityAction<ResonanceSceneButton> OnClickSceneButton;

    [SerializeField] private GameSceneData gameSceneData;
    [SerializeField] private Button sceneButton;
    [SerializeField] private TextMeshProUGUI sceneText;
    [SerializeField] private List<ResonanceRegionButton> regionButtonList;

    public void Initialize(GameSceneData gameSceneData)
    {
        this.gameSceneData = gameSceneData;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        sceneButton = GetButton((int)BUTTON.Prefab_Button_Scene);
        sceneButton.onClick.AddListener(ClickSceneButton);

        sceneText = GetText((int)TEXT.Scene_Text);

        SetSceneButton();
        regionButtonList = new List<ResonanceRegionButton>();
    }

    public void SetSceneButton()
    {
        sceneText.text = gameSceneData.sceneName;
    }

    public void ClickSceneButton()
    {
        OnClickSceneButton?.Invoke(this);
    }

    #region Property
    public GameSceneData GameSceneData { get { return gameSceneData; } }
    public Button SceneButton { get { return sceneButton; } }
    public TextMeshProUGUI SceneText { get { return sceneText; } }
    public List<ResonanceRegionButton> RegionButtonList { get { return regionButtonList; } }
    #endregion
}
