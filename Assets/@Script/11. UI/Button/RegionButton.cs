using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class RegionButton : UIBase
{
    public enum TEXT
    {
        Region_Text
    }

    public enum BUTTON
    {
        Region_Button
    }
    public event UnityAction<RegionButton> OnClickRegionButton;

    private Button button;
    private TextMeshProUGUI regionText;
    private WayPointObjectData wayPointData;

    public void Initialize(WayPointObjectData wayPointData)
    {
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        regionText = GetText((int)TEXT.Region_Text);
        button = GetButton((int)BUTTON.Region_Button);
        button.onClick.AddListener(ClickRegionButton);

        SetRegionButton(wayPointData);
    }

    public void SetRegionButton(WayPointObjectData wayPointData)
    {
        this.wayPointData = wayPointData;
        regionText.text = wayPointData.name;
    }

    public void ClickRegionButton()
    {
        OnClickRegionButton?.Invoke(this);
    }

    #region Property
    public WayPointObjectData ResonanceInformation { get { return wayPointData; } }
    public Button Button { get { return button; } }
    public TextMeshProUGUI QuestTitleText { get { return regionText; } }
    #endregion
}
