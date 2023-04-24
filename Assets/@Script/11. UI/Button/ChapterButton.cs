using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ChapterButton : UIBase
{
    public enum BUTTON
    {
        Prefab_Chapter_Button
    }

    public enum TEXT
    {
        Prefab_Chapter_Text
    }

    public event UnityAction<ChapterButton> OnClickChapterButton;

    [SerializeField] private GameSceneData gameSceneData;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI chapterText;

    public void Initialize(GameSceneData gameSceneData)
    {
        this.gameSceneData = gameSceneData;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        button = GetButton((int)BUTTON.Prefab_Chapter_Button);
        button.onClick.AddListener(ClickChapterButton);
        chapterText = GetText((int)TEXT.Prefab_Chapter_Text);

        SetChapterButton();
    }

    public void SetChapterButton()
    {
        chapterText.text = gameSceneData.sceneName;
    }

    public void ClickChapterButton()
    {
        OnClickChapterButton?.Invoke(this);
    }

    #region Property
    public GameSceneData ChapterInformation { get { return gameSceneData; } }
    public Button Button { get { return button; } }
    public TextMeshProUGUI QuestTitleText { get { return chapterText; } }
    #endregion
}
