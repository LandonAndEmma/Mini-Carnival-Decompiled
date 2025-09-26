using UnityEngine;

public class RPGSkill_10 : RPGTSkill
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[10];
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
		if (beAttack.CalcAttr.Attrs[22] >= 100f)
		{
			return dam;
		}
		RPGTBuff rPGTBuff = beAttack.gameObject.AddComponent<RPGBuff_SnowMan>();
		rPGTBuff.OffsetAttr.Attrs[4] = (int)rPGSkillUnit.ParamLst[0];
		rPGTBuff.SecondType = 110;
		rPGTBuff.IsOverlap = false;
		rPGTBuff.BoutCount = (int)rPGSkillUnit.ParamLst[1];
		if (beAttack.AddBuff(rPGTBuff) != -1f)
		{
			Debug.Log("Generate Buff:" + rPGTBuff.name + " to " + beAttack.gameObject.name);
		}
		Object obj = Resources.Load("Particle/effect/Skill/RPG_Icon_Increase_attack/RPG_Icon_Increase_attack_Under");
		if (obj != null)
		{
			GameObject gameObject = Object.Instantiate(obj) as GameObject;
			gameObject.transform.parent = beAttack.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
			Object.DestroyObject(gameObject, 1f);
		}
		return dam;
	}
}
