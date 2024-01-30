using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystemPanelCanvas : UIBaseCanvas
{
    private DragSlotPanel dragSlotPanel;
    private FadePanel fadePanel;
    private SystemMessagePanel systemMessagePanel;

    protected override void Awake()
    {
        base.Awake();
        canvas.sortingOrder = 4;

        dragSlotPanel = GetComponentInChildren<DragSlotPanel>(true);
        dragSlotPanel.Initialize();
        fadePanel = GetComponentInChildren<FadePanel>(true);
        fadePanel.Initialize();
        systemMessagePanel = GetComponentInChildren<SystemMessagePanel>(true);
        systemMessagePanel.Initialize();
    }

    #region Property
    public DragSlotPanel DragSlotPanel { get { return dragSlotPanel; } }
    public FadePanel FadePanel { get { return fadePanel; } }
    public SystemMessagePanel SystemMessagePanel { get { return systemMessagePanel; } }
    #endregion
}
