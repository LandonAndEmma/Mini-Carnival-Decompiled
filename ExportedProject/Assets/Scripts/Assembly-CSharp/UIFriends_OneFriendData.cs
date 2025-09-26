using System;
using UnityEngine;

public class UIFriends_OneFriendData : IComparable<UIFriends_OneFriendData>
{
	private bool _bAddBtn;

	private string _gid;

	private string _strName;

	private Texture2D _tex2D;

	private DateTime _dt;

	private UIFriends_OneFriend _control;

	private string _url = string.Empty;

	public UIFriends_OneFriend DataControl
	{
		get
		{
			return _control;
		}
		set
		{
			_control = value;
		}
	}

	public bool IsAddBtn
	{
		get
		{
			return _bAddBtn;
		}
		set
		{
			_bAddBtn = value;
			if (DataControl != null)
			{
				DataControl.DataChanged();
			}
		}
	}

	public string GID
	{
		get
		{
			return _gid;
		}
		set
		{
			_gid = value;
		}
	}

	public string FriendName
	{
		get
		{
			return _strName;
		}
		set
		{
			_strName = value;
			if (DataControl != null)
			{
				DataControl.DataChanged();
			}
		}
	}

	public string URL
	{
		get
		{
			return _url;
		}
		set
		{
			_url = value;
		}
	}

	public Texture2D FriendTex2D
	{
		get
		{
			return _tex2D;
		}
		set
		{
			_tex2D = value;
			if (DataControl != null)
			{
				DataControl.DataChanged();
			}
		}
	}

	public UIFriends_OneFriendData()
	{
		_bAddBtn = true;
		_strName = string.Empty;
		_tex2D = null;
	}

	public int CompareTo(UIFriends_OneFriendData other)
	{
		if (_dt > other._dt)
		{
			return -1;
		}
		if (_dt < other._dt)
		{
			return 1;
		}
		return 0;
	}
}
