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

    [SerializeField] private RectTransform chapterButtonRoot;
    [SerializeField] private RectTransform regionButtonRoot;

    [SerializeField] private List<ChapterButton> chapterButtonList = new List<ChapterButton>();
    [SerializeField] private List<RegionButton> regionButtonList = new List<RegionButton>();

    private PlayerLocationData playerLocationData;
    private Button moveButton;

    private ResonanceObjectData targetResonanceObject;

    public void Initialize(CharacterData characterData)
    {
        BindImage(typeof(IMAGE));
        BindButton(typeof(BUTTON));

        moveButton = GetButton((int)BUTTON.Move_Button);
        moveButton.onClick.AddListener(MoveResonancePoint);
        moveButton.enabled = false;

        chapterButtonRoot = Functions.FindChild<RectTransform>(gameObject, "Chapter_Content", true);
        regionButtonRoot = Functions.FindChild<RectTransform>(gameObject, "Region_Content", true);

        playerLocationData = characterData.LocationData;
        playerLocationData.OnChangeResonancePointData -= RefreshChapterList;
        playerLocationData.OnChangeResonancePointData += RefreshChapterList;

        RefreshChapterList(playerLocationData);
    }

    public void ClearPanel()
    {
        regionButtonList.Clear();
        chapterButtonList.Clear();
    }

    public void RefreshChapterList(PlayerLocationData playerLocationData)
    {
        ClearPanel();

        // Load ChapterList from Player Data
        foreach (var sceneResonanceData in playerLocationData.ResonancePointEnableDictionary)
        {
            bool isEnabled = false;
            for (int i = 0; i < sceneResonanceData.Value.Length; ++i)
            {
                if (sceneResonanceData.Value[i] == true)
                {
                    isEnabled = true;
                    break;
                }
            }
            RegisterChapterButton(Managers.DataManager.GameSceneTable[sceneResonanceData.Key], isEnabled);
        }
    }

    public void RegisterChapterButton(GameSceneData wayPointData, bool isEnabled)
    {
        ChapterButton newChapterButton = Managers.ResourceManager?.InstantiatePrefabSync("Prefab_Chapter_Button", chapterButtonRoot.transform).GetComponent<ChapterButton>();
        newChapterButton.Initialize(wayPointData);
        newChapterButton.OnClickChapterButton -= ShowRegionList;
        newChapterButton.OnClickChapterButton += ShowRegionList;
        newChapterButton.enabled = isEnabled;
        chapterButtonList.Add(newChapterButton);
    }

    public void ShowRegionList(ChapterButton chapterButton)
    {
        regionButtonList.Clear();

        for (int i = 0; i < playerLocationData.ResonancePointEnableDictionary[chapterButton.ChapterInformation.scene].Length; ++i)
        {
            bool isEnabled = playerLocationData.ResonancePointEnableDictionary[chapterButton.ChapterInformation.scene][i];
            RegisterRegionButton(Managers.DataManager.GameSceneTable[chapterButton.ChapterInformation.scene].resonanceObjectDataList[i], isEnabled);
        }
    }

    public void RegisterRegionButton(ResonanceObjectData objectData, bool isEnabled)
    {
        RegionButton newRegionButton = Managers.ResourceManager?.InstantiatePrefabSync("Prefab_Region_Button", regionButtonRoot.transform).GetComponent<RegionButton>();
        newRegionButton.Initialize(objectData);
        newRegionButton.OnClickRegionButton -= EnableMoveButton;
        newRegionButton.OnClickRegionButton += EnableMoveButton;
        newRegionButton.enabled = isEnabled;
        regionButtonList.Add(newRegionButton);
    }

    public void EnableMoveButton(RegionButton regionButton)
    {
        targetResonanceObject = regionButton.ResonanceObjectData;
        moveButton.enabled = true;
    }

    public void MoveResonancePoint()
    {
        Managers.DataManager.CurrentCharacterData.LocationData.SetLastPosition(targetResonanceObject.GetPosition());
        Managers.SceneManagerCS.LoadSceneAsync(targetResonanceObject.scene);
    }
}
