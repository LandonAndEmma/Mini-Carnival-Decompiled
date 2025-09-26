public class RPGBuff_AddHp : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 4;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}
}
