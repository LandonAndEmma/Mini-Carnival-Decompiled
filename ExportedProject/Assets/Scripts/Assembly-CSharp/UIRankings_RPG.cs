using MC_UIToolKit;
using MessageID;
using NGUI_COMUI;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using UIGlobal;
using UnityEngine;

public class UIRankings_RPG : UIEntity
{
	[SerializeField]
	private UIRankings_Container_PRG _uiRPGContainer;

	[SerializeField]
	private UILabel _labelSelfRank;

	[SerializeField]
	private UILabel _labelSelfName;

	[SerializeField]
	private UILabel _labelSelfScore;

	private bool _bEnterWorldPage;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRankings_GotoSquare, this, GotoSquare);
		RegisterMessage(EUIMessageID.UIRPG_DragMedalRankResult, this, DragMedalRankResult);
		RegisterMessage(EUIMessageID.UIRPGRanking_GotoSquare, this, GotoSquareClick);
		_bEnterWorldPage = false;
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRankings_GotoSquare, this);
		UnregisterMessage(EUIMessageID.UIRPG_DragMedalRankResult, this);
		UnregisterMessage(EUIMessageID.UIRPGRanking_GotoSquare, this);
		if (UIDataBufferCenter.Instance != null)
		{
			UIDataBufferCenter.Instance.EnableToFriendTab = false;
		}
		_bEnterWorldPage = false;
	}

	private void RefreshRPGContainer_World()
	{
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		DragMedalRankCmd extraInfo = new DragMedalRankCmd();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, extraInfo);
	}

	private bool DragMedalRankResult(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		DragMedalRankResultCmd dragMedalRankResultCmd = (DragMedalRankResultCmd)msg._pExtraInfo;
		int num = ((dragMedalRankResultCmd != null) ? dragMedalRankResultCmd.m_list.Count : 0);
		_uiRPGContainer.ClearContainer();
		_uiRPGContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		_uiRPGContainer.InitBoxs(num, true);
		for (int i = 0; i < num; i++)
		{
			UIRankings_RPGBoxData data = new UIRankings_RPGBoxData();
			data.PlayerID = dragMedalRankResultCmd.m_list[i].m_player_id;
			data.PlayerScore = dragMedalRankResultCmd.m_list[i].m_medal;
			data.Rank = i;
			_uiRPGContainer.SetBoxData(i, data);
			UIDataBufferCenter.Instance.FetchPlayerRPGData(data.PlayerID, delegate(PlayerRpgDataCmd rpgInfo)
			{
				data.LV = (int)rpgInfo.m_rpg_level;
				data.SetDirty();
			});
			UIDataBufferCenter.Instance.FetchPlayerProfile(data.PlayerID, delegate(WatchRoleInfo playerInfo)
			{
				if (playerInfo != null)
				{
					data.watchInfo = playerInfo;
					data.PlayerName = playerInfo.m_name;
					Debug.Log("Name:" + playerInfo.m_name);
					data.SetDirty();
					UIDataBufferCenter.Instance.FetchFacebookIconByTID(playerInfo.m_player_id, delegate(Texture2D tex)
					{
						data.Tex = tex;
						data.SetDirty();
					});
				}
			});
		}
		return true;
	}

	private bool GotoSquareScene(object obj)
	{
		Debug.Log("GotoSquareScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(true);
		}
		return true;
	}

	private bool GotoSquare(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockForTUIMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoSquareScene);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}

	private void Awake()
	{
	}

	private void Start()
	{
		RefreshRPGContainer_World();
	}

	private void LimitContainerMove(int nBoxCount, int nBoxLimit, NGUI_COMUI.UI_Container container)
	{
		if (nBoxCount > nBoxLimit)
		{
			container.SetMoveForce(new Vector3(0f, 1f, 0f));
		}
		else
		{
			container.SetMoveForce(Vector3.zero);
		}
	}

	protected override void Tick()
	{
	}

	private bool GotoSquare(object obj)
	{
		Debug.Log("GotoSquare");
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.LoadOnlyAnDestroyPre);
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_LoadLevel, null, extraInfo);
		if (COMA_Scene_PlayerController.Instance != null)
		{
			COMA_Scene_PlayerController.Instance.gameObject.SetActive(false);
		}
		return true;
	}

	private bool GotoSquareClick(TUITelegram msg)
	{
		UIGolbalStaticFun.PopBlockOnlyMessageBox();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SetASAniEvent, this, GotoSquare);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenExitASAni, this, null);
		return true;
	}
}
