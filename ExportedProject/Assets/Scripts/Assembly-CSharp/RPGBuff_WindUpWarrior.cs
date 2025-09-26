using UnityEngine;

public class RPGBuff_WindUpWarrior : RPGTBuff
{
	[SerializeField]
	public int _reflectdam;

	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 28;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}

	private new void Update()
	{
		base.Update();
	}

	public override void BeAttackAppend(RPGEntity enemy, int dam)
	{
		if (enemy.GetAttackSkill().SkillUnit.SkillId != 38)
		{
			float value = (float)dam * ((float)_reflectdam / 100f);
			value = Mathf.Clamp(value, 0f, base.RPGEntityOwner.MaxHp);
			enemy.CurHp -= value;
			SSHurtNum.Instance.HitNormalFont(value, enemy.transform);
		}
	}
}
