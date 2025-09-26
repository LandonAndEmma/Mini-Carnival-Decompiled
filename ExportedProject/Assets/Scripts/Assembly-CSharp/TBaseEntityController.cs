public class TBaseEntityController
{
	protected TBaseEntity _OwnEntity;

	public TBaseEntityController(TBaseEntity own)
	{
		_OwnEntity = own;
		InitController();
	}

	public TBaseEntity GetOwner()
	{
		return _OwnEntity;
	}

	public virtual bool CanHandleMessage(TTelegram msg)
	{
		return true;
	}

	protected virtual int InitController()
	{
		return 0;
	}

	public virtual bool HandleMessage(TTelegram msg)
	{
		return CanHandleMessage(msg);
	}

	public virtual void Tick()
	{
	}

	public virtual void FixedTick()
	{
	}
}
