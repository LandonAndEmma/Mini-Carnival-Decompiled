using MessageID;
using UnityEngine;

public class UIADAvatar_CountdownEntity : UIEntity
{
	[SerializeField]
	private GameObject _objShow;

	private bool _countdown;

	private uint _AdTime;

	[SerializeField]
	private UILabel _labelTime;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIBackpack_SellListSelChanged, this, SellListSelChanged);
		_countdown = false;
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
				if (uIBackpack_SellInfoBoxData.ADTime != 0 && serverTime - (double)uIBackpack_SellInfoBoxData.ADTime <= (double)(COMA_DataConfig.Instance._sysConfig.Ad.day_limit * 24 * 60 * 60))
				{
					_objShow.SetActive(true);
					_countdown = true;
					_AdTime = uIBackpack_SellInfoBoxData.ADTime;
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

	private string ConvertDoubleTimeToFormatStr(double db)
	{
		double num = db / 3600.0;
		double num2 = (num - (double)(int)num) * 60.0;
		string text = string.Empty;
		if ((int)num < 10)
		{
			text += "0";
		}
		text += (int)num;
		text += ":";
		if ((int)num2 < 10)
		{
			text += "0";
		}
		return text + (int)num2;
	}

	protected override void Tick()
	{
		if (_countdown)
		{
			double serverTime = COMA_Server_Account.Instance.GetServerTime();
			_labelTime.text = ConvertDoubleTimeToFormatStr((double)(COMA_DataConfig.Instance._sysConfig.Ad.day_limit * 24 * 60 * 60) - (serverTime - (double)_AdTime));
		}
	}
}
