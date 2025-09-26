using UnityEngine;

public class RPGSkill_56 : RPGSkill_GenerateBuff
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[56];
	}

	private void Update()
	{
	}

	public override float ActiveSkill()
	{
		base.ActiveSkill();
		Debug.Log("Generate Buff:" + base.name);
		GameObject obj = _skillOwner.gameObject;
		RPGTBuff rPGTBuff = GenerateBuff(obj, "RPGBuff_AddCriRateNOMsg", 126, false);
		if (_skillOwner.AddBuff(rPGTBuff) != -1f)
		{
			Debug.Log("Generate Buff:" + rPGTBuff.name + " to " + _skillOwner.gameObject.name);
		}
		return 0f;
	}

	public override int UnactiveSkill()
	{
		base.UnactiveSkill();
		return 0;
	}
}
