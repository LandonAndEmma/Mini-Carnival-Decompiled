public class RPGSkill_20 : RPGSkill_GenerateTactic
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[20];
	}

	private void Update()
	{
	}

	public override float ActiveSkill()
	{
		base.ActiveSkill();
		int id = (int)base.SkillUnit.ParamLst[0];
		return base.SkillOwner.TeamOwner.EquipTactic(id, base.SkillOwner);
	}

	public override int UnactiveSkill()
	{
		base.UnactiveSkill();
		return 0;
	}
}
