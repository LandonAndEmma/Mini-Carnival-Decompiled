using System.Collections.Generic;
using UnityEngine;

public class RPGSkill_36 : RPGTSkill
{
	private bool _bShareDMA = true;

	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[36];
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
		List<RPGEntity> list2 = new List<RPGEntity>();
		List<RPGEntity> list3 = new List<RPGEntity>();
		for (int j = 0; j < enemyTeam.MemberLst.Count; j++)
		{
			if (enemyTeam.MemberLst[j].CurHp > 0f)
			{
				if (!enemyTeam.MemberLst[j].IsExitBuff(220))
				{
					list3.Add(enemyTeam.MemberLst[j]);
				}
				list2.Add(enemyTeam.MemberLst[j]);
			}
		}
		if (rPGEntity.IsExitBuff(221))
		{
			for (int k = 0; k < teamOwner.MemberLst.Count; k++)
			{
				if (teamOwner.MemberLst[k].CurHp > 0f && teamOwner.MemberLst[k] != rPGEntity)
				{
					list.Add(teamOwner.MemberLst[k]);
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
			int num = Random.Range(0, 100);
			if (num < 20 && list2.Count > 0)
			{
				controller.MainAim.Add(list2[Random.Range(0, list2.Count)]);
			}
			else
			{
				int num2 = Random.Range(0, 100);
				list.Sort(base.CompareAimSel);
				if (num2 > 50)
				{
					int index2 = 0;
					for (int l = 1; l < list.Count; l++)
					{
						if (list[l].CareerUnit.BeAttackPriority > list[0].CareerUnit.BeAttackPriority)
						{
							index2 = l;
							break;
						}
					}
					controller.MainAim.Add(list[index2]);
				}
				else
				{
					controller.MainAim.Add(list[0]);
				}
			}
		}
		return controller.MainAim.Count;
	}

	public override int Skill_ProcessShareDAM(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		if (_bShareDMA && beAttack.IsExitBuff(114))
		{
			RPGTeam teamOwner = beAttack.TeamOwner;
			List<RPGEntity> list = new List<RPGEntity>();
			for (int i = 0; i < teamOwner.MemberLst.Count; i++)
			{
				if (teamOwner.MemberLst[i].IsExistSkill(20) && teamOwner.MemberLst[i].CurHp > 0f)
				{
					list.Add(teamOwner.MemberLst[i]);
				}
			}
			if (!beAttack.IsExistSkill(20))
			{
				int num = dam / (list.Count + 1);
				for (int j = 0; j < list.Count; j++)
				{
					list[j].CurHp -= num;
					SSHurtNum.Instance.HitNormalFont(num, list[j].transform);
					PlayShareDAMEffect(list[j]);
				}
				return num;
			}
			if (list.Count > 1)
			{
				int num2 = dam / list.Count;
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k] != beAttack)
					{
						list[k].CurHp -= num2;
						SSHurtNum.Instance.HitNormalFont(num2, list[k].transform);
						PlayShareDAMEffect(list[k]);
					}
				}
				return num2;
			}
		}
		return dam;
	}

	public override int Skill_DAM(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		int num = (int)rPGSkillUnit.ParamLst[0];
		int num2 = Random.Range(0, 100);
		if (num2 <= num)
		{
			_bShareDMA = false;
			return RPGTSkill.MAX_DAM;
		}
		return dam;
	}
}
