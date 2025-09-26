using NGUI_COMUI;

public class UIBackpack_SellInfoBoxData : NGUI_COMUI.UI_BoxData
{
	private uint _ADTime;

	private int _soldedNum;

	private int _balanceNum;

	private int _favourNum;

	public uint ADTime
	{
		get
		{
			return _ADTime;
		}
		set
		{
			_ADTime = value;
		}
	}

	public int SoldedNum
	{
		get
		{
			return _soldedNum;
		}
		set
		{
			_soldedNum = value;
		}
	}

	public int BalanceNum
	{
		get
		{
			return _balanceNum;
		}
		set
		{
			_balanceNum = value;
		}
	}

	public int FavourNum
	{
		get
		{
			return _favourNum;
		}
		set
		{
			_favourNum = value;
		}
	}

	public UIBackpack_SellInfoBoxData()
	{
		_tex = null;
		_soldedNum = 0;
		_balanceNum = 0;
		_favourNum = 0;
		_ADTime = 0u;
	}
}
