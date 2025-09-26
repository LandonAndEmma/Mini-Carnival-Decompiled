using UnityEngine;

public class UIMarket_AvatarShopData : UI_BoxData
{
	[SerializeField]
	private bool _bOffical;

	[SerializeField]
	private int _nPrice;

	[SerializeField]
	private bool _bSuit;

	[SerializeField]
	private Texture2D _avatarIcon;

	private string _avatarIconName;

	[SerializeField]
	private string _nPartType;

	public string itemName = string.Empty;

	public bool OfficalIcon
	{
		get
		{
			return _bOffical;
		}
		set
		{
			_bOffical = value;
			DataChanged();
		}
	}

	public int AvatarPrice
	{
		get
		{
			return _nPrice;
		}
		set
		{
			_nPrice = value;
			DataChanged();
		}
	}

	public bool Suited
	{
		get
		{
			return _bSuit;
		}
		set
		{
			_bSuit = value;
			DataChanged();
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

	public string AvatarIconName
	{
		get
		{
			return _avatarIconName;
		}
		set
		{
			_avatarIconName = value;
			DataChanged();
		}
	}

	public string PartType
	{
		get
		{
			return _nPartType;
		}
		set
		{
			_nPartType = value;
			DataChanged();
		}
	}

	public UIMarket_AvatarShopData()
	{
		_bOffical = false;
		_nPrice = 0;
		_bSuit = false;
		_avatarIcon = null;
		_nPartType = string.Empty;
		_avatarIconName = string.Empty;
	}

	public UIMarket_AvatarShopData(bool offical, int price, bool suit, Texture2D icon)
	{
		_bOffical = offical;
		_nPrice = price;
		_bSuit = suit;
		_avatarIcon = icon;
		_nPartType = string.Empty;
		_avatarIconName = string.Empty;
	}

	public UIMarket_AvatarShopData(int price, string partType, Texture2D icon)
	{
		_bOffical = false;
		_nPrice = price;
		_bSuit = false;
		_avatarIcon = icon;
		_nPartType = partType;
		_avatarIconName = string.Empty;
	}

	public UIMarket_AvatarShopData(int price, string partType, Texture2D icon, string name)
	{
		_bOffical = false;
		_nPrice = price;
		_bSuit = false;
		_avatarIcon = icon;
		_nPartType = partType;
		_avatarIconName = name;
	}

	public UIMarket_AvatarShopData(UIMarket_AvatarShopData shopData)
	{
		_bOffical = shopData.OfficalIcon;
		_nPrice = shopData.AvatarPrice;
		_bSuit = shopData.Suited;
		_avatarIcon = shopData.AvatarIcon;
		_nPartType = shopData.PartType;
		_avatarIconName = shopData.AvatarIconName;
		DataChanged();
	}
}
