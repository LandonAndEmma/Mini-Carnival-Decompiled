using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol.Role.C2S;
using Protocol.Shop.C2S;
using UnityEngine;

public class UIMarketBoxRefreshMgr : UIEntity
{
	[SerializeField]
	private GameObject _freeObj;

	[SerializeField]
	private GameObject _payObj;

	[SerializeField]
	private UILabel _timeLabel;

	[SerializeField]
	private NGUI_COMUI.UI_Box _ownerBox;

	public string ResetRefreshCurTime
	{
		set
		{
			_timeLabel.text = value;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIMarket_BoxRefresh_Free, this, BoxRefresh_Free);
		RegisterMessage(EUIMessageID.UIMarket_BoxRefresh_Charge, this, BoxRefresh_Charge);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIMarket_BoxRefresh_Free, this);
		UnregisterMessage(EUIMessageID.UIMarket_BoxRefresh_Charge, this);
	}

	protected override void Tick()
	{
		float num = 0f;
		num = ((((UIMarket_BoxData)_ownerBox.BoxData).BoxOwerInCaptionType != 2) ? (Time.time - COMA_Sys.Instance.marketRefreshTime_ADRandom) : (Time.time - COMA_Sys.Instance.marketRefreshTime));
		num = Mathf.Clamp(num, 0f, COMA_Sys.Instance.marketRefreshInterval);
		float value = (float)COMA_Sys.Instance.marketRefreshInterval - num;
		value = Mathf.Clamp(value, 0f, COMA_Sys.Instance.marketRefreshInterval);
		int value2 = Mathf.CeilToInt(value);
		value2 = Mathf.Clamp(value2, 0, COMA_Sys.Instance.marketRefreshInterval);
		string formatTime = GetFormatTime(value2);
		ResetRefreshCurTime = formatTime;
		if (value2 == 0)
		{
			_payObj.SetActive(false);
			_freeObj.SetActive(true);
		}
		else
		{
			_payObj.SetActive(true);
			_freeObj.SetActive(false);
		}
	}

	private string GetFormatTime(int time)
	{
		string empty = string.Empty;
		int num = time / 60;
		int num2 = time % 60;
		empty = string.Format("{0:00}", num);
		empty += ":";
		string text = string.Format("{0:00}", num2);
		return empty + text;
	}

	private bool BoxRefresh_Free(TUITelegram msg)
	{
		Debug.Log(base.name + "Refresh Time Free;" + (GetShopListCmd.Code)((UIMarket_BoxData)_ownerBox.BoxData).BoxOwerInCaptionType);
		if (((UIMarket_BoxData)_ownerBox.BoxData).BoxOwerInCaptionType == 2)
		{
			COMA_Sys.Instance.marketRefreshTime = Time.time;
		}
		else
		{
			COMA_Sys.Instance.marketRefreshTime_ADRandom = Time.time;
		}
		return true;
	}

	private bool BoxRefresh_Charge(TUITelegram msg)
	{
		if (COMA_Pref.Instance.GetCrystal() > 0)
		{
			Debug.Log(base.name + "Refresh Time Charge;" + (GetShopListCmd.Code)((UIMarket_BoxData)_ownerBox.BoxData).BoxOwerInCaptionType);
			if (((UIMarket_BoxData)_ownerBox.BoxData).BoxOwerInCaptionType == 2)
			{
				COMA_Sys.Instance.marketRefreshTime = Time.time;
			}
			else
			{
				COMA_Sys.Instance.marketRefreshTime_ADRandom = Time.time;
			}
			ModifyCrystalCmd modifyCrystalCmd = new ModifyCrystalCmd();
			modifyCrystalCmd.m_val = -1;
			modifyCrystalCmd.m_op = 1;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, modifyCrystalCmd);
		}
		else
		{
			UIGolbalStaticFun.PopMsgBox_LackMoney();
		}
		return true;
	}
}
