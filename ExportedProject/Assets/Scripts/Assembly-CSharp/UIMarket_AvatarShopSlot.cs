using UnityEngine;

public class UIMarket_AvatarShopSlot : UI_BoxSlot
{
	[SerializeField]
	private GameObject _officalIcon;

	[SerializeField]
	private GameObject _selectedIcon;

	[SerializeField]
	private GameObject[] _currencyIcons;

	[SerializeField]
	private GameObject _onSaleIcon;

	private bool _bSelected;

	[SerializeField]
	private TUIMeshSprite _avatarIcon;

	[SerializeField]
	private TUILabel _priceLabel;

	public bool AvatarSelected
	{
		get
		{
			return _bSelected;
		}
		set
		{
			_bSelected = value;
			_selectedIcon.SetActive(_bSelected);
		}
	}

	private void Awake()
	{
		AvatarSelected = false;
		SetOnSaleInfo(0);
	}

	private void Start()
	{
	}

	private new void Update()
	{
	}

	public void SetOnSaleInfo(int salesOff)
	{
		if (!(_onSaleIcon == null))
		{
			if (salesOff <= 0)
			{
				_onSaleIcon.SetActive(false);
				return;
			}
			_onSaleIcon.SetActive(true);
			TUILabel componentInChildren = _onSaleIcon.transform.FindChild("Sale").GetComponentInChildren<TUILabel>();
			componentInChildren.Text = salesOff + "%";
		}
	}

	public void HandleEventButton_SelAvatar(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			if (base.BoxData != null)
			{
				AvatarSelected = !AvatarSelected;
				UIMarket uIMarket = (UIMarket)GetTUIMessageHandler(true);
				if (uIMarket != null)
				{
					uIMarket.AddShopSlot(this);
				}
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			}
			break;
		}
	}

	protected override void ProcessNullData()
	{
		base.ProcessNullData();
		_officalIcon.SetActive(false);
		_selectedIcon.SetActive(false);
		_avatarIcon.gameObject.SetActive(false);
		if (_priceLabel != null)
		{
			_priceLabel.gameObject.SetActive(false);
		}
		if (_currencyIcons.Length >= 2)
		{
			_currencyIcons[0].SetActive(false);
			_currencyIcons[1].SetActive(false);
		}
		SetOnSaleInfo(0);
	}

	public override int NotifyDataUpdate()
	{
		UIMarket_AvatarShopData uIMarket_AvatarShopData = (UIMarket_AvatarShopData)base.BoxData;
		if (base.NotifyDataUpdate() == -1)
		{
			return -1;
		}
		if (uIMarket_AvatarShopData.OfficalIcon)
		{
			_officalIcon.SetActive(true);
		}
		else
		{
			_officalIcon.SetActive(false);
		}
		if (_priceLabel != null)
		{
			_priceLabel.gameObject.SetActive(true);
			_priceLabel.Text = Mathf.Abs(uIMarket_AvatarShopData.AvatarPrice).ToString();
			if (uIMarket_AvatarShopData.AvatarPrice > 0)
			{
				_currencyIcons[0].SetActive(true);
				_currencyIcons[1].SetActive(false);
			}
			else if (uIMarket_AvatarShopData.AvatarPrice < 0)
			{
				_currencyIcons[1].SetActive(true);
				_currencyIcons[0].SetActive(false);
			}
			else
			{
				_priceLabel.gameObject.SetActive(false);
				_currencyIcons[0].SetActive(false);
				_currencyIcons[1].SetActive(false);
			}
		}
		if (uIMarket_AvatarShopData.AvatarIconName != string.Empty)
		{
			_avatarIcon.gameObject.SetActive(true);
			_avatarIcon.UseCustomize = false;
			_avatarIcon.texture = uIMarket_AvatarShopData.AvatarIconName;
			_avatarIcon.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (uIMarket_AvatarShopData.AvatarIcon != null)
		{
			_avatarIcon.gameObject.SetActive(true);
			_avatarIcon.UseCustomize = true;
			_avatarIcon.CustomizeTexture = uIMarket_AvatarShopData.AvatarIcon;
			_avatarIcon.CustomizeRect = new Rect(0f, 0f, uIMarket_AvatarShopData.AvatarIcon.width, uIMarket_AvatarShopData.AvatarIcon.height);
			_avatarIcon.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		}
		if (uIMarket_AvatarShopData.PartType == "HBL01")
		{
			SetOnSaleInfo(100 - COMA_Sys.Instance.modelSuitSaleRate);
		}
		return 0;
	}
}
