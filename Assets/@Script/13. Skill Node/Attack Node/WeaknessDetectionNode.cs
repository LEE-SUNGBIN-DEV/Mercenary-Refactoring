using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessDetectionNode : BaseSkillNode
{
    private float increasePerLevel;

    public override void Initialize(CharacterData characterData, SkillTooltipPanel tooltipPanel)
    {
        base.Initialize(characterData, tooltipPanel);

        // Skill Informations
        increasePerLevel = 1;
        skillID = 11;
        skillName = "약점 포착";
        maxSkillLevel = 5;

        UpdateSkillNode(characterData.SkillData);
    }

    public override void ReleaseSkillAbility()
    {
        characterData.StatusData.SkillCriticalChance -= currentSkillLevel * increasePerLevel;
    }
    public override void ApplySkillAbility()
    {
        characterData.StatusData.SkillCriticalChance += currentSkillLevel * increasePerLevel;
    }

    public override string GetSkillDescription()
    {
        skillDescription =
            $"크리티컬 확률을 <color=#C8A050>{currentSkillLevel * increasePerLevel}%</color> 증가 시킵니다. (레벨당 <color=#C8A050>{increasePerLevel}%</color> 증가)";

        return skillDescription;
    }
}
