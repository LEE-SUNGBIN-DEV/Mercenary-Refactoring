using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackMasteryNode : BaseSkillNode
{
    private float increasePerLevel;

    public override void Initialize(CharacterData characterData, SkillTooltipPanel tooltipPanel)
    {
        base.Initialize(characterData, tooltipPanel);

        // Skill Informations
        skillPrecondition = new SkillPrecondition(1, 5);
        increasePerLevel = 1;
        skillID = 2;
        skillName = "����� �����͸�";
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
            $"����� ������� <color=#C8A050>{currentSkillLevel * increasePerLevel}%</color> ���� ��ŵ�ϴ�. (������ <color=#C8A050>{increasePerLevel}%</color> ����)";

        return skillDescription;
    }
}
