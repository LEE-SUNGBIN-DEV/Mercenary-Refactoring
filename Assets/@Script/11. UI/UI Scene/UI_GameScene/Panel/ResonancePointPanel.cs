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

    private PlayerLocationData playerLocationData;
    private Button moveButton;

    private ResonanceObjectData selectedResonanceObject;

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

    private void OnEnable()
    {
        RefreshPanel(playerLocationData);
    }

    private void OnDestroy()
    {
        playerLocationData.OnChangeResonancePointData -= RefreshPanel;
    }

    public ResonanceSceneButton RegistSceneButton(GameSceneData gameSceneData)
    {
        ResonanceSceneButton newSceneButton = Managers.ResourceManager?.InstantiatePrefabSync(Constants.Prefab_Button_Scene, sceneButtonRoot.transform).GetComponent<ResonanceSceneButton>();
        newSceneButton.Initialize(gameSceneData);
        newSceneButton.OnClickSceneButton -= RefreshRegionList;
        newSceneButton.OnClickSceneButton += RefreshRegionList;
        newSceneButton.gameObject.SetActive(false);
        sceneButtonList.Add(newSceneButton);

        return newSceneButton;
    }

    public ResonanceRegionButton RegistRegionButton(ResonanceObjectData objectData)
    {
        ResonanceRegionButton newRegionButton = Managers.ResourceManager?.InstantiatePrefabSync(Constants.Prefab_Button_Region, regionButtonRoot.transform).GetComponent<ResonanceRegionButton>();
        newRegionButton.Initialize(objectData);
        newRegionButton.OnClickRegionButton -= EnableMoveButton;
        newRegionButton.OnClickRegionButton += EnableMoveButton;
        newRegionButton.gameObject.SetActive(false);
        regionButtonList.Add(newRegionButton);

        return newRegionButton;
    }

    public void RegistData()
    {
        foreach(var sceneData in Managers.DataManager.GameSceneTable)
        {
            ResonanceSceneButton sceneButton = RegistSceneButton(sceneData.Value);
            foreach(var objectData in sceneData.Value.resonanceObjectDataList)
            {
                sceneButton.RegionButtonList.Add(RegistRegionButton(objectData));
            }
        }
    }

    public void RefreshPanel(PlayerLocationData playerLocationData)
    {
        for(int i=0; i<sceneButtonList.Count; ++i)
            sceneButtonList[i].gameObject.SetActive(false);

        for (int i = 0; i < regionButtonList.Count; ++i)
            regionButtonList[i].gameObject.SetActive(false);

        // Activate button from Player Data
        foreach (var sceneData in playerLocationData.ResonancePointDictionary)
        {
            int sceneIndex = 0;
            for (int i = 0; i < sceneData.Value.Length; ++i)
            {
                if (sceneData.Value[i] == true)
                {
                    sceneButtonList[sceneIndex].gameObject.SetActive(true);
                    break;
                }
            }
            sceneIndex++;
        }
    }

    public void RefreshRegionList(ResonanceSceneButton sceneButton)
    {
        for (int i = 0; i < regionButtonList.Count; ++i)
            regionButtonList[i].gameObject.SetActive(false);

        for (int i = 0; i < playerLocationData.ResonancePointDictionary[sceneButton.GameSceneData.scene].Length; ++i)
        {
            bool isEnabled = playerLocationData.ResonancePointDictionary[sceneButton.GameSceneData.scene][i];
            sceneButton.RegionButtonList[i].gameObject.SetActive(isEnabled);
        }
    }

    public void EnableMoveButton(ResonanceRegionButton regionButton)
    {
        selectedResonanceObject = regionButton.ResonanceObjectData;
        moveButton.enabled = true;
    }

    public void MoveResonancePoint()
    {
        Managers.DataManager.CurrentCharacterData.LocationData.SetLastPosition(selectedResonanceObject.GetPosition());
        Managers.SceneManagerCS.LoadSceneAsync(selectedResonanceObject.scene);
    }
}
