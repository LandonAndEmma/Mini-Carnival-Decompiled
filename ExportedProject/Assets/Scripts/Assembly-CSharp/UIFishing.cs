using System.Collections.Generic;
using Protocol;
using UnityEngine;

public class UIFishing : UIChatAble
{
	[SerializeField]
	private GameObject _objPullPole;

	[SerializeField]
	private GameObject _objFishBook;

	[SerializeField]
	private UIFishing_PullProcess _pullProcess;

	[SerializeField]
	private UIFishing_GetFishItems _getfishItem;

	[SerializeField]
	private UIFishing_ChatHistoryContainer _chatHistoryContainer;

	[SerializeField]
	private UIFishing_ChatHistoryContainer _chatHistoryContainer2;

	[SerializeField]
	private Transform _transTestChatArea;

	[SerializeField]
	private TUILabel _unreadChatNum;

	[SerializeField]
	private UIFishing_CurFishPoleInfo _uiFishPoleInfo;

	[SerializeField]
	private UIFishing_Map _fishingMap;

	[SerializeField]
	private GameObject _objBuyRodBtn;

	[SerializeField]
	private GameObject _objBuyRodPlane;

	public int[] _rankAwardNum = new int[3];

	public Transform cmrTrs;

	private bool _bInitChatArea;

	private bool _bActiveChatArea;

	private int _nUnreadChatNum;

	public List<CPopupInfo> _lstPopupInfo = new List<CPopupInfo>();

	private bool _bCanPopUp = true;

	[SerializeField]
	private UIFishing_BoatLeftInfo _boatLeftInfo;

	[SerializeField]
	private UI_PreRankResult _uiPreRankResult;

	[SerializeField]
	private UIFishing_RankInfo _rankInfo;

	[SerializeField]
	private GameObject _btnAC;

	[SerializeField]
	private UIFishing_OnBoatCheck _onBoatCheck;

	[SerializeField]
	private UIFishing_OffBoatCheck _offBoatCheck;

	[SerializeField]
	private GameObject _objFold;

	[SerializeField]
	private GameObject _objUnfold;

	private int _nAskingBoatId = -1;

	private float _fPreCheckTime = -30f;

	private int UnReadChatNum
	{
		get
		{
			return _nUnreadChatNum;
		}
		set
		{
			_nUnreadChatNum = value;
			if (_nUnreadChatNum > 0)
			{
				_unreadChatNum.gameObject.SetActive(true);
				_unreadChatNum.Text = _nUnreadChatNum.ToString();
			}
			else
			{
				_unreadChatNum.gameObject.SetActive(false);
			}
		}
	}

	private void PopupSynInfo()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("UI/Fishing/OtherPlayerFishingInfo")) as GameObject;
		GameObject gameObject2 = GameObject.Find("TUI/TUIControls/lefttopadapt_ac");
		if (gameObject2 == null)
		{
			gameObject2 = new GameObject("lefttopadapt_ac");
			gameObject2.transform.parent = GameObject.Find("TUI/TUIControls").transform;
			gameObject2.transform.position = new Vector3(240f, 160f, 0f);
			gameObject2.AddComponent<TUISelfAdaptiveAnchor>();
		}
		GameObject gameObject3 = GameObject.Find("TUI/TUIControls/lefttopadapt_ac/ani");
		if (gameObject3 == null)
		{
			gameObject3 = new GameObject("ani");
			gameObject3.transform.position = new Vector3(1f, 0f, 0f);
			gameObject3.transform.parent = gameObject2.transform;
		}
		gameObject.transform.parent = gameObject3.transform;
		UI_OtherPlayerFishingInfo component = gameObject.GetComponent<UI_OtherPlayerFishingInfo>();
		if (_lstPopupInfo[0]._id == 0)
		{
			string str = TUITool.StringFormat(TUITextManager.Instance().GetString("diaoyudaojiemian_desc21"), _lstPopupInfo[0]._strInfo);
			component.Popup(str);
		}
		else if (_lstPopupInfo[0]._id == 1)
		{
			string str2 = TUITool.StringFormat(TUITextManager.Instance().GetString("diaoyudaojiemian_desc22"), _lstPopupInfo[0]._strInfo);
			component.Popup(str2);
		}
		_bCanPopUp = false;
		_lstPopupInfo.RemoveAt(0);
	}

	public void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
		TFishingAddressBook.Instance.RegisterAddr(1, GetInstanceID());
	}

	private Vector2 ScreenPosToTUIPos(Vector2 pos)
	{
		float x = (pos.x / (float)Screen.width - 0.5f) * 240f;
		float y = (pos.y / (float)Screen.height - 0.5f) * 160f;
		return new Vector2(x, y);
	}

	private void InitChatBoxArea()
	{
		Rect area = TouchScreenKeyboard.area;
		Debug.Log("============================>KeyBoard Rect:" + area.left + "," + area.top + "," + area.width + "," + area.height);
		Debug.Log("============================>Screen Rect:" + Screen.width + "," + Screen.height);
		float num = 0.45f;
		if (Screen.height <= 640)
		{
			num = 0.32f;
		}
		float y = 320f * num;
		float x = 480f;
		float newY = (1f - num + num / 2f - 0.5f) * 320f;
		_chatHistoryContainer.InitScrollList(new Vector2(x, y), newY);
	}

	private void Start()
	{
		UnReadChatNum = 0;
		_objPullPole.SetActive(false);
		_getfishItem.gameObject.SetActive(false);
		_chatBox.gameObject.SetActive(false);
		InitChatBoxArea();
		_objFold.SetActive(true);
		_objUnfold.SetActive(false);
		COMA_Server_Rank.Instance.GetRankWithMode(COMA_Server_ID.Instance.GID, COMA_Server_Rank.Instance.lstCountWorlds, COMA_CommonOperation.Instance.SceneIDToRankID(6));
		RefreshRanking();
	}

	private void ManuaRefreshRanking(COtherPlayerInfo info)
	{
		RankInfo rankInfo = new RankInfo();
		rankInfo._strName = info._name;
		rankInfo._fNum = info._weight;
		rankInfo._tex = null;
		rankInfo._strId = info._playerId;
		_rankInfo.RefreshRankInfo(rankInfo);
	}

	private void RefreshRanking()
	{
		RankItem[] array = new RankItem[3];
		if (UIDataBufferCenter.Instance.GetRank_World("ComAvatar007") == null)
		{
			return;
		}
		_rankInfo.ClearRankInfoLst();
		Debug.Log(UIDataBufferCenter.Instance.GetRank_World("ComAvatar007").m_list.Count);
		for (int i = 0; i < Mathf.Min(UIDataBufferCenter.Instance.GetRank_World("ComAvatar007").m_list.Count, 3); i++)
		{
			array[i] = UIDataBufferCenter.Instance.GetRank_World("ComAvatar007").m_list[i];
			RankInfo info0 = new RankInfo();
			info0._fNum = array[i].m_score;
			info0._tex = null;
			info0._strId = array[i].m_role_id.ToString();
			int curI = i;
			UIDataBufferCenter.Instance.FetchPlayerProfile(array[i].m_role_id, delegate(WatchRoleInfo watchInfo)
			{
				info0._strName = watchInfo.m_name;
				_rankInfo.SetRankInfo(info0, curI);
			});
			_rankAwardNum[i] = UIDataBufferCenter.Instance.GetRankAward_World("ComAvatar007")[i]._num;
			Debug.Log("============================================Fishing Award:" + _rankAwardNum[i]);
		}
	}

	private new void Update()
	{
		if (_lstPopupInfo.Count > 0 && _bCanPopUp)
		{
			PopupSynInfo();
		}
		if (_bActiveChatArea && TouchScreenKeyboard.visible)
		{
			InitChatBoxArea();
			_chatHistoryContainer.GetComponent<TUIScrollList_Avatar>().HandRefresh();
			_bActiveChatArea = false;
		}
	}

	public void HandleEventButton_AddFriend0(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			control.gameObject.SetActive(false);
			uint id = uint.Parse(_uiPreRankResult._id[0]);
			UIDataBufferCenter.Instance.TryToAddFriend(id);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_AddFriend1(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			control.gameObject.SetActive(false);
			uint id = uint.Parse(_uiPreRankResult._id[1]);
			UIDataBufferCenter.Instance.TryToAddFriend(id);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_AddFriend2(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			control.gameObject.SetActive(false);
			uint id = uint.Parse(_uiPreRankResult._id[2]);
			UIDataBufferCenter.Instance.TryToAddFriend(id);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_ClosePreReuslt(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			control.gameObject.transform.parent.gameObject.SetActive(false);
		}
	}

	public void HandleEventButton_FishBook(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			_objFishBook.SetActive(true);
		}
	}

	public void HandleEventButton_AC(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_rankInfo.gameObject.SetActive(true);
			control.gameObject.SetActive(false);
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

	public void HandleEventButton_CloseAC(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_rankInfo.gameObject.SetActive(false);
			_btnAC.SetActive(true);
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

	public void HandleEventButton_chat(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			UnReadChatNum = 0;
			_chatBox.gameObject.SetActive(true);
			Rect rect = _chatBox.SetFocus();
			InitChatBoxArea();
			Debug.Log("============================>KeyBoard Rect:" + rect.left + "," + rect.top + "," + rect.width + "," + rect.height);
			Debug.Log("============================>Screen Rect:" + Screen.width + "," + Screen.height);
			_chatHistoryContainer.GetComponent<TUIScrollList_Avatar>().HandRefresh();
			Debug.Log("Button_chat-CommandClick");
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public override void CancelOrHide()
	{
		_bActiveChatArea = false;
		_chatBox.KillFocus();
		_chatBox.gameObject.SetActive(false);
		Debug.Log(">>>>>>>>>>>>>>>>>>>>>>CancelOrHide END");
	}

	public void HandleEventButton_closeChat(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			CancelOrHide();
			Debug.Log("Button_closeChat-CommandClick");
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

	public void HandleEventButton_CloseBuyRod(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_objBuyRodPlane.SetActive(false);
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

	public void HandleEventButton_CloseOk(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			_objBuyRodPlane.SetActive(false);
			UIFishing_BuyRodPlane component = _objBuyRodPlane.GetComponent<UIFishing_BuyRodPlane>();
			if (component.CurSelBtn >= 0)
			{
				if (component.CurSelBtn == 1)
				{
					COMA_Pref.Instance.AddGold(-2000);
					COMA_HTTP_DataCollect.Instance.SendGoldInfo(COMA_Pref.Instance.GetGold().ToString(), Mathf.Abs(COMA_CommonOperation.Instance.selectedWeaponPrice).ToString(), "buygameltem");
				}
				else if (component.CurSelBtn == 2)
				{
					COMA_Pref.Instance.AddCrystal(-25);
					COMA_HTTP_DataCollect.Instance.SendCrystalInfo(COMA_Pref.Instance.GetCrystal().ToString(), Mathf.Abs(COMA_CommonOperation.Instance.selectedWeaponPrice).ToString(), "buygameltem");
				}
				if (component.CurSelBtn == 0)
				{
					COMA_Fishing.Instance.SetPoleTime(0);
				}
				else if (component.CurSelBtn == 1)
				{
					COMA_Fishing.Instance.SetPoleTime(1);
				}
				int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
				if (component.CurSelBtn == 0)
				{
					TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1, TTelegram.SEND_MSG_IMMEDIATELY, null);
				}
				else if (component.CurSelBtn == 1)
				{
					TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 2, TTelegram.SEND_MSG_IMMEDIATELY, null);
				}
				else if (component.CurSelBtn == 2)
				{
					TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 3, TTelegram.SEND_MSG_IMMEDIATELY, null);
				}
				component.CurSelBtn = -1;
				COMA_Pref.Instance.Save(true);
			}
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_OkOnBoatCheck(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			if (COMA_Pref.Instance.GetGold() >= COMA_PlayerSelf_Fishing._nOnBoatPrice)
			{
				int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
				TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 15, TTelegram.SEND_MSG_IMMEDIATELY, 1);
			}
			else
			{
				TUI_MsgBox.Instance.MessageBox(105);
				int iDByName2 = TFishingAddressBook.Instance.GetIDByName(0);
				TMessageDispatcher.Instance.DispatchMsg(-1, iDByName2, 15, TTelegram.SEND_MSG_IMMEDIATELY, 0);
			}
			_onBoatCheck.gameObject.SetActive(false);
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

	public void HandleEventButton_CloseOnBoatCheck(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			_onBoatCheck.gameObject.SetActive(false);
			int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
			TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 15, TTelegram.SEND_MSG_IMMEDIATELY, 0);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_OkOffBoatCheck(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_offBoatCheck.gameObject.SetActive(false);
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

	public void HandleEventButton_CloseOffBoatCheck(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_offBoatCheck.gameObject.SetActive(false);
			_nAskingBoatId = -1;
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

	public void GoBackToMainMenu(string param)
	{
		COMA_NetworkConnect.Instance.BackFromScene();
	}

	public void HandleEventButton_back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			Debug.Log("Button_back-CommandClick");
			if (_uiFishPoleInfo.GetCurFishPoleNum() != 0)
			{
				UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(229);
				uI_MsgBox.AddProceYesHandler(GoBackToMainMenu);
			}
			else
			{
				GoBackToMainMenu("COMA_Scene_Fishing");
			}
			COMA_Pref.Instance.Save(true);
			break;
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_buy(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Back);
			_objBuyRodPlane.SetActive(true);
			UIFishing_BuyRodPlane component = _objBuyRodPlane.GetComponent<UIFishing_BuyRodPlane>();
			component.InitTimeLimit(COMA_Fishing.Instance.GetPoleTime(0), COMA_Fishing.Instance.nTime0, 0);
			component.InitTimeLimit(COMA_Fishing.Instance.GetPoleTime(1), COMA_Fishing.Instance.nTime1, 1);
			break;
		}
		case 1:
			BtnOpenLight(control);
			break;
		case 2:
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButton_pullclick(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 1:
			_pullProcess.AddProcess();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Fishing_Powerup, base.transform, 2f, false);
			break;
		}
	}

	public void HandleEventButton_fold(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			UnReadChatNum = 0;
			_objFold.SetActive(false);
			_objUnfold.SetActive(true);
			_chatHistoryContainer2.InitScrollList();
			_chatHistoryContainer2.GetComponent<TUIScrollList_Avatar>().HandRefresh();
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public void HandleEventButton_unfold(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			_objFold.SetActive(true);
			_objUnfold.SetActive(false);
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}

	public override bool HandleMessage(TTelegram msg)
	{
		Debug.Log("UIFishing-HandleMessage:" + msg._nMsgId);
		switch (msg._nMsgId)
		{
		case 1000:
		{
			COMA_Fishing_FishableObj cOMA_Fishing_FishableObj = msg._pExtraInfo as COMA_Fishing_FishableObj;
			Debug.Log("--------------------------->UIGET:" + cOMA_Fishing_FishableObj.name);
			if (!(cOMA_Fishing_FishableObj != null))
			{
				break;
			}
			_getfishItem.gameObject.SetActive(true);
			_getfishItem._bNeedPopup = false;
			if (cOMA_Fishing_FishableObj.GetEntityType() == 100)
			{
				int weight = ((COMA_Fishing_Fish)cOMA_Fishing_FishableObj).GetWeight();
				int customID = ((COMA_Fishing_Fish)cOMA_Fishing_FishableObj).GetCustomID();
				string icon = string.Empty;
				string id = string.Empty;
				string empty = string.Empty;
				Fish_Param fishParam = COMA_Fishing_FishPool.Instance.GetFishParam(customID);
				if (fishParam != null)
				{
					icon = fishParam._strTex2d;
					id = fishParam._strNameID;
					empty = fishParam._strDesID;
				}
				string text2 = ((float)weight / 1000f).ToString("F2");
				string desLabel = TUITool.StringFormat(TUITextManager.Instance().GetString("diaoyudaojiemian_desc5"), text2, TUITextManager.Instance().GetString(id));
				_getfishItem.SetDesLabel(desLabel);
				_getfishItem.SetBtnLabel("ok");
				_getfishItem.SetIcon(icon);
				Debug.Log("######### UISYSTEM ########## GET:FISH TYPE=" + customID + " FISH WEIGHT=" + weight + "  FISH MAX=" + fishParam._nMaxWeight + "  FISH MIN=" + fishParam._nMinWeight + "  FISH RATE=" + (float)(weight / fishParam._nMaxWeight));
				COMA_FishCatalog.Instance.GetFish(customID, weight);
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Fishing_Good_Popup);
			}
			else if (cOMA_Fishing_FishableObj.GetEntityType() == 101)
			{
				COMA_Fishing_Chest cOMA_Fishing_Chest = cOMA_Fishing_FishableObj as COMA_Fishing_Chest;
				_getfishItem._bNeedPopup = true;
				_getfishItem._nCrystal = cOMA_Fishing_Chest.GetCrystalNum();
				_getfishItem._nGoldNum = cOMA_Fishing_Chest.GetGoldNum();
				_getfishItem._strDecoName = cOMA_Fishing_Chest.GetDecoName();
				_getfishItem.SetDesLabelID("diaoyudaojiemian_desc6");
				_getfishItem.SetIcon("baoxiang");
				_getfishItem.SetBtnLabel("open");
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Fishing_Good_Popup);
			}
			else if (cOMA_Fishing_FishableObj.GetEntityType() == 102)
			{
				_getfishItem.SetDesLabelID("diaoyudaojiemian_desc7");
				_getfishItem.SetIcon("xuezi");
				_getfishItem.SetBtnLabel("ok");
				COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Fishing_Useless_Popup);
			}
			break;
		}
		case 1001:
			_objPullPole.SetActive(true);
			break;
		case 1002:
			_objPullPole.SetActive(false);
			break;
		case 1003:
		{
			ChatHistoryInfo chatHistoryInfo = msg._pExtraInfo as ChatHistoryInfo;
			SaveChatHistory(chatHistoryInfo._strName, chatHistoryInfo._strWords, chatHistoryInfo._strID);
			if (!_chatBox.gameObject.activeSelf && !_objUnfold.activeSelf)
			{
				UnReadChatNum++;
			}
			break;
		}
		case 1004:
		{
			COtherPlayerInfo info = (COtherPlayerInfo)msg._pExtraInfo;
			ManuaRefreshRanking(info);
			break;
		}
		case 1017:
			_lstPopupInfo.Add((CPopupInfo)msg._pExtraInfo);
			break;
		case 1018:
			_lstPopupInfo.Add((CPopupInfo)msg._pExtraInfo);
			break;
		case 1005:
			_bCanPopUp = true;
			break;
		case 1006:
			_uiFishPoleInfo.SetCurFishPole((int)msg._pExtraInfo);
			break;
		case 1007:
		{
			int nSender = msg._nSender;
			COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Fishing;
			if (cOMA_PlayerSelf_Fishing != null && cOMA_PlayerSelf_Fishing.CurFishPole != null && nSender == cOMA_PlayerSelf_Fishing.CurFishPole.GetInstanceID())
			{
				_uiFishPoleInfo.SetCurFishPoleNum((int)msg._pExtraInfo);
				if ((int)msg._pExtraInfo == 0)
				{
					_objBuyRodBtn.SetActive(true);
				}
				else
				{
					_objBuyRodBtn.SetActive(false);
				}
			}
			break;
		}
		case 1008:
			_boatLeftInfo.gameObject.SetActive(true);
			break;
		case 1009:
			_boatLeftInfo.gameObject.SetActive(false);
			break;
		case 1010:
			_onBoatCheck.gameObject.SetActive(true);
			_nAskingBoatId = -1;
			_offBoatCheck.gameObject.SetActive(false);
			break;
		case 1011:
			_nAskingBoatId = int.Parse((string)msg._pExtraInfo);
			_offBoatCheck.InitOffBoatCheckId(_nAskingBoatId - 1);
			_offBoatCheck.gameObject.SetActive(true);
			_onBoatCheck.gameObject.SetActive(false);
			break;
		case 1020:
		{
			_offBoatCheck.gameObject.SetActive(false);
			_nAskingBoatId = -1;
			_onBoatCheck.gameObject.SetActive(false);
			COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing3 = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Fishing;
			cOMA_PlayerSelf_Fishing3.CanApplyBoat = true;
			break;
		}
		case 1019:
			if (_nAskingBoatId != -1 && (int)msg._pExtraInfo == _nAskingBoatId)
			{
				_offBoatCheck.gameObject.SetActive(false);
				_nAskingBoatId = -1;
				COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing2 = COMA_PlayerSelf.Instance as COMA_PlayerSelf_Fishing;
				cOMA_PlayerSelf_Fishing2.CanApplyBoat = true;
				cOMA_PlayerSelf_Fishing2._bStayNotifyOnBoat = true;
			}
			break;
		case 1012:
			if (Time.time - _fPreCheckTime >= 30f)
			{
				UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(231);
				uI_MsgBox.AddProceYesHandler(OffBoatCheck);
				_fPreCheckTime = Time.time;
			}
			break;
		case 1014:
		{
			PreRankResultData preRankResultData2 = msg._pExtraInfo as PreRankResultData;
			_uiPreRankResult._id[0] = preRankResultData2._strId[0];
			_uiPreRankResult.SetName(preRankResultData2._strName[0], 0);
			_uiPreRankResult.SetWeight(preRankResultData2._strWeight[0], 0);
			_uiPreRankResult.SetAward(_rankAwardNum[0].ToString(), 0);
			_uiPreRankResult._id[1] = preRankResultData2._strId[1];
			_uiPreRankResult.SetName(preRankResultData2._strName[1], 1);
			_uiPreRankResult.SetWeight(preRankResultData2._strWeight[1], 1);
			_uiPreRankResult.SetAward(_rankAwardNum[1].ToString(), 1);
			_uiPreRankResult._id[2] = preRankResultData2._strId[2];
			_uiPreRankResult.SetName(preRankResultData2._strName[2], 2);
			_uiPreRankResult.SetWeight(preRankResultData2._strWeight[2], 2);
			_uiPreRankResult.SetAward(_rankAwardNum[2].ToString(), 2);
			_uiPreRankResult.gameObject.SetActive(true);
			break;
		}
		case 1015:
			RefreshRanking();
			break;
		case 1016:
			if (COMA_Server_Rank.Instance.fishingWinnerID.Count != 0)
			{
				RefreshRanking();
				PreRankResultData preRankResultData = new PreRankResultData();
				preRankResultData._strId[0] = COMA_Server_Rank.Instance.fishingWinnerID[0];
				preRankResultData._strName[0] = COMA_Server_Rank.Instance.fishingWinnerName[0];
				preRankResultData._strWeight[0] = COMA_Server_Rank.Instance.fishingWinnerWeight[0];
				if (COMA_Server_Rank.Instance.fishingWinnerID.Count > 1)
				{
					preRankResultData._strId[1] = COMA_Server_Rank.Instance.fishingWinnerID[1];
					preRankResultData._strName[1] = COMA_Server_Rank.Instance.fishingWinnerName[1];
					preRankResultData._strWeight[1] = COMA_Server_Rank.Instance.fishingWinnerWeight[1];
				}
				else
				{
					preRankResultData._strId[1] = string.Empty;
					preRankResultData._strName[1] = string.Empty;
					preRankResultData._strWeight[1] = string.Empty;
				}
				if (COMA_Server_Rank.Instance.fishingWinnerID.Count > 2)
				{
					preRankResultData._strId[2] = COMA_Server_Rank.Instance.fishingWinnerID[2];
					preRankResultData._strName[2] = COMA_Server_Rank.Instance.fishingWinnerName[2];
					preRankResultData._strWeight[2] = COMA_Server_Rank.Instance.fishingWinnerWeight[2];
				}
				else
				{
					preRankResultData._strId[2] = string.Empty;
					preRankResultData._strName[2] = string.Empty;
					preRankResultData._strWeight[2] = string.Empty;
				}
				COMA_Sys.Instance.fishLocalInfo = string.Empty;
				for (int i = 0; i < 3; i++)
				{
					COMA_Sys.Instance.fishLocalInfo += preRankResultData._strId[i];
					COMA_Sys.Instance.fishLocalInfo += preRankResultData._strName[i];
					COMA_Sys.Instance.fishLocalInfo += preRankResultData._strWeight[i];
				}
				string text = COMA_FileIO.LoadFile(COMA_FileNameManager.Instance.GetFileName("FishLocal"));
				Debug.Log(text + " " + COMA_Sys.Instance.fishLocalInfo);
				if (text != COMA_Sys.Instance.fishLocalInfo)
				{
					int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
					TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1014, TTelegram.SEND_MSG_IMMEDIATELY, preRankResultData);
					COMA_FileIO.SaveFile(COMA_FileNameManager.Instance.GetFileName("FishLocal"), COMA_Sys.Instance.fishLocalInfo);
				}
			}
			break;
		}
		return true;
	}

	public void OffBoatCheck(string param)
	{
		int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
		TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 16, TTelegram.SEND_MSG_IMMEDIATELY, null);
	}

	public override void NotifyInputString(string str)
	{
		if (str != string.Empty)
		{
			Debug.Log("===============================================>Self Talk:" + str);
			base.NotifyInputString(str);
			SaveChatHistory(str);
		}
		CancelOrHide();
	}

	public void SaveChatHistory(string str)
	{
		SaveChatHistory(COMA_Pref.Instance.nickname, str, COMA_PlayerSelf.Instance.id.ToString());
	}

	public void SaveChatHistory(string name, string str, string id)
	{
		_chatHistoryContainer.AddOneWords(name, str, id);
		_chatHistoryContainer2.AddOneWords(name, str, id);
	}

	private Vector2 CalcPressPoint(float fW, float fL)
	{
		float x = fW * (float)Screen.width;
		float y = fL * (float)Screen.height;
		return new Vector2(x, y);
	}

	public void HandleEventButton_RotateCamera(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("wparam=" + wparam + "lparam=" + lparam);
			Vector2 vector = CalcPressPoint(wparam, lparam);
			Ray ray = cmrTrs.camera.ScreenPointToRay(vector);
			int layerMask = 1 << LayerMask.NameToLayer("3DBtn");
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 100f, layerMask))
			{
				PraiseBtn component = hitInfo.collider.transform.parent.GetComponent<PraiseBtn>();
				if (component != null)
				{
					int iDByName = TFishingAddressBook.Instance.GetIDByName(0);
					TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 17, TTelegram.SEND_MSG_IMMEDIATELY, component._fishPlayer.id);
					COMA_CD_FishingPraise cOMA_CD_FishingPraise = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.FISHING_PRAISE) as COMA_CD_FishingPraise;
					cOMA_CD_FishingPraise.nId = component._fishPlayer.id;
					cOMA_CD_FishingPraise.sendName = COMA_PlayerSelf.Instance.nickname;
					cOMA_CD_FishingPraise.recvName = component._fishPlayer.nickname;
					COMA_CommandHandler.Instance.Send(cOMA_CD_FishingPraise);
					COMA_Fishing_SceneController.Instance.RefreshPraiseTime();
				}
			}
			break;
		}
		}
	}
}
