public class RPGSkill_60 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[60];
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
}
