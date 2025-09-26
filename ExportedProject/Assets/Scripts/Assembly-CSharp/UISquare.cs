using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Hall.C2S;
using Protocol.Mail.C2S;
using Protocol.Role;
using UIGlobal;
using UnityEngine;

public class UISquare : UIEntity
{
	[SerializeField]
	private GameObject _btn_OpenMiscContent;

	[SerializeField]
	private GameObject _uiMiscContent;

	private List<UISquare_ChatRecordBoxData> _lstChatRecordBoxData = new List<UISquare_ChatRecordBoxData>();

	[SerializeField]
	private GameObject _uiMisc;

	[SerializeField]
	private GameObject _btnEnterChat;

	[SerializeField]
	private GameObject _panelChat;

	[SerializeField]
	private GameObject _contentChatMobile;

	[SerializeField]
	private GameObject _contentFriends;

	[SerializeField]
	private GameObject _uiJoyStick;

	[SerializeField]
	private GameObject _uiRateBtn;

	[SerializeField]
	private GameObject _uiMapBtn;

	[SerializeField]
	private GameObject _uiMailBtn;

	[SerializeField]
	private GameObject _uiAchieBtn;

	[SerializeField]
	private GameObject _uiRankingBtn;

	[SerializeField]
	private GameObject _uiVideoBtn;

	[SerializeField]
	private GameObject _uiBottomBtns;

	[SerializeField]
	private GameObject _uiPopBtns;

	[SerializeField]
	private GameObject _uiFriendsBtns;

	[SerializeField]
	private UIChatInputMgr inputMgr;

	[SerializeField]
	private bool _bNeedFetchMail;

	public List<UISquare_ChatRecordBoxData> LstChatRecordBoxData
	{
		get
		{
			return _lstChatRecordBoxData;
		}
	}

	protected override void Load()
	{
		if (Time.timeScale > 1f)
		{
			Time.timeScale = 1f;
		}
		RegisterMessage(EUIMessageID.UISquare_OpenMiscContentButtonOnClick, this, OpenMiscContent);
		RegisterMessage(EUIMessageID.UISquare_CloseMiscContentButtonOnClick, this, CloseMiscContent);
		RegisterMessage(EUIMessageID.UISquare_SpawnNewChatRecord, this, SpawnNewChatRecord);
		RegisterMessage(EUIMessageID.UISquare_EnterChat, this, EnterChat);
		RegisterMessage(EUIMessageID.UISquare_CloseChat, this, CloseChat);
		RegisterMessage(EUIMessageID.UISquare_OpenMobileChat, this, OpenMobileChat);
		RegisterMessage(EUIMessageID.UISquare_CloseMobileChat, this, CloseMobileChat);
		RegisterMessage(EUIMessageID.UISquare_SelPrivateChatObject, this, SelPrivateChatObject);
		RegisterMessage(EUIMessageID.UIBackpack_GotoMarketClick, this, GotoMarketClick);
		RegisterMessage(EUIMessageID.UIMarket_GotoBackpackClick, this, GotoBackpackOnClick);
		RegisterMessage(EUIMessageID.UISquare_GoAchievement, this, GoAchievement);
		RegisterMessage(EUIMessageID.UISquare_GotoRankings, this, GotoRankings);
		RegisterMessage(EUIMessageID.UISquare_GotoRankings_RPG, this, GotoRankings_RPG);
		RegisterMessage(EUIMessageID.UISquare_GotoSetting, this, GotoSetting);
		RegisterMessage(EUIMessageID.UISquare_GotoMails, this, GotoMails);
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
		RegisterMessage(EUIMessageID.UISquare_ChatChannelChanged, this, OnChatChannelChanged);
		RegisterMessage(EUIMessageID.UISquare_RefreshChatHistory, this, OnRefreshChatHistory);
		RegisterMessage(EUIMessageID.UISquare_GotoCardMgrClick, this, GotoCardMgrClick);
		RegisterMessage(EUIMessageID.UISquare_GotoRpgBagClick, this, GotoRpgBagClick);
		RegisterMessage(EUIMessageID.UISquare_GotoCompoundGemClick, this, GotoCompoundGemClick);
		RegisterMessage(EUIMessageID.UISquare_GotoTeamMgrClick, this, GotoTeamMgrClick);
		RegisterMessage(EUIMessageID.UISquare_GotoCompoundCardClick, this, GotoCompoundCardClick);
		RegisterMessage(EUIMessageID.UISquare_GotoStrengthenAvatarClick, this, GotoStrengthenAvatarClick);
		RegisterMessage(EUIMessageID.UISquare_GotoMap, this, GotoMapClick);
		_bNeedFetchMail = true;
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UISquare_OpenMiscContentButtonOnClick, this);
		UnregisterMessage(EUIMessageID.UISquare_CloseMiscContentButtonOnClick, this);
		UnregisterMessage(EUIMessageID.UISquare_SpawnNewChatRecord, this);
		UnregisterMessage(EUIMessageID.UISquare_EnterChat, this);
		UnregisterMessage(EUIMessageID.UISquare_CloseChat, this);
		UnregisterMessage(EUIMessageID.UISquare_OpenMobileChat, this);
		UnregisterMessage(EUIMessageID.UISquare_CloseMobileChat, this);
		UnregisterMessage(EUIMessageID.UISquare_SelPrivateChatObject, this);
		UnregisterMessage(EUIMessageID.UIBackpack_GotoMarketClick, this);
		UnregisterMessage(EUIMessageID.UIMarket_GotoBackpackClick, this);
		UnregisterMessage(EUIMessageID.UISquare_GoAchievement, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoRankings, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoRankings_RPG, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoSetting, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoMails, this);
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UISquare_ChatChannelChanged, this);
		UnregisterMessage(EUIMessageID.UISquare_RefreshChatHistory, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoCardMgrClick, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoRpgBagClick, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoCompoundGemClick, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoTeamMgrClick, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoCompoundCardClick, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoStrengthenAvatarClick, this);
		UnregisterMessage(EUIMessageID.UISquare_GotoMap, this);
	}

	private bool OpenMiscContent(TUITelegram msg)
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Bar_Drawout);
		_uiMiscContent.SetActive(true);
		_btn_OpenMiscContent.SetActive(false);
		return true;
	}

	private bool CloseMiscContent(TUITelegram msg)
	{
		Debug.Log("CloseMiscContent");
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Bar_Drawback);
		_uiMiscContent.GetComponent<Animation>().Play("UISquare_BottomEnter_Exit");
		return true;
	}

	private bool SpawnNewChatRecord(TUITelegram msg)
	{
		NotifyChatCmd notifyChatCmd = msg._pExtraInfo as NotifyChatCmd;
		Debug.Log("Channel:" + notifyChatCmd.m_channel + ";  ID:" + notifyChatCmd.m_sender_id + "; " + notifyChatCmd.m_sender_name + ":" + notifyChatCmd.m_content);
		Debug.Log("MC_UIToolKit.UIGolbalStaticFun.GetSelfTID()=" + UIGolbalStaticFun.GetSelfTID());
		Channel channel = (Channel)notifyChatCmd.m_channel;
		bool flag = ((notifyChatCmd.m_sender_id == UIGolbalStaticFun.GetSelfTID()) ? true : false);
		UISquare_ChatRecordBoxData uISquare_ChatRecordBoxData = new UISquare_ChatRecordBoxData(notifyChatCmd.m_sender_id, flag, notifyChatCmd.m_sender_name, notifyChatCmd.m_content, channel);
		if (flag && channel == Protocol.Channel.person)
		{
			uISquare_ChatRecordBoxData.PrivatePeopleName = msg._pExtraInfo2 as string;
		}
		if (_lstChatRecordBoxData.Count >= 50)
		{
			_lstChatRecordBoxData.RemoveAt(0);
		}
		_lstChatRecordBoxData.Add(uISquare_ChatRecordBoxData);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_ChatHistoryChanged, null, _lstChatRecordBoxData);
		return true;
	}

	private bool EnterChat(TUITelegram msg)
	{
		inputMgr.GetComponent<BoxCollider>().enabled = true;
		_uiRateBtn.SetActive(false);
		_uiJoyStick.SetActive(false);
		_btnEnterChat.SetActive(false);
		_uiMapBtn.SetActive(false);
		_uiMailBtn.SetActive(false);
		_uiAchieBtn.SetActive(false);
		_uiRankingBtn.SetActive(false);
		_uiVideoBtn.SetActive(false);
		_uiBottomBtns.SetActive(false);
		_uiFriendsBtns.SetActive(false);
		if (_uiPopBtns != null)
		{
			_uiPopBtns.SetActive(false);
		}
		_panelChat.SetActive(true);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_ChatHistoryChanged, null, _lstChatRecordBoxData, 0, false);
		return true;
	}

	private bool CloseChat(TUITelegram msg)
	{
		inputMgr.GetComponent<BoxCollider>().enabled = false;
		_uiRateBtn.SetActive(COMA_Pref.Instance.BShowRateButton());
		_uiJoyStick.SetActive(true);
		_uiMapBtn.SetActive(true);
		_uiMailBtn.SetActive(true);
		_uiAchieBtn.SetActive(true);
		_uiRankingBtn.SetActive(true);
		_uiVideoBtn.SetActive(true);
		_uiBottomBtns.SetActive(true);
		_uiFriendsBtns.SetActive(true);
		_btnEnterChat.SetActive(true);
		_panelChat.SetActive(false);
		return true;
	}

	private bool OpenMobileChat(TUITelegram msg)
	{
		_contentChatMobile.SetActive(true);
		_panelChat.SetActive(false);
		return true;
	}

	private bool CloseMobileChat(TUITelegram msg)
	{
		Debug.Log("CloseMobileChat");
		_contentChatMobile.SetActive(false);
		_panelChat.SetActive(true);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UISquare_ChatHistoryChanged, null, _lstChatRecordBoxData, 0, false);
		return true;
	}

	private bool SelPrivateChatObject(TUITelegram msg)
	{
		inputMgr.OnPrivateChatPopup();
		return true;
	}

	private bool GotoMarketScene(object obj)
	{
		Debug.Log("GotoMarketScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Market", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoMarketClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoMarketScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoBackpackScene(object obj)
	{
		Debug.Log("GotoBackpackScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Backpack", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoBackpackOnClick(TUITelegram msg)
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Backpack);
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoBackpackScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GoAchievementScene(object obj)
	{
		Debug.Log("GoAchievementScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Achievement", ELoadLevelParam.LoadOnly);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		return true;
	}

	private bool GoAchievement(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GoAchievementScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoRankingsScene(object obj)
	{
		Debug.Log("GotoRankingsScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Rankings", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoRankings(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoRankingsScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoRankingsScene_RPG(object obj)
	{
		Debug.Log("GotoRankingsScene_RPG");
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.RPG.Ranking", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoRankings_RPG(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoRankingsScene_RPG);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoSettingScene(object obj)
	{
		Debug.Log("GotoSettingScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Options", ELoadLevelParam.LoadOnly);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		return true;
	}

	private bool GotoSetting(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoSettingScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoMailsScene(object obj)
	{
		Debug.Log("GotoMailsScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Mails", ELoadLevelParam.AddHidePre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		return true;
	}

	private bool GotoMails(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoMailsScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "RateApp":
			UIOptions.Rate();
			COMA_Pref.Instance.SetRated();
			_uiRateBtn.SetActive(COMA_Pref.Instance.BShowRateButton());
			break;
		case "ChangeSquare":
		{
			Debug.Log("OnChangeSquare");
			EnterHallCmd extraInfo = new EnterHallCmd();
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
			break;
		}
		case "LoginFacebook":
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIRankings_LogFacebook, null, null);
			break;
		}
		return true;
	}

	private bool OnChatChannelChanged(TUITelegram msg)
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_ChatHistoryChanged, null, _lstChatRecordBoxData, 0);
		return true;
	}

	private bool OnRefreshChatHistory(TUITelegram msg)
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_RealRefreshChatHistory, null, _lstChatRecordBoxData, 0);
		return true;
	}

	private bool GotoCardMgrScene(object obj)
	{
		Debug.Log("GotoCardMgrScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.RPG.CardManage", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoCardMgrClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoCardMgrScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoRpgBagScene(object obj)
	{
		Debug.Log("GotoRpgBagScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.RPG.Backpack", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoRpgBagClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoRpgBagScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoCompoundGem(object obj)
	{
		Debug.Log("GotoCompoundGem");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.RPG.GenCombin", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoCompoundGemClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoCompoundGem);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoTeamMgr(object obj)
	{
		Debug.Log("GotoTeamMgr");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.RPG.MyTerm", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoTeamMgrClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoTeamMgr);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoCompoundCard(object obj)
	{
		Debug.Log("GotoCompoundCard");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.RPG.CardCombin", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoCompoundCardClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoCompoundCard);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoStrengthenAvatar(object obj)
	{
		Debug.Log("GotoStrengthenAvatar");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.RPG.AvatarEnhance", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoStrengthenAvatarClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoStrengthenAvatar);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private bool GotoMap(object obj)
	{
		Debug.Log("GotoMap");
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.RPG.Map", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoMapClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		UIDataBufferCenter.Instance.PreSceneName = "UI.Square";
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoMap);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private void Awake()
	{
		_uiRateBtn.SetActive(COMA_Pref.Instance.BShowRateButton());
	}

	protected override void Tick()
	{
		if (_bNeedFetchMail)
		{
			FetchMail();
			UpdateRoleLevel();
			_bNeedFetchMail = false;
		}
	}

	private void Start()
	{
		COMA_AudioManager.Instance.MusicPlay(AudioCategory.BGM_Castle01);
		if (COMA_Pref.Instance.NG2_1_FirstEnterSquare)
		{
			Debug.Log("--------------------Hide Square Players!");
			if (COMA_Scene_PlayerController.Instance != null)
			{
				COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
			}
		}
		else if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(true);
		}
		if (COMA_CommonOperation.Instance.bNeedFBFeed)
		{
			COMA_CommonOperation.Instance.bNeedFBFeed = false;
			UIFacebookFeedback.Instance.PublishAd();
		}
	}

	private void FetchMail()
	{
		Debug.Log("----------------------------------FetchMail");
		DragMailListCmd extraInfo = new DragMailListCmd();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, extraInfo);
	}

	private void UpdateRoleLevel()
	{
		int rpg_level = (int)UIDataBufferCenter.Instance.RPGData.m_rpg_level;
		float num = 0f;
		for (int i = 0; i < RPGGlobalData.Instance.LstRPGMaxExp.Count; i++)
		{
			RPGMaxExp rPGMaxExp = RPGGlobalData.Instance.LstRPGMaxExp[i];
			if (rpg_level < rPGMaxExp._lv_max)
			{
				num = rPGMaxExp._exp;
				break;
			}
		}
		Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%lv=" + UIDataBufferCenter.Instance.RPGData.m_rpg_lv_exp + "  max=" + num);
		float num2 = (float)UIDataBufferCenter.Instance.RPGData.m_rpg_lv_exp / num;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_RoleExpUpdate, null, rpg_level, num2);
	}
}
