using NGUI_COMUI;

public class UIRPG_MyTeamBoxData : NGUI_COMUI.UI_BoxData
{
	public enum EDataType
	{
		None = 0,
		Data = 1,
		UnLock = 2
	}

	public int _curPos;

	private uint _cardId;

	private bool _isCaptain;

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

	public UIRPG_MyTeamBoxData(int type, int pos, uint id)
	{
		_dataType = type;
		_curPos = pos;
		_cardId = id;
	}
}
