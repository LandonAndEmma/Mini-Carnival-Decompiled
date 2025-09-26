using NGUI_COMUI;
using UnityEngine;

public class UIBackpack_SellInfoBox : NGUI_COMUI.UI_Box
{
	[SerializeField]
	private UITexture _sellTex;

	[SerializeField]
	private UILabel _labelSoldedNum;

	[SerializeField]
	private UILabel _labelBalanceNum;

	[SerializeField]
	private UILabel _labelFavourNum;

	[SerializeField]
	private GameObject _objSoldout;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public override void FormatBoxName(int i)
	{
		if (i > 9)
		{
			base.gameObject.name = "UIBackpackSellInfoBox" + i;
		}
		else
		{
			base.gameObject.name = "UIBackpackSellInfoBox0" + i;
		}
	}

	public override void BoxDataChanged()
	{
		_objSoldout.SetActive(false);
		UIBackpack_SellInfoBoxData uIBackpack_SellInfoBoxData = base.BoxData as UIBackpack_SellInfoBoxData;
		if (uIBackpack_SellInfoBoxData == null)
		{
			SetLoseSelected();
			return;
		}
		_sellTex.mainTexture = uIBackpack_SellInfoBoxData.Tex;
		_labelSoldedNum.text = uIBackpack_SellInfoBoxData.SoldedNum.ToString();
		_labelBalanceNum.text = uIBackpack_SellInfoBoxData.BalanceNum.ToString();
		if (uIBackpack_SellInfoBoxData.FavourNum < 100000)
		{
			_labelFavourNum.text = uIBackpack_SellInfoBoxData.FavourNum.ToString();
		}
		else
		{
			_labelFavourNum.text = uIBackpack_SellInfoBoxData.FavourNum / 1000 + "K";
		}
		if (uIBackpack_SellInfoBoxData.SoldedNum >= COMA_DataConfig.Instance._sysConfig.Shop.item_num)
		{
			_objSoldout.SetActive(true);
		}
	}
}
