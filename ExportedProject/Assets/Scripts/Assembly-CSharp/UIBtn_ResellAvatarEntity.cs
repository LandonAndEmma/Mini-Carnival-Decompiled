using MessageID;
using UnityEngine;

public class UIBtn_ResellAvatarEntity : UIEntity
{
	[SerializeField]
	private GameObject _objShow;

	[SerializeField]
	private UILabel _resellPrice;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIBackpack_SellListSelChanged, this, SellListSelChanged);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIBackpack_SellListSelChanged, this);
	}

	private bool SellListSelChanged(TUITelegram msg)
	{
		UIBackpack_SellInfoBoxData uIBackpack_SellInfoBoxData = (UIBackpack_SellInfoBoxData)msg._pExtraInfo;
		if (uIBackpack_SellInfoBoxData != null)
		{
			if (uIBackpack_SellInfoBoxData.SoldedNum >= COMA_DataConfig.Instance._sysConfig.Shop.item_num)
			{
				_resellPrice.text = COMA_DataConfig.Instance._sysConfig.Shop.resell_price.ToString();
				_objShow.SetActive(true);
			}
			else
			{
				_objShow.SetActive(false);
			}
		}
		else
		{
			_objShow.SetActive(false);
		}
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
