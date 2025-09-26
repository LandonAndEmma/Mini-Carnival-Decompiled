using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMarket : UIMessageHandler
{
	[SerializeField]
	private UIMenu_AnimationControlMgr uiMenu_AniMgr;

	private static UIMarket _instance;

	public int framesToLate;

	public static int forSellingExit;

	[SerializeField]
	private GameObject _shopFrameBK;

	[SerializeField]
	private GameObject _mainMenu1;

	[SerializeField]
	private GameObject _mainMenu41;

	[SerializeField]
	private GameObject _marketContainer;

	[SerializeField]
	private GameObject _btnBuy;

	[SerializeField]
	private TUIScrollList _scrollList;

	[SerializeField]
	private GameObject _priceArea;

	[SerializeField]
	private TUILabel _priceLabel;

	[SerializeField]
	private GameObject _refreshMgr;

	[SerializeField]
	private GameObject _shopCaption;

	[SerializeField]
	private TUILabel _shopCaptionLabel;

	[SerializeField]
	private GameObject _sellinginfo3;

	[SerializeField]
	private GameObject _sellinginfo1;

	[SerializeField]
	private GameObject _resetRefresh;

	[SerializeField]
	private TUILabel _resetRefreshPriceLabel;

	[SerializeField]
	private TUILabel _resetRefreshTime;

	private string _strContainerExitAni = "UIMarket_RightExit";

	private string _strMenuExitAni = "UIArmory_RightExit";

	public GameObject btnAdd;

	public GameObject btnSub;

	public COMA_InitLocalAvatar avatarInitCom;

	private int _curMenuLayer = 1;

	public COMA_PlayerSelfCharacter playerOnShowCom;

	private List<UI_BoxSlot> _seletedSlots = new List<UI_BoxSlot>();

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask2;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr2;

	[SerializeField]
	private UI_NewGuideBoard _newGuideBoard;

	[SerializeField]
	private TUIMeshSprite _resetRefreshBtnIcon;

	[SerializeField]
	private TUIButtonClick _resetRefreshBtn;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr1;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask1;

	private COMA_Scene_Trade.TradeTexInfo info;

	private UIMarket_AvatarShopData curSelData;

	private GameObject _waitingBox;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask3;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr3;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask4;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr4;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask13;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr13;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask14;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr14;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask15;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr15;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask16;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr16;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask17;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr17;

	public static UIMarket Instance
	{
		get
		{
			return _instance;
		}
	}

	private int ResetRefreshPrice
	{
		set
		{
			_resetRefreshPriceLabel.Text = COMA_Sys.Instance.marketRefreshCrystal.ToString();
		}
	}

	public string ResetRefreshCurTime
	{
		set
		{
			_resetRefreshTime.Text = value;
		}
	}

	private string _shopCaptionText
	{
		set
		{
			_shopCaptionLabel.TextID = value;
		}
	}

	private Color _shopCaptionColor
	{
		set
		{
			TUIMeshSprite component = _shopCaption.GetComponent<TUIMeshSprite>();
			component.color = value;
		}
	}

	public int SellingPrice
	{
		get
		{
			return int.Parse(_priceLabel.Text);
		}
		set
		{
			int num = 1000;
			int num2 = 60000;
			if (_curMenuLayer == 411)
			{
				num = 1000;
				num2 = 20000;
			}
			else if (_curMenuLayer == 412)
			{
				num = 3000;
				num2 = 60000;
			}
			value = Mathf.Clamp(value, num, num2);
			_priceLabel.Text = value.ToString();
			if (value >= num2)
			{
				btnAdd.SetActive(false);
				btnSub.SetActive(true);
			}
			else if (value <= num)
			{
				btnAdd.SetActive(true);
				btnSub.SetActive(false);
			}
			else
			{
				btnAdd.SetActive(true);
				btnSub.SetActive(true);
			}
		}
	}

	public int CurMenuLayer
	{
		get
		{
			return _curMenuLayer;
		}
		set
		{
			bool flag = ((_curMenuLayer != 1) ? true : false);
			_curMenuLayer = value;
			switch (CurMenuLayer)
			{
			case 1:
				if (flag)
				{
					avatarInitCom.UpdateAvatar();
				}
				_marketContainer.SetActive(false);
				_priceArea.SetActive(false);
				_shopFrameBK.SetActive(false);
				_mainMenu1.SetActive(true);
				_mainMenu41.SetActive(false);
				_btnBuy.SetActive(false);
				_shopCaption.SetActive(false);
				_sellinginfo3.SetActive(false);
				_sellinginfo1.SetActive(false);
				_resetRefresh.SetActive(false);
				break;
			case 11:
				_refreshMgr.SetActive(true);
				_shopFrameBK.SetActive(true);
				_priceArea.SetActive(false);
				_marketContainer.SetActive(true);
				_mainMenu1.SetActive(false);
				_mainMenu41.SetActive(false);
				_btnBuy.SetActive(true);
				_btnBuy.GetComponent<UI_SwitchBtnLabel>().SetLabelTxt("Buy", string.Empty);
				_scrollList.NeedReleaseToRefresh = true;
				_shopCaption.SetActive(true);
				_shopCaptionText = "pifu_zhujiemian";
				_shopCaptionColor = new Color(0.16f, 0.56f, 0.82f);
				_sellinginfo3.SetActive(false);
				_sellinginfo1.SetActive(false);
				_resetRefresh.SetActive(true);
				ResetRefreshPrice = 1;
				if (COMA_Pref.Instance.bNew_BuyAvatar && _wizardMask13 != null)
				{
					_wizardMask13.gameObject.SetActive(true);
				}
				break;
			case 21:
				_refreshMgr.SetActive(false);
				_shopFrameBK.SetActive(true);
				_priceArea.SetActive(false);
				_marketContainer.SetActive(true);
				_mainMenu1.SetActive(false);
				_mainMenu41.SetActive(false);
				_btnBuy.SetActive(true);
				_btnBuy.GetComponent<UI_SwitchBtnLabel>().SetLabelTxt("Buy", string.Empty);
				_scrollList.NeedReleaseToRefresh = false;
				_shopCaption.SetActive(true);
				_shopCaptionText = "moju_zhujiemian";
				_shopCaptionColor = new Color(0.63f, 0.85f, 0f);
				_sellinginfo3.SetActive(false);
				_sellinginfo1.SetActive(false);
				_resetRefresh.SetActive(false);
				if (COMA_Pref.Instance.bNew_BuyModel && _wizardMask13 != null)
				{
					_wizardMask13.gameObject.SetActive(true);
				}
				break;
			case 31:
				_refreshMgr.SetActive(false);
				_shopFrameBK.SetActive(true);
				_priceArea.SetActive(false);
				_marketContainer.SetActive(true);
				_mainMenu1.SetActive(false);
				_mainMenu41.SetActive(false);
				_btnBuy.SetActive(true);
				_btnBuy.GetComponent<UI_SwitchBtnLabel>().SetLabelTxt("Buy", string.Empty);
				_scrollList.NeedReleaseToRefresh = false;
				_shopCaption.SetActive(true);
				_shopCaptionText = "shiping_zhujiemian";
				_shopCaptionColor = new Color(0.76f, 0.08f, 0.65f);
				_sellinginfo3.SetActive(false);
				_sellinginfo1.SetActive(false);
				_resetRefresh.SetActive(false);
				break;
			case 41:
				_refreshMgr.SetActive(false);
				_shopFrameBK.SetActive(false);
				_marketContainer.SetActive(false);
				_mainMenu1.SetActive(false);
				_mainMenu41.SetActive(true);
				_btnBuy.SetActive(false);
				_priceArea.SetActive(false);
				_scrollList.NeedReleaseToRefresh = false;
				_shopCaption.SetActive(false);
				_sellinginfo3.SetActive(false);
				_sellinginfo1.SetActive(false);
				_resetRefresh.SetActive(false);
				break;
			case 411:
			{
				_refreshMgr.SetActive(false);
				_shopFrameBK.SetActive(true);
				_marketContainer.SetActive(true);
				_mainMenu1.SetActive(false);
				_mainMenu41.SetActive(false);
				_btnBuy.SetActive(true);
				_priceArea.SetActive(true);
				_scrollList.NeedReleaseToRefresh = false;
				_btnBuy.GetComponent<UI_SwitchBtnLabel>().SetLabelTxt("sell", string.Empty);
				SellingPrice = 1000;
				_shopCaption.SetActive(true);
				_shopCaptionText = "maidange_zhujiemian";
				_shopCaptionColor = new Color(0.93f, 0.81f, 0f);
				_sellinginfo3.SetActive(false);
				string text3 = COMA_Sys.Instance.tax.ToString();
				string text4 = TUITool.StringFormat(TUITextManager.Instance().GetString("jiaoyijiemian_desc5"), text3);
				_sellinginfo1.GetComponent<TUILabel>().Text = text4;
				_sellinginfo1.SetActive(true);
				_resetRefresh.SetActive(false);
				if (COMA_Pref.Instance.bNew_SellSingle && _wizardMask13 != null)
				{
					_wizardMask13.gameObject.SetActive(true);
				}
				break;
			}
			case 412:
			{
				_refreshMgr.SetActive(false);
				_shopFrameBK.SetActive(true);
				_marketContainer.SetActive(true);
				_mainMenu1.SetActive(false);
				_mainMenu41.SetActive(false);
				_btnBuy.SetActive(true);
				_priceArea.SetActive(true);
				_scrollList.NeedReleaseToRefresh = false;
				_btnBuy.GetComponent<UI_SwitchBtnLabel>().SetLabelTxt("sell", string.Empty);
				SellingPrice = 3000;
				_shopCaption.SetActive(true);
				_shopCaptionText = "maizhengtao_zhujiemian";
				_shopCaptionColor = new Color(0.93f, 0.81f, 0f);
				string text = COMA_Sys.Instance.tax.ToString();
				string text2 = TUITool.StringFormat(TUITextManager.Instance().GetString("jiaoyijiemian_desc5"), text);
				_sellinginfo3.transform.FindChild("tax").gameObject.GetComponent<TUILabel>().Text = text2;
				_sellinginfo3.SetActive(true);
				_sellinginfo1.SetActive(false);
				_resetRefresh.SetActive(false);
				if (COMA_Pref.Instance.bNew_SellSingle && _wizardMask13 != null)
				{
					_wizardMask13.gameObject.SetActive(true);
				}
				break;
			}
			case 413:
				_refreshMgr.SetActive(false);
				_shopFrameBK.SetActive(true);
				_marketContainer.SetActive(true);
				_mainMenu1.SetActive(false);
				_mainMenu41.SetActive(false);
				_btnBuy.SetActive(false);
				_priceArea.SetActive(false);
				_scrollList.NeedReleaseToRefresh = false;
				_shopCaption.SetActive(true);
				_shopCaptionText = "maichuxinxi_zhujiemian";
				_shopCaptionColor = new Color(0.93f, 0.81f, 0f);
				_sellinginfo3.SetActive(false);
				_sellinginfo1.SetActive(false);
				_resetRefresh.SetActive(false);
				break;
			}
			MenuLayerChanged();
		}
	}

	public List<UI_BoxSlot> SeletedSlots
	{
		get
		{
			return _seletedSlots;
		}
	}

	private new void OnEnable()
	{
		_instance = this;
	}

	private new void OnDisable()
	{
		_instance = null;
	}

	public void CloseRefreshMgr()
	{
		_refreshMgr.SetActive(false);
	}

	public void ClearSelectedSlots()
	{
		_seletedSlots.Clear();
	}

	public void RemoveSlot(UI_BoxSlot slot)
	{
		if (_seletedSlots.Contains(slot))
		{
			_seletedSlots.Remove(slot);
		}
	}

	public void AddSlot(UI_BoxSlot slot)
	{
		((UIMarket_AvatarShopSlot)slot).AvatarSelected = true;
		_seletedSlots.Add(slot);
	}

	public void ResetAvatarAlpha()
	{
		playerOnShowCom.bodyObjs[0].renderer.material.color = new Color(1f, 1f, 1f, 1f);
		playerOnShowCom.bodyObjs[1].renderer.material.color = new Color(1f, 1f, 1f, 1f);
		playerOnShowCom.bodyObjs[2].renderer.material.color = new Color(1f, 1f, 1f, 1f);
	}

	public void AddShopSlot(UIMarket_AvatarShopSlot slot)
	{
		switch (CurMenuLayer)
		{
		case 11:
		{
			AddAvatarShopSlot(slot);
			int iD = slot.GetID();
			COMA_Scene_Trade.TradeTexInfo tradeTexInfo = null;
			if (iD == 0)
			{
				tradeTexInfo = COMA_Scene_Trade.Instance.officialTex;
			}
			else if (iD < 1 + COMA_Scene_Trade.suitCount)
			{
				tradeTexInfo = COMA_Scene_Trade.Instance.suitTex[iD - 1];
			}
			else if (iD < 1 + COMA_Scene_Trade.suitCount + COMA_Scene_Trade.partCount)
			{
				tradeTexInfo = COMA_Scene_Trade.Instance.partTex[iD - 1 - COMA_Scene_Trade.suitCount];
			}
			if (tradeTexInfo == null)
			{
				break;
			}
			ResetAvatarAlpha();
			if (!tradeTexInfo.isSuit)
			{
				if (tradeTexInfo.serialName[0].StartsWith("Head"))
				{
					playerOnShowCom.bodyObjs[0].renderer.material.mainTexture = tradeTexInfo.tex[0];
					playerOnShowCom.bodyObjs[1].renderer.material.color = new Color(1f, 1f, 1f, 0.78f);
					playerOnShowCom.bodyObjs[2].renderer.material.color = new Color(1f, 1f, 1f, 0.78f);
				}
				else if (tradeTexInfo.serialName[0].StartsWith("Body"))
				{
					playerOnShowCom.bodyObjs[1].renderer.material.mainTexture = tradeTexInfo.tex[0];
					playerOnShowCom.bodyObjs[2].renderer.material.color = new Color(1f, 1f, 1f, 0.78f);
					playerOnShowCom.bodyObjs[0].renderer.material.color = new Color(1f, 1f, 1f, 0.78f);
				}
				else if (tradeTexInfo.serialName[0].StartsWith("Leg"))
				{
					playerOnShowCom.bodyObjs[2].renderer.material.mainTexture = tradeTexInfo.tex[0];
					playerOnShowCom.bodyObjs[0].renderer.material.color = new Color(1f, 1f, 1f, 0.78f);
					playerOnShowCom.bodyObjs[1].renderer.material.color = new Color(1f, 1f, 1f, 0.78f);
				}
			}
			else
			{
				playerOnShowCom.bodyObjs[0].renderer.material.mainTexture = tradeTexInfo.tex[0];
				playerOnShowCom.bodyObjs[1].renderer.material.mainTexture = tradeTexInfo.tex[1];
				playerOnShowCom.bodyObjs[2].renderer.material.mainTexture = tradeTexInfo.tex[2];
			}
			break;
		}
		case 21:
			AddAvatarShopSlot(slot);
			break;
		case 31:
			AddAvatarShopSlot(slot);
			playerOnShowCom.CreateAccouterment(COMA_Scene_Shop.Instance.shopData_Accessories[slot.GetID()].PartType);
			break;
		case 411:
			AddAvatarShopSlot(slot);
			break;
		case 412:
			AddAvatarShopBatch(slot);
			break;
		}
	}

	protected void AddAvatarShopSlot(UIMarket_AvatarShopSlot slot)
	{
		foreach (UIMarket_AvatarShopSlot seletedSlot in _seletedSlots)
		{
			seletedSlot.AvatarSelected = false;
		}
		ClearSelectedSlots();
		AddSlot(slot);
	}

	protected void AddAvatarShopBatch(UIMarket_AvatarShopSlot slot)
	{
		UIMarket_AvatarShopData uIMarket_AvatarShopData = slot.BoxData as UIMarket_AvatarShopData;
		foreach (UIMarket_AvatarShopSlot seletedSlot in _seletedSlots)
		{
			UIMarket_AvatarShopData uIMarket_AvatarShopData2 = seletedSlot.BoxData as UIMarket_AvatarShopData;
			if (uIMarket_AvatarShopData2.PartType == uIMarket_AvatarShopData.PartType)
			{
				seletedSlot.AvatarSelected = false;
				RemoveSlot(seletedSlot);
				break;
			}
		}
		AddSlot(slot);
	}

	public void HandleEventButtonNewGuide(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonNewGuide-CommandClick");
			if (_newGuideBoard != null)
			{
				_newGuideBoard.ProcessEvent();
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

	private void Awake()
	{
		_wizardMask2.ProceSingleBtn += ProcessSingleUIEvent;
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 2)
		{
			_wizardMask1.gameObject.SetActive(true);
		}
		else if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 13)
		{
			_wizardMask13.gameObject.SetActive(true);
		}
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
		CurMenuLayer = 1;
		_scrollList.ProceStartReleaseHandler += HandleRefreshShop;
	}

	private void Start()
	{
		RefreshGoldAndCrystal();
		if (_newGuideBoard != null)
		{
			_newGuideBoard.EndLabel();
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

	private new void Update()
	{
		if (CurMenuLayer == 11)
		{
			float value = Time.time - COMA_Sys.Instance.marketRefreshTime;
			value = Mathf.Clamp(value, 0f, COMA_Sys.Instance.marketRefreshInterval);
			float value2 = (float)COMA_Sys.Instance.marketRefreshInterval - value;
			value2 = Mathf.Clamp(value2, 0f, COMA_Sys.Instance.marketRefreshInterval);
			int value3 = Mathf.CeilToInt(value2);
			value3 = Mathf.Clamp(value3, 0, COMA_Sys.Instance.marketRefreshInterval);
			string formatTime = GetFormatTime(value3);
			ResetRefreshCurTime = formatTime;
			if (value3 == 0)
			{
				_resetRefreshBtn.m_bDisable = true;
				_resetRefreshBtnIcon.GrayStyle = true;
				BtnCloseLight(_resetRefreshBtn);
				_resetRefreshBtn.m_bPressed = false;
			}
			else
			{
				_resetRefreshBtn.m_bDisable = false;
				_resetRefreshBtnIcon.GrayStyle = false;
			}
		}
	}

	private void MenuLayerChanged()
	{
		switch (CurMenuLayer)
		{
		case 11:
			_marketContainer.GetComponent<UIMarket_Container>().SimulateCall_AvatarInit();
			break;
		case 21:
			_marketContainer.GetComponent<UIMarket_Container>().SimulateCall_MoldsInit();
			break;
		case 31:
			_marketContainer.GetComponent<UIMarket_Container>().SimulateCall_AccessoriesInit();
			break;
		case 411:
			_marketContainer.GetComponent<UIMarket_Container>().SimulateCall_SellingSingle();
			break;
		case 412:
			_marketContainer.GetComponent<UIMarket_Container>().SimulateCall_SellingBatch();
			break;
		case 413:
			_marketContainer.GetComponent<UIMarket_Container>().SimulateCall_SellingInfo();
			break;
		}
	}

	public void HandleEventButton_PlayerRotate(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		float y = (0f - wparam) * COMA_Sys.Instance.sensitivity * 314f * 2f / (float)Screen.width;
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		playerOnShowCom.transform.rotation *= quaternion;
	}

	public void ExitMarketContainer()
	{
		_marketContainer.GetComponent<UIMarket_Container>().ExitContainer();
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			Debug.Log("Button_back-CommandClick");
			ResetAvatarAlpha();
			ClearSelectedSlots();
			switch (CurMenuLayer)
			{
			case 1:
				if (_fadeMgr != null)
				{
					_fadeMgr.FadeOut();
				}
				_aniControl.PlayExitAni("UI.MainMenu");
				if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 5)
				{
					COMA_Sys.Instance.nTeachingId++;
					_wizardMgr4.ResetUIControllersZ();
					_wizardMgr4.gameObject.SetActive(false);
					_wizardMask4.gameObject.SetActive(false);
				}
				break;
			case 11:
				uiMenu_AniMgr._nMenyLayer = 1;
				_marketContainer.animation.Play(_strContainerExitAni);
				break;
			case 21:
				if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 4)
				{
					COMA_Sys.Instance.nTeachingId++;
				}
				uiMenu_AniMgr._nMenyLayer = 1;
				_marketContainer.animation.Play(_strContainerExitAni);
				break;
			case 31:
				uiMenu_AniMgr._nMenyLayer = 1;
				_marketContainer.animation.Play(_strContainerExitAni);
				break;
			case 41:
				uiMenu_AniMgr._nMenyLayer = 1;
				_mainMenu41.animation.Play(_strMenuExitAni);
				break;
			case 411:
				if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 17)
				{
					_wizardMgr17.ResetUIControllersZ();
					_wizardMgr17.gameObject.SetActive(false);
					COMA_Sys.Instance.nTeachingId++;
					COMA_Sys.Instance.bFirstGame = false;
					COMA_Sys.Instance.bMemFirstGame = false;
					COMA_Pref.Instance.Save(true);
					_aniControl.PlayExitAni("UI.MainMenu");
				}
				else
				{
					uiMenu_AniMgr._nMenyLayer = 41;
					_marketContainer.animation.Play(_strContainerExitAni);
				}
				break;
			case 412:
				uiMenu_AniMgr._nMenyLayer = 41;
				_marketContainer.animation.Play(_strContainerExitAni);
				break;
			case 413:
				_marketContainer.GetComponent<UIMarket_Container>().UnActiveInfoLabel();
				uiMenu_AniMgr._nMenyLayer = 41;
				_marketContainer.animation.Play(_strContainerExitAni);
				break;
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
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_IapEntry-CommandClick");
			EnterIAPUI("UI.Market", _aniControl);
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

	public void HandleEventButton_ResetRefresh(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_ResetRefresh-CommandClick");
			if (COMA_Pref.Instance.GetCrystal() < COMA_Sys.Instance.marketRefreshCrystal)
			{
				UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(109);
				uI_MsgBox.AddProceYesHandler(NeedMoreCoin);
				break;
			}
			COMA_Sys.Instance.marketRefreshTime = -5000000f;
			COMA_Pref.Instance.AddCrystal(-COMA_Sys.Instance.marketRefreshCrystal);
			RefreshGoldAndCrystal();
			COMA_Pref.Instance.Save(true);
			COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), COMA_Sys.Instance.marketRefreshCrystal.ToString(), "refresh shop");
			COMA_HTTP_DataCollect.Instance.SendRefreshShop(COMA_Sys.Instance.marketRefreshCrystal.ToString());
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

	public void HandleEventButton_Avatar(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_Avatar-CommandClick");
			if (COMA_Sys.Instance.bLockMarket)
			{
				TUI_MsgBox.Instance.MessageBox(121);
				break;
			}
			uiMenu_AniMgr._nMenyLayer = 11;
			control.transform.parent.gameObject.animation.Play(_strMenuExitAni);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_Molds(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_Molds-CommandClick");
			if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 2)
			{
				_wizardMgr1.ResetUIControllersZ();
				_wizardMgr1.gameObject.SetActive(false);
				COMA_Sys.Instance.nTeachingId++;
				_wizardMask2.gameObject.SetActive(true);
			}
			uiMenu_AniMgr._nMenyLayer = 21;
			control.transform.parent.gameObject.animation.Play(_strMenuExitAni);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_Accessories(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_Accessories-CommandClick");
			uiMenu_AniMgr._nMenyLayer = 31;
			control.transform.parent.gameObject.animation.Play(_strMenuExitAni);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_Sell(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("HandleEventButton_Sell-CommandClick");
			bool flag = true;
			if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 13)
			{
				_wizardMgr13.ResetUIControllersZ();
				_wizardMgr13.gameObject.SetActive(false);
				COMA_Sys.Instance.nTeachingId++;
				_wizardMask14.gameObject.SetActive(true);
				flag = false;
			}
			if (flag && COMA_Sys.Instance.bLockMarket)
			{
				TUI_MsgBox.Instance.MessageBox(121);
				break;
			}
			uiMenu_AniMgr._nMenyLayer = 41;
			control.transform.parent.gameObject.animation.Play(_strMenuExitAni);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_SellSingle(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_SellSingle-CommandClick");
			if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 14)
			{
				_wizardMgr14.ResetUIControllersZ();
				_wizardMgr14.gameObject.SetActive(false);
				COMA_Sys.Instance.nTeachingId++;
				_wizardMask15.ProceSingleBtn += ProcessEnterTeaching16;
				_wizardMask15.gameObject.SetActive(true);
			}
			uiMenu_AniMgr._nMenyLayer = 411;
			control.transform.parent.gameObject.animation.Play(_strMenuExitAni);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_SellBatch(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_SellBatch-CommandClick");
			uiMenu_AniMgr._nMenyLayer = 412;
			control.transform.parent.gameObject.animation.Play(_strMenuExitAni);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_SellingInfo(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_SellingInfo-CommandClick");
			uiMenu_AniMgr._nMenyLayer = 413;
			control.transform.parent.gameObject.animation.Play(_strMenuExitAni);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_AddMoney(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_AddMoney-CommandClick");
			SellingPrice += 500;
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_SubtractMoney(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_AddMoney-CommandClick");
			SellingPrice -= 500;
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void NeedMoreCoin(string param)
	{
		EnterIAPUI("UI.Market", null);
	}

	private void UnLockedSlot(string param)
	{
		int num = 1;
		if (param == "Avatar" && info.isSuit)
		{
			num = 3 - COMA_Pref.Instance.PackageNullCount();
		}
		if (param == "Model" && curSelData.PartType == "HBL01")
		{
			num = 3 - COMA_Pref.Instance.PackageNullCount();
		}
		int num2 = COMA_Package.unlockPrice * num;
		if (COMA_Pref.Instance.GetCrystal() < num2)
		{
			UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(109);
			uI_MsgBox.AddProceYesHandler(NeedMoreCoin);
			return;
		}
		COMA_Pref.Instance.AddCrystal(-num2);
		RefreshGoldAndCrystal();
		COMA_Package.slotUnlocked += num;
		switch (param)
		{
		case "Avatar":
			BuyAvatar();
			break;
		case "Model":
			BuyModel();
			break;
		case "Adornment":
			BuyAdornment();
			break;
		}
		COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), num2.ToString(), "unlock slot");
		COMA_HTTP_DataCollect.Instance.SendUnlockPackage(num2.ToString(), num.ToString());
	}

	private void BuyAvatar()
	{
		COMA_Pref.Instance.AddGold(-info.price);
		RefreshGoldAndCrystal();
		string text = "BuyAvatars:";
		text += info.tid;
		Debug.Log("------------------------------------>" + text);
		COMA_HTTP_DataCollect.Instance.SendGoldInfo(COMA_Pref.Instance.GetGold().ToString(), Mathf.Abs(curSelData.AvatarPrice).ToString(), text);
		int num = ((!info.isSuit) ? 1 : 3);
		for (int i = 0; i < num; i++)
		{
			COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
			cOMA_PackageItem.serialName = info.serialName[i];
			cOMA_PackageItem.itemName = cOMA_PackageItem.itemName;
			cOMA_PackageItem.num = 0;
			cOMA_PackageItem.part = 0;
			cOMA_PackageItem.textureName = COMA_FileNameManager.Instance.GetFileName(cOMA_PackageItem.serialName);
			cOMA_PackageItem.texture = info.tex[i];
			cOMA_PackageItem.SavePNG();
			cOMA_PackageItem.iconTexture = info.data.AvatarIcon;
			cOMA_PackageItem.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem);
		}
		if (info.isSuit)
		{
			COMA_Server_Texture.Instance.TextureSuit_Buy(info.tid);
		}
		else
		{
			COMA_Server_Texture.Instance.TextureSell_Buy(info.tid);
		}
		COMA_Pref.Instance.Save(true);
		TUI_MsgBox.Instance.TipBox(2, 1, string.Empty, curSelData.AvatarIcon);
		string strType = ((!info.isSuit) ? "single" : "suit");
		COMA_HTTP_DataCollect.Instance.SendBuyAvatar(string.Empty, strType, info.tid, info.price.ToString());
	}

	private void BuyModel()
	{
		COMA_Pref.Instance.AddGold(-curSelData.AvatarPrice);
		RefreshGoldAndCrystal();
		if (curSelData.PartType == "HBL01")
		{
			UIMarket_Container component = _marketContainer.GetComponent<UIMarket_Container>();
			UIMarket_AvatarShopData[] array = new UIMarket_AvatarShopData[3];
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				array[i] = component.LstBoxDatas[i] as UIMarket_AvatarShopData;
				COMA_Pref.Instance.BoughtChanged(i, array[i]);
				num += array[i].AvatarPrice;
			}
			curSelData.AvatarPrice = (int)((float)(num * COMA_Sys.Instance.modelSuitSaleRate) / 100f);
			for (int j = 0; j < 3; j++)
			{
				string strPurpose = "BuyTemplates:" + array[j].PartType;
				COMA_HTTP_DataCollect.Instance.SendGoldInfo(COMA_Pref.Instance.GetGold().ToString(), Mathf.Abs(array[j].AvatarPrice).ToString(), strPurpose);
			}
			for (int k = 0; k < 3; k++)
			{
				COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
				cOMA_PackageItem.serialName = array[k].PartType;
				cOMA_PackageItem.itemName = array[k].itemName;
				cOMA_PackageItem.num = COMA_TexBase.Instance.texCountToSell - 1;
				cOMA_PackageItem.textureName = COMA_FileNameManager.Instance.GetFileName(cOMA_PackageItem.serialName);
				cOMA_PackageItem.LoadPNG();
				cOMA_PackageItem.CreateIconTexture();
				cOMA_PackageItem.state = COMA_PackageItem.PackageItemStatus.None;
				COMA_Pref.Instance.GetAnItem(cOMA_PackageItem);
			}
			COMA_Pref.Instance.Save(true);
			TUI_MsgBox.Instance.TipBox(2, 1, string.Empty, curSelData.AvatarIcon);
			COMA_HTTP_DataCollect.Instance.SendBuyModel(curSelData.PartType, curSelData.AvatarPrice.ToString());
		}
		else
		{
			if (curSelData.PartType == "Head01")
			{
				COMA_Pref.Instance.BoughtChanged(0, curSelData);
			}
			else if (curSelData.PartType == "Body01")
			{
				COMA_Pref.Instance.BoughtChanged(1, curSelData);
			}
			else if (curSelData.PartType == "Leg01")
			{
				COMA_Pref.Instance.BoughtChanged(2, curSelData);
			}
			string text = "BuyTemplates:";
			text += curSelData.PartType;
			Debug.Log("------------------------------------>" + text);
			COMA_HTTP_DataCollect.Instance.SendGoldInfo(COMA_Pref.Instance.GetGold().ToString(), Mathf.Abs(curSelData.AvatarPrice).ToString(), text);
			COMA_PackageItem cOMA_PackageItem2 = new COMA_PackageItem();
			cOMA_PackageItem2.serialName = curSelData.PartType;
			cOMA_PackageItem2.itemName = curSelData.itemName;
			cOMA_PackageItem2.num = COMA_TexBase.Instance.texCountToSell - 1;
			cOMA_PackageItem2.textureName = COMA_FileNameManager.Instance.GetFileName(cOMA_PackageItem2.serialName);
			cOMA_PackageItem2.LoadPNG();
			cOMA_PackageItem2.CreateIconTexture();
			cOMA_PackageItem2.state = COMA_PackageItem.PackageItemStatus.None;
			COMA_Pref.Instance.GetAnItem(cOMA_PackageItem2);
			COMA_Pref.Instance.Save(true);
			TUI_MsgBox.Instance.TipBox(2, 1, cOMA_PackageItem2.itemName, cOMA_PackageItem2.iconTexture);
			COMA_HTTP_DataCollect.Instance.SendBuyModel(curSelData.PartType, curSelData.AvatarPrice.ToString());
		}
	}

	private void BuyAdornment()
	{
		if (curSelData.AvatarPrice > 0)
		{
			COMA_Pref.Instance.AddGold(-curSelData.AvatarPrice);
			string text = "buyAccessoriesG:";
			text += curSelData.PartType;
			Debug.Log("------------------------------------>" + text);
			COMA_HTTP_DataCollect.Instance.SendGoldInfo(COMA_Pref.Instance.GetGold().ToString(), Mathf.Abs(curSelData.AvatarPrice).ToString(), text);
		}
		else
		{
			COMA_Pref.Instance.AddCrystal(curSelData.AvatarPrice);
			string text2 = "buyAccessories:";
			text2 += curSelData.PartType;
			Debug.Log("------------------------------------>" + text2);
			COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), Mathf.Abs(curSelData.AvatarPrice).ToString(), text2);
		}
		RefreshGoldAndCrystal();
		COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
		cOMA_PackageItem.serialName = curSelData.PartType;
		cOMA_PackageItem.itemName = curSelData.itemName;
		cOMA_PackageItem.part = COMA_PackageItem.NameToPart(cOMA_PackageItem.serialName);
		cOMA_PackageItem.CreateIconTexture();
		cOMA_PackageItem.state = COMA_PackageItem.PackageItemStatus.None;
		COMA_Pref.Instance.GetAnItem(cOMA_PackageItem);
		COMA_Pref.Instance.Save(true);
		TUI_MsgBox.Instance.TipBox(2, 1, cOMA_PackageItem.itemName, null, cOMA_PackageItem.serialName);
		if (curSelData.AvatarPrice >= 0)
		{
			COMA_HTTP_DataCollect.Instance.SendBuyAccessory_Gold(curSelData.PartType, Mathf.Abs(curSelData.AvatarPrice).ToString());
		}
		else
		{
			COMA_HTTP_DataCollect.Instance.SendBuyAccessory_Crystal(curSelData.PartType, Mathf.Abs(curSelData.AvatarPrice).ToString());
		}
	}

	public void HandleEventButton_Buy(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButton_Buy-CommandClick");
			switch (CurMenuLayer)
			{
			case 11:
			{
				if (SeletedSlots.Count != 1)
				{
					break;
				}
				UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot7 = SeletedSlots[0] as UIMarket_AvatarShopSlot;
				curSelData = uIMarket_AvatarShopSlot7.BoxData as UIMarket_AvatarShopData;
				if (curSelData == null || curSelData.AvatarIcon == null)
				{
					break;
				}
				info = COMA_Scene_Trade.Instance.GetTexInfoBySlotID(uIMarket_AvatarShopSlot7.GetID());
				if (COMA_Sys.Instance.IsCleaner)
				{
					if (info.isSuit)
					{
						COMA_Server_Texture.Instance.TextureSuit_DeleteToServer(info.tid);
					}
					else
					{
						COMA_Server_Texture.Instance.TextureSell_DeleteToServer(info.tid);
					}
					uIMarket_AvatarShopSlot7.BoxData = null;
				}
				else if (COMA_Pref.Instance.GetGold() < info.price)
				{
					UI_MsgBox uI_MsgBox6 = TUI_MsgBox.Instance.MessageBox(108);
					uI_MsgBox6.AddProceYesHandler(NeedMoreCoin);
				}
				else if (info.isSuit && COMA_Pref.Instance.PackageNullCount() < 3)
				{
					Debug.Log("Suit : " + COMA_Pref.Instance.PackageNullCount());
					int num3 = 3 - COMA_Pref.Instance.PackageNullCount();
					if (COMA_Package.maxCount - COMA_Package.slotUnlocked < num3)
					{
						TUI_MsgBox.Instance.MessageBox(107);
						break;
					}
					UI_MsgBox uI_MsgBox7 = TUI_MsgBox.Instance.MessageBox(123, null, num3, COMA_Package.unlockPrice * num3);
					uI_MsgBox7.AddProceYesHandler(UnLockedSlot);
					uI_MsgBox7.param = "Avatar";
				}
				else if (!info.isSuit && COMA_Pref.Instance.PackageNullCount() < 1)
				{
					Debug.Log("Single : " + COMA_Pref.Instance.PackageNullCount());
					if (COMA_Pref.Instance.PackageNullCountWithLocked() < 1)
					{
						TUI_MsgBox.Instance.MessageBox(107);
						break;
					}
					UI_MsgBox uI_MsgBox8 = TUI_MsgBox.Instance.MessageBox(123, null, 1, COMA_Package.unlockPrice);
					uI_MsgBox8.AddProceYesHandler(UnLockedSlot);
					uI_MsgBox8.param = "Avatar";
				}
				else
				{
					BuyAvatar();
				}
				break;
			}
			case 21:
			{
				if (SeletedSlots.Count != 1)
				{
					break;
				}
				UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot5 = SeletedSlots[0] as UIMarket_AvatarShopSlot;
				curSelData = uIMarket_AvatarShopSlot5.BoxData as UIMarket_AvatarShopData;
				if (curSelData == null)
				{
					break;
				}
				if (COMA_Pref.Instance.GetGold() < curSelData.AvatarPrice)
				{
					UI_MsgBox uI_MsgBox4 = TUI_MsgBox.Instance.MessageBox(108);
					uI_MsgBox4.AddProceYesHandler(NeedMoreCoin);
					break;
				}
				int num = 1;
				if (curSelData.PartType == "HBL01")
				{
					num = 3;
				}
				if (COMA_Pref.Instance.PackageNullCount() < num)
				{
					int num2 = num - COMA_Pref.Instance.PackageNullCount();
					if (COMA_Package.maxCount - COMA_Package.slotUnlocked < num2)
					{
						TUI_MsgBox.Instance.MessageBox(107);
						break;
					}
					UI_MsgBox uI_MsgBox5 = TUI_MsgBox.Instance.MessageBox(123, null, num2, COMA_Package.unlockPrice * num2);
					uI_MsgBox5.AddProceYesHandler(UnLockedSlot);
					uI_MsgBox5.param = "Model";
				}
				else
				{
					BuyModel();
				}
				break;
			}
			case 31:
			{
				if (SeletedSlots.Count != 1)
				{
					break;
				}
				UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot4 = SeletedSlots[0] as UIMarket_AvatarShopSlot;
				curSelData = uIMarket_AvatarShopSlot4.BoxData as UIMarket_AvatarShopData;
				if (curSelData == null)
				{
					break;
				}
				if (curSelData.AvatarPrice >= 0 && COMA_Pref.Instance.GetGold() < curSelData.AvatarPrice)
				{
					UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(108);
					uI_MsgBox.AddProceYesHandler(NeedMoreCoin);
				}
				else if (curSelData.AvatarPrice < 0 && COMA_Pref.Instance.GetCrystal() < -curSelData.AvatarPrice)
				{
					UI_MsgBox uI_MsgBox2 = TUI_MsgBox.Instance.MessageBox(109);
					uI_MsgBox2.AddProceYesHandler(NeedMoreCoin);
				}
				else if (COMA_Pref.Instance.PackageNullCount() < 1)
				{
					Debug.Log("Adornment : " + COMA_Pref.Instance.PackageNullCount());
					if (COMA_Package.maxCount - COMA_Package.slotUnlocked < 1)
					{
						TUI_MsgBox.Instance.MessageBox(107);
						break;
					}
					UI_MsgBox uI_MsgBox3 = TUI_MsgBox.Instance.MessageBox(123, null, 1, COMA_Package.unlockPrice);
					uI_MsgBox3.AddProceYesHandler(UnLockedSlot);
					uI_MsgBox3.param = "Adornment";
				}
				else
				{
					BuyAdornment();
				}
				break;
			}
			case 411:
			{
				if (SeletedSlots.Count != 1)
				{
					break;
				}
				UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot6 = SeletedSlots[0] as UIMarket_AvatarShopSlot;
				UIMarket_AvatarShopData uIMarket_AvatarShopData4 = uIMarket_AvatarShopSlot6.BoxData as UIMarket_AvatarShopData;
				if (uIMarket_AvatarShopData4 == null)
				{
					break;
				}
				Debug.Log("Selling Price:" + SellingPrice);
				int iD = uIMarket_AvatarShopSlot6.GetID();
				string tex4 = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[iD].texture.EncodeToPNG());
				int kind = COMA_PackageItem.NameToKind(COMA_Pref.Instance.package.pack[iD].serialName);
				if (COMA_Pref.Instance.package.pack[iD].num == COMA_TexBase.Instance.texCountToSell - 1)
				{
					TUI_MsgBox.Instance.MessageBox(134);
					break;
				}
				COMA_Achievement.Instance.GetRich++;
				if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 16)
				{
					_wizardMgr16.ResetUIControllersZ();
					_wizardMgr16.gameObject.SetActive(false);
					_wizardMask16.gameObject.SetActive(false);
					COMA_Sys.Instance.nTeachingId++;
					_wizardMask17.gameObject.SetActive(true);
					_wizardMgr17.gameObject.SetActive(true);
					_wizardMgr17.RefreshUIControllers();
					uIMarket_AvatarShopSlot6.BoxData = null;
				}
				else
				{
					COMA_Server_Texture.Instance.TextureSell_InitToServer(tex4, kind, SellingPrice, COMA_Pref.Instance.package.pack[iD].num, this);
					WaitingBox();
					SceneTimerInstance.Instance.Add(COMA_ServerManager.Instance.serverAddr_Save_OutTime, DestroyWaitingBox);
				}
				break;
			}
			case 412:
			{
				if (SeletedSlots.Count != 3)
				{
					TUI_MsgBox.Instance.MessageBox(110);
					break;
				}
				UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot = SeletedSlots[0] as UIMarket_AvatarShopSlot;
				UIMarket_AvatarShopData uIMarket_AvatarShopData = uIMarket_AvatarShopSlot.BoxData as UIMarket_AvatarShopData;
				UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot2 = SeletedSlots[1] as UIMarket_AvatarShopSlot;
				UIMarket_AvatarShopData uIMarket_AvatarShopData2 = uIMarket_AvatarShopSlot2.BoxData as UIMarket_AvatarShopData;
				UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot3 = SeletedSlots[2] as UIMarket_AvatarShopSlot;
				UIMarket_AvatarShopData uIMarket_AvatarShopData3 = uIMarket_AvatarShopSlot3.BoxData as UIMarket_AvatarShopData;
				if (uIMarket_AvatarShopData == null || uIMarket_AvatarShopData2 == null || uIMarket_AvatarShopData3 == null)
				{
					TUI_MsgBox.Instance.MessageBox(110);
					break;
				}
				Debug.Log("Selling Price:" + SellingPrice);
				int[] array = new int[3] { -1, -1, -1 };
				array[COMA_PackageItem.NameToKind(uIMarket_AvatarShopData.PartType)] = uIMarket_AvatarShopSlot.GetID();
				array[COMA_PackageItem.NameToKind(uIMarket_AvatarShopData2.PartType)] = uIMarket_AvatarShopSlot2.GetID();
				array[COMA_PackageItem.NameToKind(uIMarket_AvatarShopData3.PartType)] = uIMarket_AvatarShopSlot3.GetID();
				if (COMA_Pref.Instance.package.pack[array[0]].num == COMA_TexBase.Instance.texCountToSell - 1 && COMA_Pref.Instance.package.pack[array[1]].num == COMA_TexBase.Instance.texCountToSell - 1 && COMA_Pref.Instance.package.pack[array[2]].num == COMA_TexBase.Instance.texCountToSell - 1)
				{
					TUI_MsgBox.Instance.MessageBox(134);
				}
				else if (array[0] >= 0 && array[1] >= 0 && array[2] >= 0)
				{
					string tex = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[array[0]].texture.EncodeToPNG());
					string tex2 = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[array[1]].texture.EncodeToPNG());
					string tex3 = COMA_TexBase.Instance.TextureBytesToString(COMA_Pref.Instance.package.pack[array[2]].texture.EncodeToPNG());
					COMA_Server_Texture.Instance.TextureSuit_InitToServer(tex, tex2, tex3, -1, SellingPrice, COMA_Pref.Instance.package.pack[array[0]].num, this);
					WaitingBox();
					SceneTimerInstance.Instance.Add(COMA_ServerManager.Instance.serverAddr_Save_OutTime, DestroyWaitingBox);
				}
				else
				{
					TUI_MsgBox.Instance.MessageBox(110);
				}
				break;
			}
			}
			break;
		case 1:
			switch (CurMenuLayer)
			{
			case 11:
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_BuyItem);
				break;
			case 21:
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_BuyItem);
				break;
			case 31:
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_BuyItem);
				break;
			case 411:
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SellItem);
				break;
			case 412:
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SellItem);
				break;
			}
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void SellTexSuccess(string tid)
	{
		if (SeletedSlots.Count == 1)
		{
			UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot = SeletedSlots[0] as UIMarket_AvatarShopSlot;
			if (uIMarket_AvatarShopSlot.BoxData != null)
			{
				COMA_Pref.Instance.package.pack[uIMarket_AvatarShopSlot.GetID()].num = 0;
				uIMarket_AvatarShopSlot.BoxData = null;
				COMA_TexOnSale.COMA_TexOnSaleItem cOMA_TexOnSaleItem = new COMA_TexOnSale.COMA_TexOnSaleItem();
				cOMA_TexOnSaleItem.isSuit = false;
				cOMA_TexOnSaleItem.tid = tid;
				COMA_TexOnSale.Instance.items.Add(cOMA_TexOnSaleItem);
				COMA_Pref.Instance.Save(true);
				SceneTimerInstance.Instance.Remove(DestroyWaitingBox);
				DestroyWaitingBox();
				TUI_MsgBox.Instance.MessageBox(114);
				COMA_HTTP_DataCollect.Instance.SendTextureSell("single", tid, SellingPrice.ToString());
			}
		}
	}

	public void SellSuitSuccess(string tid)
	{
		if (SeletedSlots.Count == 3)
		{
			UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot = SeletedSlots[0] as UIMarket_AvatarShopSlot;
			UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot2 = SeletedSlots[1] as UIMarket_AvatarShopSlot;
			UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot3 = SeletedSlots[2] as UIMarket_AvatarShopSlot;
			if (uIMarket_AvatarShopSlot.BoxData != null && uIMarket_AvatarShopSlot2.BoxData != null && uIMarket_AvatarShopSlot3.BoxData != null)
			{
				COMA_Pref.Instance.package.pack[uIMarket_AvatarShopSlot.GetID()].num = 0;
				COMA_Pref.Instance.package.pack[uIMarket_AvatarShopSlot2.GetID()].num = 0;
				COMA_Pref.Instance.package.pack[uIMarket_AvatarShopSlot3.GetID()].num = 0;
				uIMarket_AvatarShopSlot.BoxData = null;
				uIMarket_AvatarShopSlot2.BoxData = null;
				uIMarket_AvatarShopSlot3.BoxData = null;
				COMA_TexOnSale.COMA_TexOnSaleItem cOMA_TexOnSaleItem = new COMA_TexOnSale.COMA_TexOnSaleItem();
				cOMA_TexOnSaleItem.isSuit = true;
				cOMA_TexOnSaleItem.tid = tid;
				COMA_TexOnSale.Instance.items.Add(cOMA_TexOnSaleItem);
				COMA_Pref.Instance.Save(true);
				SceneTimerInstance.Instance.Remove(DestroyWaitingBox);
				DestroyWaitingBox();
				ClearSelectedSlots();
				TUI_MsgBox.Instance.MessageBox(114);
				COMA_HTTP_DataCollect.Instance.SendTextureSell("suit", tid, SellingPrice.ToString());
			}
		}
	}

	public void ProcessClaim(UIMarket_SellingInfoSlot slot)
	{
		Debug.Log("---ProcessClaim");
		if (slot != null)
		{
			UIMarket_SellingInfoData uIMarket_SellingInfoData = slot.BoxData as UIMarket_SellingInfoData;
			int iD = slot.GetID();
			int num = uIMarket_SellingInfoData.SoldNum - COMA_TexOnSale.Instance.GetNumGet(uIMarket_SellingInfoData.TexID);
			COMA_Achievement.Instance.Drawer += num;
			COMA_Achievement.Instance.Artist += num;
			COMA_Achievement.Instance.DrawingExpert += num;
			COMA_Achievement.Instance.DrawingMaster += num;
			COMA_Achievement.Instance.DrawingGrandMaster += num;
			COMA_Pref.Instance.AddGold(uIMarket_SellingInfoData.BalanceNum);
			TUI_MsgBox.Instance.TipBox(0, uIMarket_SellingInfoData.BalanceNum, string.Empty, null);
			COMA_TexOnSale.Instance.SetNumGet(uIMarket_SellingInfoData.TexID, uIMarket_SellingInfoData.SoldNum);
			COMA_TexOnSale.Instance.SetGoldGet(uIMarket_SellingInfoData.TexID, uIMarket_SellingInfoData.BalanceNum);
			uIMarket_SellingInfoData.BalanceNum = 0;
			uIMarket_SellingInfoData.Cliamed = ((uIMarket_SellingInfoData.BalanceNum > 0) ? true : false);
			RefreshGoldAndCrystal();
			COMA_Pref.Instance.Save(true);
		}
	}

	private bool IsAllowRefresh()
	{
		if (Time.time - COMA_Sys.Instance.marketRefreshTime >= (float)COMA_Sys.Instance.marketRefreshInterval)
		{
			Debug.Log("--------------time:" + Time.time);
			COMA_Sys.Instance.marketRefreshTime = Time.time;
			return true;
		}
		return false;
	}

	protected void HandleRefreshShop()
	{
		Debug.Log("Call：HandleRefreshShop");
		if (IsAllowRefresh())
		{
			SceneTimerInstance.Instance.Add(10f, ReadyToRefresh);
			COMA_Scene_Trade.Instance.RequestItemsOnBuyShell();
			WaitingBox();
		}
		else
		{
			TUI_MsgBox.Instance.MessageBox(120);
			NotifyRefreshEnd();
		}
	}

	protected void NotifyRefreshEnd()
	{
		DestroyWaitingBox();
		_scrollList.EndRefresh();
	}

	public bool ReadyToRefresh()
	{
		if (CurMenuLayer == 11)
		{
			UIMarket_Container component = _marketContainer.GetComponent<UIMarket_Container>();
			component.SimulateCall_AvatarRefresh();
		}
		NotifyRefreshEnd();
		return false;
	}

	public void RefreshFinish()
	{
		ReadyToRefresh();
		SceneTimerInstance.Instance.Remove(ReadyToRefresh);
	}

	private void WaitingBox()
	{
		GameObject gameObject = Resources.Load("UI/Misc/IAPBox") as GameObject;
		Debug.Log("------waitingBoxPerfab" + gameObject.name);
		UI_MsgBox uI_MsgBox = MessageBox("Loading...", string.Empty, 0, gameObject, -300f);
		_waitingBox = uI_MsgBox.gameObject;
	}

	private bool DestroyWaitingBox()
	{
		if (_waitingBox != null)
		{
			Debug.Log("--DestroyImmediate");
			Object.Destroy(_waitingBox);
			_waitingBox = null;
		}
		return false;
	}

	public void RefreshSellingInfo(string tid, Texture2D tex, int kind, int gold, int num, float leftTime, int slotID, int forSellingID)
	{
		if (CurMenuLayer == 413 && forSellingID == forSellingExit)
		{
			StartCoroutine(RenderIcon(tid, tex, kind, gold, num, leftTime, slotID));
		}
	}

	private IEnumerator RenderIcon(string tid, Texture2D tex, int kind, int gold, int num, float leftTime, int slotID)
	{
		for (int k = 0; k < framesToLate; k++)
		{
			yield return new WaitForEndOfFrame();
		}
		framesToLate++;
		UIMarket_Container com = _marketContainer.GetComponent<UIMarket_Container>();
		UIMarket_SellingInfoData data = (UIMarket_SellingInfoData)com.LstBoxDatas[slotID];
		data.TexID = tid;
		GameObject tarObj = Object.Instantiate(Resources.Load("FBX/Player/Part/PFB/" + COMA_PackageItem.KindToName(kind))) as GameObject;
		tarObj.transform.GetChild(0).renderer.material.mainTexture = tex;
		data.AvatarIcon = IconShot.Instance.GetIconPic(tarObj);
		Object.DestroyObject(tarObj);
		data.DayNum = Mathf.FloorToInt(leftTime / 86400f);
		leftTime %= 86400f;
		data.HourNum = Mathf.FloorToInt(leftTime / 3600f);
		leftTime %= 3600f;
		data.MinuteNum = Mathf.FloorToInt(leftTime / 60f);
		data.SoldNum = COMA_TexBase.Instance.texCountToSell - num;
		int availableNum = data.SoldNum - COMA_TexOnSale.Instance.GetNumGet(data.TexID);
		if (availableNum < 0)
		{
			availableNum = 0;
		}
		data.BalanceNum = Mathf.FloorToInt((float)(availableNum * gold) * (1f - (float)COMA_Sys.Instance.tax / 100f));
		data.Cliamed = ((data.BalanceNum > 0) ? true : false);
		if (leftTime <= 0f && data.BalanceNum <= 0)
		{
			COMA_Server_Texture.Instance.TextureSell_DeleteToServer(tid);
			COMA_TexOnSale.Instance.DeleteWithtid(tid);
			COMA_Pref.Instance.Save(true);
		}
		yield return 0;
	}

	public void RefreshSellingInfo(string tid, Texture2D[] tex, int gold, int num, float leftTime, int slotID, int forSellingID)
	{
		if (CurMenuLayer == 413 && forSellingID == forSellingExit)
		{
			StartCoroutine(RenderIcon(tid, tex, gold, num, leftTime, slotID));
		}
	}

	private IEnumerator RenderIcon(string tid, Texture2D[] tex, int gold, int num, float leftTime, int slotID)
	{
		for (int k = 0; k < framesToLate; k++)
		{
			yield return new WaitForEndOfFrame();
		}
		framesToLate++;
		UIMarket_Container com = _marketContainer.GetComponent<UIMarket_Container>();
		UIMarket_SellingInfoData data = (UIMarket_SellingInfoData)com.LstBoxDatas[slotID];
		data.TexID = tid;
		GameObject tarObj = Object.Instantiate(Resources.Load("FBX/Player/Part/PFB/All")) as GameObject;
		tarObj.transform.FindChild("head").renderer.material.mainTexture = tex[0];
		tarObj.transform.FindChild("body").renderer.material.mainTexture = tex[1];
		tarObj.transform.FindChild("leg").renderer.material.mainTexture = tex[2];
		data.AvatarIcon = IconShot.Instance.GetIconPic(tarObj);
		Object.DestroyObject(tarObj);
		data.DayNum = Mathf.FloorToInt(leftTime / 86400f);
		leftTime %= 86400f;
		data.HourNum = Mathf.FloorToInt(leftTime / 3600f);
		leftTime %= 3600f;
		data.MinuteNum = Mathf.FloorToInt(leftTime / 60f);
		data.SoldNum = COMA_TexBase.Instance.texCountToSell - num;
		int availableNum = data.SoldNum - COMA_TexOnSale.Instance.GetNumGet(data.TexID);
		if (availableNum < 0)
		{
			availableNum = 0;
		}
		data.BalanceNum = Mathf.FloorToInt((float)(availableNum * gold) * (1f - (float)COMA_Sys.Instance.tax / 100f));
		data.Cliamed = ((data.BalanceNum > 0) ? true : false);
		if (leftTime <= 0f && data.BalanceNum <= 0)
		{
			COMA_Server_Texture.Instance.TextureSuit_DeleteToServer(tid);
			COMA_TexOnSale.Instance.DeleteWithtid(tid);
			COMA_Pref.Instance.Save(true);
		}
		yield return 0;
	}

	public void ProcessUIEnter()
	{
	}

	private void ProcessEnterTeaching16()
	{
		UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot = (UIMarket_AvatarShopSlot)_marketContainer.GetComponent<UIMarket_Container>().LstBoxs[0].Slots[3];
		uIMarket_AvatarShopSlot.AvatarSelected = !uIMarket_AvatarShopSlot.AvatarSelected;
		AddShopSlot(uIMarket_AvatarShopSlot);
		COMA_Sys.Instance.nTeachingId++;
		_wizardMgr15.ResetUIControllersZ();
		_wizardMgr15.gameObject.SetActive(false);
		_wizardMask16.gameObject.SetActive(true);
		_wizardMgr16.gameObject.SetActive(true);
		_wizardMgr16.RefreshUIControllers();
	}

	public void ProcessUIEnterEnd()
	{
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 2)
		{
			_wizardMgr1.RefreshUIControllers();
		}
		else if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 3)
		{
			_wizardMgr2.gameObject.SetActive(true);
			_wizardMgr2.RefreshUIControllers();
		}
		else if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 13)
		{
			_wizardMgr13.gameObject.SetActive(true);
			_wizardMgr13.RefreshUIControllers();
		}
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 14)
		{
			_wizardMask14.gameObject.SetActive(true);
			_wizardMgr14.gameObject.SetActive(true);
			_wizardMgr14.RefreshUIControllers();
		}
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 15)
		{
			_wizardMgr15.gameObject.SetActive(true);
			_wizardMgr15.RefreshUIControllers();
		}
		if (COMA_Pref.Instance.bNew_BuyAvatar && CurMenuLayer == 11)
		{
			_newGuideBoard.InitLabel(new Vector2(41f, -61f), "NoviceProcess_35");
			_newGuideBoard.AddProceHandler(ProcessGuideBoard2);
			_newGuideBoard.SetArrow(new Vector2(-64f, 85f), Vector3.zero);
		}
		if (COMA_Pref.Instance.bNew_BuyModel && CurMenuLayer == 21)
		{
			_newGuideBoard.SetArrow(new Vector2(0f, 85f), Vector3.zero);
			_newGuideBoard.InitLabel(new Vector2(76f, -64f), "NoviceProcess_37");
			_newGuideBoard.AddProceHandler(ProcessGuideBoard4);
		}
		if (COMA_Pref.Instance.bNew_SellSingle && (CurMenuLayer == 411 || CurMenuLayer == 412))
		{
			_newGuideBoard.SetArrow(new Vector2(126f, 34f), new Vector3(0f, 180f, 90f));
			_newGuideBoard.InitLabel(new Vector2(-114f, 4f), "NoviceProcess_39");
			_newGuideBoard.AddProceHandler(ProcessGuideBoard6);
		}
	}

	private void ProcessGuideBoard2()
	{
		_newGuideBoard.InitLabel(new Vector2(41f, -61f), "NoviceProcess_36");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard3);
	}

	private void ProcessGuideBoard3()
	{
		_newGuideBoard.EndLabel();
		_wizardMask13.gameObject.SetActive(false);
		COMA_Pref.Instance.bNew_BuyAvatar = false;
		COMA_Pref.Instance.Save(true);
	}

	private void ProcessGuideBoard4()
	{
		_newGuideBoard.InitLabel(new Vector2(76f, -64f), "NoviceProcess_38");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard5);
	}

	private void ProcessGuideBoard5()
	{
		_newGuideBoard.EndLabel();
		_wizardMask13.gameObject.SetActive(false);
		COMA_Pref.Instance.bNew_BuyModel = false;
		COMA_Pref.Instance.Save(true);
	}

	private void ProcessGuideBoard6()
	{
		_newGuideBoard.InitLabel(new Vector2(-114f, 4f), "NoviceProcess_40");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard7);
	}

	private void ProcessGuideBoard7()
	{
		_newGuideBoard.InitLabel(new Vector2(-114f, 4f), "NoviceProcess_41");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard8);
	}

	private void ProcessGuideBoard8()
	{
		_newGuideBoard.EndLabel();
		_wizardMask13.gameObject.SetActive(false);
		COMA_Pref.Instance.bNew_SellSingle = false;
		COMA_Pref.Instance.Save(true);
	}

	public void ProcessSingleUIEvent()
	{
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 3)
		{
			UIMarket_AvatarShopSlot uIMarket_AvatarShopSlot = (UIMarket_AvatarShopSlot)_marketContainer.GetComponent<UIMarket_Container>().LstBoxs[0].Slots[0];
			uIMarket_AvatarShopSlot.AvatarSelected = !uIMarket_AvatarShopSlot.AvatarSelected;
			AddAvatarShopSlot(uIMarket_AvatarShopSlot);
			COMA_Sys.Instance.nTeachingId++;
			_wizardMgr2.ResetUIControllersZ();
			_wizardMgr2.gameObject.SetActive(false);
			_wizardMask2.gameObject.SetActive(false);
			_wizardMask3.gameObject.SetActive(true);
			_wizardMgr3.gameObject.SetActive(true);
			_wizardMgr3.RefreshUIControllers();
		}
	}
}
