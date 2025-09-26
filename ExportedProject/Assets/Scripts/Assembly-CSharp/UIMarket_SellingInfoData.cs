using UnityEngine;

public class UIMarket_SellingInfoData : UI_BoxData
{
	[SerializeField]
	private string _texId;

	[SerializeField]
	private Texture2D _avatarIcon;

	[SerializeField]
	private int _nDay;

	[SerializeField]
	private int _nHour;

	[SerializeField]
	private int _nMinute;

	[SerializeField]
	private int _nSold;

	[SerializeField]
	private int _nBalance;

	[SerializeField]
	private bool _bClaimed;

	public string TexID
	{
		get
		{
			return _texId;
		}
		set
		{
			_texId = value;
		}
	}

	public Texture2D AvatarIcon
	{
		get
		{
			return _avatarIcon;
		}
		set
		{
			_avatarIcon = value;
			DataChanged();
		}
	}

	public int DayNum
	{
		get
		{
			return _nDay;
		}
		set
		{
			_nDay = value;
			DataChanged();
		}
	}

	public int HourNum
	{
		get
		{
			return _nHour;
		}
		set
		{
			_nHour = value;
			DataChanged();
		}
	}

	public int MinuteNum
	{
		get
		{
			return _nMinute;
		}
		set
		{
			_nMinute = value;
			DataChanged();
		}
	}

	public int SoldNum
	{
		get
		{
			return _nSold;
		}
		set
		{
			_nSold = value;
			DataChanged();
		}
	}

	public int BalanceNum
	{
		get
		{
			return _nBalance;
		}
		set
		{
			_nBalance = value;
			DataChanged();
		}
	}

	public bool Cliamed
	{
		get
		{
			return _bClaimed;
		}
		set
		{
			_bClaimed = value;
			DataChanged();
		}
	}

	public UIMarket_SellingInfoData()
	{
		_bClaimed = true;
		_nBalance = 9000;
		_nSold = 30;
		_nMinute = 12;
		_nHour = 20;
		_nDay = 21;
		_avatarIcon = null;
	}

	public UIMarket_SellingInfoData(bool bClaimed, int balance, int sold, int minute, int hour, int day, Texture2D tex)
	{
		_bClaimed = bClaimed;
		_nBalance = balance;
		_nSold = sold;
		_nMinute = minute;
		_nHour = hour;
		_nDay = day;
		_avatarIcon = tex;
	}
}
