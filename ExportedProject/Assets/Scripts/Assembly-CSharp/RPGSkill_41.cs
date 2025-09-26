using UnityEngine;

public class RPGSkill_41 : RPGSkill_GenerateBuff
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[41];
	}

	private void Update()
	{
	}

	public override float ActiveSkill()
	{
		base.ActiveSkill();
		Debug.Log("Generate Buff:" + base.name);
		GameObject gameObject = _skillOwner.gameObject;
		RPGBuff_Revenant rPGBuff_Revenant = gameObject.AddComponent("RPGBuff_Revenant") as RPGBuff_Revenant;
		rPGBuff_Revenant.SecondType = 124;
		rPGBuff_Revenant.IsOverlap = false;
		rPGBuff_Revenant._reveP = (int)base.SkillUnit.ParamLst[1];
		rPGBuff_Revenant._reveHp = (int)base.SkillUnit.ParamLst[2];
		if (_skillOwner.AddBuff(rPGBuff_Revenant) != -1f)
		{
			Debug.Log("Generate Buff:" + rPGBuff_Revenant.name + " to " + _skillOwner.gameObject.name);
		}
		return 0f;
	}

	public override int UnactiveSkill()
	{
		base.UnactiveSkill();
		return 0;
	}
}
