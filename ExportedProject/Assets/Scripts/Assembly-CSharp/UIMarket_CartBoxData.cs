using NGUI_COMUI;
using UIGlobal;

public class UIMarket_CartBoxData : NGUI_COMUI.UI_BoxData
{
	private UIMarket_BoxData _marketBoxData;

	private ECurrencyType _currencyType;

	private int _nPrice;

	private string[] _units = new string[3];

	public UIMarket_BoxData MarketBoxData
	{
		get
		{
			return _marketBoxData;
		}
		set
		{
			_marketBoxData = value;
		}
	}

	public ECurrencyType CurrencyType
	{
		get
		{
			return _currencyType;
		}
		set
		{
			_currencyType = value;
		}
	}

	public int Price
	{
		get
		{
			return _nPrice;
		}
		set
		{
			_nPrice = value;
		}
	}

	public string[] Units
	{
		get
		{
			return _units;
		}
		set
		{
			_units = value;
		}
	}

	public UIMarket_CartBoxData()
	{
	}

	public UIMarket_CartBoxData(UIMarket_BoxData data)
	{
		_marketBoxData = data;
		base.DataType = data.DataType;
		Price = data.Price;
		base.Tex = data.Tex;
		Units = data.Units;
		base.ItemId = data.ItemId;
		CurrencyType = data.CurrencyType;
		base.Unit = data.Unit;
	}
}
