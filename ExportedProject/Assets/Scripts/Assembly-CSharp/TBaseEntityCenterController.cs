using System.Collections.Generic;

public class TBaseEntityCenterController : TBaseEntityController
{
	protected Dictionary<int, int> _mapChildSystem = new Dictionary<int, int>();

	public TBaseEntityCenterController(TBaseEntity own)
		: base(own)
	{
	}

	public int MapChildSysTag(int tag, int sysType)
	{
		if (_mapChildSystem.ContainsKey(tag))
		{
			return -1;
		}
		_mapChildSystem.Add(tag, sysType);
		return 0;
	}

	public int GetChildSysTypeByTag(int tag)
	{
		if (_mapChildSystem.ContainsKey(tag))
		{
			return _mapChildSystem[tag];
		}
		return -1;
	}

	public override bool CanHandleMessage(TTelegram msg)
	{
		return true;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		_OwnEntity.TransmitMsgToSystemCtrl(msg);
		return CanHandleMessage(msg);
	}

	public override void Tick()
	{
	}

	public override void FixedTick()
	{
	}
}
