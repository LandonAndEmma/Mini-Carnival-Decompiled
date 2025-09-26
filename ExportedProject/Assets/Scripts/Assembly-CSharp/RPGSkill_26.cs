using UnityEngine;

public class RPGSkill_26 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[26];
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
		if (num < (int)rPGSkillUnit.ParamLst[0] + (int)attack.CalcAttr.Attrs[19])
		{
			flag = ((num > (int)beAttack.CalcAttr.Attrs[16]) ? true : false);
		}
		Debug.Log("-----------------Frozen=" + flag + "  nR=" + num);
		if (flag)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.RPG_Ice_debuff);
			RPGBuff_Frozen rPGBuff_Frozen = beAttack.gameObject.AddComponent<RPGBuff_Frozen>();
			rPGBuff_Frozen.OffsetAttr.Attrs[6] = -100000f;
			rPGBuff_Frozen.SecondType = 220;
			rPGBuff_Frozen.IsOverlap = false;
			rPGBuff_Frozen.BoutCount = 1;
			if (beAttack.AddBuff(rPGBuff_Frozen) != -1f)
			{
				Debug.Log("Generate Buff:" + rPGBuff_Frozen.name + " to " + beAttack.gameObject.name);
			}
		}
		return dam;
	}
}
