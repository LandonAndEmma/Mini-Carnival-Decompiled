public class UIRPG_MyTeam_SelCardSelBtnData
{
	private int _curPos;

	private uint _cardId;

	private ulong _ItemId;

	private bool _isPutOn;

	public int CurPos
	{
		get
		{
			return _curPos;
		}
		set
		{
			_curPos = value;
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

	public ulong ItemId
	{
		get
		{
			return _ItemId;
		}
		set
		{
			_ItemId = value;
		}
	}

	public bool IsPutOn
	{
		get
		{
			return _isPutOn;
		}
		set
		{
			_isPutOn = value;
		}
	}

	public UIRPG_MyTeam_SelCardSelBtnData(uint cardId)
	{
		_cardId = cardId;
	}
}
