using System.Collections.Generic;
using UnityEngine;

public class RPGSkill_43 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[43];
	}

	private void Update()
	{
	}

	public override float ActiveSkill()
	{
		base.ActiveSkill();
		return 0f;
	}

	public override int UnactiveSkill()
	{
		base.UnactiveSkill();
		return 0;
	}

	public override int Skill_SelectAttackAim(RPGCenterController_Auto controller)
	{
		RPGEntity rPGEntity = controller.GetOwner() as RPGEntity;
		RPGTeam teamOwner = rPGEntity.TeamOwner;
		RPGTeam enemyTeam = teamOwner.Refree.GetEnemyTeam(teamOwner);
		RPGTSkill attackSkill = rPGEntity.GetAttackSkill();
		controller.MainAim.Clear();
		List<RPGEntity> list = new List<RPGEntity>();
		for (int i = 0; i < enemyTeam.MemberLst.Count; i++)
		{
			if (enemyTeam.MemberLst[i].CurHp > 0f)
			{
				if (enemyTeam.MemberLst[i].CareerUnit.CareerId == 6)
				{
					controller.MainAim.Add(enemyTeam.MemberLst[i]);
					break;
				}
				list.Add(enemyTeam.MemberLst[i]);
			}
		}
		if (rPGEntity.IsExitBuff(221))
		{
			for (int j = 0; j < teamOwner.MemberLst.Count; j++)
			{
				if (teamOwner.MemberLst[j].CurHp > 0f && teamOwner.MemberLst[j] != rPGEntity)
				{
					list.Add(teamOwner.MemberLst[j]);
				}
			}
			if (list.Count > 0)
			{
				int index = Random.Range(0, list.Count);
				controller.MainAim.Clear();
				controller.MainAim.Add(list[index]);
			}
		}
		else if (controller.MainAim.Count == 0 && list.Count > 0)
		{
			int index2 = Random.Range(0, list.Count);
			controller.MainAim.Add(list[index2]);
		}
		return controller.MainAim.Count;
	}

	public override void Skill_InitAttackCount(RPGCenterController_Auto controller)
	{
		RPGEntity rPGEntity = controller.GetOwner() as RPGEntity;
		RPGTeam teamOwner = rPGEntity.TeamOwner;
		RPGTeam enemyTeam = teamOwner.Refree.GetEnemyTeam(teamOwner);
		RPGTSkill attackSkill = rPGEntity.GetAttackSkill();
		int attackCount = (int)attackSkill.SkillUnit.ParamLst[0];
		controller._attackCount = attackCount;
		controller._curAttackCount = 0;
	}
}
