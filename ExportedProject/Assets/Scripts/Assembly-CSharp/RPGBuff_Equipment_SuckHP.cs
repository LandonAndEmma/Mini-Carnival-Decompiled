using UnityEngine;

public class RPGBuff_Equipment_SuckHP : RPGTBuff
{
	public float _suckR;

	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 110;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}

	public override void AttackAppend(RPGEntity enemy, int dam)
	{
		if (dam > 0 && base.RPGEntityOwner.CurHp > 0f && _suckR > 0f)
		{
			Debug.Log("Suck Hp");
			float num = (float)dam * _suckR / 100f;
			base.RPGEntityOwner.CurHp += num;
			SSHurtNum.Instance.HealingFont(num, base.RPGEntityOwner.transform);
		}
	}
}
