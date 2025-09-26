using NGUI_COMUI;
using UnityEngine;

public class UIMarket_FittingBoxData : NGUI_COMUI.UI_BoxData
{
	private string[] _units = new string[3];

	private byte _avatarAttribute;

	private int _singleAvatar = -1;

	public string[] Units
	{
		get
		{
			return _units;
		}
		set
		{
			_units = value;
			byte b = 0;
			if (_units[0] != string.Empty)
			{
				b |= 4;
			}
			if (_units[1] != string.Empty)
			{
				b |= 2;
			}
			if (_units[2] != string.Empty)
			{
				b |= 1;
			}
			AvatarAttribute = b;
		}
	}

	public byte AvatarAttribute
	{
		get
		{
			return _avatarAttribute;
		}
		set
		{
			_avatarAttribute = value;
		}
	}

	public int SingleAvatar
	{
		get
		{
			return _singleAvatar;
		}
		set
		{
			_singleAvatar = value;
		}
	}

	public UIMarket_FittingBoxData()
	{
		base.Tex = null;
		base.Unit = string.Empty;
		_singleAvatar = -1;
	}

	public UIMarket_FittingBoxData(UIMarket_BoxData data)
	{
		base.DataType = data.DataType;
		base.Tex = data.Tex;
		base.Unit = data.Unit;
		Units = data.Units;
		_singleAvatar = -1;
		Debug.Log("-----Units[0]=" + data.Units[0]);
	}
}
