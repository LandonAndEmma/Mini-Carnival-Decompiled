public class TBuff
{
	public static int _nNextValidID;

	private int _nID;

	private int _nType = -1;

	private float _fStartTime;

	private float _fDurTime;

	private bool _bMutex = true;

	public TBuff()
	{
		SetID(_nNextValidID);
	}

	private void SetID(int id)
	{
		_nID = id;
		_nNextValidID = _nID + 1;
	}

	public int GetID()
	{
		return _nID;
	}

	public void SetBuffType(int type)
	{
		_nType = type;
	}

	public int GetBuffType()
	{
		return _nType;
	}

	public void SetStartTime(float sT)
	{
		_fStartTime = sT;
	}

	public float GetStartTime()
	{
		return _fStartTime;
	}

	public void SetDurTime(float dT)
	{
		_fDurTime = dT;
	}

	public float GetDurTime()
	{
		return _fDurTime;
	}

	public bool IsMutex()
	{
		return _bMutex;
	}

	public void SetMutex(bool b)
	{
		_bMutex = b;
	}
}
