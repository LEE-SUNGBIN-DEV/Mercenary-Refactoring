using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScenePanelCanvas : UIBaseCanvas
{
    private UIScenePanel activedScenePanel;

    private TitleScenePanel titleScenePanel;
    private SelectionScenePanel selectionScenePanel;

    protected override void Awake()
    {
        base.Awake();
        canvas.sortingOrder = 0;
        titleScenePanel = GetComponentInChildren<TitleScenePanel>(true);
        selectionScenePanel = GetComponentInChildren<SelectionScenePanel>(true);
        activedScenePanel = null;
    }

    public void OpenScenePanel(UIScenePanel requestedScenePanel)
    {
        // Already Opened
        if (activedScenePanel == requestedScenePanel)
        {
            return;
        }
        // Open
        if (activedScenePanel == null)
        {
            activedScenePanel = requestedScenePanel;
            requestedScenePanel.OpenScenePanel();
        }
        // Switch
        else
        {
            activedScenePanel.CloseScenePanel();
            activedScenePanel = requestedScenePanel;
            activedScenePanel.OpenScenePanel();
        }
    }
    public void CloseScenePanel(UIScenePanel requestedScenePanel)
    {
        // Not Opened
        if (activedScenePanel == null)
        {
            return;
        }
        // Close
        if (activedScenePanel == requestedScenePanel)
        {
            activedScenePanel.CloseScenePanel();
            activedScenePanel = null;
        }
    }

    public void CloseActivedScenePanel()
    {
        if (activedScenePanel != null)
        {
            activedScenePanel.CloseScenePanel();
            activedScenePanel = null;
        }
    }

    #region Property
    public TitleScenePanel TitleScenePanel { get { return titleScenePanel; } }
    public SelectionScenePanel SelectionScenePanel { get { return selectionScenePanel; } }
    #endregion
}
