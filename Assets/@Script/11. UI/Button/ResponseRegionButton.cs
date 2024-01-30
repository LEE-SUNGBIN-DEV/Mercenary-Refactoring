using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ResponseRegionButton : UIBase
{
    public enum BUTTON
    {
        PREFAB_RESPONSE_POINT_REGION_BUTTON
    }

    public enum TEXT
    {
        PREFAB_RESPONSE_POINT_REGION_TEXT
    }

    public event UnityAction<ResponseRegionButton> OnClickRegionButton;

    [SerializeField] private ResponseCrystalData responseCrystalData;
    [SerializeField] private Button resgionButton;
    [SerializeField] private TextMeshProUGUI regionText;

    public void Initialize(ResponseCrystalData responseCrystalData)
    {
        this.responseCrystalData = responseCrystalData;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        resgionButton = GetButton((int)BUTTON.PREFAB_RESPONSE_POINT_REGION_BUTTON);
        resgionButton.onClick.AddListener(ClickRegionButton);
        regionText = GetText((int)TEXT.PREFAB_RESPONSE_POINT_REGION_TEXT);

        SetRegionButton();
    }

    public void SetRegionButton()
    {
        regionText.text = responseCrystalData.regionName;
    }

    public void ClickRegionButton()
    {
        OnClickRegionButton?.Invoke(this);
    }

    #region Property
    public ResponseCrystalData ResponseCrystalData { get { return responseCrystalData; } }
    public Button RegionButton { get { return resgionButton; } }
    public TextMeshProUGUI RegionText { get { return regionText; } }
    #endregion
}
