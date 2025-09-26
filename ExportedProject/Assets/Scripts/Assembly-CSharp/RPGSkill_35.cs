using UnityEngine;

public class RPGSkill_35 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[35];
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

	public override int Skill_DAM_Cri(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGRoleAttr calcAttr = beAttack.CalcAttr;
		RPGRoleAttr calcAttr2 = attack.CalcAttr;
		float num = (float)dam / (1f + calcAttr.Attrs[10] / 100f);
		float num2 = num / calcAttr2.Attrs[9];
		int num3 = Mathf.FloorToInt(num2 * (calcAttr2.Attrs[9] + (float)(int)rPGSkillUnit.ParamLst[0]));
		return Mathf.FloorToInt((float)num3 * (1f + calcAttr.Attrs[10] / 100f));
	}
}
