using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ChapterButton : UIBase
{
    public enum TEXT
    {
        Chapter_Text
    }

    public enum BUTTON
    {
        Chapter_Button
    }
    public event UnityAction<ChapterButton> OnClickChapterButton;

    private Button button;
    private TextMeshProUGUI chapterText;
    private WayPointData chapterInformation;

    public void Initialize(WayPointData chapterInformation)
    {
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        chapterText = GetText((int)TEXT.Chapter_Text);
        button = GetButton((int)BUTTON.Chapter_Button);
        button.onClick.AddListener(ClickChapterButton);

        SetChapterButton(chapterInformation);
        this.chapterInformation = chapterInformation;
        chapterText.text = null;
    }

    public void SetChapterButton(WayPointData chapterInformation)
    {
        this.chapterInformation = chapterInformation;
        chapterText.text = chapterInformation.name;
    }

    public void ClickChapterButton()
    {
        OnClickChapterButton?.Invoke(this);
    }

    #region Property
    public WayPointData ChapterInformation { get { return chapterInformation; } }
    public Button Button { get { return button; } }
    public TextMeshProUGUI QuestTitleText { get { return chapterText; } }
    #endregion
}
