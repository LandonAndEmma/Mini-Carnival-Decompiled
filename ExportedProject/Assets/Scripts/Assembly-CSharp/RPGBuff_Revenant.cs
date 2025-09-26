using UnityEngine;

public class RPGBuff_Revenant : RPGTBuff
{
	[SerializeField]
	public int _reveP;

	[SerializeField]
	public int _reveHp;

	[SerializeField]
	public int _durBout = 100000000;

	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 23;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}

	private new void Update()
	{
		base.Update();
	}

	public override float ReVerdictDeath()
	{
		float result = 0f;
		int num = Random.Range(0, 100);
		if (_reveP > num)
		{
			float num2 = (float)_reveHp / 100f * base.RPGEntityOwner.MaxHp;
			result = num2;
		}
		_durBout--;
		if (_durBout <= 0)
		{
			base.RPGEntityOwner.RemoveBuff(GetInstanceID());
		}
		return result;
	}
}
