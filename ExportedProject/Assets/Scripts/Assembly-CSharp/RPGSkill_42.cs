using UnityEngine;

public class RPGSkill_42 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[42];
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
		int num = (int)attackSkill.SkillUnit.ParamLst[0];
		controller.MainAim.Clear();
		if (rPGEntity.IsExitBuff(221))
		{
			int num2 = Random.Range(0, 100);
			if (num2 > 50 && teamOwner.GetAliveMemExceptSelfCout(rPGEntity) > 0)
			{
				for (int i = 0; i < teamOwner.MemberLst.Count; i++)
				{
					if (teamOwner.MemberLst[i].CurHp > 0f)
					{
						controller.MainAim.Add(teamOwner.MemberLst[i]);
					}
				}
			}
			else
			{
				for (int j = 0; j < enemyTeam.MemberLst.Count; j++)
				{
					if (enemyTeam.MemberLst[j].CurHp > 0f)
					{
						controller.MainAim.Add(enemyTeam.MemberLst[j]);
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < enemyTeam.MemberLst.Count; k++)
			{
				if (enemyTeam.MemberLst[k].CurHp > 0f)
				{
					controller.MainAim.Add(enemyTeam.MemberLst[k]);
				}
			}
		}
		return controller.MainAim.Count;
	}

	public override int Skill_SelectAttackAim_Manual(RPGCenterController_Auto controller)
	{
		return Skill_SelectAttackAim(controller);
	}

	public override int Skill_DAM(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		bool flag = false;
		int num = Random.Range(0, 100);
		if (num < (int)rPGSkillUnit.ParamLst[0] + (int)attack.CalcAttr.Attrs[19])
		{
			flag = ((num > (int)beAttack.CalcAttr.Attrs[16]) ? true : false);
		}
		Debug.Log("-----------------Frozen=" + flag + "  nR=" + num);
		if (flag)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Ice_debuff);
			RPGBuff_Frozen rPGBuff_Frozen = beAttack.gameObject.AddComponent<RPGBuff_Frozen>();
			rPGBuff_Frozen.OffsetAttr.Attrs[6] = -100000f;
			rPGBuff_Frozen.SecondType = 220;
			rPGBuff_Frozen.IsOverlap = false;
			rPGBuff_Frozen.BoutCount = 1;
			if (beAttack.AddBuff(rPGBuff_Frozen) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGBuff_Frozen.name + " to " + beAttack.gameObject.name);
			}
		}
		return dam;
	}

	public override int Skill_BeAttackEffect(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		if (RPGGlobalData.Instance.BeAttackEffectPool._dict.ContainsKey(attack.CareerUnit.CareerId) && !beAttack.TeamOwner.ArearAttackHas)
		{
			beAttack.TeamOwner.ArearAttackHas = true;
			RPGBeAttackEffectUnit rPGBeAttackEffectUnit = RPGGlobalData.Instance.BeAttackEffectPool._dict[attack.CareerUnit.CareerId];
			Object obj = Resources.Load(rPGBeAttackEffectUnit.EffectPath);
			if (obj != null)
			{
				GameObject gameObject = Object.Instantiate(obj, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				gameObject.transform.parent = beAttack.TeamOwner.AreaAttackTran;
				gameObject.transform.localPosition = Vector3.zero;
				RPGEffectContr component = gameObject.GetComponent<RPGEffectContr>();
				if (component != null)
				{
					if (component._beAttackFollowWeapon)
					{
						gameObject.transform.parent = base.transform;
						gameObject.AddComponent<RPGMarkAimFollow>();
					}
					if (!component._rotateByModel)
					{
						gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
					}
					else
					{
						gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
					}
					Object.DestroyObject(gameObject, component._effectDurationTime);
				}
				else
				{
					Object.DestroyObject(gameObject);
				}
			}
		}
		return dam;
	}
}
