using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessStrikeNode : BaseSkillNode
{
    private float increasePerLevel;

    public override void Initialize(CharacterData characterData, SkillTooltipPanel tooltipPanel)
    {
        base.Initialize(characterData, tooltipPanel);

        // Skill Informations
        increasePerLevel = 2;
        skillID = 12;
        skillName = "���� Ÿ��";
        maxSkillLevel = 5;

        UpdateSkillNode(characterData.SkillData);
    }

    public override void ReleaseSkillAbility()
    {
        characterData.StatusData.SkillCriticalDamage -= currentSkillLevel * increasePerLevel;
    }
    public override void ApplySkillAbility()
    {
        characterData.StatusData.SkillCriticalDamage += currentSkillLevel * increasePerLevel;
    }

    public override string GetSkillDescription()
    {
        skillDescription =
            $"ũ��Ƽ�� ������� <color=#C8A050>{currentSkillLevel * increasePerLevel}%</color> ���� ��ŵ�ϴ�. (������ <color=#C8A050>{increasePerLevel}%</color> ����)";

        return skillDescription;
    }
}
