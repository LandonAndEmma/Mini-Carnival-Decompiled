using NGUI_COMUI;
using UnityEngine;

public class UISquare_ChatFriendBoxData : NGUI_COMUI.UI_BoxData
{
	private string _friendName;

	private uint _friendID;

	private Texture2D _friendTexture;

	public string FriendName
	{
		get
		{
			return _friendName;
		}
		set
		{
			_friendName = value;
		}
	}

	public uint FriendID
	{
		get
		{
			return _friendID;
		}
		set
		{
			_friendID = value;
		}
	}

	public Texture2D FriendTexture
	{
		get
		{
			return _friendTexture;
		}
		set
		{
			_friendTexture = value;
		}
	}

	public UISquare_ChatFriendBoxData(string name)
	{
		_friendName = name;
		_friendID = 0u;
		_friendTexture = null;
	}

	public UISquare_ChatFriendBoxData(string name, uint id)
	{
		_friendName = name;
		_friendID = id;
		_friendTexture = null;
	}
}
