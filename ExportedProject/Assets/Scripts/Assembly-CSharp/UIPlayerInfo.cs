using System;
using UnityEngine;

[Serializable]
public class UIPlayerInfo : IComparable<UIPlayerInfo>
{
	[SerializeField]
	private string _strName;

	[SerializeField]
	private int _nNum;

	[SerializeField]
	private float _fHp;

	[SerializeField]
	private Texture2D _tex;

	private UIInGame_PlayersUIMgr _mgr;

	public UIInGame_PlayersUIMgr MGR
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

	public int Num
	{
		get
		{
			return _nNum;
		}
		set
		{
			_nNum = value;
			if (MGR != null)
			{
				MGR.DataChanged();
			}
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
			UI_InGamePlayerInfo uIByData = MGR.GetUIByData(this);
			if (uIByData != null)
			{
				uIByData.RefreshUI();
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
			UI_InGamePlayerInfo uIByData = MGR.GetUIByData(this);
			if (uIByData != null)
			{
				uIByData.RefreshUI();
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

	public UIPlayerInfo(string name, int num, float hp)
	{
		_strName = name;
		_nNum = num;
		_fHp = hp;
	}

	public UIPlayerInfo()
	{
		_strName = string.Empty;
		_nNum = 0;
		_fHp = 1f;
	}

	public int CompareTo(UIPlayerInfo other)
	{
		if (Num > other.Num)
		{
			return -1;
		}
		if (Num < other.Num)
		{
			return 1;
		}
		return 0;
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
