using System;
using UnityEngine;

[Serializable]
public class UIHungersPlayerInfo
{
	[SerializeField]
	private string _strName;

	[SerializeField]
	private float _fHp;

	[SerializeField]
	private bool _bSelf;

	[SerializeField]
	private Texture2D _tex;

	private UIInGameHungers_PlayersMgr _mgr;

	private UIInGame_HungersPlayerInfo _uiInfo;

	public UIInGameHungers_PlayersMgr MGR
	{
		get
		{
			return _mgr;
		}
		set
		{
			_mgr = value;
		}
	}

	public UIInGame_HungersPlayerInfo UIInfo
	{
		get
		{
			return _uiInfo;
		}
		set
		{
			_uiInfo = value;
		}
	}

	public float HP
	{
		get
		{
			return _fHp;
		}
		set
		{
			_fHp = value;
			if (UIInfo != null)
			{
				UIInfo.RefreshUI();
			}
		}
	}

	public string Name
	{
		get
		{
			return _strName;
		}
		set
		{
			_strName = value;
			if (UIInfo != null)
			{
				UIInfo.RefreshUI();
			}
		}
	}

	public bool IsSelf
	{
		get
		{
			return _bSelf;
		}
		set
		{
			_bSelf = value;
			if (UIInfo != null)
			{
				UIInfo.RefreshUI();
			}
		}
	}

	public Texture2D Tex2D
	{
		get
		{
			return _tex;
		}
		set
		{
			_tex = value;
		}
	}

	public UIHungersPlayerInfo(string name, float hp)
	{
		_strName = name;
		_fHp = hp;
		_bSelf = false;
	}

	public UIHungersPlayerInfo(string name, float hp, bool self)
	{
		_strName = name;
		_fHp = hp;
		_bSelf = self;
	}

	public UIHungersPlayerInfo()
	{
		_strName = string.Empty;
		_fHp = 1f;
		_bSelf = false;
	}

	public void DelayAssignment(Texture2D tex)
	{
		Tex2D = tex;
		if (MGR != null && _tex != null)
		{
			MGR.DelayConcentData(this, _tex);
		}
	}
}
