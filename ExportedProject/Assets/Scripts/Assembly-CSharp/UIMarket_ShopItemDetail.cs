using MC_UIToolKit;
using MessageID;
using UnityEngine;

public class UIMarket_ShopItemDetail : UIEntity
{
	[SerializeField]
	private GameObject _content;

	[SerializeField]
	private GameObject _leftDetail;

	[SerializeField]
	private UILabel _labelLeftNum;

	[SerializeField]
	private UILabel _labelLikeNum;

	[SerializeField]
	private UILabel _labelPriceNum;

	[SerializeField]
	private GameObject _praiseBtn;

	[SerializeField]
	private UIMarket_ButtonFavorites _btnFavoritesCmp;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this, ShopItemDetailRefresh);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this);
	}

	private bool ShopItemDetailRefresh(TUITelegram msg)
	{
		UIMarket_BoxData uIMarket_BoxData = msg._pExtraInfo as UIMarket_BoxData;
		if (uIMarket_BoxData == null)
		{
			_content.SetActive(false);
			return true;
		}
		_content.SetActive(true);
		_leftDetail.SetActive((uIMarket_BoxData.RemainNum < 10) ? true : false);
		if (uIMarket_BoxData.PraiseNum < 100000)
		{
			_labelLikeNum.text = uIMarket_BoxData.PraiseNum.ToString();
		}
		else
		{
			_labelLikeNum.text = (int)(uIMarket_BoxData.PraiseNum / 1000) + "K";
		}
		_labelPriceNum.text = uIMarket_BoxData.Price.ToString();
		uint authorId = uIMarket_BoxData.AuthorId;
		uint avatarID = (uint)uIMarket_BoxData.ItemId;
		_praiseBtn.SetActive(!UIGolbalStaticFun.IsAvatarInPraiseLst(avatarID));
		_btnFavoritesCmp.BtnState = (UIGolbalStaticFun.IsAvatarInFavoriteLst(avatarID) ? UIMarket_ButtonFavorites.State.Collected : UIMarket_ButtonFavorites.State.UnCollected);
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
