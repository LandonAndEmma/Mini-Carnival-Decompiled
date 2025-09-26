public class RPGBuff_SnowMan : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 25;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}

	private new void Update()
	{
		base.Update();
	}
}
