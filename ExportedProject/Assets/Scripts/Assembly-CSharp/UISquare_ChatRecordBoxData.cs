using NGUI_COMUI;
using Protocol;
using UnityEngine;

public class UISquare_ChatRecordBoxData : NGUI_COMUI.UI_BoxData
{
	private bool _bSelfRecord;

	private Channel _channel;

	private string _otherPeopleName;

	private string _otherPeopleChatContent;

	private uint _otherPeopleID;

	private Texture2D _otherPeopleIcon;

	private string _privatePeopleName;

	public bool SelfRecord
	{
		get
		{
			return _bSelfRecord;
		}
		set
		{
			_bSelfRecord = value;
		}
	}

	public Channel Channel
	{
		get
		{
			return _channel;
		}
	}

	public string OtherPeopleName
	{
		get
		{
			return _otherPeopleName;
		}
	}

	public string OtherPeopleChatContent
	{
		get
		{
			return _otherPeopleChatContent;
		}
	}

	public uint OtherPeopleID
	{
		get
		{
			return _otherPeopleID;
		}
	}

	public Texture2D OtherPeopleIcon
	{
		get
		{
			return _otherPeopleIcon;
		}
		set
		{
			_otherPeopleIcon = value;
		}
	}

	public string PrivatePeopleName
	{
		get
		{
			return _privatePeopleName;
		}
		set
		{
			_privatePeopleName = value;
		}
	}

	public UISquare_ChatRecordBoxData()
	{
		_otherPeopleID = 11u;
		_otherPeopleName = "tim";
		_otherPeopleChatContent = "Hello!";
		_channel = Channel.hall;
		_bSelfRecord = false;
	}

	public UISquare_ChatRecordBoxData(string otherPeopleName, string otherPeopleChatContent, Channel channel)
	{
		_otherPeopleID = 22u;
		_otherPeopleName = otherPeopleName;
		_otherPeopleChatContent = otherPeopleChatContent;
		_channel = channel;
		_bSelfRecord = false;
	}

	public UISquare_ChatRecordBoxData(uint playerID, bool bSelfRecord, string otherPeopleName, string otherPeopleChatContent, Channel channel)
	{
		_otherPeopleID = playerID;
		_otherPeopleName = otherPeopleName;
		_otherPeopleChatContent = otherPeopleChatContent;
		_channel = channel;
		_bSelfRecord = bSelfRecord;
	}

	public UISquare_ChatRecordBoxData(UISquare_ChatRecordBoxData data)
	{
		_otherPeopleID = data.OtherPeopleID;
		_otherPeopleIcon = data.OtherPeopleIcon;
		_otherPeopleName = data.OtherPeopleName;
		_otherPeopleChatContent = data.OtherPeopleChatContent;
		_channel = data.Channel;
		_bSelfRecord = data.SelfRecord;
	}
}
