using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using UIGlobal;
using UnityEngine;

public class UICardMgr_PickCard : UIEntity
{
	[SerializeField]
	private UISprite _sprite_light;

	[SerializeField]
	private UISprite _sprite_arrow;

	[SerializeField]
	private UIRPGCardMgr _cardMgr;

	[SerializeField]
	private UIRPGCardManage_Container _cardMgrContainer;

	[SerializeField]
	private UILabel _labelCouponNum;

	private void Start()
	{
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UING_RPG_CardMgr_Click1, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	protected override void Load()
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 0f;
		base.transform.localPosition = localPosition;
		_sprite_light.enabled = false;
		_sprite_arrow.enabled = false;
		Debug.Log("......................Pick Card Btn " + UIDataBufferCenter.Instance.CurNGIndex);
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.CurNGIndex == 1)
		{
			localPosition = base.transform.localPosition;
			localPosition.z = -150f;
			base.transform.localPosition = localPosition;
			_sprite_light.enabled = true;
			_sprite_arrow.enabled = true;
		}
		RegisterMessage(EUIMessageID.UING_RPG_CardMgr_Click1, this, NGCardMgr_Click1);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "LackCoupon":
			_cardMgr.PopUpObtainCoupon.SetActive(true);
			break;
		}
		return true;
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare)
		{
			UIDataBufferCenter.Instance.CurNGIndex++;
			TLoadScene extraInfo = new TLoadScene("UI.RPG.CardManage_FreeCardA", ELoadLevelParam.AddHidePre);
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
			int num = int.Parse(_labelCouponNum.text);
			_labelCouponNum.text = (num - 5).ToString();
			return;
		}
		if (UIDataBufferCenter.Instance.RPGData.m_coupon < 5)
		{
			string des = TUITool.StringFormat(Localization.instance.Get("getcard_desc5"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, des);
			uIMessage_CommonBoxData.Mark = "LackCoupon";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
			return;
		}
		int count = _cardMgrContainer.LstBoxs.Count;
		int maxCapacity_CardBag = RPGGlobalData.Instance.RpgMiscUnit._maxCapacity_CardBag;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < count; i++)
		{
			if (_cardMgrContainer.LstBoxs[i].BoxData.DataType == 3)
			{
				num2++;
			}
			else if (_cardMgrContainer.LstBoxs[i].BoxData.DataType == 0)
			{
				num3++;
			}
		}
		Debug.Log("bagCurCapacity : " + count);
		Debug.Log("bagCurDataCapacity : " + num2);
		Debug.Log("bagCurNoneCapacity : " + num3);
		if (num2 >= maxCapacity_CardBag || (num2 + num3 >= maxCapacity_CardBag && num3 < RPGGlobalData.Instance.RpgMiscUnit._cardNum_extractCard))
		{
			string des2 = TUITool.StringFormat(Localization.instance.Get("cardbag_desc2"));
			UIMessage_CommonBoxData uIMessage_CommonBoxData2 = new UIMessage_CommonBoxData(1, des2);
			uIMessage_CommonBoxData2.Mark = "OverBagMaxCapacity";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData2);
			return;
		}
		if (num3 >= RPGGlobalData.Instance.RpgMiscUnit._cardNum_extractCard)
		{
			TLoadScene extraInfo2 = new TLoadScene("UI.RPG.CardManage_FreeCardA", ELoadLevelParam.AddHidePre);
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo2);
			return;
		}
		int num4 = RPGGlobalData.Instance.RpgMiscUnit._cardNum_extractCard - num3;
		string text = TUITool.StringFormat(Localization.instance.Get("cardbag_desc3"), RPGGlobalData.Instance.RpgMiscUnit._unitCardBagPrice * num4, num4);
		Debug.Log("strTxt + " + text);
		UIMessage_CommonBoxData uIMessage_CommonBoxData3 = new UIMessage_CommonBoxData(0, text);
		uIMessage_CommonBoxData3.Mark = "UnlockBagForDrawCard";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData3);
	}

	private bool NGCardMgr_Click1(TUITelegram msg)
	{
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare && UIDataBufferCenter.Instance.CurNGIndex == 2)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.z = -150f;
			base.transform.localPosition = localPosition;
			_sprite_light.enabled = true;
			_sprite_arrow.enabled = true;
		}
		return true;
	}
}
