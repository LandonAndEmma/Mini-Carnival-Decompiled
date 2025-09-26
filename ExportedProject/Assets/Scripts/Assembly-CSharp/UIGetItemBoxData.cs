using UnityEngine;

public class UIGetItemBoxData : UIMessage_BoxData
{
	public enum EGetItemType
	{
		Gold = 0,
		Crystal = 1,
		Deco = 2,
		Other = 3,
		Gem = 4,
		Rpg_DropDeco = 5
	}

	private int _num;

	private int _GemId;

	public int GetNum
	{
		get
		{
			return _num;
		}
		set
		{
			_num = value;
		}
	}

	public int GemId
	{
		get
		{
			return _GemId;
		}
		set
		{
			_GemId = value;
		}
	}

	public UIGetItemBoxData(EGetItemType dataType, int num)
	{
		_dataType = (int)dataType;
		_num = num;
		_type = UIMessageBoxMgr.EMessageBoxType.GetItems;
		_layout = 0;
		_channel = (int)_type;
	}

	public UIGetItemBoxData(EGetItemType dataType, int num, int gemId)
	{
		_dataType = (int)dataType;
		_num = num;
		_type = UIMessageBoxMgr.EMessageBoxType.GetItems;
		_layout = 0;
		_channel = (int)_type;
		_GemId = gemId;
	}

	public UIGetItemBoxData(EGetItemType dataType, string deco)
	{
		_dataType = (int)dataType;
		_spriteName = deco;
		_type = UIMessageBoxMgr.EMessageBoxType.GetItems;
		_layout = 0;
		_channel = (int)_type;
	}

	public UIGetItemBoxData(EGetItemType dataType, Texture2D otherTex)
	{
		_dataType = (int)dataType;
		_tex = otherTex;
		_type = UIMessageBoxMgr.EMessageBoxType.GetItems;
		_layout = 0;
		_channel = (int)_type;
	}
}
