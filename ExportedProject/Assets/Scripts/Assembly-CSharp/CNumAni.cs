using UnityEngine;

public class CNumAni
{
	public uint _initNum;

	public uint _endNum;

	public float _fDurTime = 3f;

	public float _fStartTime;

	public UILabel _uiLabel;

	private bool _bAniEnd;

	public CNumAni(uint init, uint end, float startTime, UILabel label)
	{
		_initNum = init;
		_endNum = end;
		_fStartTime = startTime;
		_uiLabel = label;
		_bAniEnd = false;
	}

	public bool UpdateAni()
	{
		if (_bAniEnd)
		{
			return true;
		}
		int num = (int)((float)(int)_initNum + (float)(int)(_endNum - _initNum) * ((Time.time - _fStartTime) / _fDurTime));
		if (Time.time - _fStartTime < 0f)
		{
			_uiLabel.text = _initNum.ToString();
			return false;
		}
		if ((int)(_endNum - _initNum) >= 0)
		{
			if ((int)(_endNum - _initNum) < 3)
			{
				_bAniEnd = true;
				num = (int)_endNum;
			}
			else
			{
				if (num >= _endNum)
				{
					_bAniEnd = true;
					num = (int)_endNum;
				}
				if (num < 0)
				{
					_bAniEnd = true;
					num = 0;
				}
			}
		}
		else if ((int)(_endNum - _initNum) > -3)
		{
			_bAniEnd = true;
			num = (int)_endNum;
		}
		else if (num <= _endNum)
		{
			Debug.Log("------End Floor:" + num);
			_bAniEnd = true;
			num = (int)_endNum;
		}
		_uiLabel.text = num.ToString();
		return _bAniEnd;
	}
}
