using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLightweightingNode : BaseSkillNode
{
    private float increasePerLevel;

    public override void Initialize(CharacterData characterData, SkillTooltipPanel tooltipPanel)
    {
        base.Initialize(characterData, tooltipPanel);

        // Skill Informations
        increasePerLevel = 1;
        skillID = 21;
        skillName = "���� �淮ȭ";
        maxSkillLevel = 5;

        UpdateSkillNode(characterData.SkillData);
    }

    public override void ReleaseSkillAbility()
    {
        characterData.StatusData.SkillAttackSpeed -= currentSkillLevel * increasePerLevel;
    }
    public override void ApplySkillAbility()
    {
        characterData.StatusData.SkillAttackSpeed += currentSkillLevel * increasePerLevel;
    }

    public override string GetSkillDescription()
    {
        skillDescription =
            $"���� �ӵ��� <color=#C8A050>{currentSkillLevel * increasePerLevel}%</color> ���� ��ŵ�ϴ�. (������ <color=#C8A050>{increasePerLevel}%</color> ����)";

        return skillDescription;
    }
}
