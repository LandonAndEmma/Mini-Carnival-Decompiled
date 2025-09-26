public class RPGSkill_52 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[52];
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

	public override float Skill_BeginBout(RPGRole_PlayerState_Begin_Bout state, RPGCenterController_Auto t)
	{
		if (base.SkillUnit.SkillTrigCond == RPGSkillUnit.ESkillTriggerCond.Begin_Bout)
		{
			ActiveSkill();
		}
		base.SkillOwner.ReCalcAttr();
		base.SkillOwner.CalcAttr.Attrs[4] *= 1f + (float)_skillUnit.ParamLst[1] * ((base.SkillOwner.MaxHp - base.SkillOwner.CurHp) / base.SkillOwner.MaxHp);
		return 0f;
	}
}
