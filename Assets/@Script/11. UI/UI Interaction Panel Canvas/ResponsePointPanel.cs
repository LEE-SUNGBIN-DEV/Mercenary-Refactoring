using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;

public class ResponsePointPanel : UIPanel, IInteractionPanel
{
    public enum IMAGE
    {
        Region_Image
    }

    public enum BUTTON
    {
        Move_Button
    }

    public event UnityAction<IInteractionPanel> OnOpenPanel;
    public event UnityAction<IInteractionPanel> OnClosePanel;

    [SerializeField] private RectTransform sceneButtonRoot;
    [SerializeField] private RectTransform regionButtonRoot;

    [SerializeField] private List<ResponseSceneButton> sceneButtonList = new List<ResponseSceneButton>();
    [SerializeField] private List<ResponseRegionButton> regionButtonList = new List<ResponseRegionButton>();

    private Dictionary<SCENE_ID, ResponseSceneButton> sceneButtonDictionary = new Dictionary<SCENE_ID, ResponseSceneButton>();
    private Dictionary<string, ResponseRegionButton> regionButtonDictionary = new Dictionary<string, ResponseRegionButton>();

    private CharacterLocationData playerLocationData;
    private Button moveButton;

    private ResponseCrystalData selectResponseCrystal;

    #region Private
    private void OnEnable()
    {
        ConnectData();
    }
    private void OnDisable()
    {
        DisconnectData();
    }
    private void ConnectData()
    {
        playerLocationData = Managers.DataManager.CurrentCharacterData.LocationData;
        if (playerLocationData != null)
        {
            playerLocationData.OnChangeResponsePointData -= UpdatePanel;
            playerLocationData.OnChangeResponsePointData += UpdatePanel;
            UpdatePanel(playerLocationData);
        }
    }
    private void DisconnectData()
    {
        if (playerLocationData != null)
        {
            playerLocationData.OnChangeResponsePointData -= UpdatePanel;
            playerLocationData = null;
        }
    }
    private ResponseSceneButton AddSceneButton(GameSceneData gameSceneData)
    {
        ResponseSceneButton newSceneButton = Managers.ResourceManager?.InstantiatePrefabSync(Constants.PREFAB_RESPONSE_POINT_SCENE_BUTTON, sceneButtonRoot.transform).GetComponent<ResponseSceneButton>();
        newSceneButton.Initialize(gameSceneData);
        newSceneButton.OnClickSceneButton -= RefreshRegionList;
        newSceneButton.OnClickSceneButton += RefreshRegionList;
        newSceneButton.gameObject.SetActive(false);
        sceneButtonList.Add(newSceneButton);
        sceneButtonDictionary.Add(newSceneButton.GameSceneData.sceneID, newSceneButton);

        return newSceneButton;
    }
    private ResponseRegionButton AddRegionButton(ResponseCrystalData crystalData)
    {
        ResponseRegionButton newRegionButton = Managers.ResourceManager?.InstantiatePrefabSync(Constants.PREFAB_RESPONSE_POINT_REGION_BUTTON, regionButtonRoot.transform).GetComponent<ResponseRegionButton>();
        newRegionButton.Initialize(crystalData);
        newRegionButton.OnClickRegionButton -= EnableMoveButton;
        newRegionButton.OnClickRegionButton += EnableMoveButton;
        newRegionButton.gameObject.SetActive(false);
        regionButtonList.Add(newRegionButton);
        regionButtonDictionary.Add(newRegionButton.ResponseCrystalData.responseCrystalID, newRegionButton);

        return newRegionButton;
    }
    private void RefreshRegionList(ResponseSceneButton sceneButton)
    {
        for (int i = 0; i < regionButtonList.Count; ++i)
            regionButtonList[i].gameObject.SetActive(false);

        foreach (var responsePointID in playerLocationData.UnlockedPointHashSet)
        {
            if (Managers.DataManager.ResponseCrystalTable.TryGetValue(responsePointID, out ResponseCrystalData responseCrystalData)
                && responseCrystalData.locatedScene == sceneButton.GameSceneData.sceneID
                && regionButtonDictionary[responseCrystalData.responseCrystalID].isActiveAndEnabled == false)
            {
                regionButtonDictionary[responsePointID].gameObject.SetActive(true);
            }
        }
    }
    #endregion

    public void Initialize()
    {
        BindImage(typeof(IMAGE));
        BindButton(typeof(BUTTON));

        moveButton = GetButton((int)BUTTON.Move_Button);
        moveButton.onClick.AddListener(MoveResonancePoint);
        moveButton.enabled = false;

        sceneButtonRoot = Functions.FindChild<RectTransform>(gameObject, "Scene_Content", true);
        regionButtonRoot = Functions.FindChild<RectTransform>(gameObject, "Region_Content", true);

        foreach (var sceneData in Managers.DataManager.GameSceneTable)
        {
            ResponseSceneButton sceneButton = AddSceneButton(sceneData.Value);
        }
        foreach (ResponseCrystalData crystalData in Managers.DataManager.ResponseCrystalTable.Values)
        {
            sceneButtonDictionary[crystalData.locatedScene].RegionButtonList.Add(AddRegionButton(crystalData));
        }
    }

    public void OpenPanel()
    {
        OnOpenPanel?.Invoke(this);
        FadeInPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE);
    }
    public void ClosePanel()
    {
        OnClosePanel?.Invoke(this);
        FadeOutPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE, () => gameObject.SetActive(false));
    }
    
    public void UpdatePanel(CharacterLocationData playerLocationData)
    {
        for (int i = 0; i < sceneButtonList.Count; ++i)
            sceneButtonList[i].gameObject.SetActive(false);

        for (int i = 0; i < regionButtonList.Count; ++i)
            regionButtonList[i].gameObject.SetActive(false);

        // Activate button from Player Data
        foreach (var responsePointID in playerLocationData.UnlockedPointHashSet)
        {
            if (Managers.DataManager.ResponseCrystalTable.TryGetValue(responsePointID, out ResponseCrystalData responseCrystalData)
                && sceneButtonDictionary[responseCrystalData.locatedScene].isActiveAndEnabled == false)
            {
                sceneButtonDictionary[responseCrystalData.locatedScene].gameObject.SetActive(true);
            }
        }
    }

    public void EnableMoveButton(ResponseRegionButton regionButton)
    {
        selectResponseCrystal = regionButton.ResponseCrystalData;
        moveButton.enabled = true;
    }

    public void MoveResonancePoint()
    {
        Managers.DataManager.CurrentCharacterData.LocationData.SetLastResponsePoint(selectResponseCrystal.responseCrystalID);
        Managers.SceneManagerEX.LoadSceneAsync(selectResponseCrystal.locatedScene);
    }
}
