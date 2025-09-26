using MC_UIToolKit;
using MessageID;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UnityEngine;

public class UIRPG_CardManage_ObtainCouponMgr : UIEntity
{
	[SerializeField]
	private UILabel[] _desLabel;

	protected override void Load()
	{
		InitContentData();
		RegisterMessage(EUIMessageID.UIRPG_ObtainCouponBtnClick, this, HandleObtainCouponBtnClick);
		RegisterMessage(EUIMessageID.UIRPG_NotifyBuyCouponResult, this, HandleNotifyBuyCouponResult);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_ObtainCouponBtnClick, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyBuyCouponResult, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	private void InitContentData()
	{
		string colorStringByRPGAndValue = UIRPG_DataBufferCenter.GetColorStringByRPGAndValue("00aaff", RPGGlobalData.Instance.RpgMiscUnit._couponPrice_five);
		_desLabel[0].text = TUITool.StringFormat(Localization.instance.Get("getcard_desc1"), colorStringByRPGAndValue, 5);
		colorStringByRPGAndValue = UIRPG_DataBufferCenter.GetColorStringByRPGAndValue("00aaff", RPGGlobalData.Instance.RpgMiscUnit._couponPrice_fifty);
		_desLabel[1].text = TUITool.StringFormat(Localization.instance.Get("getcard_desc1"), colorStringByRPGAndValue, 50);
	}

	private bool HandleObtainCouponBtnClick(TUITelegram msg)
	{
		UIRPG_CardManage_ObtainCouponBuyBtn.ObtainCouponNum obtainCouponNum = (UIRPG_CardManage_ObtainCouponBuyBtn.ObtainCouponNum)(int)msg._pExtraInfo;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		BuyCouponsShopCmd buyCouponsShopCmd = new BuyCouponsShopCmd();
		switch (obtainCouponNum)
		{
		case UIRPG_CardManage_ObtainCouponBuyBtn.ObtainCouponNum.Five:
			buyCouponsShopCmd.m_index = 0;
			num2 = 1;
			num = RPGGlobalData.Instance.RpgMiscUnit._couponPrice_five;
			break;
		case UIRPG_CardManage_ObtainCouponBuyBtn.ObtainCouponNum.Fifty:
			buyCouponsShopCmd.m_index = 1;
			num3 = 1;
			num = RPGGlobalData.Instance.RpgMiscUnit._couponPrice_fifty;
			break;
		}
		if (UIDataBufferCenter.Instance.playerInfo.m_crystal < num)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("shangdianjiemian_desc28"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
			uIMessage_CommonBoxData.Mark = "CrystalNoEnough";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		}
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, buyCouponsShopCmd);
		COMA_HTTP_DataCollect.Instance.SendBuyCouponByGemCount(num2.ToString(), num3.ToString());
		return true;
	}

	private bool HandleNotifyBuyCouponResult(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		BuyCouponsShopResultCmd buyCouponsShopResultCmd = msg._pExtraInfo as BuyCouponsShopResultCmd;
		if (buyCouponsShopResultCmd.m_result == 0)
		{
			string str = TUITool.StringFormat(Localization.instance.Get("cardbag_desc4"));
			UIGolbalStaticFun.PopupTipsBox(str);
		}
		return true;
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "CrystalNoEnough":
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
			break;
		}
		return true;
	}
}
