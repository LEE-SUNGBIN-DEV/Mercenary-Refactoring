using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRefinementNode : BaseSkillNode
{
    private float increasePerLevel;

    public override void Initialize(CharacterData characterData, SkillTooltipPanel tooltipPanel)
    {
        base.Initialize(characterData, tooltipPanel);

        // Skill Informations
        skillPrecondition = null;
        increasePerLevel = 1;
        skillID = 1;
        skillName = "���� ���";
        maxSkillLevel = 5;

        UpdateSkillNode(characterData.SkillData);
    }

    public override void ReleaseSkillAbility()
    {
        characterData.StatusData.SkillLightAttackDamageRatio -= currentSkillLevel * increasePerLevel;
    }
    public override void ApplySkillAbility()
    {
        characterData.StatusData.SkillLightAttackDamageRatio += currentSkillLevel * increasePerLevel;
    }

    public override string GetSkillDescription()
    {
        skillDescription =
            $"���� ���ݷ��� <color=#C8A050>{currentSkillLevel * increasePerLevel}%</color> ���� ��ŵ�ϴ�. (������ <color=#C8A050>{increasePerLevel}%</color> ����)";

        return skillDescription;
    }
}
