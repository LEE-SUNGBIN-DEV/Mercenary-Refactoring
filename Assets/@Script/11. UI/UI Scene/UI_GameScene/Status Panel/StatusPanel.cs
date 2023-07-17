using UnityEngine;
using UnityEngine.Events;

public class StatusPanel : UIPanel
{
    private OverallStatusPanel overallStatusPanel;
    private EquipStatusPanel equipStatusPanel;
    private AbilityStatusPanel abilityStatusPanel;

    public void Initialize(CharacterData characterData)
    {
        overallStatusPanel = GetComponentInChildren<OverallStatusPanel>(true);
        overallStatusPanel.Initialize(characterData);

        equipStatusPanel = GetComponentInChildren<EquipStatusPanel>(true);
        equipStatusPanel.Initialize(characterData);

        abilityStatusPanel = GetComponentInChildren<AbilityStatusPanel>(true);
        abilityStatusPanel.Initialize(characterData);
    }
}
