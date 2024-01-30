using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractionPanelCanvas : UIBaseCanvas
{
    private IInteractionPanel[] interactionPanels;
    private IInteractionPanel activedInteractionPanel;

    private ResponseTracePanel responseTracePanel;
    private ResponsePointPanel responsePointPanel;
    private NPCPanel npcPanel;
    private StorePanel storePanel;

    protected override void Awake()
    {
        base.Awake();
        canvas.sortingOrder = 3;

        interactionPanels = GetComponentsInChildren<IInteractionPanel>(true);
        for(int i=0; i<interactionPanels.Length; i++)
        {
            interactionPanels[i].OnOpenPanel -= SwitchPanel;
            interactionPanels[i].OnOpenPanel += SwitchPanel;
            interactionPanels[i].OnClosePanel -= CloseActivePanel;
            interactionPanels[i].OnClosePanel += CloseActivePanel;
        }

        responseTracePanel = GetComponentInChildren<ResponseTracePanel>(true);
        responseTracePanel.Initialize();
        responsePointPanel = GetComponentInChildren<ResponsePointPanel>(true);
        responsePointPanel.Initialize();
        npcPanel = GetComponentInChildren<NPCPanel>(true);
        npcPanel.Initialize();
        storePanel = GetComponentInChildren<StorePanel>(true);
    }

    private void SwitchPanel(IInteractionPanel interactionPanel)
    {
        if (activedInteractionPanel != null && activedInteractionPanel != interactionPanel)
        {
            activedInteractionPanel?.ClosePanel();
            activedInteractionPanel = null;
            activedInteractionPanel = interactionPanel;
        }
    }
    private void CloseActivePanel(IInteractionPanel interactionPanel)
    {
        if (activedInteractionPanel != null && activedInteractionPanel == interactionPanel)
        {
            activedInteractionPanel = null;
        }
    }

    public ResponseTracePanel ResponseTracePanel { get { return responseTracePanel; } }
    public ResponsePointPanel ResponsePointPanel { get { return responsePointPanel; } }
    public NPCPanel NPCPanel { get { return npcPanel; } }
    public StorePanel StorePanel { get { return storePanel; } }
}
