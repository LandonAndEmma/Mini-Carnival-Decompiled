using NGUI_COMUI;
using Protocol;
using UnityEngine;

public class UIRankings_SearchFriendBoxData : NGUI_COMUI.UI_BoxData
{
	public WatchRoleInfo watchInfo;

	private uint _id;

	private string _name;

	private Texture2D _icon;

	public uint PlayerID
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	public string PlayerName
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public Texture2D PlayerIcon
	{
		get
		{
			return _icon;
		}
		set
		{
			_icon = value;
		}
	}

	public UIRankings_SearchFriendBoxData()
	{
		_id = 0u;
		_name = string.Empty;
		_icon = null;
	}

	public UIRankings_SearchFriendBoxData(uint id, string name)
	{
		_id = id;
		_name = name;
		_icon = null;
	}
}
