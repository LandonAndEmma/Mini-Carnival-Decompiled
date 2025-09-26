using System;
using NGUI_COMUI;

public class UIRPG_MyTeam_SelCardBoxData : NGUI_COMUI.UI_BoxData, IComparable
{
	private bool _isCaptain;

	private bool _isSel;

	private uint _cardId;

	private byte _cardGrade;

	public bool IsCaptain
	{
		get
		{
			return _isCaptain;
		}
		set
		{
			_isCaptain = value;
		}
	}

	public bool IsSel
	{
		get
		{
			return _isSel;
		}
		set
		{
			_isSel = value;
		}
	}

	public uint CardId
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

	public byte CardGrade
	{
		get
		{
			return _cardGrade;
		}
	}

	public UIRPG_MyTeam_SelCardBoxData(ulong itemId, uint cardId, byte grade)
	{
		_itemId = itemId;
		_cardId = cardId;
		_cardGrade = grade;
	}

	public int CompareTo(object obj)
	{
		UIRPG_MyTeam_SelCardBoxData uIRPG_MyTeam_SelCardBoxData = (UIRPG_MyTeam_SelCardBoxData)obj;
		if (CardGrade == uIRPG_MyTeam_SelCardBoxData.CardGrade)
		{
			if (CardId == uIRPG_MyTeam_SelCardBoxData.CardId)
			{
				return 0;
			}
			return (CardId <= uIRPG_MyTeam_SelCardBoxData.CardId) ? 1 : (-1);
		}
		return (CardGrade <= uIRPG_MyTeam_SelCardBoxData.CardGrade) ? 1 : (-1);
	}
}
