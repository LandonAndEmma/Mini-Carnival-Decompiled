using UnityEngine;

public class RPGSkill_61 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[61];
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

	public override void Skill_Feedback(RPGEntity attack, RPGEntity beAttack, int dam)
	{
		RPGSkillUnit rPGSkillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[attack.CareerUnit.SkillLst[0]];
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		base.Skill_Feedback(attack, beAttack, dam);
		if (beAttack.CurHp <= 0f)
		{
			float num = (float)(dam * (int)rPGSkillUnit.ParamLst[1]) / 100f;
			attack.CurHp += num;
			SSHurtNum.Instance.HealingFont(num, attack.transform);
		}
	}
}
