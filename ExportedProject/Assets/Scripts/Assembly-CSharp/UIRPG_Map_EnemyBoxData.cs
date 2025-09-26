using System;
using NGUI_COMUI;

public class UIRPG_Map_EnemyBoxData : NGUI_COMUI.UI_BoxData, IComparable
{
	private int _cardId;

	private int _cardGrade;

	public int CardId
	{
		get
		{
			return _cardId;
		}
		set
		{
			_cardId = value;
		}
	}

	public int CardGrade
	{
		get
		{
			return _cardGrade;
		}
		set
		{
			_cardGrade = value;
		}
	}

	public int CompareTo(object obj)
	{
		int num = 0;
		UIRPG_Map_EnemyBoxData uIRPG_Map_EnemyBoxData = obj as UIRPG_Map_EnemyBoxData;
		if (_cardId == uIRPG_Map_EnemyBoxData.CardId)
		{
			return (_cardGrade >= uIRPG_Map_EnemyBoxData.CardGrade) ? 1 : (-1);
		}
		return (_cardId > uIRPG_Map_EnemyBoxData.CardId) ? 1 : (-1);
	}
}
