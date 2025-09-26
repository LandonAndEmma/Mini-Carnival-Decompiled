using NGUI_COMUI;
using Protocol;
using UIGlobal;

public class UIMarket_BoxData : NGUI_COMUI.UI_BoxData
{
	public enum EDataType
	{
		RefreshBtnOnly = 0,
		NormalSlot = 1,
		SystemSlot = 2,
		FacebookIcon = 3,
		Gem = 4
	}

	private ECurrencyType _currencyType;

	private int _nPrice;

	private string _strName;

	private uint _authorId;

	private byte _remain_num;

	private uint _praiseNum;

	private string[] _units = new string[3];

	private byte _avatarAttribute;

	private bool _isAdItem;

	private byte _boxOwerInCaptionType;

	private int _inListID;

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

	public string Name
	{
		get
		{
			return _strName;
		}
		set
		{
			_strName = value;
		}
	}

	public uint AuthorId
	{
		get
		{
			return _authorId;
		}
		set
		{
			_authorId = value;
		}
	}

	public byte RemainNum
	{
		get
		{
			return _remain_num;
		}
		set
		{
			_remain_num = value;
		}
	}

	public uint PraiseNum
	{
		get
		{
			return _praiseNum;
		}
		set
		{
			_praiseNum = value;
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
			byte b = 0;
			if (_units[0] != string.Empty)
			{
				b |= 4;
			}
			if (_units[1] != string.Empty)
			{
				b |= 2;
			}
			if (_units[2] != string.Empty)
			{
				b |= 1;
			}
			AvatarAttribute = b;
		}
	}

	public byte AvatarAttribute
	{
		get
		{
			return _avatarAttribute;
		}
		set
		{
			_avatarAttribute = value;
		}
	}

	public bool IsAdItem
	{
		get
		{
			return _isAdItem;
		}
		set
		{
			_isAdItem = value;
		}
	}

	public byte BoxOwerInCaptionType
	{
		get
		{
			return _boxOwerInCaptionType;
		}
		set
		{
			_boxOwerInCaptionType = value;
		}
	}

	public int InListID
	{
		get
		{
			return _inListID;
		}
		set
		{
			_inListID = value;
		}
	}

	public UIMarket_BoxData(UIMarket_BoxData data)
	{
		base.ItemId = data.ItemId;
		base.DataType = data.DataType;
		CurrencyType = data.CurrencyType;
		Price = data.Price;
		base.Tex = data.Tex;
		AuthorId = data.AuthorId;
		RemainNum = data.RemainNum;
		PraiseNum = data.PraiseNum;
		base.SpriteName = data.SpriteName;
		base.Unit = data.Unit;
		Units = data.Units;
		_isAdItem = data.IsAdItem;
		BoxOwerInCaptionType = data.BoxOwerInCaptionType;
	}

	public UIMarket_BoxData(ShopItem data)
	{
		base.DataType = 1;
		base.Tex = null;
		base.SpriteName = string.Empty;
		base.Unit = string.Empty;
		_isAdItem = false;
		Price = (int)data.m_price;
		CurrencyType = (ECurrencyType)data.m_price_type;
		base.ItemId = data.m_id;
		AuthorId = data.m_author;
		RemainNum = data.m_remain_num;
		PraiseNum = data.m_praise;
		Units = data.m_unit;
	}

	public UIMarket_BoxData()
	{
		base.ItemId = 0uL;
		base.DataType = 1;
		CurrencyType = ECurrencyType.Unknow;
		Price = 0;
		base.Tex = null;
		base.SpriteName = string.Empty;
		base.Unit = string.Empty;
		_isAdItem = false;
	}
}
