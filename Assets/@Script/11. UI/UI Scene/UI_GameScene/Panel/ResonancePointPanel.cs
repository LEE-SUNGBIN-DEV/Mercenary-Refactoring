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

    private GameObject chapterButtonRoot;
    private GameObject regionButtonRoot;

    private List<ChapterButton> chapterButtonList;
    private List<RegionButton> regionButtonList;

    private PlayerWayPointData wayPointData;

    public void Initialize(CharacterData characterData)
    {
        BindImage(typeof(IMAGE));

        chapterButtonRoot = Functions.FindChild<GameObject>(gameObject, "Chapter_Content", true);
        regionButtonRoot = Functions.FindChild<GameObject>(gameObject, "Region_Content", true);

        wayPointData = characterData.WayPointData;
        wayPointData.OnChangeWayPointData -= RefreshChapterList;
        wayPointData.OnChangeWayPointData += RefreshChapterList;

        RefreshChapterList(wayPointData);
    }

    public void ClearPanel()
    {
        regionButtonList.Clear();
        chapterButtonList.Clear();
    }

    public void RefreshChapterList(PlayerWayPointData resonancePointData)
    {
        ClearPanel();

        // Load ChapterList from Player Data
        for (int chapterIndex = 0; chapterIndex < resonancePointData.IsEnableResonancePoint.Length; ++chapterIndex)
        {
            if (resonancePointData.IsEnableResonancePoint[chapterIndex].Length > 0)
            {
                WayPointData wayPointData = Managers.DataManager.WayPointTable[(CHAPTER_LIST)chapterIndex];
                RegisterChapterButton(wayPointData);

                for (int objectIndex = 0; objectIndex < resonancePointData.IsEnableResonancePoint[chapterIndex].Length; ++objectIndex)
                {
                    if (resonancePointData.IsEnableResonancePoint[chapterIndex][objectIndex] == true)
                    {
                        RegisterRegionButton(wayPointData.wayPointObjects[objectIndex]);
                    }
                }
            }
        }
    }

    public void RegisterChapterButton(WayPointData wayPointData)
    {
        ChapterButton newChapterButton = Managers.ResourceManager?.InstantiatePrefabSync("Prefab_Chapter_Button", regionButtonRoot.transform).GetComponent<ChapterButton>();
        newChapterButton.Initialize(wayPointData);
        newChapterButton.OnClickChapterButton -= RefreshRegionList;
        newChapterButton.OnClickChapterButton += RefreshRegionList;
        chapterButtonList.Add(newChapterButton);
    }

    public void RefreshRegionList(ChapterButton chapterButton)
    {

    }

    public void RegisterRegionButton(WayPointObjectData objectData)
    {
        RegionButton newRegionButton = Managers.ResourceManager?.InstantiatePrefabSync("Prefab_Region_Button", chapterButtonRoot.transform).GetComponent<RegionButton>();
        newRegionButton.Initialize(objectData);
        newRegionButton.OnClickRegionButton -= ShowConfirmWindow;
        newRegionButton.OnClickRegionButton += ShowConfirmWindow;
        regionButtonList.Add(newRegionButton);
    }

    public void ShowConfirmWindow(RegionButton regionButton)
    {

    }
}
