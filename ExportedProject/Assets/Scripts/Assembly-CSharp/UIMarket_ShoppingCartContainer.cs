using MessageID;
using NGUI_COMUI;
using Protocol.Role.S2C;
using UIGlobal;
using UnityEngine;

public class UIMarket_ShoppingCartContainer : NGUI_COMUI.UI_Container
{
	[SerializeField]
	private UILabel _totalPriceLabel_Gold;

	[SerializeField]
	private UILabel _totalPriceLabel_Crystal;

	[SerializeField]
	private GameObject _icon_Gold;

	[SerializeField]
	private GameObject _icon_Crystal;

	public UICheckbox _equipedCheck;

	[SerializeField]
	private Transform _tranCenterPrice_Gold;

	[SerializeField]
	private Transform _tranCenterPrice_Crystal;

	protected override void Load()
	{
		base.Load();
	}

	protected override void UnLoad()
	{
		base.UnLoad();
	}

	public bool IsEnoughMoneyToBuy()
	{
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		if (_totalPriceLabel_Gold.enabled && notifyRoleDataCmd.m_info.m_gold < int.Parse(_totalPriceLabel_Gold.text))
		{
			return false;
		}
		if (_totalPriceLabel_Crystal.enabled && notifyRoleDataCmd.m_info.m_crystal < int.Parse(_totalPriceLabel_Crystal.text))
		{
			return false;
		}
		return true;
	}

	public void RefreshPrice()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < base.LstBoxs.Count; i++)
		{
			UIMarket_CartBoxData uIMarket_CartBoxData = base.LstBoxs[i].BoxData as UIMarket_CartBoxData;
			if (uIMarket_CartBoxData.CurrencyType == ECurrencyType.Gold)
			{
				num += uIMarket_CartBoxData.Price;
			}
			else if (uIMarket_CartBoxData.CurrencyType == ECurrencyType.Crystal)
			{
				num2 += uIMarket_CartBoxData.Price;
			}
		}
		int num3 = num;
		_totalPriceLabel_Gold.text = num3.ToString();
		_totalPriceLabel_Crystal.text = num2.ToString();
		bool flag = ((num != 0) ? true : false);
		_icon_Gold.SetActive(flag);
		_totalPriceLabel_Gold.enabled = flag;
		bool flag2 = ((num2 != 0) ? true : false);
		_icon_Crystal.SetActive(flag2);
		_totalPriceLabel_Crystal.enabled = flag2;
		if (flag && !flag2)
		{
			_tranCenterPrice_Gold.localPosition = new Vector3(90f, 0f, 0f);
			return;
		}
		if (!flag && flag2)
		{
			_tranCenterPrice_Crystal.localPosition = new Vector3(-80f, 0f, 0f);
			return;
		}
		_tranCenterPrice_Gold.localPosition = new Vector3(0f, 0f, 0f);
		_tranCenterPrice_Crystal.localPosition = new Vector3(0f, 0f, 0f);
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}

	protected override bool IsCanSelBox(NGUI_COMUI.UI_Box box, out NGUI_COMUI.UI_Box loseSel)
	{
		loseSel = null;
		return false;
	}

	protected override void ProcessBoxBeDeleted(NGUI_COMUI.UI_Box box)
	{
		UIMarket_BoxData marketBoxData = ((UIMarket_CartBoxData)box.BoxData).MarketBoxData;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_DelNewShoppingItem, this, marketBoxData);
		RefreshPrice();
	}

	protected override void ProcessBoxCanntSelected(NGUI_COMUI.UI_Box box)
	{
	}
}
