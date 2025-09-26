using UnityEngine;

public class RPGSkill_32 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[32];
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
		RPGSkillUnit rPGSkillUnit2 = RPGGlobalData.Instance.SkillUnitPool._dict[beAttack.CareerUnit.SkillLst[0]];
		bool flag = false;
		int num = Random.Range(0, 100);
		if (num < (int)rPGSkillUnit.ParamLst[0] + (int)attack.CalcAttr.Attrs[21])
		{
			flag = ((num > (int)beAttack.CalcAttr.Attrs[18]) ? true : false);
		}
		Debug.Log("-----------------Chaos=" + flag + "  nR=" + num);
		if (flag)
		{
			RPGTBuff rPGTBuff = beAttack.gameObject.AddComponent<RPGBuff_Chaos>();
			rPGTBuff.SecondType = 221;
			rPGTBuff.IsOverlap = false;
			rPGTBuff.BoutCount = (int)rPGSkillUnit.ParamLst[1];
			if (beAttack.AddBuff(rPGTBuff) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGTBuff.name + " to " + beAttack.gameObject.name);
			}
		}
		return dam;
	}
}
