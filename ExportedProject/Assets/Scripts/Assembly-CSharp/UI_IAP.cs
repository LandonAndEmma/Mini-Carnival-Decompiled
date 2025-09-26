using UnityEngine;

public class UI_IAP : UIMessageHandler
{
	public static readonly string[] IAPKeys = new string[5] { "com.trinitigame.callofminiavatar.099centsv10", "com.trinitigame.callofminiavatar.499cents", "com.trinitigame.callofminiavatar.999cents", "com.trinitigame.callofminiavatar.1999cents", "com.trinitigame.callofminiavatar.4999cents" };

	private static UI_IAP _instance = null;

	private GameObject _waitingBox;

	public static UI_IAP Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
		iServerIAPVerify.GetInstance().SetPurchaseCallBack(OnAddMoney);
	}

	private new void OnEnable()
	{
		_instance = this;
	}

	private new void OnDisable()
	{
		_instance = null;
	}

	public bool IsWaitingIAP()
	{
		return _waitingBox != null;
	}

	private void Start()
	{
		RefreshGoldAndCrystal();
	}

	private new void RefreshGoldAndCrystal()
	{
		if (_goldLabel != null)
		{
			_goldLabel.Text = COMA_Pref.Instance.GetGold().ToString();
		}
		if (_gemLabel != null)
		{
			_gemLabel.Text = COMA_Pref.Instance.GetCrystal().ToString();
		}
	}

	private new void Update()
	{
	}

	public void OnVerifySuccess(bool bS, string sKey, string sIdentifier, string sReceipt, string sSignature)
	{
		COMA_IAPCheck.Instance.RemoveToLocal(sKey, sIdentifier, sReceipt, sSignature);
		DestoryWaitingBox();
		if (bS)
		{
			COMA_HTTP_DataCollect.Instance.SendIAPInfo(sKey, sReceipt);
			Debug.Log("cmp srv return-----purchase success");
		}
		else
		{
			Debug.Log("cmp srv return----- illegal purchase ");
			TUI_MsgBox.Instance.MessageBox(223);
		}
	}

	public void OnIAPVerifySubmitSuccess(string sKey, string sIdentifier, string sReceipt, string sSignature, string sRandom, int nRat, int nRatA, int nRatB)
	{
		Debug.Log("UI_IAP  verify submit success at the background");
		COMA_IAPCheck.Instance.RemoveToLocal(sKey, sIdentifier, sReceipt, sSignature);
		COMA_IAPCheck.Instance.AddToLocal(sKey, sIdentifier, sReceipt, sSignature, sRandom, nRat.ToString(), nRatA.ToString(), nRatB.ToString());
		string text = TUITextManager.Instance().GetString("beibaojiemian_desc16");
		text += "...";
		if (_waitingBox != null)
		{
			UICommonMsgBox component = _waitingBox.GetComponent<UICommonMsgBox>();
			if (component != null)
			{
				component.ChangeText(text);
			}
		}
	}

	public void OnVerifyFailed(string sKey, string sIdentifier, string sReceipt, string sSignature)
	{
		Debug.Log("cmp srv return-----purchase failed!");
		TUI_MsgBox.Instance.MessageBox(224);
		DestoryWaitingBox();
	}

	public void OnAddMoney(string sIAPKey)
	{
		if (sIAPKey == IAPKeys[0])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(25);
			TUI_MsgBox.Instance.TipBox(4, 25, string.Empty, null);
		}
		else if (sIAPKey == IAPKeys[1])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(150);
			TUI_MsgBox.Instance.TipBox(4, 150, string.Empty, null);
		}
		else if (sIAPKey == IAPKeys[2])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(350);
			TUI_MsgBox.Instance.TipBox(4, 350, string.Empty, null);
		}
		else if (sIAPKey == IAPKeys[3])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(850);
			TUI_MsgBox.Instance.TipBox(4, 850, string.Empty, null);
		}
		else if (sIAPKey == IAPKeys[4])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(2500);
			TUI_MsgBox.Instance.TipBox(4, 2500, string.Empty, null);
		}
		COMA_Pref.Instance.Save(true);
		RefreshGoldAndCrystal();
	}

	public void OnNetError(string sIAPKey)
	{
		Debug.Log("cmp srv return-----error");
		MsgTips("fengmian_lianjietishi1");
		DestoryWaitingBox();
	}

	public void OnIAPPurchaseSuccess(string sIAPKey, string sIdentifier, string sReceipt)
	{
		Debug.Log("apple return---purchase success!");
		COMA_IAPCheck.Instance.AddToLocal(sIAPKey, sIdentifier, sReceipt, string.Empty);
		if (iServerIAPVerify.GetInstance().IsCanVerify())
		{
			iServerIAPVerify.GetInstance().VerifyIAP(sIAPKey, sIdentifier, sReceipt, string.Empty, OnVerifySuccess, OnVerifyFailed, OnNetError, OnIAPVerifySubmitSuccess);
			string text = TUITextManager.Instance().GetString("beibaojiemian_desc15");
			text += "...";
			if (_waitingBox != null)
			{
				UICommonMsgBox component = _waitingBox.GetComponent<UICommonMsgBox>();
				if (component != null)
				{
					component.ChangeText(text);
				}
			}
		}
		else
		{
			Debug.Log("cmp srv cannot process");
			MsgTips("fengmian_lianjietishi1");
			DestoryWaitingBox();
		}
	}

	public void OnEvent_Failed()
	{
		Debug.Log("app srv return---purchase failed!");
		DestoryWaitingBox();
	}

	public void OnEvent_Cancel()
	{
		Debug.Log("app srv return---purchase cancel!");
		DestoryWaitingBox();
	}

	public void OnEvent_NetError()
	{
		Debug.Log("app srv return---purchase net error!");
		MsgTips("fengmian_lianjietishi1");
		DestoryWaitingBox();
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			Debug.Log("Button_back-CommandClick");
			COMA_Pref.Instance.Save(true);
			if (UICOM._preSceneName == string.Empty)
			{
				_aniControl.PlayExitAni("UI.MainMenu");
			}
			else
			{
				_aniControl.PlayExitAni(UICOM._preSceneName);
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_IapEntry(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_IapEntry-CommandClick");
		}
	}

	public void HandleEventButton_tapjoy(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_tapjoy-CommandClick");
			MyTapjoy.Show();
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Crystal);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton1(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Crystal);
			Debug.Log("HandleEventButton1-CommandClick");
			if (iIAPManager.GetInstance().IsCanPurchase())
			{
				WaitingBox();
				iIAPManager.GetInstance().Purchase(IAPKeys[0], OnIAPPurchaseSuccess, OnEvent_Failed, OnEvent_Cancel, OnEvent_NetError);
			}
			else
			{
				Debug.Log("Can not purchase!");
				MsgTips("fengmian_lianjietishi1");
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton2(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Crystal);
			if (iIAPManager.GetInstance().IsCanPurchase())
			{
				Debug.Log("Can purchase!send message...");
				WaitingBox();
				iIAPManager.GetInstance().Purchase(IAPKeys[1], OnIAPPurchaseSuccess, OnEvent_Failed, OnEvent_Cancel, OnEvent_NetError);
			}
			else
			{
				Debug.Log("Can not purchase!");
				MsgTips("fengmian_lianjietishi1");
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton3(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Crystal);
			Debug.Log("HandleEventButton3-CommandClick");
			if (iIAPManager.GetInstance().IsCanPurchase())
			{
				Debug.Log("Can purchase!send message...");
				WaitingBox();
				iIAPManager.GetInstance().Purchase(IAPKeys[2], OnIAPPurchaseSuccess, OnEvent_Failed, OnEvent_Cancel, OnEvent_NetError);
			}
			else
			{
				Debug.Log("Can not purchase!");
				MsgTips("fengmian_lianjietishi1");
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton4(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Crystal);
			Debug.Log("HandleEventButton4-CommandClick");
			if (iIAPManager.GetInstance().IsCanPurchase())
			{
				Debug.Log("Can purchase!send message...");
				WaitingBox();
				iIAPManager.GetInstance().Purchase(IAPKeys[3], OnIAPPurchaseSuccess, OnEvent_Failed, OnEvent_Cancel, OnEvent_NetError);
			}
			else
			{
				Debug.Log("Can not purchase!");
				MsgTips("fengmian_lianjietishi1");
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton5(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Crystal);
			Debug.Log("HandleEventButton5-CommandClick");
			if (iIAPManager.GetInstance().IsCanPurchase())
			{
				Debug.Log("Can purchase!send message...");
				WaitingBox();
				iIAPManager.GetInstance().Purchase(IAPKeys[4], OnIAPPurchaseSuccess, OnEvent_Failed, OnEvent_Cancel, OnEvent_NetError);
			}
			else
			{
				Debug.Log("Can not purchase!");
				MsgTips("fengmian_lianjietishi1");
			}
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton6(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton6-CommandClick");
			if (COMA_Pref.Instance.GetCrystal() < 150)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Fail);
				MsgTips();
				break;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Gold);
			COMA_Pref.Instance.AddCrystal(-150);
			COMA_Pref.Instance.AddGold(4000);
			COMA_Pref.Instance.Save(true);
			RefreshGoldAndCrystal();
			TUI_MsgBox.Instance.TipBox(3, 4000, string.Empty, null);
			COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), "150", "buygold");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton7(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton7-CommandClick");
			if (COMA_Pref.Instance.GetCrystal() < 350)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Fail);
				MsgTips();
				break;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Gold);
			COMA_Pref.Instance.AddCrystal(-350);
			COMA_Pref.Instance.AddGold(11000);
			COMA_Pref.Instance.Save(true);
			RefreshGoldAndCrystal();
			TUI_MsgBox.Instance.TipBox(3, 11000, string.Empty, null);
			COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), "350", "buygold");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton8(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton8-CommandClick");
			if (COMA_Pref.Instance.GetCrystal() < 850)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Fail);
				MsgTips();
				break;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Gold);
			COMA_Pref.Instance.AddCrystal(-850);
			COMA_Pref.Instance.AddGold(31000);
			COMA_Pref.Instance.Save(true);
			RefreshGoldAndCrystal();
			TUI_MsgBox.Instance.TipBox(3, 31000, string.Empty, null);
			COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), "850", "buygold");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton9(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton9-CommandClick");
			if (COMA_Pref.Instance.GetCrystal() < 2500)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Fail);
				MsgTips();
				break;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Gold);
			COMA_Pref.Instance.AddCrystal(-2500);
			COMA_Pref.Instance.AddGold(125000);
			COMA_Pref.Instance.Save(true);
			RefreshGoldAndCrystal();
			TUI_MsgBox.Instance.TipBox(3, 125000, string.Empty, null);
			COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), "2500", "buygold");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton10(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton10-CommandClick");
			if (COMA_Pref.Instance.GetCrystal() < 6200)
			{
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Fail);
				MsgTips();
				break;
			}
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Buy_Gold);
			COMA_Pref.Instance.AddCrystal(-6200);
			COMA_Pref.Instance.AddGold(400000);
			COMA_Pref.Instance.Save(true);
			RefreshGoldAndCrystal();
			TUI_MsgBox.Instance.TipBox(3, 400000, string.Empty, null);
			COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), "6200", "buygold");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void WaitingBox()
	{
		GameObject boxPrefab = Resources.Load("UI/Misc/IAPBoxNew") as GameObject;
		string text = TUITextManager.Instance().GetString("beibaojiemian_desc14");
		text += "...";
		UI_MsgBox uI_MsgBox = MessageBox(text, string.Empty, 0, boxPrefab, -300f);
		_waitingBox = uI_MsgBox.gameObject;
	}

	private void DestoryWaitingBox()
	{
		if (_waitingBox != null)
		{
			Debug.Log("--DestroyImmediate");
			Object.Destroy(_waitingBox);
			_waitingBox = null;
		}
	}

	private void MsgTips()
	{
		MsgTips(string.Empty);
	}

	private void MsgTips(string strMsgID)
	{
		string empty = string.Empty;
		empty = ((strMsgID.Length != 0) ? strMsgID : "shangdianjiemian_desc8");
		string text = TUITextManager.Instance().GetString(empty);
		Debug.Log("---MsgTips----" + text);
		UI_MsgBox uI_MsgBox = MessageBox(text, string.Empty, 101, null, -390f);
		if (_msgBoxNode != null)
		{
			uI_MsgBox.transform.parent = _msgBoxNode.transform;
			uI_MsgBox.transform.localPosition = new Vector3(0f, 0f, -390f);
		}
	}

	public void TapjoyBack(int diamondUp)
	{
		COMA_Pref.Instance.AddCrystal(diamondUp);
		TUI_MsgBox.Instance.TipBox(4, diamondUp, string.Empty, null);
		COMA_Pref.Instance.Save(true);
		RefreshGoldAndCrystal();
	}
}
