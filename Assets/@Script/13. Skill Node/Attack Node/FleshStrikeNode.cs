using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshStrikeNode : BaseSkillNode
{
    private float increasePerLevel;

    public override void Initialize(CharacterData characterData, SkillTooltipPanel tooltipPanel)
    {
        base.Initialize(characterData, tooltipPanel);

        // Skill Informations
        increasePerLevel = 2;
        skillID = 22;
        skillName = "살점 타격";
        maxSkillLevel = 5;

        UpdateSkillNode(characterData.SkillData);
    }

    public override void ReleaseSkillAbility()
    {
        characterData.StatusData.SkillFixedDamage -= currentSkillLevel * increasePerLevel;
    }
    public override void ApplySkillAbility()
    {
        characterData.StatusData.SkillFixedDamage += currentSkillLevel * increasePerLevel;
    }

    public override string GetSkillDescription()
    {
        skillDescription =
            $"고정 대미지를 <color=#C8A050>{currentSkillLevel * increasePerLevel}%</color> 증가 시킵니다. (레벨당 <color=#C8A050>{increasePerLevel}%</color> 증가)";

        return skillDescription;
    }
}
