using System;
using UnityEngine;

public class TUITelegram : IComparable
{
	private const double _dSmallestDelay = 0.25;

	public int _nChannel;

	public object _pSender;

	public int _nReceive;

	public int _nMsgId;

	public float _dDispathTime;

	public object _pExtraInfo;

	public object _pExtraInfo2;

	public UIAcrossSceneAniController.UIAniLinkEvent linkEvent;

	public static readonly float SEND_MSG_IMMEDIATELY;

	public TUITelegram()
	{
		_pSender = null;
		_nReceive = -1;
		_nMsgId = -1;
		_nChannel = 0;
		_dDispathTime = -1f;
		_pExtraInfo = null;
		_pExtraInfo2 = null;
	}

	public TUITelegram(object sender, int receive, int msg, float dispathTime)
	{
		_pSender = sender;
		_nReceive = receive;
		_nMsgId = msg;
		_nChannel = 0;
		_dDispathTime = dispathTime;
		_pExtraInfo = null;
		_pExtraInfo2 = null;
	}

	public TUITelegram(object sender, int receive, int msg, float dispathTime, object extraInfo)
	{
		_pSender = sender;
		_nReceive = receive;
		_nMsgId = msg;
		_nChannel = 0;
		_dDispathTime = dispathTime;
		_pExtraInfo = extraInfo;
		_pExtraInfo2 = null;
	}

	public TUITelegram(object sender, int receive, int msg, float dispathTime, object extraInfo, object extraInfo2)
	{
		_pSender = sender;
		_nReceive = receive;
		_nMsgId = msg;
		_nChannel = 0;
		_dDispathTime = dispathTime;
		_pExtraInfo = extraInfo;
		_pExtraInfo2 = extraInfo2;
	}

	public TUITelegram(object sender, int receive, int msg, float dispathTime, object extraInfo, object extraInfo2, UIAcrossSceneAniController.UIAniLinkEvent ExtaraEvent)
	{
		_pSender = sender;
		_nReceive = receive;
		_nMsgId = msg;
		_nChannel = 0;
		_dDispathTime = dispathTime;
		_pExtraInfo = extraInfo;
		_pExtraInfo2 = extraInfo2;
		linkEvent = ExtaraEvent;
	}

	public TUITelegram(object sender, int receive, int msg, float dispathTime, int channel, object extraInfo)
	{
		_pSender = sender;
		_nReceive = receive;
		_nMsgId = msg;
		_nChannel = channel;
		_dDispathTime = dispathTime;
		_pExtraInfo = extraInfo;
		_pExtraInfo2 = null;
	}

	public TUITelegram(object sender, int receive, int msg, float dispathTime, int channel, object extraInfo, object extraInfo2)
	{
		_pSender = sender;
		_nReceive = receive;
		_nMsgId = msg;
		_nChannel = channel;
		_dDispathTime = dispathTime;
		_pExtraInfo = extraInfo;
		_pExtraInfo2 = extraInfo2;
	}

	public int CompareTo(object obj)
	{
		TUITelegram tUITelegram = obj as TUITelegram;
		if ((double)Mathf.Abs(_dDispathTime - tUITelegram._dDispathTime) < 0.25 && _pSender == tUITelegram._pSender && _nReceive == tUITelegram._nReceive && _nMsgId == tUITelegram._nMsgId)
		{
			return 0;
		}
		return (_dDispathTime - tUITelegram._dDispathTime > 0f) ? 1 : (-1);
	}
}
