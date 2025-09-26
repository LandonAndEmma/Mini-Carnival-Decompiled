using UnityEngine;

public class RPGSkill_9 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[9];
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
		RPGRoleAttr calcAttr = attack.CalcAttr;
		float num = Random.Range((float)rPGSkillUnit.ParamLst[0], (float)rPGSkillUnit.ParamLst[1]);
		float f = calcAttr.Attrs[4] * num;
		Debug.Log("Skill9:M=" + num);
		return Mathf.FloorToInt(f);
	}
}
