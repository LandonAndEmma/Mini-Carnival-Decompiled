using NGUI_COMUI;
using Protocol;

public class UIRankings_WorldBoxData : NGUI_COMUI.UI_BoxData
{
	public WatchRoleInfo watchInfo;

	private uint _id;

	private string _name;

	private uint _score;

	private int _awardCrystal;

	private string _awardSerialName;

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

	public int AwardCrystal
	{
		get
		{
			return _awardCrystal;
		}
		set
		{
			_awardCrystal = value;
		}
	}

	public string AwardSerialName
	{
		get
		{
			return _awardSerialName;
		}
		set
		{
			_awardSerialName = value;
		}
	}

	public UIRankings_WorldBoxData()
	{
		_id = 0u;
		_name = string.Empty;
		_score = 0u;
		_awardCrystal = 0;
		_awardSerialName = string.Empty;
	}
}
