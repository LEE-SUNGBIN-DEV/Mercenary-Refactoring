using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorRefinementNode : BaseSkillNode
{
    private float increasePerLevel;

    public override void Initialize(CharacterData characterData, SkillTooltipPanel tooltipPanel)
    {
        base.Initialize(characterData, tooltipPanel);

        // Skill Informations
        increasePerLevel = 1;
        skillID = 101;
        skillName = "∞©ø  ¿Á∑√";
        maxSkillLevel = 5;

        UpdateSkillNode(characterData.SkillData);
    }

    public override void ReleaseSkillAbility()
    {
        characterData.StatusData.SkillArmorDefensePowerRatio -= currentSkillLevel * increasePerLevel;
    }
    public override void ApplySkillAbility()
    {
        characterData.StatusData.SkillArmorDefensePowerRatio += currentSkillLevel * increasePerLevel;
    }

    public override string GetSkillDescription()
    {
        skillDescription =
            $"∞©ø ¿« πÊæÓ∑¬¿ª <color=#C8A050>{currentSkillLevel * increasePerLevel}%</color> ¡ı∞° Ω√≈µ¥œ¥Ÿ. (∑π∫ß¥Á <color=#C8A050>{increasePerLevel}%</color> ¡ı∞°)";

        return skillDescription;
    }
}
