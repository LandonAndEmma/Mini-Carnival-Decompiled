using MC_UIToolKit;
using MessageID;
using UnityEngine;

public class UIIAP : UIEntity
{
	public enum EPurchaseType
	{
		PurchaseCrystal1 = 0,
		PurchaseCrystal2 = 1,
		PurchaseCrystal3 = 2,
		PurchaseCrystal4 = 3,
		PurchaseCrystal5 = 4,
		PurchaseGold1 = 5,
		PurchaseGold2 = 6,
		PurchaseGold3 = 7,
		PurchaseGold4 = 8,
		PurchaseGold5 = 9,
		Free = 10
	}

	public static readonly string[] IAPKeys = new string[5] { "com.trinitigame.callofminiavatar.099centsv10", "com.trinitigame.callofminiavatar.499cents", "com.trinitigame.callofminiavatar.999cents", "com.trinitigame.callofminiavatar.1999cents", "com.trinitigame.callofminiavatar.4999cents" };

	public static readonly int[,] IAPGold = new int[10, 2]
	{
		{ 0, 0 },
		{ 0, 0 },
		{ 0, 0 },
		{ 0, 0 },
		{ 0, 0 },
		{ 25, 2800 },
		{ 100, 22500 },
		{ 500, 225000 },
		{ 1000, 900000 },
		{ 2500, 4500000 }
	};

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_PurchaseIAPButtonClick, this, PurchaseIAPButtonClick);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_PurchaseIAPButtonClick, this);
	}

	private bool PurchaseIAPButtonClick(TUITelegram msg)
	{
		EPurchaseType ePurchaseType = (EPurchaseType)(int)msg._pExtraInfo;
		switch (ePurchaseType)
		{
		case EPurchaseType.PurchaseCrystal1:
		case EPurchaseType.PurchaseCrystal2:
		case EPurchaseType.PurchaseCrystal3:
		case EPurchaseType.PurchaseCrystal4:
		case EPurchaseType.PurchaseCrystal5:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Crystal);
			if (iIAPManager.GetInstance().IsCanPurchase())
			{
				UIGolbalStaticFun.PopIAPBlockMessageBox();
				iIAPManager.GetInstance().GooglePurchase(IAPKeys[(int)ePurchaseType]);
			}
			else
			{
				Debug.Log("Can not purchase!");
				UIMessage_CommonBoxData data2 = new UIMessage_CommonBoxData(1, Localization.instance.Get("fengmian_lianjietishi1"));
				UIGolbalStaticFun.PopCommonMessageBox(data2);
			}
			break;
		case EPurchaseType.PurchaseGold1:
		case EPurchaseType.PurchaseGold2:
		case EPurchaseType.PurchaseGold3:
		case EPurchaseType.PurchaseGold4:
		case EPurchaseType.PurchaseGold5:
		{
			if (COMA_Pref.Instance.GetCrystal() < IAPGold[(int)ePurchaseType, 0])
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Fail);
				UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("shangdianjiemian_desc8"));
				UIGolbalStaticFun.PopCommonMessageBox(data);
				break;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Gold);
			Debug.Log("#######################  -" + IAPGold[(int)ePurchaseType, 0] + " TCrystal!");
			COMA_Pref.Instance.AddCrystal(-IAPGold[(int)ePurchaseType, 0]);
			COMA_Pref.Instance.AddGold(IAPGold[(int)ePurchaseType, 1]);
			COMA_Pref.Instance.Save(true);
			string strCSpendNum = IAPGold[(int)ePurchaseType, 0].ToString();
			COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), strCSpendNum, "buygold");
			break;
		}
		case EPurchaseType.Free:
			Debug.Log("Free IAP!");
			MyTapjoy.Show();
			break;
		}
		return true;
	}

	public void OnVerifySuccess(bool bS, string sKey, string sIdentifier, string sReceipt, string sSignature)
	{
		COMA_IAPCheck.Instance.RemoveToLocal(sKey, sIdentifier, sReceipt, sSignature);
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
		if (bS)
		{
			COMA_HTTP_DataCollect.Instance.SendIAPInfo(sKey, sReceipt);
			Debug.Log("cmp srv return-----purchase success");
		}
		else
		{
			Debug.Log("cmp srv return----- illegal purchase ");
			UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("beibaojiemian_desc12"));
			UIGolbalStaticFun.PopCommonMessageBox(data);
		}
	}

	public void OnIAPVerifySubmitSuccess(string sKey, string sIdentifier, string sReceipt, string sSignature, string sRandom, int nRat, int nRatA, int nRatB)
	{
		Debug.Log("UIIAP.HM   verify submit success at the background");
		COMA_IAPCheck.Instance.RemoveToLocal(sKey, sIdentifier, sReceipt, sSignature);
		COMA_IAPCheck.Instance.AddToLocal(sKey, sIdentifier, sReceipt, sSignature, sRandom, nRat.ToString(), nRatA.ToString(), nRatB.ToString());
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ChangeTextIAPBlockBox, this, Localization.instance.Get("beibaojiemian_desc16"));
	}

	public void OnVerifyFailed(string sKey, string sIdentifier, string sReceipt, string sSignature)
	{
		Debug.Log("cmp srv return-----purchase failed!");
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("beibaojiemian_desc13"));
		UIGolbalStaticFun.PopCommonMessageBox(data);
	}

	public void OnAddMoney(string sIAPKey)
	{
		if (sIAPKey == IAPKeys[0])
		{
			Debug.Log("can add money...25");
			COMA_Pref.Instance.AddCrystal(25);
		}
		else if (sIAPKey == IAPKeys[1])
		{
			Debug.Log("can add money...150");
			COMA_Pref.Instance.AddCrystal(150);
		}
		else if (sIAPKey == IAPKeys[2])
		{
			Debug.Log("can add money...350");
			COMA_Pref.Instance.AddCrystal(350);
		}
		else if (sIAPKey == IAPKeys[3])
		{
			Debug.Log("can add money...850");
			COMA_Pref.Instance.AddCrystal(850);
		}
		else if (sIAPKey == IAPKeys[4])
		{
			Debug.Log("can add money...2500");
			COMA_Pref.Instance.AddCrystal(2500);
		}
		COMA_Pref.Instance.Save(true);
	}

	public void OnNetError(string sIAPKey)
	{
		Debug.Log("cmp srv return-----error");
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("fengmian_lianjietishi1"));
		UIGolbalStaticFun.PopCommonMessageBox(data);
	}

	public void OnIAPPurchaseSuccess(string sIAPKey, string sIdentifier, string sReceipt)
	{
		Debug.Log("apple return---purchase success!");
		COMA_IAPCheck.Instance.AddToLocal(sIAPKey, sIdentifier, sReceipt, string.Empty);
		if (iServerIAPVerify.GetInstance().IsCanVerify())
		{
			iServerIAPVerify.GetInstance().VerifyIAP(sIAPKey, sIdentifier, sReceipt, string.Empty, OnVerifySuccess, OnVerifyFailed, OnNetError, OnIAPVerifySubmitSuccess);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ChangeTextIAPBlockBox, this, Localization.instance.Get("beibaojiemian_desc15"));
			return;
		}
		Debug.Log("cmp srv cannot process");
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("fengmian_lianjietishi1"));
		UIGolbalStaticFun.PopCommonMessageBox(data);
	}

	public void OnEvent_Failed()
	{
		Debug.Log("app srv return---purchase failed!");
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
	}

	public void OnEvent_Cancel()
	{
		Debug.Log("app srv return---purchase cancel!");
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
	}

	public void OnEvent_NetError()
	{
		Debug.Log("app srv return---purchase net error!");
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("fengmian_lianjietishi1"));
		UIGolbalStaticFun.PopCommonMessageBox(data);
	}

	private void Awake()
	{
		iServerIAPVerify.GetInstance().SetPurchaseCallBack(OnAddMoney);
	}

	protected override void Tick()
	{
	}
}
