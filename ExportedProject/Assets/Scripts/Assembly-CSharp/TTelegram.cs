using System;
using UnityEngine;

public class TTelegram : IComparable
{
	private const double _dSmallestDelay = 0.25;

	public int _nSender;

	public int _nReceive;

	public int _nMsgId;

	public float _dDispathTime;

	public object _pExtraInfo;

	public object _pExtraInfo2;

	public static readonly float SEND_MSG_IMMEDIATELY;

	public TTelegram()
	{
		_nSender = -1;
		_nReceive = -1;
		_nMsgId = -1;
		_dDispathTime = -1f;
		_pExtraInfo = null;
		_pExtraInfo2 = null;
	}

	public TTelegram(int sender, int receive, int msg, float dispathTime)
	{
		_nSender = sender;
		_nReceive = receive;
		_nMsgId = msg;
		_dDispathTime = dispathTime;
		_pExtraInfo = null;
		_pExtraInfo2 = null;
	}

	public TTelegram(int sender, int receive, int msg, float dispathTime, object extraInfo)
	{
		_nSender = sender;
		_nReceive = receive;
		_nMsgId = msg;
		_dDispathTime = dispathTime;
		_pExtraInfo = extraInfo;
		_pExtraInfo2 = null;
	}

	public TTelegram(int sender, int receive, int msg, float dispathTime, object extraInfo, object extraInfo2)
	{
		_nSender = sender;
		_nReceive = receive;
		_nMsgId = msg;
		_dDispathTime = dispathTime;
		_pExtraInfo = extraInfo;
		_pExtraInfo2 = extraInfo2;
	}

	public int CompareTo(object obj)
	{
		TTelegram tTelegram = obj as TTelegram;
		if ((double)Mathf.Abs(_dDispathTime - tTelegram._dDispathTime) < 0.25 && _nSender == tTelegram._nSender && _nReceive == tTelegram._nReceive && _nMsgId == tTelegram._nMsgId)
		{
			return 0;
		}
		return (_dDispathTime - tTelegram._dDispathTime > 0f) ? 1 : (-1);
	}
}
