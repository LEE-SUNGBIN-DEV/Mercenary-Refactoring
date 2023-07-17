using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResonancePointPanel : UIPanel
{
    public enum IMAGE
    {
        Region_Image
    }

    public enum BUTTON
    {
        Move_Button
    }

    [SerializeField] private RectTransform sceneButtonRoot;
    [SerializeField] private RectTransform regionButtonRoot;

    [SerializeField] private List<ResonanceSceneButton> sceneButtonList = new List<ResonanceSceneButton>();
    [SerializeField] private List<ResonanceRegionButton> regionButtonList = new List<ResonanceRegionButton>();

    private Dictionary<SCENE_LIST, ResonanceSceneButton> sceneButtonDictionary = new Dictionary<SCENE_LIST, ResonanceSceneButton>();
    private Dictionary<int, ResonanceRegionButton> regionButtonDictionary = new Dictionary<int, ResonanceRegionButton>();

    private CharacterLocationData playerLocationData;
    private Button moveButton;

    private ResonanceCrystalData selectedResonanceObject;

    public void Initialize(CharacterData characterData)
    {
        BindImage(typeof(IMAGE));
        BindButton(typeof(BUTTON));

        moveButton = GetButton((int)BUTTON.Move_Button);
        moveButton.onClick.AddListener(MoveResonancePoint);
        moveButton.enabled = false;

        sceneButtonRoot = Functions.FindChild<RectTransform>(gameObject, "Scene_Content", true);
        regionButtonRoot = Functions.FindChild<RectTransform>(gameObject, "Region_Content", true);

        playerLocationData = characterData.LocationData;
        playerLocationData.OnChangeResonancePointData -= RefreshPanel;
        playerLocationData.OnChangeResonancePointData += RefreshPanel;

        RegistData();
        RefreshPanel(playerLocationData);
    }

    private void OnDestroy()
    {
        playerLocationData.OnChangeResonancePointData -= RefreshPanel;
    }

    public void RefreshPanel(CharacterLocationData playerLocationData)
    {
        for (int i = 0; i < sceneButtonList.Count; ++i)
            sceneButtonList[i].gameObject.SetActive(false);

        for (int i = 0; i < regionButtonList.Count; ++i)
            regionButtonList[i].gameObject.SetActive(false);

        // Activate button from Player Data
        foreach (var resonancePointData in playerLocationData.ResonancePointDictionary)
        {
            if (resonancePointData.Value == true && sceneButtonDictionary[(SCENE_LIST)(resonancePointData.Key / 100)].isActiveAndEnabled == false)
                sceneButtonDictionary[(SCENE_LIST)(resonancePointData.Key / 100)].gameObject.SetActive(true);
        }
    }

    public ResonanceSceneButton RegistSceneButton(GameSceneData gameSceneData)
    {
        ResonanceSceneButton newSceneButton = Managers.ResourceManager?.InstantiatePrefabSync(Constants.Prefab_Button_Scene, sceneButtonRoot.transform).GetComponent<ResonanceSceneButton>();
        newSceneButton.Initialize(gameSceneData);
        newSceneButton.OnClickSceneButton -= RefreshRegionList;
        newSceneButton.OnClickSceneButton += RefreshRegionList;
        newSceneButton.gameObject.SetActive(false);
        sceneButtonList.Add(newSceneButton);
        sceneButtonDictionary.Add(newSceneButton.GameSceneData.scene, newSceneButton);

        return newSceneButton;
    }

    public ResonanceRegionButton RegistRegionButton(ResonanceCrystalData objectData)
    {
        ResonanceRegionButton newRegionButton = Managers.ResourceManager?.InstantiatePrefabSync(Constants.Prefab_Button_Region, regionButtonRoot.transform).GetComponent<ResonanceRegionButton>();
        newRegionButton.Initialize(objectData);
        newRegionButton.OnClickRegionButton -= EnableMoveButton;
        newRegionButton.OnClickRegionButton += EnableMoveButton;
        newRegionButton.gameObject.SetActive(false);
        regionButtonList.Add(newRegionButton);
        regionButtonDictionary.Add(newRegionButton.ResonanceCrystalData.id, newRegionButton);

        return newRegionButton;
    }

    public void RegistData()
    {
        foreach(var sceneData in Managers.DataManager.GameSceneTable)
        {
            ResonanceSceneButton sceneButton = RegistSceneButton(sceneData.Value);
            foreach(var objectData in sceneData.Value.resonanceCrystalDataList)
            {
                sceneButton.RegionButtonList.Add(RegistRegionButton(objectData));
            }
        }
    }

    public void RefreshRegionList(ResonanceSceneButton sceneButton)
    {
        for (int i = 0; i < regionButtonList.Count; ++i)
            regionButtonList[i].gameObject.SetActive(false);

        foreach (var resonancePointData in playerLocationData.ResonancePointDictionary)
        {
            if ((SCENE_LIST)(resonancePointData.Key / 100) == sceneButton.GameSceneData.scene)
            {
                bool isEnabled = resonancePointData.Value;
                sceneButton.RegionButtonList[resonancePointData.Key % 100].gameObject.SetActive(isEnabled);
            }
        }
    }

    public void EnableMoveButton(ResonanceRegionButton regionButton)
    {
        selectedResonanceObject = regionButton.ResonanceCrystalData;
        moveButton.enabled = true;
    }

    public void MoveResonancePoint()
    {
        Managers.DataManager.CurrentCharacterData.LocationData.SetLastResonancePoint(selectedResonanceObject.id);
        Managers.SceneManagerCS.LoadSceneAsync(selectedResonanceObject.GetLocatedScene());
    }
}
