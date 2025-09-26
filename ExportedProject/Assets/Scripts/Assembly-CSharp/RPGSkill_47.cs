using UnityEngine;

public class RPGSkill_47 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[47];
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

	public override void Skill_IsAT(ref bool at, RPGEntity attack, RPGEntity beAttack)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGRoleAttr calcAttr = beAttack.CalcAttr;
		RPGRoleAttr calcAttr2 = attack.CalcAttr;
		float num = (float)(int)attack.GetAttackSkill().SkillUnit.ParamLst[0] / 100f;
		if (beAttack.CurHp / beAttack.MaxHp <= num)
		{
			at = true;
		}
	}

	public override int Skill_DAM(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGRoleAttr calcAttr = beAttack.CalcAttr;
		RPGRoleAttr calcAttr2 = attack.CalcAttr;
		float num = (float)(int)attack.GetAttackSkill().SkillUnit.ParamLst[0] / 100f;
		if (beAttack.CurHp / beAttack.MaxHp <= num)
		{
			return RPGTSkill.MAX_DAM;
		}
		return dam;
	}

	public override int Skill_BeAttackEffect(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		if (RPGGlobalData.Instance.BeAttackEffectPool._dict.ContainsKey(attack.CareerUnit.CareerId))
		{
			RPGBeAttackEffectUnit rPGBeAttackEffectUnit = RPGGlobalData.Instance.BeAttackEffectPool._dict[attack.CareerUnit.CareerId];
			Object obj = Resources.Load(rPGBeAttackEffectUnit.EffectPath);
			Debug.Log(rPGBeAttackEffectUnit.EffectPath);
			if (obj != null)
			{
				GameObject gameObject = Object.Instantiate(obj, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				gameObject.transform.parent = beAttack.transform;
				gameObject.transform.localPosition = Vector3.zero;
				RPGEffectContr component = gameObject.GetComponent<RPGEffectContr>();
				if (component != null)
				{
					if (component._beAttackFollowWeapon)
					{
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
					gameObject.transform.parent = null;
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
