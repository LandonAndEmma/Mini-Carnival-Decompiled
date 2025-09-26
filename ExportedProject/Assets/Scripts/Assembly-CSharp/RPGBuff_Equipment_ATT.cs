public class RPGBuff_Equipment_ATT : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 100;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}
}
