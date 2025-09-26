using System;
using NGUI_COMUI;

public class UIRPG_CardMgr_Card_BoxData : NGUI_COMUI.UI_BoxData, IComparable
{
	public enum EDataType
	{
		None = 0,
		FirstLocked = 1,
		Locked = 2,
		Data = 3
	}

	private int _grade;

	private int _id;

	private string _name;

	private bool _bLimitSel;

	private bool _bShowInfo = true;

	public int CardGrade
	{
		get
		{
			return _grade;
		}
		set
		{
			_grade = value;
		}
	}

	public int CardId
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

	public string CardName
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

	public bool LimitSel
	{
		get
		{
			return _bLimitSel;
		}
		set
		{
			_bLimitSel = value;
		}
	}

	public bool ShowInfoBtn
	{
		get
		{
			return _bShowInfo;
		}
		set
		{
			_bShowInfo = value;
		}
	}

	public UIRPG_CardMgr_Card_BoxData()
	{
		base.DataType = 0;
		_bLimitSel = false;
		_bShowInfo = true;
	}

	public UIRPG_CardMgr_Card_BoxData(int grade, int id, string name)
	{
		_grade = grade;
		_id = id;
		_name = name;
		base.DataType = 3;
		_bLimitSel = false;
		_bShowInfo = true;
	}

	public UIRPG_CardMgr_Card_BoxData(int grade, int id, string name, bool showInfoBtn)
	{
		_grade = grade;
		_id = id;
		_name = name;
		base.DataType = 3;
		_bLimitSel = false;
		_bShowInfo = showInfoBtn;
	}

	public int CompareTo(object obj)
	{
		UIRPG_CardMgr_Card_BoxData uIRPG_CardMgr_Card_BoxData = (UIRPG_CardMgr_Card_BoxData)obj;
		if (CardGrade == uIRPG_CardMgr_Card_BoxData.CardGrade)
		{
			if (CardId - uIRPG_CardMgr_Card_BoxData.CardId == 0)
			{
				return 0;
			}
			return (CardId <= uIRPG_CardMgr_Card_BoxData.CardId) ? 1 : (-1);
		}
		return (CardGrade <= uIRPG_CardMgr_Card_BoxData.CardGrade) ? 1 : (-1);
	}
}
