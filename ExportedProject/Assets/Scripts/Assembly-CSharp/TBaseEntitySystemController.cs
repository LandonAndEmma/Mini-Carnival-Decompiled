public class TBaseEntitySystemController : TBaseEntityController
{
	protected int _nType;

	private static int _nNextVaildType = 1;

	public TBaseEntitySystemController(TBaseEntity own)
		: base(own)
	{
		SetSysType(_nNextVaildType);
	}

	private void SetSysType(int type)
	{
		_nType = type;
		_nNextVaildType++;
	}

	public int GetSysType()
	{
		return _nType;
	}

	public override bool CanHandleMessage(TTelegram msg)
	{
		return true;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		return CanHandleMessage(msg);
	}

	public override void Tick()
	{
	}

	public override void FixedTick()
	{
	}
}
