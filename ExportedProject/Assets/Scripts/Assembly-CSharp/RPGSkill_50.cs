using UnityEngine;

public class RPGSkill_50 : RPGSkill_GenerateBuff
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[50];
	}

	private void Update()
	{
	}

	public override float ActiveSkill()
	{
		base.ActiveSkill();
		Debug.Log("Generate Buff:" + base.name);
		GameObject gameObject = _skillOwner.gameObject;
		RPGBuff_WindUpWarrior rPGBuff_WindUpWarrior = gameObject.AddComponent("RPGBuff_WindUpWarrior") as RPGBuff_WindUpWarrior;
		rPGBuff_WindUpWarrior.SecondType = 111;
		rPGBuff_WindUpWarrior.IsOverlap = false;
		rPGBuff_WindUpWarrior._reflectdam = (int)base.SkillUnit.ParamLst[1];
		if (_skillOwner.AddBuff(rPGBuff_WindUpWarrior) != -1f)
		{
			Debug.Log("Generate Buff:" + rPGBuff_WindUpWarrior.name + " to " + _skillOwner.gameObject.name);
		}
		return 0f;
	}

	public override int UnactiveSkill()
	{
		base.UnactiveSkill();
		return 0;
	}
}
