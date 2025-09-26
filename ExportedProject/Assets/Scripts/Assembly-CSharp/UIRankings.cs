using System;
using System.Collections.Generic;
using LitJson;
using MC_UIToolKit;
using MessageID;
using MiscToolKits;
using NGUI_COMUI;
using Protocol;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using Protocol.Rank.C2S;
using Protocol.Rank.S2C;
using Protocol.Role;
using Protocol.Role.C2S;
using Protocol.Role.S2C;
using UIGlobal;
using UnityEngine;

public class UIRankings : UIEntity
{
	public enum EGameModeType
	{
		Mode_Run = 0,
		Mode_Rocket = 1,
		Mode_Flag = 2,
		Mode_Maze = 3,
		Mode_Hunger = 4,
		Mode_Castle = 5,
		Mode_Fishing = 6,
		Mode_Blood = 7,
		Mode_Tank = 8,
		Mode_BlackHouse = 9,
		Mode_Flappy = 10,
		Count = 11,
		Mode_RPG = 12
	}

	[SerializeField]
	private GameObject _colliderMask;

	private bool _bHasSendLogRequest;

	[SerializeField]
	private GameObject _pageWorld;

	[SerializeField]
	private GameObject _pageFriend;

	[SerializeField]
	private GameObject _pageSearchFriend;

	[SerializeField]
	private GameObject _pageSearchFriendRefresh;

	[SerializeField]
	private GameObject _uiCaption;

	[SerializeField]
	private GameObject _extraPageWorld;

	[SerializeField]
	private GameObject _extraPageFriend;

	[SerializeField]
	private UIRankings_WorldContainer _uiWorldContainer;

	[SerializeField]
	private UIRankings_FriendContainer _uiFriendContainer;

	[SerializeField]
	private UIRankings_SearchFriendsContainer _uiSearchFriendContainer;

	[SerializeField]
	private UILabel _labelSelfRank;

	[SerializeField]
	private UILabel _labelSelfName;

	[SerializeField]
	private UILabel _labelSelfScore;

	[SerializeField]
	private UILabel _labelWorldTimeLeft;

	[SerializeField]
	private UILabel _labelFriendTimeLeft;

	[SerializeField]
	private UILabel _labelFriendID;

	[SerializeField]
	private UILabel _labelFriendFriends;

	[SerializeField]
	private UILabel _labelSearchFriends;

	private bool _bIsNeedOpenFriendPageOnEnable;

	private bool _bEnterWorldPage;

	[SerializeField]
	private GameObject[] _leftTimeInfos;

	private UIRankings_ButtonDelFriend _com;

	private byte pageSize = 20;

	private uint pageIndex = 1u;

	private uint pageCount = 1u;

	private string tarNickName = string.Empty;

	private int curNum;

	[SerializeField]
	private GameObject _addFriendsPanel;

	protected override void Load()
	{
		_bHasSendLogRequest = false;
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
		RegisterMessage(EUIMessageID.UICOMBox_NoClick, this, OnPopBoxClick_No);
		RegisterMessage(EUIMessageID.UIRankings_CaptionTypeSwitched, this, CaptionTypeSwitched);
		RegisterMessage(EUIMessageID.UIRankings_WorldGameModeSelChanged, this, WorldGameModeSelChanged);
		RegisterMessage(EUIMessageID.UIRankings_FriendGameModeSelChanged, this, FriendGameModeSelChanged);
		RegisterMessage(EUIMessageID.UIRankings_OpenAddFriendPanel, this, OpenAddFriendPanel);
		RegisterMessage(EUIMessageID.UIRankings_CloseAddFriendPanel, this, CloseAddFriendPanel);
		RegisterMessage(EUIMessageID.UIRankings_GetSelfScoreFinish, this, GetSelfScore);
		RegisterMessage(EUIMessageID.UIRankings_WorldRankListFinish, this, GetWorldRankList);
		RegisterMessage(EUIMessageID.UIRankings_FriendRankListFinish, this, GetFriendRankList);
		RegisterMessage(EUIMessageID.UI_InputNameEnd_Search, this, InputNameEnd);
		RegisterMessage(EUIMessageID.UIRankings_SearchFriendFinish, this, OnSearchFriend);
		RegisterMessage(EUIMessageID.UIRankings_SearchFriendRefresh, this, OnSearchFriendRefresh);
		RegisterMessage(EUIMessageID.UIRankings_SearchFriendAdd, this, OnSearchFriendAdd);
		RegisterMessage(EUIMessageID.UIRankings_FriendDel, this, OnFriendDelete);
		RegisterMessage(EUIMessageID.UIRankings_GotoSquare, this, GotoSquare);
		RegisterMessage(EUIMessageID.UIRPG_DragFriendsMedalRankResult, this, DragFriendsMedalRankResult);
		_bIsNeedOpenFriendPageOnEnable = UIDataBufferCenter.Instance.EnableToFriendTab;
		_bEnterWorldPage = false;
	}

	protected override void UnLoad()
	{
		_bHasSendLogRequest = false;
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UICOMBox_NoClick, this);
		UnregisterMessage(EUIMessageID.UIRankings_CaptionTypeSwitched, this);
		UnregisterMessage(EUIMessageID.UIRankings_WorldGameModeSelChanged, this);
		UnregisterMessage(EUIMessageID.UIRankings_FriendGameModeSelChanged, this);
		UnregisterMessage(EUIMessageID.UIRankings_OpenAddFriendPanel, this);
		UnregisterMessage(EUIMessageID.UIRankings_CloseAddFriendPanel, this);
		UnregisterMessage(EUIMessageID.UIRankings_GetSelfScoreFinish, this);
		UnregisterMessage(EUIMessageID.UIRankings_WorldRankListFinish, this);
		UnregisterMessage(EUIMessageID.UIRankings_FriendRankListFinish, this);
		UnregisterMessage(EUIMessageID.UI_InputNameEnd_Search, this);
		UnregisterMessage(EUIMessageID.UIRankings_SearchFriendFinish, this);
		UnregisterMessage(EUIMessageID.UIRankings_SearchFriendRefresh, this);
		UnregisterMessage(EUIMessageID.UIRankings_SearchFriendAdd, this);
		UnregisterMessage(EUIMessageID.UIRankings_FriendDel, this);
		UnregisterMessage(EUIMessageID.UIRankings_GotoSquare, this);
		UnregisterMessage(EUIMessageID.UIRPG_DragFriendsMedalRankResult, this);
		if (UIDataBufferCenter.Instance != null)
		{
			UIDataBufferCenter.Instance.EnableToFriendTab = false;
		}
		_bEnterWorldPage = false;
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "DelFriend":
		{
			OnFriendDelete();
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			int count = notifyRoleDataCmd.m_friend_list.Count;
			LimitContainerMove(count, 5, _uiFriendContainer);
			break;
		}
		}
		return true;
	}

	private bool OnPopBoxClick_No(TUITelegram msg)
	{
		return true;
	}

	private void OpenPageWorld()
	{
		if (_pageWorld != null)
		{
			_pageWorld.SetActive(true);
		}
		if (_pageFriend != null)
		{
			_pageFriend.SetActive(false);
		}
		if (_uiCaption != null)
		{
			_uiCaption.SetActive(true);
		}
		if (_pageSearchFriend != null)
		{
			_pageSearchFriend.SetActive(false);
		}
	}

	private void OpenPageFriend()
	{
		if (_pageWorld != null)
		{
			_pageWorld.SetActive(false);
		}
		if (_pageFriend != null)
		{
			_pageFriend.SetActive(true);
		}
		if (_uiCaption != null)
		{
			_uiCaption.SetActive(true);
		}
		if (_pageSearchFriend != null)
		{
			_pageSearchFriend.SetActive(false);
		}
		Debug.Log("OpenPageFriend");
	}

	private bool CaptionTypeSwitched(TUITelegram msg)
	{
		switch ((UIRankings_ButtonCaptionType.ECaptionType)(int)msg._pExtraInfo)
		{
		case UIRankings_ButtonCaptionType.ECaptionType.World_Unsel:
			OpenPageWorld();
			break;
		case UIRankings_ButtonCaptionType.ECaptionType.Friend_Unsel:
			OpenPageFriend();
			break;
		case UIRankings_ButtonCaptionType.ECaptionType.SearchFriend:
			if (_pageWorld != null)
			{
				_pageWorld.SetActive(false);
			}
			if (_pageFriend != null)
			{
				_pageFriend.SetActive(false);
			}
			if (_uiCaption != null)
			{
				_uiCaption.SetActive(false);
			}
			if (_pageSearchFriend != null)
			{
				_pageSearchFriend.SetActive(true);
			}
			if (_pageSearchFriendRefresh != null)
			{
				_pageSearchFriendRefresh.SetActive(false);
			}
			RefreshSearchFriendContainer();
			break;
		case UIRankings_ButtonCaptionType.ECaptionType.SearchInput:
			_colliderMask.SetActive(true);
			COMA_CommonOperation.Instance.inputKind = InputBoardCategory.SearchFriend;
			Application.LoadLevelAdditive("UI.InputName");
			break;
		}
		return true;
	}

	private void RefreshWorldContainer(EGameModeType type)
	{
		if (type == (EGameModeType)901 || (!(COMA_Login.Instance == null) && COMA_Login.Instance.IsModeRankOn((int)type)))
		{
			_extraPageWorld.SetActive(true);
			_extraPageFriend.SetActive(false);
			RefreshWorldContainerFromSrv(null);
			GetRankListCmd getRankListCmd = new GetRankListCmd();
			getRankListCmd.m_rank_name = COMA_CommonOperation.Instance.SceneIDToRankID((int)type);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, getRankListCmd);
			GetScoreCmd getScoreCmd = new GetScoreCmd();
			getScoreCmd.m_rank_name = COMA_CommonOperation.Instance.SceneIDToRankID((int)type);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, getScoreCmd);
		}
	}

	private void RefreshWorldContainerFromSrv(GetRankListResultCmd cmd)
	{
		try
		{
			if (!_uiWorldContainer.gameObject.activeInHierarchy)
			{
				return;
			}
			JsonData jsonData = null;
			if (cmd != null)
			{
				Debug.LogWarning(cmd.m_award);
				jsonData = JsonMapper.ToObject<JsonData>(cmd.m_award);
			}
			int num = ((cmd != null) ? cmd.m_list.Count : 0);
			_uiWorldContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
			_uiWorldContainer.InitBoxs(num, true);
			LimitContainerMove(num, 6, _uiWorldContainer);
			Debug.Log("boxCount : " + num);
			for (int i = 0; i < num; i++)
			{
				UIRankings_WorldBoxData data = new UIRankings_WorldBoxData();
				data.PlayerID = cmd.m_list[i].m_role_id;
				data.PlayerScore = cmd.m_list[i].m_score;
				if (jsonData != null)
				{
					JsonData jsonData2 = jsonData[i];
					Debug.Log(i + "  " + jsonData2);
					if (jsonData2.HasMember("bag"))
					{
						string awardSerialName = jsonData2["bag"].ToString();
						data.AwardSerialName = awardSerialName;
					}
					else if (jsonData2.HasMember("res"))
					{
						string text = jsonData2["res"].ToString();
						string[] array = text.Split(',');
						if (array[0] == "2")
						{
							int awardCrystal = int.Parse(array[1]);
							data.AwardCrystal = awardCrystal;
						}
					}
				}
				_uiWorldContainer.SetBoxData(i, data);
				UIDataBufferCenter.Instance.FetchPlayerProfile(cmd.m_list[i].m_role_id, delegate(WatchRoleInfo playerInfo)
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
			if (cmd != null)
			{
				UIDataBufferCenter.Instance.FetchServerTime(delegate(uint time)
				{
					uint seconds = cmd.m_end_time - time;
					TimeSpan timeSpan = new TimeSpan(0, 0, (int)seconds);
					_labelWorldTimeLeft.text = timeSpan.Days + "D " + timeSpan.Hours + "H";
				});
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.StackTrace);
			Debug.Log(ex.Message);
		}
	}

	private void RefreshFriendContainer(EGameModeType type)
	{
		if (type != (EGameModeType)901 && type != EGameModeType.Mode_RPG && (COMA_Login.Instance == null || !COMA_Login.Instance.IsModeRankOn((int)type)))
		{
			return;
		}
		_extraPageWorld.SetActive(false);
		_extraPageFriend.SetActive(true);
		if (UIDataBufferCenter.Instance == null)
		{
			return;
		}
		if (type == EGameModeType.Mode_RPG)
		{
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			GetFriendMedalListCmd extraInfo = new GetFriendMedalListCmd();
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, extraInfo);
			for (int i = 0; i < _leftTimeInfos.Length; i++)
			{
				_leftTimeInfos[i].SetActive(false);
			}
		}
		else
		{
			for (int j = 0; j < _leftTimeInfos.Length; j++)
			{
				_leftTimeInfos[j].SetActive(true);
			}
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			int count = notifyRoleDataCmd.m_friend_list.Count;
			_uiFriendContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
			_uiFriendContainer.InitBoxs(count, true);
			LimitContainerMove(count, 5, _uiFriendContainer);
			Debug.Log("boxCount : " + count);
			for (int k = 0; k < count; k++)
			{
				UIRankings_FriendBoxData data = new UIRankings_FriendBoxData();
				_uiFriendContainer.SetBoxData(k, data);
				Debug.Log(notifyRoleDataCmd.m_friend_list[k]);
				data.PlayerID = notifyRoleDataCmd.m_friend_list[k];
				UIDataBufferCenter.Instance.FetchPlayerProfile(notifyRoleDataCmd.m_friend_list[k], delegate(WatchRoleInfo playerInfo)
				{
					data.watchInfo = playerInfo;
					data.PlayerName = playerInfo.m_name;
					data.SetDirty();
					UIDataBufferCenter.Instance.FetchFacebookIconByTID(playerInfo.m_player_id, delegate(Texture2D tex)
					{
						data.Tex = tex;
						data.SetDirty();
					});
				});
			}
			GetFriendListScoreCmd getFriendListScoreCmd = new GetFriendListScoreCmd();
			getFriendListScoreCmd.m_rank_name = COMA_CommonOperation.Instance.SceneIDToRankID((int)type);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, getFriendListScoreCmd);
		}
		_labelFriendID.text = UIGolbalStaticFun.GetSelfTID().ToString();
		_labelFriendFriends.text = UIDataBufferCenter.Instance.GetFriendsCount().ToString() + "/" + COMA_DataConfig.Instance._sysConfig.Friend.max_size;
		_labelSearchFriends.text = UIDataBufferCenter.Instance.GetFriendsCount().ToString() + "/" + COMA_DataConfig.Instance._sysConfig.Friend.max_size;
	}

	private bool DragFriendsMedalRankResult(TUITelegram msg)
	{
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		GetFriendMedalListResultCmd getFriendMedalListResultCmd = (GetFriendMedalListResultCmd)msg._pExtraInfo;
		int num = ((getFriendMedalListResultCmd != null) ? getFriendMedalListResultCmd.m_list.Count : 0);
		_uiFriendContainer.ClearContainer();
		_uiFriendContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		_uiFriendContainer.InitBoxs(num, true);
		getFriendMedalListResultCmd.m_list.Sort();
		Dictionary<uint, bool> dictionary = new Dictionary<uint, bool>();
		for (int i = 0; i < num; i++)
		{
			if (!dictionary.ContainsKey(getFriendMedalListResultCmd.m_list[i].m_player_id))
			{
				dictionary.Add(getFriendMedalListResultCmd.m_list[i].m_player_id, true);
			}
			UIRankings_FriendBoxData data = new UIRankings_FriendBoxData();
			data.PlayerID = getFriendMedalListResultCmd.m_list[i].m_player_id;
			data.PlayerScore = getFriendMedalListResultCmd.m_list[i].m_medal;
			data.PlayerRank = (uint)i;
			data.IsRPG = true;
			_uiFriendContainer.SetBoxData(i, data);
			Debug.LogWarning("Friend Medal = " + getFriendMedalListResultCmd.m_list[i].m_medal);
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
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		int count = notifyRoleDataCmd.m_friend_list.Count;
		if (num != count)
		{
			for (int num2 = 0; num2 < count; num2++)
			{
				if (dictionary.ContainsKey(notifyRoleDataCmd.m_friend_list[num2]))
				{
					continue;
				}
				uint playerID = notifyRoleDataCmd.m_friend_list[num2];
				UIRankings_FriendBoxData data2 = new UIRankings_FriendBoxData();
				data2.PlayerID = playerID;
				data2.PlayerScore = 0u;
				data2.LV = 0;
				data2.IsRPG = true;
				_uiFriendContainer.SetBoxData(_uiFriendContainer.AddBox(), data2);
				UIDataBufferCenter.Instance.FetchPlayerProfile(data2.PlayerID, delegate(WatchRoleInfo playerInfo)
				{
					if (playerInfo != null)
					{
						data2.watchInfo = playerInfo;
						data2.PlayerName = playerInfo.m_name;
						Debug.Log("Name:" + playerInfo.m_name);
						data2.SetDirty();
						UIDataBufferCenter.Instance.FetchFacebookIconByTID(playerInfo.m_player_id, delegate(Texture2D tex)
						{
							data2.Tex = tex;
							data2.SetDirty();
						});
					}
				});
			}
		}
		return true;
	}

	private void RefreshFriendContainer(GetFriendListScoreResultCmd cmd)
	{
		if (!_uiFriendContainer.gameObject.activeInHierarchy)
		{
			return;
		}
		Debug.Log(cmd.m_rank_name + " " + cmd.m_list_num);
		Dictionary<uint, uint> dictionary = new Dictionary<uint, uint>();
		int i = 0;
		for (int count = cmd.m_list.Count; i < count; i++)
		{
			dictionary.Add(cmd.m_list[i].m_role_id, cmd.m_list[i].m_score);
		}
		if (dictionary.ContainsKey(153u))
		{
			Debug.LogWarning("before:" + dictionary[153u]);
		}
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		int j = 0;
		for (int count2 = notifyRoleDataCmd.m_friend_list.Count; j < count2; j++)
		{
			if (!dictionary.ContainsKey(notifyRoleDataCmd.m_friend_list[j]))
			{
				dictionary.Add(notifyRoleDataCmd.m_friend_list[j], 0u);
			}
		}
		if (dictionary.ContainsKey(153u))
		{
			Debug.LogWarning("after:" + dictionary[153u]);
		}
		List<UIRankings_FriendBoxData> list = new List<UIRankings_FriendBoxData>();
		List<NGUI_COMUI.UI_Box> lstBoxs = _uiFriendContainer.LstBoxs;
		int k = 0;
		for (int count3 = lstBoxs.Count; k < count3; k++)
		{
			UIRankings_FriendBoxData uIRankings_FriendBoxData = (UIRankings_FriendBoxData)lstBoxs[k].BoxData;
			uIRankings_FriendBoxData.PlayerScore = dictionary[uIRankings_FriendBoxData.PlayerID];
			int l;
			for (l = 0; l < list.Count && uIRankings_FriendBoxData.PlayerScore <= list[l].PlayerScore && (uIRankings_FriendBoxData.PlayerScore != list[l].PlayerScore || uIRankings_FriendBoxData.PlayerName.CompareTo(list[l].PlayerName) >= 0); l++)
			{
			}
			list.Insert(l, uIRankings_FriendBoxData);
			uIRankings_FriendBoxData.SetDirty();
		}
		int m = 0;
		for (int count4 = lstBoxs.Count; m < count4; m++)
		{
			list[m].PlayerRank = (uint)(m + 1);
			_uiFriendContainer.SetBoxData(m, list[m]);
		}
		if (cmd != null)
		{
			UIDataBufferCenter.Instance.FetchServerTime(delegate(uint time)
			{
				uint seconds = cmd.m_end_time - time;
				TimeSpan timeSpan = new TimeSpan(0, 0, (int)seconds);
				_labelFriendTimeLeft.text = timeSpan.Days + "D " + timeSpan.Hours + "H";
			});
		}
	}

	private bool OnFriendDelete(TUITelegram msg)
	{
		_com = (UIRankings_ButtonDelFriend)msg._pExtraInfo;
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("haoyoujiemian_desc3"));
		uIMessage_CommonBoxData.Mark = "DelFriend";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		return true;
	}

	private void OnFriendDelete()
	{
		_com.gameObject.SetActive(false);
		uint tarID = _com.tarID;
		List<NGUI_COMUI.UI_Box> lstBoxs = _uiFriendContainer.LstBoxs;
		int i = 0;
		for (int count = lstBoxs.Count; i < count; i++)
		{
			UIRankings_FriendBoxData uIRankings_FriendBoxData = (UIRankings_FriendBoxData)lstBoxs[i].BoxData;
			if (uIRankings_FriendBoxData.PlayerID == tarID)
			{
				_uiFriendContainer.DelBox(lstBoxs[i]);
				break;
			}
		}
		DeleteFriendCmd deleteFriendCmd = new DeleteFriendCmd();
		deleteFriendCmd.m_who = tarID;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, deleteFriendCmd);
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		notifyRoleDataCmd.m_friend_list.Remove(tarID);
		if (UIDataBufferCenter.Instance.Online_friend_list.Contains(tarID))
		{
			UIDataBufferCenter.Instance.Online_friend_list.Remove(tarID);
		}
	}

	private void RefreshSearchFriendContainer()
	{
		RefreshSearchFriendContainer(null);
	}

	private void RefreshSearchFriendContainer(List<SearchRoleInfo> lst)
	{
		_extraPageWorld.SetActive(false);
		_extraPageFriend.SetActive(false);
		int num = ((lst != null) ? lst.Count : 0);
		_uiSearchFriendContainer.InitContainer(NGUI_COMUI.UI_Container.EBoxSelType.Single);
		_uiSearchFriendContainer.InitBoxs(num, true);
		curNum = num;
		LimitContainerMove(num, 5, _uiSearchFriendContainer);
		for (int i = 0; i < num; i++)
		{
			UIRankings_SearchFriendBoxData data = new UIRankings_SearchFriendBoxData();
			data.PlayerID = lst[i].m_player_id;
			data.PlayerName = lst[i].m_name;
			Debug.Log(data.PlayerID + " " + data.PlayerName);
			_uiSearchFriendContainer.SetBoxData(i, data);
			UIDataBufferCenter.Instance.FetchPlayerProfile(data.PlayerID, delegate(WatchRoleInfo playerInfo)
			{
				data.watchInfo = playerInfo;
				UIDataBufferCenter.Instance.FetchFacebookIconByTID(data.PlayerID, delegate(Texture2D tex)
				{
					data.Tex = tex;
					data.SetDirty();
				});
			});
		}
	}

	private void UploadSearchRequest(string tar_nickname, byte page_size, uint page_index)
	{
		Debug.Log(tar_nickname);
		if (tar_nickname == string.Empty)
		{
			return;
		}
		if (MiscStaticTools.SF_IsPureNumber(tar_nickname))
		{
			SearchPlayerResultCmd dataCmd = new SearchPlayerResultCmd();
			dataCmd.m_page_sum = 1u;
			dataCmd.m_page_index = 1u;
			SearchRoleInfo info = new SearchRoleInfo();
			info.m_player_id = uint.Parse(tar_nickname);
			UIDataBufferCenter.Instance.FetchPlayerProfile(info.m_player_id, delegate(WatchRoleInfo watchInfo)
			{
				info.m_name = watchInfo.m_name;
				if (watchInfo.m_level != 0)
				{
					dataCmd.m_list.Add(info);
				}
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_SearchFriendFinish, this, dataCmd);
			});
		}
		else
		{
			SearchPlayerCmd searchPlayerCmd = new SearchPlayerCmd();
			searchPlayerCmd.m_nickname = tar_nickname;
			searchPlayerCmd.m_page_size = page_size;
			searchPlayerCmd.m_page_index = page_index;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, searchPlayerCmd);
		}
	}

	private bool InputNameEnd(TUITelegram msg)
	{
		_colliderMask.SetActive(false);
		tarNickName = (string)msg._pExtraInfo;
		if (tarNickName != string.Empty)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_CaptionTypeSwitched, null, UIRankings_ButtonCaptionType.ECaptionType.SearchFriend);
			pageIndex = 1u;
			UploadSearchRequest(tarNickName, pageSize, pageIndex);
		}
		return true;
	}

	private bool OnSearchFriend(TUITelegram msg)
	{
		SearchPlayerResultCmd searchPlayerResultCmd = (SearchPlayerResultCmd)msg._pExtraInfo;
		Debug.Log("PageIndex:" + searchPlayerResultCmd.m_page_index + " PageCount:" + searchPlayerResultCmd.m_page_sum + " ListCount:" + searchPlayerResultCmd.m_list.Count);
		pageIndex = searchPlayerResultCmd.m_page_index;
		pageCount = searchPlayerResultCmd.m_page_sum;
		bool flag = false;
		bool flag2 = false;
		for (int num = searchPlayerResultCmd.m_list.Count - 1; num >= 0; num--)
		{
			if (searchPlayerResultCmd.m_list[num].m_player_id == UIGolbalStaticFun.GetSelfTID())
			{
				searchPlayerResultCmd.m_list.RemoveAt(num);
				flag = true;
			}
			else if (UIGolbalStaticFun.IsFriend(searchPlayerResultCmd.m_list[num].m_player_id))
			{
				searchPlayerResultCmd.m_list.RemoveAt(num);
				flag2 = true;
			}
		}
		if (searchPlayerResultCmd.m_list.Count <= 0)
		{
			if (flag)
			{
				UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("haoyoujiemian_desc13"));
				UIGolbalStaticFun.PopCommonMessageBox(data);
			}
			else if (flag2)
			{
				UIMessage_CommonBoxData data2 = new UIMessage_CommonBoxData(1, Localization.instance.Get("haoyoujiemian_desc11"));
				UIGolbalStaticFun.PopCommonMessageBox(data2);
			}
			else
			{
				UIMessage_CommonBoxData data3 = new UIMessage_CommonBoxData(1, Localization.instance.Get("haoyoujiemian_desc10"));
				UIGolbalStaticFun.PopCommonMessageBox(data3);
			}
		}
		if (_pageSearchFriend.activeInHierarchy)
		{
			RefreshSearchFriendContainer(searchPlayerResultCmd.m_list);
			if (pageCount > 1)
			{
				_pageSearchFriendRefresh.SetActive(true);
			}
		}
		return true;
	}

	private bool OnSearchFriendRefresh(TUITelegram msg)
	{
		if (pageIndex < pageCount)
		{
			pageIndex++;
		}
		else
		{
			pageIndex = 1u;
		}
		UploadSearchRequest(tarNickName, pageSize, pageIndex);
		return true;
	}

	private bool OnSearchFriendAdd(TUITelegram msg)
	{
		if (msg._pExtraInfo2 != null)
		{
			UIRankings_SearchFriendBox box = (UIRankings_SearchFriendBox)msg._pExtraInfo2;
			_uiSearchFriendContainer.DelBox(box);
			curNum--;
			Debug.LogWarning(curNum);
			LimitContainerMove(curNum, 5, _uiSearchFriendContainer);
		}
		uint id = (uint)msg._pExtraInfo;
		UIDataBufferCenter.Instance.TryToAddFriend(id);
		return true;
	}

	private bool GetSelfScore(TUITelegram msg)
	{
		GetScoreResultCmd getScoreResultCmd = (GetScoreResultCmd)msg._pExtraInfo;
		Debug.Log(getScoreResultCmd.m_score + " " + getScoreResultCmd.m_pos);
		_labelSelfRank.text = ((getScoreResultCmd.m_pos >= 1000) ? "999+" : getScoreResultCmd.m_pos.ToString());
		if (getScoreResultCmd.m_score > 99999)
		{
			_labelSelfScore.text = getScoreResultCmd.m_score / 1000 + "K";
		}
		else
		{
			_labelSelfScore.text = getScoreResultCmd.m_score.ToString();
		}
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		_labelSelfName.text = notifyRoleDataCmd.m_info.m_name;
		return true;
	}

	private bool GetWorldRankList(TUITelegram msg)
	{
		GetRankListResultCmd cmd = (GetRankListResultCmd)msg._pExtraInfo;
		RefreshWorldContainerFromSrv(cmd);
		return true;
	}

	private bool GetFriendRankList(TUITelegram msg)
	{
		GetFriendListScoreResultCmd cmd = (GetFriendListScoreResultCmd)msg._pExtraInfo;
		RefreshFriendContainer(cmd);
		return true;
	}

	private bool WorldGameModeSelChanged(TUITelegram msg)
	{
		if (_bIsNeedOpenFriendPageOnEnable)
		{
			Debug.Log("Direct to Friend Page;");
			return true;
		}
		EGameModeType eGameModeType = (EGameModeType)(int)msg._pExtraInfo;
		Debug.Log("WorldGameModeSelChanged : " + eGameModeType);
		RefreshWorldContainer(eGameModeType);
		GameObject gameObject = (GameObject)msg._pExtraInfo2;
		if (gameObject != null)
		{
			gameObject.GetComponent<UICheckbox_Ranking>().isChecked = true;
		}
		return true;
	}

	private bool FriendGameModeSelChanged(TUITelegram msg)
	{
		EGameModeType eGameModeType = (EGameModeType)(int)msg._pExtraInfo;
		Debug.Log("FriendGameModeSelChanged : " + eGameModeType);
		RefreshFriendContainer(eGameModeType);
		GameObject gameObject = (GameObject)msg._pExtraInfo2;
		if (gameObject != null)
		{
			gameObject.GetComponent<UICheckbox_Ranking>().isChecked = true;
		}
		return true;
	}

	private bool OpenAddFriendPanel(TUITelegram msg)
	{
		_addFriendsPanel.SetActive(true);
		return true;
	}

	private bool CloseAddFriendPanel(TUITelegram msg)
	{
		_addFriendsPanel.SetActive(false);
		return true;
	}

	private bool FacebookLogFailed(TUITelegram msg)
	{
		return true;
	}

	private bool GotoSquareScene(object obj)
	{
		Debug.Log("GotoSquareScene");
		UIGolbalStaticFun.CloseBlockForTUIMessageBox();
		TLoadScene extraInfo = new TLoadScene("UI.Square", ELoadLevelParam.AddHidePre);
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

	private new void OnEnable()
	{
		base.OnEnable();
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
		if (_bIsNeedOpenFriendPageOnEnable)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_CaptionTypeButtonSelChanged, null, UIRankings_ButtonCaptionType.ECaptionType.Friend_Unsel);
			_bIsNeedOpenFriendPageOnEnable = false;
			_bEnterWorldPage = true;
			Debug.Log("_bIsNeedOpenFriendPageOnEnable=" + _bIsNeedOpenFriendPageOnEnable);
		}
		else if (!_bEnterWorldPage)
		{
			_bEnterWorldPage = true;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_CaptionTypeButtonSelChanged, null, UIRankings_ButtonCaptionType.ECaptionType.World_Unsel);
		}
	}
}
