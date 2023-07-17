using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalBoostNode : BaseSkillNode
{
    private float increasePerLevel;

    public override void Initialize(CharacterData characterData, SkillTooltipPanel tooltipPanel)
    {
        base.Initialize(characterData, tooltipPanel);

        // Skill Informations
        increasePerLevel = 1;
        skillID = 203;
        skillName = "신체 강화";
        maxSkillLevel = 5;

        UpdateSkillNode(characterData.SkillData);
    }

    public override void ReleaseSkillAbility()
    {
        characterData.StatusData.SkillAllStatsRatio -= currentSkillLevel * increasePerLevel;
    }
    public override void ApplySkillAbility()
    {
        characterData.StatusData.SkillAllStatsRatio += currentSkillLevel * increasePerLevel;
    }

    public override string GetSkillDescription()
    {
        skillDescription =
            $"모든 어빌리티를 <color=#C8A050>{currentSkillLevel * increasePerLevel}%</color> 증가 시킵니다. (레벨당 <color=#C8A050>{increasePerLevel}%</color> 증가)";

        return skillDescription;
    }
}
