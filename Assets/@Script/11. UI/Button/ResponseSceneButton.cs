using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ResponseSceneButton : UIBase
{
    public enum BUTTON
    {
        PREFAB_RESPONSE_POINT_SCENE_BUTTON
    }

    public enum TEXT
    {
        PREFAB_RESPONSE_POINT_SCENE_TEXT
    }

    public event UnityAction<ResponseSceneButton> OnClickSceneButton;

    [SerializeField] private GameSceneData gameSceneData;
    [SerializeField] private Button sceneButton;
    [SerializeField] private TextMeshProUGUI sceneText;
    [SerializeField] private List<ResponseRegionButton> regionButtonList;

    public void Initialize(GameSceneData gameSceneData)
    {
        this.gameSceneData = gameSceneData;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        sceneButton = GetButton((int)BUTTON.PREFAB_RESPONSE_POINT_SCENE_BUTTON);
        sceneButton.onClick.AddListener(ClickSceneButton);

        sceneText = GetText((int)TEXT.PREFAB_RESPONSE_POINT_SCENE_TEXT);

        SetSceneButton();
        regionButtonList = new List<ResponseRegionButton>();
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
    public List<ResponseRegionButton> RegionButtonList { get { return regionButtonList; } }
    #endregion
}
