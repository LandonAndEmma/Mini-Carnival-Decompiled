public class RPGSkill_27 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[27];
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
		t.HealingHPToAllTeamMem();
		state.DealyAttack(t, base.SkillOwner.PlayAni(RPGEntity.EAniLST.Buff));
		return 0f;
	}
}
