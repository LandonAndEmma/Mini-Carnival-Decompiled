using MessageID;
using UnityEngine;

public class UIBtn_ADAvatarEntity : UIEntity
{
	[SerializeField]
	private GameObject _objShow;

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
				_objShow.SetActive(false);
			}
			else
			{
				double serverTime = COMA_Server_Account.Instance.GetServerTime();
				if (uIBackpack_SellInfoBoxData.ADTime == 0 || serverTime - (double)uIBackpack_SellInfoBoxData.ADTime > (double)(COMA_DataConfig.Instance._sysConfig.Ad.day_limit * 24 * 60 * 60))
				{
					_objShow.SetActive(true);
				}
				else
				{
					_objShow.SetActive(false);
				}
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
