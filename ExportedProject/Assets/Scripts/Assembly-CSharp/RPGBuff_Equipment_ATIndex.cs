public class RPGBuff_Equipment_ATIndex : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 104;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return true;
	}
}
