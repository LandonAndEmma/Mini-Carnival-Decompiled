using UnityEngine;

public class RPGSkill_31 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[31];
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
		return Mathf.FloorToInt((float)dam * ((float)(int)rPGSkillUnit.ParamLst[0] / 100f));
	}

	public override void Skill_AttackEffect(RPGCenterController_Auto controller)
	{
		if (controller.MainAim.Count <= 0 || !RPGGlobalData.Instance.AttackEffectPool._dict.ContainsKey(base.SkillOwner.CareerUnit.CareerId))
		{
			return;
		}
		RPGAttackEffectUnit rPGAttackEffectUnit = RPGGlobalData.Instance.AttackEffectPool._dict[base.SkillOwner.CareerUnit.CareerId];
		Object obj = Resources.Load(rPGAttackEffectUnit.EffectPath);
		if (!(obj != null) || !(_attTran != null))
		{
			return;
		}
		Vector3 position = _attTran.position;
		GameObject gameObject = Object.Instantiate(obj, position, Quaternion.Euler(Vector3.zero)) as GameObject;
		RPGEffectContr component = gameObject.GetComponent<RPGEffectContr>();
		if (component != null)
		{
			float fDur = component._effectDurationTime;
			if (component._effectFlyTime > 0.001f)
			{
				fDur = component._effectFlyTime;
			}
			Ammo_Fly ammo_Fly = gameObject.AddComponent<Ammo_Fly>();
			ammo_Fly.StartFly(controller.MainAim[0].TeamOwner.AreaAttackTran, fDur, component._effectFlyOffsetTime);
		}
		else
		{
			Object.DestroyObject(gameObject);
		}
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
			Debug.Log(rPGBeAttackEffectUnit.EffectPath);
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
