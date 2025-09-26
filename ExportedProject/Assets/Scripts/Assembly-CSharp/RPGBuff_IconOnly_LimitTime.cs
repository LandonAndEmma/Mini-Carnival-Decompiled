using UnityEngine;

public class RPGBuff_IconOnly_LimitTime : RPGBuff_IconOnly
{
	[SerializeField]
	private float _fDur = 2f;

	private float _awakeTime;

	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 100;
		_awakeTime = Time.time;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}

	protected new void Update()
	{
		base.Update();
		if (Time.time - _awakeTime >= _fDur)
		{
			Object.Destroy(this);
		}
	}
}
