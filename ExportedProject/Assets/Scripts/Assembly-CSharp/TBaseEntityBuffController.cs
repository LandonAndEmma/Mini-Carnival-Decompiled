using System.Collections.Generic;

public class TBaseEntityBuffController : TBaseEntitySystemController
{
	private List<TBuff> _lstBuff = new List<TBuff>();

	public TBaseEntityBuffController(TBaseEntity own)
		: base(own)
	{
		_lstBuff.Clear();
	}

	public virtual int AddBuff(TBuff bf)
	{
		int num = -1;
		if (bf.IsMutex())
		{
			for (int i = 0; i < _lstBuff.Count; i++)
			{
				if (_lstBuff[i].GetBuffType() == bf.GetBuffType())
				{
					_lstBuff.RemoveAt(i);
					break;
				}
			}
		}
		_lstBuff.Add(bf);
		return _lstBuff.Count - 1;
	}

	public virtual int RemoveBuff(int id)
	{
		for (int i = 0; i < _lstBuff.Count; i++)
		{
			if (_lstBuff[i].GetID() == id)
			{
				_lstBuff.RemoveAt(i);
				return 0;
			}
		}
		return -1;
	}

	public virtual int RemoveBuffByType(int type)
	{
		for (int i = 0; i < _lstBuff.Count; i++)
		{
			if (_lstBuff[i].GetBuffType() == type)
			{
				_lstBuff.RemoveAt(i);
				return 0;
			}
		}
		return -1;
	}

	public virtual bool IsExitBuff(int type)
	{
		for (int i = 0; i < _lstBuff.Count; i++)
		{
			if (_lstBuff[i].GetBuffType() == type)
			{
				return true;
			}
		}
		return false;
	}

	public override bool CanHandleMessage(TTelegram msg)
	{
		return true;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		if (!base.HandleMessage(msg))
		{
			return false;
		}
		return true;
	}

	public override void Tick()
	{
	}

	public override void FixedTick()
	{
	}
}
