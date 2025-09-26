using UnityEngine;

public class RPGSkill_5 : RPGSkill_GenerateBuff
{
	private void Awake()
	{
		_skillUnit = RPGGlobalData.Instance.SkillUnitPool._dict[5];
	}

	private void Update()
	{
	}

	public override float ActiveSkill()
	{
		base.ActiveSkill();
		Debug.Log("Generate Buff:" + base.name);
		GameObject obj = _skillOwner.gameObject;
		RPGTBuff rPGTBuff = GenerateBuff(obj, "RPGBuff_AttractFire", 104, false);
		float fLen = 0f;
		if (_skillOwner.AddBuff(rPGTBuff, true, ref fLen, _skillOwner) != -1f)
		{
			Debug.Log("Generate Buff:" + rPGTBuff.name + " to " + _skillOwner.gameObject.name);
		}
		return fLen;
	}

	public override int UnactiveSkill()
	{
		base.UnactiveSkill();
		return 0;
	}
}
