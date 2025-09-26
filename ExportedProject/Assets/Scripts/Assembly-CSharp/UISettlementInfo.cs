using System;
using UnityEngine;

[Serializable]
public class UISettlementInfo : IComparable<UISettlementInfo>
{
	[SerializeField]
	private string _strName;

	[SerializeField]
	private int _nStarNum;

	[SerializeField]
	private int _nAddExpNum;

	[SerializeField]
	private int _nAddGoldNum;

	[SerializeField]
	private int _nAddGemNum;

	[SerializeField]
	private int _nLv;

	[SerializeField]
	private float _fExp;

	[SerializeField]
	private float _fAddExpRatio;

	[SerializeField]
	private RenderTexture _tex;

	private UIInGame_SettlementMgr _mgr;

	public UIInGame_SettlementMgr MGR
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

	public int StarNum
	{
		get
		{
			return _nStarNum;
		}
	}

	public int AddExpNum
	{
		get
		{
			return _nAddExpNum;
		}
	}

	public int GoldNum
	{
		get
		{
			return _nAddGoldNum;
		}
	}

	public int GemNum
	{
		get
		{
			return _nAddGemNum;
		}
	}

	public string Name
	{
		get
		{
			return _strName;
		}
	}

	public int LV
	{
		get
		{
			return _nLv;
		}
		set
		{
			_nLv = value;
		}
	}

	public float ExpRatio
	{
		get
		{
			return _fExp;
		}
		set
		{
			_fExp = value;
		}
	}

	public float AddExpRatio
	{
		get
		{
			return _fAddExpRatio;
		}
		set
		{
			_fAddExpRatio = value;
		}
	}

	public RenderTexture Tex2D
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

	public UISettlementInfo()
	{
	}

	public UISettlementInfo(string name, int starNum, int addExpNum, int addGoldNum, int addGemNum, int lv, float exp, float addExpRatio)
	{
		SetInfo(name, starNum, addExpNum, addGoldNum, addGemNum, lv, exp, addExpRatio);
	}

	public void SetInfo(string name, int starNum, int addExpNum, int addGoldNum, int addGemNum, int lv, float exp, float addExpRatio)
	{
		_strName = name;
		_nStarNum = starNum;
		_nAddExpNum = addExpNum;
		_nAddGoldNum = addGoldNum;
		_nAddGemNum = addGemNum;
		_nLv = lv;
		_fExp = exp;
		_fAddExpRatio = addExpRatio;
	}

	public int CompareTo(UISettlementInfo other)
	{
		if (_nStarNum > other._nStarNum)
		{
			return -1;
		}
		if (_nStarNum < other._nStarNum)
		{
			return 1;
		}
		return 0;
	}
}
