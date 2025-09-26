using NGUI_COMUI;
using Protocol;

public class UIRankings_FriendBoxData : NGUI_COMUI.UI_BoxData
{
	public WatchRoleInfo watchInfo;

	private int _lv;

	private bool _rpg;

	private uint _id;

	private string _name;

	private uint _score;

	private uint _rank;

	public int LV
	{
		get
		{
			return _lv;
		}
		set
		{
			_lv = value;
		}
	}

	public bool IsRPG
	{
		get
		{
			return _rpg;
		}
		set
		{
			_rpg = value;
		}
	}

	public uint PlayerID
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

	public string PlayerName
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

	public uint PlayerScore
	{
		get
		{
			return _score;
		}
		set
		{
			_score = value;
		}
	}

	public uint PlayerRank
	{
		get
		{
			return _rank;
		}
		set
		{
			_rank = value;
		}
	}

	public UIRankings_FriendBoxData()
	{
		_id = 0u;
		_name = string.Empty;
		_score = 0u;
		_rank = 0u;
		_rpg = false;
	}
}
