using UnityEngine;

public class UIMainMenu : UIMessageHandler
{
	private GameObject gameRulesBoxPrefab;

	[SerializeField]
	private TUILabel nameLabel;

	public Transform playerOnShowTrs;

	public TUILabel goldCom;

	public TUILabel crystalCom;

	public UIMainMenu_PlayerInfo playerInfoCom;

	[SerializeField]
	private TUILabel _canGetACNum;

	[SerializeField]
	private GameObject _wizardMask;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask6;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr6;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask11;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask12;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr12;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMask18;

	[SerializeField]
	private UI_NewcomersWizardMgr _wizardMgr18;

	[SerializeField]
	private TUILabel _pendingFriendsNum;

	[SerializeField]
	private TUILabel _awardsNum;

	private bool _bEnterEnd;

	[SerializeField]
	private COMA_PlayerSelfCharacter _selfChara;

	[SerializeField]
	private UI_NewGuideBoard _newGuideBoard;

	[SerializeField]
	private GameObject recordNumObj;

	private UI_MsgBox msgBox;

	[SerializeField]
	private GameObject _shopLight_t;

	[SerializeField]
	private GameObject _inventoryLight_t;

	[SerializeField]
	private TUIControl[] _modeBtns;

	[SerializeField]
	private TUIScrollList_Avatar _scrollListModes;

	[SerializeField]
	private TUIScrollListFix_Avatar _scrollListFixModes;

	[SerializeField]
	private UI_NewcomersWizardMask _wizardMaskMode;

	[SerializeField]
	private Transform _playerPic;

	[SerializeField]
	private GameObject _quickEditGroup;

	[SerializeField]
	private GameObject _wizardMask2;

	[SerializeField]
	private GameObject _wizardMgr;

	private GameObject _waitingBox;

	private int PendingFriendsNum
	{
		set
		{
			if (value <= 0)
			{
				_pendingFriendsNum.gameObject.transform.parent.gameObject.SetActive(false);
				return;
			}
			_pendingFriendsNum.gameObject.transform.parent.gameObject.SetActive(true);
			_pendingFriendsNum.Text = value.ToString();
		}
	}

	public int AwardsNum
	{
		set
		{
			if (value <= 0)
			{
				_awardsNum.gameObject.transform.parent.parent.gameObject.SetActive(false);
				return;
			}
			_awardsNum.gameObject.transform.parent.parent.gameObject.SetActive(true);
			_awardsNum.Text = value.ToString();
		}
	}

	private bool ProcessEnterTeaching12()
	{
		_wizardMask11.gameObject.SetActive(false);
		COMA_Sys.Instance.nTeachingId++;
		_wizardMask12.gameObject.SetActive(true);
		_wizardMgr12.gameObject.SetActive(true);
		_wizardMgr12.RefreshUIControllers();
		return false;
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

	public void ProcessEnterEnd()
	{
		_bEnterEnd = true;
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 6 && _wizardMgr6 != null)
		{
			_wizardMgr6.gameObject.SetActive(true);
			_wizardMgr6.RefreshUIControllers();
		}
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 10)
		{
			COMA_Achievement.Instance.Apprentice++;
			COMA_Sys.Instance.nTeachingId++;
			_wizardMask11.gameObject.SetActive(true);
			SceneTimerInstance.Instance.Add(3f, ProcessEnterTeaching12);
		}
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 0)
		{
			_wizardMgr18.gameObject.SetActive(true);
			_wizardMgr18.RefreshUIControllers();
		}
		if (COMA_Pref.Instance.bNew_MainMenu && COMA_Pref.Instance.bNew_guidelinesId == 0)
		{
			_selfChara.Enter();
			COMA_Pref.Instance.bNew_guidelinesId = 1;
		}
	}

	public void ProcessGuideMoveDownEnd()
	{
		_newGuideBoard.InitLabel(new Vector2(42f, 14f), "NoviceProcess_26");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard1);
	}

	private bool DProcessGuideBoard2()
	{
		_newGuideBoard.AddProceHandler(ProcessGuideBoard2);
		return false;
	}

	private void ProcessGuideBoard1()
	{
		_newGuideBoard.InitLabel(new Vector2(42f, 14f), "NoviceProcess_27");
		SceneTimerInstance.Instance.Add(0.1f, DProcessGuideBoard2);
		Debug.Log("------------End--ProcessGuideBoard1()");
	}

	private void ProcessGuideBoard2()
	{
		Debug.Log("ProcessGuideBoard2;");
		_newGuideBoard.EndLabel();
		_wizardMgr18.gameObject.SetActive(true);
		_wizardMgr18.RefreshUIControllers(-220f);
		_selfChara.PointDown();
	}

	public void ProcessGuidePointDownEnd()
	{
		_newGuideBoard.InitLabel(new Vector2(42f, 14f), "NoviceProcess_28");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard3);
	}

	private void ProcessGuideBoard3()
	{
		_wizardMgr18.ResetUIControllersZ();
		_wizardMgr18.gameObject.SetActive(false);
		_newGuideBoard.EndLabel();
		_selfChara.MoveRight();
	}

	public void ProcessGuidePointRightupEnd()
	{
		_wizardMgr12.gameObject.SetActive(true);
		_wizardMgr12.RefreshUIControllers(-220f);
		_newGuideBoard.InitLabel(new Vector2(-42f, 14f), "NoviceProcess_29");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard4);
	}

	private void ProcessGuideBoard4()
	{
		_wizardMgr12.ResetUIControllersZ();
		_wizardMgr12.gameObject.SetActive(false);
		_wizardMgr6.gameObject.SetActive(true);
		_wizardMgr6.RefreshUIControllers(-220f);
		_selfChara.PointRightDown();
		_newGuideBoard.InitLabel(new Vector2(-42f, 14f), "NoviceProcess_30");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard5);
	}

	private void ProcessGuideBoard5()
	{
		_wizardMgr6.ResetUIControllersZ();
		_wizardMgr6.gameObject.SetActive(false);
		_selfChara.Bow();
		_newGuideBoard.InitLabel(new Vector2(-42f, 14f), "NoviceProcess_31");
		_newGuideBoard.AddProceHandler(ProcessGuideBoard6);
	}

	private void ProcessGuideBoard6()
	{
		_newGuideBoard.EndLabel();
		_selfChara.MoveLeft();
	}

	public void ProcessMoveLeftEnd()
	{
		COMA_Pref.Instance.bNew_MainMenu = false;
		COMA_Pref.Instance.Save(true);
		_playerPic.localPosition = new Vector3(0f, 0f, 0f);
		_wizardMgr18.ResetUIControllersZ();
		_wizardMgr18.gameObject.SetActive(false);
		_wizardMask18.gameObject.SetActive(false);
		COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
		cOMA_PackageItem.serialName = "Head01";
		cOMA_PackageItem.itemName = "Head";
		cOMA_PackageItem.num = COMA_TexBase.Instance.texCountToSell - 1;
		cOMA_PackageItem.textureName = COMA_FileNameManager.Instance.GetFileName(cOMA_PackageItem.serialName);
		cOMA_PackageItem.LoadPNG();
		cOMA_PackageItem.CreateIconTexture();
		cOMA_PackageItem.state = COMA_PackageItem.PackageItemStatus.None;
		COMA_Pref.Instance.GetAnItem(cOMA_PackageItem);
		COMA_Pref.Instance.Save(true);
		TUI_MsgBox.Instance.TipBox(2, 1, cOMA_PackageItem.itemName, cOMA_PackageItem.iconTexture);
	}

	private void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
		gameRulesBoxPrefab = Resources.Load("UI/Misc/GameRulesBox") as GameObject;
		_quickEditGroup.SetActive(false);
		TUIControl[] modeBtns = _modeBtns;
		foreach (TUIControl tUIControl in modeBtns)
		{
			tUIControl.gameObject.SetActive(false);
		}
		int num = COMA_Version.Instance.tickets.Length;
		int[] array = new int[num];
		int num2 = 0;
		for (int j = 0; j < num; j++)
		{
			if (COMA_Version.Instance.tickets[j] > 0)
			{
				array[num2] = COMA_Version.Instance.orders[j];
				num2++;
			}
		}
		TUIControl[] array2 = new TUIControl[num2];
		for (int k = 0; k < num2; k++)
		{
			array2[k] = _modeBtns[array[k]];
			array2[k].gameObject.SetActive(true);
		}
		if (num2 < 6)
		{
			_scrollListModes.InitScrollList(null);
			_scrollListFixModes.InitScrollList(array2);
		}
		else
		{
			_scrollListModes.InitScrollList(array2);
		}
		_wizardMaskMode.InitLightIcons(array, num2);
	}

	public void Start()
	{
		if (COMA_Server_Award.Instance.daylyBonus > 0)
		{
			AwardsNum = COMA_Server_Award.Instance.lst_awards.Count + COMA_Server_Award.Instance.lst_ranking_awards.Count + 1;
		}
		else
		{
			AwardsNum = COMA_Server_Award.Instance.lst_awards.Count + COMA_Server_Award.Instance.lst_ranking_awards.Count;
		}
		if (null != UI_PlayerInfo.Instance)
		{
			nameLabel.Text = UI_PlayerInfo.Instance.Nickname;
		}
		if (goldCom != null)
		{
			goldCom.Text = COMA_Pref.Instance.GetGold().ToString();
		}
		if (crystalCom != null)
		{
			crystalCom.Text = COMA_Pref.Instance.GetCrystal().ToString();
		}
		playerInfoCom.RefreshName(COMA_Pref.Instance.nickname);
		playerInfoCom.RefreshLevel(COMA_Pref.Instance.lv);
		float f = (float)COMA_Pref.Instance.exp / (float)COMA_Pref.Instance.expLv[COMA_Pref.Instance.lv - 1];
		playerInfoCom.RefreshExp(f);
		UI_GlobalData.Instance.CanGetACNum = COMA_Achievement.Instance.GetAcceptableCount();
		Debug.Log("Achievement acceptable : " + UI_GlobalData.Instance.CanGetACNum);
		if (UI_GlobalData.Instance.CanGetACNum == 0)
		{
			_canGetACNum.gameObject.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			_canGetACNum.gameObject.SetActive(true);
			_canGetACNum.Text = UI_GlobalData.Instance.CanGetACNum.ToString();
		}
		if (_wizardMask != null)
		{
			_wizardMask.GetComponent<UI_NewcomersWizardMask>().ProceFullScreenBtn += ProcessStartNW;
			if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 1)
			{
				_wizardMgr18.gameObject.SetActive(false);
				_wizardMask18.gameObject.SetActive(false);
				_wizardMask.SetActive(true);
			}
			else
			{
				_wizardMask.SetActive(false);
			}
		}
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 6 && _wizardMask6 != null)
		{
			_wizardMask6.gameObject.SetActive(true);
		}
		if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 0 && _wizardMask18 != null)
		{
			_wizardMask18.gameObject.SetActive(true);
		}
		if (_selfChara != null)
		{
			_selfChara.InitCharacter();
		}
		if (COMA_Pref.Instance.bNew_MainMenu && COMA_Pref.Instance.bNew_guidelinesId == 0)
		{
			if (_wizardMask18 != null)
			{
				_wizardMask18.gameObject.SetActive(true);
			}
			_playerPic.localPosition = new Vector3(0f, 0f, -300f);
		}
		else
		{
			if (_wizardMask18 != null)
			{
				_wizardMask18.gameObject.SetActive(false);
			}
			_playerPic.localPosition = new Vector3(0f, 0f, 0f);
		}
		if (_newGuideBoard != null)
		{
			_newGuideBoard.gameObject.SetActive(false);
		}
		RequireListCountInterval();
		if (COMA_CommonOperation.Instance.bfriendRequireInverval)
		{
			SceneTimerInstance.Instance.Add(10f, RequireListCountInterval);
		}
		if (COMA_VideoController.Instance.nVideo != 0)
		{
			if (COMA_VideoController.Instance.bRecorded)
			{
				recordNumObj.SetActive(true);
			}
			else
			{
				recordNumObj.SetActive(false);
			}
		}
		else
		{
			recordNumObj.transform.parent.gameObject.SetActive(false);
		}
		Resources.UnloadUnusedAssets();
	}

	public bool RequireListCountInterval()
	{
		COMA_Server_Friends.Instance.RequireList(COMA_Server_ID.Instance.GID, "count");
		return true;
	}

	public new void Update()
	{
		PendingFriendsNum = COMA_Server_Friends.Instance.requireCount;
	}

	private void NotifyDecorateBtnDown(TUIControl control)
	{
		UIMainMenu_BtnDecorate component = control.gameObject.GetComponent<UIMainMenu_BtnDecorate>();
		if (component != null)
		{
			component.BtnDown();
		}
	}

	private void NotifyDecorateBtnUp(TUIControl control)
	{
		UIMainMenu_BtnDecorate component = control.gameObject.GetComponent<UIMainMenu_BtnDecorate>();
		if (component != null)
		{
			component.BtnUp();
		}
	}

	public void HandleEventButtonArmory(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_EnterMarket);
			Debug.Log("HandleEventButtonMarket-CommandClick");
			if (_wizardMgr != null && COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 1)
			{
				Debug.Log("Reset market");
				_wizardMgr.GetComponent<UI_NewcomersWizardMgr>().ResetUIControllersZ();
				COMA_Sys.Instance.nTeachingId++;
			}
			if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 12)
			{
				_wizardMgr12.ResetUIControllersZ();
				_wizardMgr12.gameObject.SetActive(false);
				COMA_Sys.Instance.nTeachingId++;
			}
			if (_fadeMgr != null)
			{
				_fadeMgr.FadeOut();
			}
			_aniControl.PlayExitAni("UI.Market");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnScale(control);
			BtnOpenLight(control);
			break;
		case 2:
			BtnRestoreScale(control);
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButtonTrade(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonTrade-CommandClick");
			_aniControl.PlayExitAni("UI.Trade");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnScale(control);
			BtnOpenLight(control);
			break;
		case 2:
			BtnRestoreScale(control);
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButtonBag(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			if (COMA_Sys.Instance.bMemFirstGame && COMA_Sys.Instance.nTeachingId == 6)
			{
				_wizardMgr6.ResetUIControllersZ();
				_wizardMgr6.gameObject.SetActive(false);
				COMA_Sys.Instance.nTeachingId++;
			}
			_aniControl.PlayExitAni("UI.DoodleMgr");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			BtnScale(control);
			BtnOpenLight(control);
			break;
		case 2:
			BtnRestoreScale(control);
			BtnCloseLight(control);
			break;
		}
	}

	public void HandleEventButtonQuickEdit(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonQuickEdit-CommandClick");
			if (!_quickEditGroup.active)
			{
				_quickEditGroup.SetActive(true);
			}
			else
			{
				_quickEditGroup.GetComponent<UIMainMenu_QuickEdits>().Exit();
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

	public void HandleEventButtonQuickEditHead(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonQuickEditHead-CommandClick");
			_quickEditGroup.GetComponent<UIMainMenu_QuickEdits>().Exit();
			COMA_Scene_Inventroy.soonEditorID = 0;
			_aniControl.PlayExitAni("UI.Doodle1");
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

	public void HandleEventButtonQuickEditBody(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonQuickEditBody-CommandClick");
			_quickEditGroup.GetComponent<UIMainMenu_QuickEdits>().Exit();
			COMA_Scene_Inventroy.soonEditorID = 1;
			_aniControl.PlayExitAni("UI.Doodle1");
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

	public void HandleEventButtonQuickEditLeg(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonQuickEditLeg-CommandClick");
			_quickEditGroup.GetComponent<UIMainMenu_QuickEdits>().Exit();
			COMA_Scene_Inventroy.soonEditorID = 2;
			_aniControl.PlayExitAni("UI.Doodle1");
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

	public void HandleEventButtonOptions(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonOptions-CommandClick");
			Application.LoadLevel("UI.Options");
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

	public void HandleEventButtonNews(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonNews-CommandClick");
			TUI_MsgBox.Instance.MessageBox(1212);
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

	public void HandleEventButtonFriends(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonFriends-CommandClick");
			Application.LoadLevel("UI.Friends");
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

	public void HandleEventButtonRankList(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonRankList-CommandClick");
			Application.LoadLevel("UI.RankingList");
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

	public void HandleEventButtonAward(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("HandleEventButtonAward-CommandClick");
			GameObject gameObject = Object.Instantiate(Resources.Load("UI/Misc/Awards")) as GameObject;
			gameObject.transform.parent = base.TUIControls.transform;
			gameObject.transform.position = new Vector3(0f, 0f, -380f);
			UIAwards component = gameObject.GetComponent<UIAwards>();
			int num = 0;
			int num2 = COMA_Server_Award.Instance.lst_awards.Count + COMA_Server_Award.Instance.lst_ranking_awards.Count;
			int num3 = num2 / 3 + 1;
			UI_OneAwardData[][] datas = new UI_OneAwardData[num3][];
			for (int i = 0; i < num3; i++)
			{
				datas[i] = new UI_OneAwardData[3];
				for (int j = 0; j < 3; j++)
				{
					if (num < COMA_Server_Award.Instance.lst_awards.Count)
					{
						datas[i][j] = new UI_OneAwardData();
						datas[i][j].Award = COMA_Server_Award.Instance.lst_awards[num];
						num++;
					}
					else if (num - COMA_Server_Award.Instance.lst_awards.Count < COMA_Server_Award.Instance.lst_ranking_awards.Count)
					{
						datas[i][j] = new UI_OneAwardData();
						datas[i][j].Award = COMA_Server_Award.Instance.lst_ranking_awards[num - COMA_Server_Award.Instance.lst_awards.Count];
						num++;
					}
					else if (COMA_Server_Award.Instance.daylyBonus > 0)
					{
						datas[i][j] = new UI_OneAwardData();
						datas[i][j].Award.nAwardNum = COMA_Server_Award.Instance.daylyBonus;
						break;
					}
				}
			}
			component.InitList(ref datas);
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

	public void HandleEventButtonRecord(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonRecord-CommandClick");
			if (COMA_VideoController.Instance.bRecorded)
			{
				COMA_VideoController.Instance.bVideoPopView = true;
				COMA_VideoController.Instance.PlayLastRecording();
			}
			else
			{
				COMA_VideoController.Instance.bVideoPopView = true;
				COMA_VideoController.Instance.Show();
			}
			COMA_VideoController.Instance.bRecorded = false;
			recordNumObj.SetActive(false);
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

	public void HandleEventButtonAC(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("HandleEventButtonAC-CommandClick");
			Application.LoadLevel("UI.Achievement");
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

	private string GetGoldAndGemStr()
	{
		string text = COMA_Pref.Instance.GetGold().ToString();
		string text2 = COMA_Pref.Instance.GetCrystal().ToString();
		return text + "," + text2;
	}

	private string GetGameModeStrByIndex(int n, out int nMode)
	{
		string result = string.Empty;
		if (n == 1)
		{
			n = Random.Range(2, 9);
		}
		nMode = n;
		switch (n)
		{
		case 2:
			result = TUITextManager.Instance().GetString("jiemoshi_zhujiemian");
			break;
		case 3:
			result = TUITextManager.Instance().GetString("chuansongdaimoshi_zhujiemian");
			break;
		case 4:
			result = TUITextManager.Instance().GetString("duoqimoshi_zhujiemian");
			break;
		case 5:
			result = TUITextManager.Instance().GetString("jiangshiweichengmoshi_zhujiemian");
			break;
		case 6:
			result = TUITextManager.Instance().GetString("migong_zhujiemian");
			break;
		case 7:
			result = TUITextManager.Instance().GetString("paoku_zhujiemian");
			break;
		case 8:
			result = TUITextManager.Instance().GetString("paoku_zhujiemian");
			break;
		case 9:
			result = TUITextManager.Instance().GetString("paoku_zhujiemian");
			break;
		case 10:
			result = TUITextManager.Instance().GetString("paoku_zhujiemian");
			break;
		case 11:
			result = TUITextManager.Instance().GetString("paoku_zhujiemian");
			break;
		}
		return result;
	}

	private void HandleEnterDefand(string param)
	{
		DeductMoney(param);
		int nMode = 0;
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(5, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("5");
		WaitingBox();
	}

	public void HandleEventButton_Defand(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_Defand-CommandClick");
			if (!COMA_Sys.Instance.bMemFirstGame || COMA_Sys.Instance.nTeachingId == 0)
			{
			}
			string strTheme = TUITextManager.Instance().GetString("moshitanchukuang_desc6");
			int num = 4;
			Object.DestroyObject(msgBox);
			msgBox = MessageBox(strTheme, string.Empty, num + 1, gameRulesBoxPrefab, -120f, GetGoldAndGemStr());
			msgBox.AddProceYesHandler(HandleEnterDefand);
			UIGameRulesMsgBox uIGameRulesMsgBox = msgBox as UIGameRulesMsgBox;
			int[] money = new int[3] { 0, 100, 1000 };
			string[] tex2D = new string[3] { "prop_weapon5", "prop_weapon4", "prop_weapon3" };
			string[] des = new string[3] { "zhuangbeijiemian_weapon6", "zhuangbeijiemian_weapon3", "zhuangbeijiemian_weapon8" };
			uIGameRulesMsgBox.SetRuleBoxWeaponInfo(num, money, tex2D, des);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void HandleEnterDrivingband(string param)
	{
		DeductMoney(param);
		int nMode = 0;
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(3, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("1");
		WaitingBox();
	}

	public void HandleEventButton_Drivingband(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_Drivingband-CommandClick");
			if (!COMA_Sys.Instance.bMemFirstGame || COMA_Sys.Instance.nTeachingId == 0)
			{
			}
			string strTheme = TUITextManager.Instance().GetString("moshitanchukuang_desc8");
			int num = 2;
			Object.DestroyObject(msgBox);
			msgBox = MessageBox(strTheme, string.Empty, num + 1, gameRulesBoxPrefab, -120f, GetGoldAndGemStr());
			msgBox.AddProceYesHandler(HandleEnterDrivingband);
			UIGameRulesMsgBox uIGameRulesMsgBox = msgBox as UIGameRulesMsgBox;
			int[] money = new int[3] { 0, 200, -3 };
			string[] tex2D = new string[3] { "prop_gold", "prop_speed", "prop_spring" };
			string[] des = new string[3] { "zhuangbeijiemian_weapon9", "zhuangbeijiemian_weapon10", "zhuangbeijiemian_weapon15" };
			uIGameRulesMsgBox.SetRuleBoxWeaponInfo(num, money, tex2D, des);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void HandleEnterBlood(string param)
	{
		DeductMoney(param);
		int nMode = 0;
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(9, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("7");
		WaitingBox();
	}

	public void HandleEventButton_Blood(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_Fishing-CommandClick");
			string strTheme = TUITextManager.Instance().GetString("yewaiqiangzhan_desc02");
			int num = 8;
			Object.DestroyObject(msgBox);
			msgBox = MessageBox(strTheme, string.Empty, num + 1, gameRulesBoxPrefab, -120f, GetGoldAndGemStr());
			msgBox.AddProceYesHandler(HandleEnterBlood);
			UIGameRulesMsgBox uIGameRulesMsgBox = msgBox as UIGameRulesMsgBox;
			int[] money = new int[3] { 0, 500, -3 };
			string[] tex2D = new string[3] { "19", "21", "20" };
			string[] des = new string[3] { "qiangzhanjiemian_desc05", "qiangzhanjiemian_desc07", "qiangzhanjiemian_desc06" };
			uIGameRulesMsgBox.SetRuleBoxWeaponInfo(num, money, tex2D, des);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void HandleEnterTank(string param)
	{
		DeductMoney(param);
		int nMode = 0;
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(10, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("8");
		WaitingBox();
	}

	public void HandleEventButton_Tank(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_Tank-CommandClick");
			string strTheme = TUITextManager.Instance().GetString("tank_description");
			int num = 9;
			GameObject boxPrefab = Resources.Load("UI/Misc/GameRulesBoxTank") as GameObject;
			Object.DestroyObject(msgBox);
			msgBox = MessageBox(strTheme, string.Empty, num + 1, boxPrefab, -120f, GetGoldAndGemStr());
			msgBox.AddProceYesHandler(HandleEnterTank);
			UIGameRulesMsgBox uIGameRulesMsgBox = msgBox as UIGameRulesMsgBox;
			int[] money = new int[5] { 0, 1000, 1000, -3, -3 };
			string[] tex2D = new string[5] { "22", "24", "25", "23", "26" };
			string[] des = new string[5] { "tank_tank01_description", "tank_tank02_description", "tank_tank03_description", "tank_tank04_description", "tank_tank05_description" };
			uIGameRulesMsgBox.SetRuleBoxWeaponInfo(num, money, tex2D, des);
			TUIRect component = uIGameRulesMsgBox.transform.FindChild("weaponGroup/ShowRect").GetComponent<TUIRect>();
			component.currentCamera = GameObject.Find("TUICamera").GetComponent<Camera>();
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void HandleEnterBlackHouse(string param)
	{
		DeductMoney(param);
		int nMode = 0;
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(10, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("9");
		WaitingBox();
	}

	public void HandleEventButton_BlackHouse(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_Fishing-CommandClick");
			string strTheme = TUITextManager.Instance().GetString("yewaiqiangzhan_desc02");
			int num = 10;
			Object.DestroyObject(msgBox);
			msgBox = MessageBox(strTheme, string.Empty, num + 1, gameRulesBoxPrefab, -120f, GetGoldAndGemStr());
			msgBox.AddProceYesHandler(HandleEnterBlackHouse);
			UIGameRulesMsgBox uIGameRulesMsgBox = msgBox as UIGameRulesMsgBox;
			int[] money = new int[3];
			string[] tex2D = new string[3] { "19", "21", "20" };
			string[] des = new string[3] { "qiangzhanjiemian_desc05", "qiangzhanjiemian_desc07", "qiangzhanjiemian_desc06" };
			uIGameRulesMsgBox.SetRuleBoxWeaponInfo(num, money, tex2D, des);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void HandleEnterFlappy(string param)
	{
		DeductMoney(param);
		int nMode = 0;
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(11, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("10");
		WaitingBox();
	}

	public void HandleEventButton_Flappy(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			COMA_NetworkConnect.Instance.TryToEnterRoom("10");
			break;
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void HandleEnterFishing(string param)
	{
		DeductMoney(param);
		int nMode = 0;
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(8, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("6");
		WaitingBox();
	}

	public void HandleEventButton_Fishing(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_Fishing-CommandClick");
			if (!COMA_Sys.Instance.bMemFirstGame || COMA_Sys.Instance.nTeachingId == 0)
			{
			}
			string strTheme = TUITextManager.Instance().GetString("tanchukuang_desc1");
			int num = 7;
			Object.DestroyObject(msgBox);
			msgBox = MessageBox(strTheme, string.Empty, num + 1, gameRulesBoxPrefab, -120f, GetGoldAndGemStr());
			msgBox.AddProceYesHandler(HandleEnterFishing);
			UIGameRulesMsgBox uIGameRulesMsgBox = msgBox as UIGameRulesMsgBox;
			int[] money = new int[3] { 0, 2000, -25 };
			string[] tex2D = new string[3] { "prop_blackpole", "prop_silverpole", "prop_goldpole" };
			string[] des = new string[3] { "yuganjiemian1", "yuganjiemian2", "yuganjiemian3" };
			uIGameRulesMsgBox.InitTimeLimit(COMA_Fishing.Instance.GetPoleTime(0), COMA_Fishing.Instance.nTime0, 0);
			uIGameRulesMsgBox.InitTimeLimit(COMA_Fishing.Instance.GetPoleTime(1), COMA_Fishing.Instance.nTime1, 1);
			uIGameRulesMsgBox.SetRuleBoxWeaponInfo(num, money, tex2D, des);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void HandleEnterHunger(string param)
	{
		int nMode = 0;
		DeductMoney(param);
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(2, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("4");
		WaitingBox();
	}

	public void HandleEventButton_Hunger(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_Hunger-CommandClick");
			if (!COMA_Sys.Instance.bMemFirstGame || COMA_Sys.Instance.nTeachingId == 0)
			{
			}
			string strTheme = TUITextManager.Instance().GetString("moshitanchukuang_desc3");
			int num = 1;
			Object.DestroyObject(msgBox);
			msgBox = MessageBox(strTheme, string.Empty, num + 1, gameRulesBoxPrefab, -120f, GetGoldAndGemStr());
			msgBox.AddProceYesHandler(HandleEnterHunger);
			UIGameRulesMsgBox uIGameRulesMsgBox = msgBox as UIGameRulesMsgBox;
			int[] money = new int[3] { 0, 200, -3 };
			string[] tex2D = new string[3] { "prop_meat", "prop_hide", "prop_speed" };
			string[] des = new string[3] { "zhuangbeijiemian_weapon12", "zhuangbeijiemian_weapon13", "zhuangbeijiemian_weapon14" };
			uIGameRulesMsgBox.SetRuleBoxWeaponInfo(num, money, tex2D, des);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void HandleEnterRandom(string param)
	{
		int nMode = 0;
		DeductMoney(param);
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(7, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("0");
		WaitingBox();
	}

	public void HandleEventButton_Random(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_Random-CommandClick");
			if (!COMA_Sys.Instance.bMemFirstGame || COMA_Sys.Instance.nTeachingId == 0)
			{
			}
			string strTheme = TUITextManager.Instance().GetString("moshitanchukuang_desc15");
			int num = 6;
			Object.DestroyObject(msgBox);
			msgBox = MessageBox(strTheme, string.Empty, num + 1, gameRulesBoxPrefab, -120f, GetGoldAndGemStr());
			msgBox.AddProceYesHandler(HandleEnterRandom);
			UIGameRulesMsgBox uIGameRulesMsgBox = msgBox as UIGameRulesMsgBox;
			int[] money = new int[3] { 0, 300, -3 };
			string[] tex2D = new string[3] { "prop_gold", "prop_random", "prop_speed" };
			string[] des = new string[3] { "zhuangbeijiemian_weapon9", "zhuangbeijiemian_weapon16", "zhuangbeijiemian_weapon17" };
			uIGameRulesMsgBox.SetRuleBoxWeaponInfo(num, money, tex2D, des);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			Debug.Log("Button_Random-CommandDown");
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			Debug.Log("Button_Random-CommandUp");
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void HandleEnterTakeFlages(string param)
	{
		int nMode = 0;
		DeductMoney(param);
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(4, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("2");
		WaitingBox();
	}

	public void HandleEventButton_TakeFlages(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_TakeFlages-CommandClick");
			if (!COMA_Sys.Instance.bMemFirstGame || COMA_Sys.Instance.nTeachingId == 0)
			{
			}
			string strTheme = TUITextManager.Instance().GetString("moshitanchukuang_desc5");
			int num = 3;
			Object.DestroyObject(msgBox);
			msgBox = MessageBox(strTheme, string.Empty, num + 1, gameRulesBoxPrefab, -120f, GetGoldAndGemStr());
			msgBox.AddProceYesHandler(HandleEnterTakeFlages);
			UIGameRulesMsgBox uIGameRulesMsgBox = msgBox as UIGameRulesMsgBox;
			int[] money = new int[3] { 0, 200, -3 };
			string[] tex2D = new string[3] { "3", "1", "prop_weapon3" };
			string[] des = new string[3] { "zhuangbeijiemian_weapon7", "zhuangbeijiemian_weapon2", "zhuangbeijiemian_weapon8" };
			uIGameRulesMsgBox.SetRuleBoxWeaponInfo(num, money, tex2D, des);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	private void HandleEnterTreasure(string param)
	{
		DeductMoney(param);
		int nMode = 0;
		UI_GameMode.Instance.GameMode = GetGameModeStrByIndex(6, out nMode);
		COMA_NetworkConnect.Instance.TryToEnterRoom("3");
		WaitingBox();
	}

	public void HandleEventButton_Treasure(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			Debug.Log("Button_Treasure-CommandClick");
			if (!COMA_Sys.Instance.bMemFirstGame || COMA_Sys.Instance.nTeachingId == 0)
			{
			}
			string strTheme = TUITextManager.Instance().GetString("moshitanchukuang_desc9");
			int num = 5;
			Object.DestroyObject(msgBox);
			msgBox = MessageBox(strTheme, string.Empty, num + 1, gameRulesBoxPrefab, -120f, GetGoldAndGemStr());
			msgBox.AddProceYesHandler(HandleEnterTreasure);
			UIGameRulesMsgBox uIGameRulesMsgBox = msgBox as UIGameRulesMsgBox;
			int[] money = new int[3] { 0, 200, -3 };
			string[] tex2D = new string[3] { "prop_gold", "prop_speed", "prop_perception" };
			string[] des = new string[3] { "zhuangbeijiemian_weapon9", "zhuangbeijiemian_weapon10", "zhuangbeijiemian_weapon11" };
			uIGameRulesMsgBox.SetRuleBoxWeaponInfo(num, money, tex2D, des);
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_SelectMode);
			NotifyDecorateBtnDown(control);
			BtnOpenLight(control);
			BtnScale(control);
			break;
		case 2:
			NotifyDecorateBtnUp(control);
			BtnCloseLight(control);
			BtnRestoreScale(control);
			break;
		}
	}

	public void HandleEventButton_IapEntry(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
			Debug.Log("Button_IapEntry-CommandClick");
			EnterIAPUI("UI.MainMenu", _aniControl);
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

	public void HandleEventButton_Back(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("Button_Back-CommandClick");
		}
	}

	public void HandleEventButton_PlayerRotate(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		float y = (0f - wparam) * COMA_Sys.Instance.sensitivity * 314f * 2f / (float)Screen.width;
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		playerOnShowTrs.rotation *= quaternion;
	}

	private void DeductMoney(string param)
	{
		Debug.Log("扣钱：" + param);
		string[] array = param.Split(',');
		if (array.Length > 1)
		{
			int num = int.Parse(array[0]);
			int num2 = int.Parse(array[1]);
			switch (num)
			{
			case 1:
				COMA_CommonOperation.Instance.selectedWeaponPrice = num2;
				break;
			case 2:
				COMA_CommonOperation.Instance.selectedWeaponPrice = -num2;
				break;
			}
		}
	}

	private void ProcessStartNW()
	{
		if (!_bEnterEnd)
		{
			Debug.Log("----ProcessStartNW");
			return;
		}
		if (_wizardMask != null)
		{
			_wizardMask.SetActive(false);
		}
		if (_wizardMask2 != null)
		{
			_wizardMask2.SetActive(true);
		}
		if (_wizardMgr != null)
		{
			_wizardMgr.SetActive(true);
			_wizardMgr.GetComponent<UI_NewcomersWizardMgr>().RefreshUIControllers();
		}
	}

	private void WaitingBox()
	{
		GameObject gameObject = Resources.Load("UI/Misc/IAPBox") as GameObject;
		Debug.Log("------waitingBoxPerfab" + gameObject.name);
		UI_MsgBox uI_MsgBox = MessageBox("Loading...", string.Empty, 0, gameObject, -300f);
		_waitingBox = uI_MsgBox.gameObject;
		SceneTimerInstance.Instance.Add(10f, DestoryWaitingBox);
	}

	public bool DestoryWaitingBox()
	{
		COMA_NetworkConnect.Instance.CancelToEnterRoom();
		if (_waitingBox != null)
		{
			Debug.Log("--DestroyImmediate");
			Object.Destroy(_waitingBox);
			_waitingBox = null;
		}
		UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(102);
		uI_MsgBox.AddProceYesHandler(OKToReconnect);
		return false;
	}

	public void OKToReconnect(string param)
	{
		COMA_Login.Instance.ReconnectGameServer();
	}
}
