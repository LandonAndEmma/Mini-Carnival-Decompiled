using UnityEngine;

public class RPGSkill_55 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[55];
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

	public override int Skill_DAM(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		int num = (int)rPGSkillUnit.ParamLst[0];
		int num2 = Random.Range(0, 100);
		if (num2 <= num)
		{
			return RPGTSkill.MAX_DAM;
		}
		return dam;
	}
}
