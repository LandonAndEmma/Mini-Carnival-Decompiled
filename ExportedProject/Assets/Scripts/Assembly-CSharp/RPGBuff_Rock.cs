public class RPGBuff_Rock : RPGTBuff
{
	protected new void Awake()
	{
		base.Awake();
		base.ConfId = 24;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		int nMsgId = msg._nMsgId;
		if (nMsgId == 5007 && base.RPGEntityOwner != null)
		{
			base.RPGEntityOwner.RemoveBuff(GetInstanceID());
		}
		return true;
	}

	protected override void InitBufferEffect()
	{
		base.InitBufferEffect();
	}
}
