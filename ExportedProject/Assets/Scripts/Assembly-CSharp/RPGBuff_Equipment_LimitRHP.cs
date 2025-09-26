using UnityEngine;

public class RPGBuff_Equipment_LimitRHP : RPGTBuff
{
	public float _RHPR;

	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 111;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}

	public override void ActionLimit()
	{
		Debug.Log("ActionLimit " + base.RPGEntityOwner.CurHp + " ,,,," + _RHPR);
		if (base.RPGEntityOwner.CurHp > 0f && _RHPR > 0f)
		{
			float num = base.RPGEntityOwner.MaxHp * _RHPR / 100f;
			base.RPGEntityOwner.CurHp += num;
			SSHurtNum.Instance.HealingFont(num, base.RPGEntityOwner.transform);
		}
	}
}
