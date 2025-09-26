using System;
using System.Collections.Generic;
using System.Text;
using LitJson;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Mail.S2C;
using Protocol.RPG.C2S;
using Protocol.RPG.S2C;
using Protocol.Rank.S2C;
using Protocol.Role;
using Protocol.Role.C2S;
using Protocol.Role.S2C;
using Protocol.Shop.C2S;
using Protocol.Shop.S2C;
using UnityEngine;

public class UIDataBufferCenter : UIEntity
{
	public enum EDataType
	{
		Role = 0
	}

	public enum ERoleDataType
	{
		Unknow = -1,
		RoleInfo = 0,
		BagData = 1,
		CollectLst = 2,
		FollowLst = 3
	}

	public enum ELobbySrvColseReason
	{
		Unknow = 0,
		Maintain = 1,
		Busy = 2,
		TimeOut = 4
	}

	public enum EReVerifyStep
	{
		None = 0,
		Sending = 1,
		Confirmed = 2
	}

	public enum EShowChatChannel
	{
		None = 0,
		All = 1,
		PM = 2
	}

	private DragMailListResultCmd _mailBufferInfo;

	private Action<ReportMapBattleResultCmd> _battleResult;

	private Dictionary<ReportMapBattleCmd, Action<ReportMapBattleResultCmd>> _dictBattleResult = new Dictionary<ReportMapBattleCmd, Action<ReportMapBattleResultCmd>>();

	private bool _active_Debug_nofog;

	private int cur_debug_nofog;

	private Dictionary<int, Action<ReqBattleResultCmd>> _dictReqBattleResult = new Dictionary<int, Action<ReqBattleResultCmd>>();

	public static UIDataBufferCenter Instance;

	private NotifyRoleDataCmd _roleData;

	private GetScoreResultCmd _rankSelfData;

	private Dictionary<string, GetRankListResultCmd> _rankWorldData = new Dictionary<string, GetRankListResultCmd>();

	private Dictionary<string, List<WorldRankAward>> _rankWorldAward = new Dictionary<string, List<WorldRankAward>>();

	private Dictionary<string, GetFriendListScoreResultCmd> _rankFriendData = new Dictionary<string, GetFriendListScoreResultCmd>();

	private Dictionary<string, List<Action<byte[]>>> _dictFileData = new Dictionary<string, List<Action<byte[]>>>();

	private Dictionary<uint, Action<List<ShopItem>>> _dictPlayerSellList = new Dictionary<uint, Action<List<ShopItem>>>();

	private List<uint> addedFriendIDs = new List<uint>();

	private Dictionary<string, bool> _dicFetchMap = new Dictionary<string, bool>();

	private Dictionary<uint, Texture2D> _playerIDToPlayerIconMD5 = new Dictionary<uint, Texture2D>();

	private Texture2D texDefault;

	private Dictionary<CSuitMD5, Action<List<byte[]>>> _dictSuitFun = new Dictionary<CSuitMD5, Action<List<byte[]>>>();

	private Dictionary<CSuitMD5, List<byte[]>> _dictSuitData = new Dictionary<CSuitMD5, List<byte[]>>();

	private Dictionary<byte, Action<List<ShopItem>>> _dictShopList = new Dictionary<byte, Action<List<ShopItem>>>();

	private Dictionary<uint, WatchRoleInfo> _dictPlayerProfileBuffer = new Dictionary<uint, WatchRoleInfo>();

	private Dictionary<uint, List<Action<WatchRoleInfo>>> _dictPlayerProfileList = new Dictionary<uint, List<Action<WatchRoleInfo>>>();

	private Dictionary<uint, PlayerRpgDataCmd> _dictPlayerRPGBuffer = new Dictionary<uint, PlayerRpgDataCmd>();

	private Dictionary<uint, List<Action<PlayerRpgDataCmd>>> _dictPlayerPRGList = new Dictionary<uint, List<Action<PlayerRpgDataCmd>>>();

	private Dictionary<uint, ExtInfo> _dicExtInfoBuffer = new Dictionary<uint, ExtInfo>();

	private Dictionary<uint, List<Action<ExtInfo>>> _dictPlayerExtInfoList = new Dictionary<uint, List<Action<ExtInfo>>>();

	private Dictionary<uint, bool> _dictPraiseAvataLst = new Dictionary<uint, bool>();

	private Action<List<ShopItem>> collectAvatarLstFun;

	private UIBackpack_BoxData _selectBoxDataForDesign;

	private Dictionary<ulong, Action<string>> _dictUploadFile = new Dictionary<ulong, Action<string>>();

	private bool _bEnableToFriendTab;

	private List<uint> _online_friend_list = new List<uint>();

	private int lobbySrvCloseReason;

	private bool _bNeedRestart;

	private EReVerifyStep _reVerifyStep;

	private float _fSendingTime;

	private List<Action<uint>> lstSyncServerTimeFinish = new List<Action<uint>>();

	public bool _bSellListChanged;

	public bool _bManualCloseMobileChat;

	private EShowChatChannel _preChatChannel;

	private EShowChatChannel _curChatChannel = EShowChatChannel.All;

	private Dictionary<uint, bool> _dictChatBlockMap = new Dictionary<uint, bool>();

	public Dictionary<string, uint> _gameModeNum = new Dictionary<string, uint>();

	public bool _bAcceptFriendRequest;

	private NotifyRPGDataCmd _rpgData;

	private PlayerRpgDataCmd _rpgData_Enemy;

	private int _curBattleLevelIndex = -1;

	private int _curBattleLevelLV = -1;

	private string[] _rpgConfig_LevelValid = new string[100];

	private int _rpgFirstLoginAward_PerDay = -1;

	private bool _curBattleResult;

	private bool _curBattleResult_Common;

	private int _curNGIndex;

	private string _preSceneName;

	public DragMailListResultCmd MailBufferInfo
	{
		get
		{
			return _mailBufferInfo;
		}
	}

	public RoleInfo playerInfo
	{
		get
		{
			return _roleData.m_info;
		}
	}

	public Dictionary<uint, ExtInfo> DicExtInfoBuffer
	{
		get
		{
			return _dicExtInfoBuffer;
		}
	}

	public Dictionary<uint, bool> DictPraiseAvataLst
	{
		get
		{
			return _dictPraiseAvataLst;
		}
	}

	public UIBackpack_BoxData SelectBoxDataForDesign
	{
		get
		{
			return _selectBoxDataForDesign;
		}
		set
		{
			_selectBoxDataForDesign = value;
		}
	}

	public bool EnableToFriendTab
	{
		get
		{
			return _bEnableToFriendTab;
		}
		set
		{
			_bEnableToFriendTab = value;
		}
	}

	public List<uint> Online_friend_list
	{
		get
		{
			return _online_friend_list;
		}
	}

	public int LobbySrvCloseReason
	{
		get
		{
			return lobbySrvCloseReason;
		}
	}

	public EReVerifyStep ReVerifyStep
	{
		get
		{
			return _reVerifyStep;
		}
		set
		{
			_reVerifyStep = value;
			if (_reVerifyStep == EReVerifyStep.Sending)
			{
				_fSendingTime = Time.time;
			}
			if (_reVerifyStep == EReVerifyStep.Confirmed)
			{
				UIGolbalStaticFun.CloseBlockOnlyMessageBox();
			}
		}
	}

	public EShowChatChannel PreChatChannel
	{
		get
		{
			return _preChatChannel;
		}
	}

	public EShowChatChannel CurChatChannel
	{
		get
		{
			return _curChatChannel;
		}
		set
		{
			_curChatChannel = value;
			if (_curChatChannel != _preChatChannel)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_ChatChannelChanged, this, _curChatChannel);
			}
			_preChatChannel = _curChatChannel;
		}
	}

	public NotifyRPGDataCmd RPGData
	{
		get
		{
			return _rpgData;
		}
		set
		{
			_rpgData = value;
		}
	}

	public PlayerRpgDataCmd RPGData_Enemy
	{
		get
		{
			return _rpgData_Enemy;
		}
		set
		{
			_rpgData_Enemy = value;
		}
	}

	public int CurBattleLevelIndex
	{
		get
		{
			return _curBattleLevelIndex;
		}
		set
		{
			_curBattleLevelIndex = value;
		}
	}

	public int CurBattleLevelLV
	{
		get
		{
			return _curBattleLevelLV;
		}
		set
		{
			_curBattleLevelLV = value;
		}
	}

	public string[] RpgConfig_LevelValid
	{
		get
		{
			return _rpgConfig_LevelValid;
		}
		set
		{
			_rpgConfig_LevelValid = value;
		}
	}

	public int RPGFirstLoginAward_PerDay
	{
		get
		{
			return _rpgFirstLoginAward_PerDay;
		}
		set
		{
			_rpgFirstLoginAward_PerDay = value;
		}
	}

	public bool CurBattleResult
	{
		get
		{
			return _curBattleResult;
		}
		set
		{
			_curBattleResult = value;
		}
	}

	public bool CurBattleResult_Common
	{
		get
		{
			return _curBattleResult_Common;
		}
		set
		{
			_curBattleResult_Common = value;
		}
	}

	public int CurNGIndex
	{
		get
		{
			return _curNGIndex;
		}
		set
		{
			_curNGIndex = value;
		}
	}

	public string PreSceneName
	{
		get
		{
			return _preSceneName;
		}
		set
		{
			_preSceneName = value;
		}
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIDataBuffer_SetData, this, SetData);
		RegisterMessage(EUIMessageID.UIDataBuffer_DataDirty, this, DataDirty);
		RegisterMessage(EUIMessageID.UIDataBuffer_RemoteFileDataArrived, this, RemoteFileDataArrived);
		RegisterMessage(EUIMessageID.UIDataBuffer_RemotePlayerSellListArrived, this, RemotePlayerSellListArrived);
		RegisterMessage(EUIMessageID.UIDataBuffer_RemoteMarketShopListArrived, this, RemoteMarketShopListArrived);
		RegisterMessage(EUIMessageID.UIDataBuffer_RemoteWatchInfoArrived, this, RemoteWatchInfoArrived);
		RegisterMessage(EUIMessageID.UIRPG_NotifyOtherPlayerData, this, RemoteRPGPlayerDataArrived);
		RegisterMessage(EUIMessageID.UIRPG_NotifyOtherPlayerDataError, this, RemoteRPGPlayerDataErrorArrived);
		RegisterMessage(EUIMessageID.UIDataBuffer_RemoteMailInfoArrived, this, RemoteMailInfoArrived);
		RegisterMessage(EUIMessageID.UIDataBuffer_RemotePlayerExtInfoArrived, this, RemotePlayerExtInfoArrived);
		RegisterMessage(EUIMessageID.UIDataBuffer_CollectMarketShopListArrived, this, CollectMarketShopListArrived);
		RegisterMessage(EUIMessageID.UIDataBuffer_UploadFileArrived, this, UploadFileArrived);
		RegisterMessage(EUIMessageID.UI_Role_ServerTimeSync, this, SyncServerTimeArrived);
		RegisterMessage(EUIMessageID.UIRPG_NotifyMapBattleResult, this, NotifyMapBattleResult);
		RegisterMessage(EUIMessageID.UIRPG_NotifyReqBattleResult, this, NotifyReqBattleResult_Debug);
		Instance = this;
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIDataBuffer_SetData, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_DataDirty, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RemoteFileDataArrived, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RemotePlayerSellListArrived, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RemoteMarketShopListArrived, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RemoteWatchInfoArrived, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyOtherPlayerData, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyOtherPlayerDataError, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RemoteMailInfoArrived, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_RemotePlayerExtInfoArrived, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_CollectMarketShopListArrived, this);
		UnregisterMessage(EUIMessageID.UIDataBuffer_UploadFileArrived, this);
		UnregisterMessage(EUIMessageID.UI_Role_ServerTimeSync, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyMapBattleResult, this);
		UnregisterMessage(EUIMessageID.UIRPG_NotifyReqBattleResult, this);
		Instance = null;
	}

	private void DataChanged(object data, bool bRePoint)
	{
		DataChanged(data, bRePoint, null);
	}

	private void DataChanged(object data, bool bRePoint, object EDataType)
	{
		Type type = data.GetType();
		Debug.Log("Data Buffer Center : DataChanged=" + type.ToString());
		if (type.ToString() == "Protocol.Role.S2C.NotifyRoleDataCmd")
		{
			if (bRePoint)
			{
				_roleData = (NotifyRoleDataCmd)data;
			}
			if (EDataType != null)
			{
				if ((int)EDataType == 0)
				{
					Debug.Log("DataCenter--->UIDataBuffer_RoleData_RoleInfoChanged");
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this, _roleData);
				}
				else if ((int)EDataType == 1)
				{
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RoleData_BagDataChanged, this, _roleData);
				}
				else if ((int)EDataType == 2)
				{
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RoleData_CollectLstChanged, this, _roleData);
				}
				else if ((int)EDataType == 3)
				{
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RoleData_FollowLstChanged, this, _roleData);
				}
			}
			else
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RoleDataChanged, this, _roleData);
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RoleData_RoleInfoChanged, this, _roleData);
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RoleData_BagDataChanged, this, _roleData);
			}
		}
		else if (type.ToString() == "Protocol.Mail.S2C.DragMailListResultCmd")
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_MailDataChanged, this, data);
		}
		else if (type.ToString() == "Protocol.Rank.S2C.GetScoreResultCmd")
		{
			if (bRePoint)
			{
				_rankSelfData = (GetScoreResultCmd)data;
			}
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_GetSelfScoreFinish, this, data);
		}
		else if (type.ToString() == "Protocol.Rank.S2C.GetRankListResultCmd")
		{
			if (bRePoint)
			{
				GetRankListResultCmd getRankListResultCmd = (GetRankListResultCmd)data;
				if (_rankWorldData.ContainsKey(getRankListResultCmd.m_rank_name))
				{
					_rankWorldData[getRankListResultCmd.m_rank_name] = getRankListResultCmd;
				}
				else
				{
					_rankWorldData.Add(getRankListResultCmd.m_rank_name, getRankListResultCmd);
					if (getRankListResultCmd.m_award == string.Empty)
					{
						Debug.LogWarning("no rank award!!");
					}
					else
					{
						Debug.Log(getRankListResultCmd.m_award);
						List<WorldRankAward> list = new List<WorldRankAward>();
						JsonData jsonData = JsonMapper.ToObject<JsonData>(getRankListResultCmd.m_award);
						for (int i = 0; i < jsonData.Count; i++)
						{
							JsonData jsonData2 = jsonData[i];
							if (jsonData2.HasMember("bag"))
							{
								string accessoriesSerial = jsonData2["bag"].ToString();
								WorldRankAward item = new WorldRankAward(accessoriesSerial, 0, 0);
								list.Add(item);
							}
							else if (jsonData2.HasMember("res"))
							{
								string text = jsonData2["res"].ToString();
								string[] array = text.Split(',');
								int type2 = int.Parse(array[0]);
								int num = int.Parse(array[1]);
								WorldRankAward item2 = new WorldRankAward(string.Empty, type2, num);
								list.Add(item2);
							}
						}
						_rankWorldAward.Add(getRankListResultCmd.m_rank_name, list);
						if (getRankListResultCmd.m_rank_name == "ComAvatar007" && COMA_CommonOperation.Instance.IsMode_Fishing(Application.loadedLevelName))
						{
							int iDByName = TFishingAddressBook.Instance.GetIDByName(1);
							TMessageDispatcher.Instance.DispatchMsg(-1, iDByName, 1015, TTelegram.SEND_MSG_IMMEDIATELY, null);
						}
						if (getRankListResultCmd.m_rank_name == "ComAvatar007")
						{
							Debug.Log("ComAvatar007");
							if (getRankListResultCmd.m_last_rank == string.Empty)
							{
								Debug.LogWarning("no last rank!!");
							}
							else
							{
								Debug.Log("ComAvatar007_1");
								JsonData lst = JsonMapper.ToObject<JsonData>(getRankListResultCmd.m_last_rank);
								Debug.Log("ComAvatar007_2");
								List<WatchRoleInfo> tempList = new List<WatchRoleInfo>();
								for (int j = 0; j < lst.Count; j++)
								{
									Debug.Log("ComAvatar007_3 :" + j);
									JsonData jsonData3 = lst[j];
									Debug.Log("ComAvatar007_4 :" + j);
									if (!jsonData3.HasMember("id"))
									{
										continue;
									}
									int num2 = int.Parse(jsonData3["id"].ToString());
									COMA_Server_Rank.Instance.fishingWinnerID.Add(num2.ToString());
									string text2 = jsonData3["score"].ToString();
									COMA_Server_Rank.Instance.fishingWinnerWeight.Add(text2);
									Debug.Log("Parse Rank Reward: " + j + " " + num2 + " " + text2);
									Instance.FetchPlayerProfile((uint)num2, delegate(WatchRoleInfo roleInfo)
									{
										tempList.Add(roleInfo);
										if (tempList.Count == lst.Count)
										{
											Debug.LogWarning(COMA_Server_Rank.Instance.fishingWinnerID.Count);
											if (COMA_Server_Rank.Instance.fishingWinnerID.Count > 0)
											{
												for (int k = 0; k < tempList.Count; k++)
												{
													if (COMA_Server_Rank.Instance.fishingWinnerID[0] == tempList[k].m_player_id.ToString())
													{
														COMA_Server_Rank.Instance.fishingWinnerName.Add(tempList[k].m_name);
														break;
													}
												}
											}
											if (COMA_Server_Rank.Instance.fishingWinnerID.Count > 1)
											{
												for (int l = 0; l < tempList.Count; l++)
												{
													if (COMA_Server_Rank.Instance.fishingWinnerID[1] == tempList[l].m_player_id.ToString())
													{
														COMA_Server_Rank.Instance.fishingWinnerName.Add(tempList[l].m_name);
														break;
													}
												}
											}
											if (COMA_Server_Rank.Instance.fishingWinnerID.Count > 2)
											{
												for (int m = 0; m < tempList.Count; m++)
												{
													if (COMA_Server_Rank.Instance.fishingWinnerID[2] == tempList[m].m_player_id.ToString())
													{
														COMA_Server_Rank.Instance.fishingWinnerName.Add(tempList[m].m_name);
														break;
													}
												}
											}
											int iDByName2 = TFishingAddressBook.Instance.GetIDByName(1);
											TMessageDispatcher.Instance.DispatchMsg(-1, iDByName2, 1016, TTelegram.SEND_MSG_IMMEDIATELY, null);
										}
									});
									RankItem rankItem = new RankItem();
									rankItem.m_role_id = (uint)num2;
									getRankListResultCmd.m_list.Add(rankItem);
								}
							}
						}
					}
				}
			}
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_WorldRankListFinish, this, data);
		}
		else
		{
			if (!(type.ToString() == "Protocol.Rank.S2C.GetFriendListScoreResultCmd"))
			{
				return;
			}
			if (bRePoint)
			{
				GetFriendListScoreResultCmd getFriendListScoreResultCmd = (GetFriendListScoreResultCmd)data;
				if (_rankFriendData.ContainsKey(getFriendListScoreResultCmd.m_rank_name))
				{
					_rankFriendData[getFriendListScoreResultCmd.m_rank_name] = getFriendListScoreResultCmd;
				}
				else
				{
					_rankFriendData.Add(getFriendListScoreResultCmd.m_rank_name, getFriendListScoreResultCmd);
				}
			}
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_FriendRankListFinish, this, data);
		}
	}

	private bool SetData(TUITelegram msg)
	{
		DataChanged(msg._pExtraInfo, true);
		return true;
	}

	private bool DataDirty(TUITelegram msg)
	{
		DataChanged(msg._pExtraInfo, false, msg._pExtraInfo2);
		return true;
	}

	private bool RemoteFileDataArrived(TUITelegram msg)
	{
		RemoteFileData remoteFileData = (RemoteFileData)msg._pExtraInfo;
		if (_dictFileData.ContainsKey(remoteFileData._md5))
		{
			for (int num = _dictFileData[remoteFileData._md5].Count - 1; num >= 0; num--)
			{
				Action<byte[]> action = _dictFileData[remoteFileData._md5][num];
				action(remoteFileData._data);
				_dictFileData[remoteFileData._md5].Remove(action);
			}
		}
		if (_dicFetchMap.ContainsKey(remoteFileData._md5))
		{
			_dicFetchMap.Remove(remoteFileData._md5);
		}
		return true;
	}

	private bool RemotePlayerSellListArrived(TUITelegram msg)
	{
		RemotePlayerSellListData remotePlayerSellListData = (RemotePlayerSellListData)msg._pExtraInfo;
		if (_dictPlayerSellList.ContainsKey(remotePlayerSellListData._id))
		{
			_dictPlayerSellList[remotePlayerSellListData._id](remotePlayerSellListData._lstShopItem);
		}
		return true;
	}

	private bool RemoteMarketShopListArrived(TUITelegram msg)
	{
		RemoteMarketShopListData remoteMarketShopListData = (RemoteMarketShopListData)msg._pExtraInfo;
		Debug.Log("RemoteMarketShopListArrived  -----  Key=" + (GetShopListCmd.Code)remoteMarketShopListData._type);
		if (_dictShopList.ContainsKey(remoteMarketShopListData._type))
		{
			Debug.Log("RemoteMarketShopListArrived----- ContainersKey! Key=" + (GetShopListCmd.Code)remoteMarketShopListData._type);
			_dictShopList[remoteMarketShopListData._type](remoteMarketShopListData._lstShopItem);
		}
		return true;
	}

	private bool RemoteWatchInfoArrived(TUITelegram msg)
	{
		WatchRoleInfo watchRoleInfo = msg._pExtraInfo as WatchRoleInfo;
		if (watchRoleInfo == null)
		{
			GetWatchInfoErrorCmd getWatchInfoErrorCmd = msg._pExtraInfo2 as GetWatchInfoErrorCmd;
			Debug.LogWarning("Player " + getWatchInfoErrorCmd.m_who + " is not exist!!");
			watchRoleInfo = new WatchRoleInfo();
			watchRoleInfo.m_player_id = getWatchInfoErrorCmd.m_who;
			watchRoleInfo.m_face_image_md5 = string.Empty;
			watchRoleInfo.m_name = "unknown";
			watchRoleInfo.m_level = 0u;
			watchRoleInfo.m_head = "6ba2377776d6c137ee29551baff81bb5";
			watchRoleInfo.m_body = "54245d0a0b0c5c8305976247da71f59f";
			watchRoleInfo.m_leg = "9a53aef61db65e1ed1298fca0cc15a3d";
			watchRoleInfo.m_head_top = string.Empty;
			watchRoleInfo.m_head_front = string.Empty;
			watchRoleInfo.m_head_back = string.Empty;
			watchRoleInfo.m_head_left = string.Empty;
			watchRoleInfo.m_head_right = string.Empty;
			watchRoleInfo.m_chest_front = string.Empty;
			watchRoleInfo.m_chest_back = string.Empty;
		}
		uint player_id = watchRoleInfo.m_player_id;
		if (_dictPlayerProfileBuffer.ContainsKey(player_id))
		{
			_dictPlayerProfileBuffer[player_id] = watchRoleInfo;
		}
		else
		{
			_dictPlayerProfileBuffer.Add(player_id, watchRoleInfo);
		}
		if (_dictPlayerProfileList.ContainsKey(player_id))
		{
			foreach (Action<WatchRoleInfo> item in _dictPlayerProfileList[player_id])
			{
				item(watchRoleInfo);
			}
			_dictPlayerProfileList[player_id].Clear();
			_dictPlayerProfileList.Remove(player_id);
		}
		return true;
	}

	private bool RemoteRPGPlayerDataArrived(TUITelegram msg)
	{
		PlayerRpgDataCmd playerRpgDataCmd = msg._pExtraInfo as PlayerRpgDataCmd;
		uint who = playerRpgDataCmd.who;
		if (_dictPlayerProfileBuffer.ContainsKey(who))
		{
			_dictPlayerRPGBuffer[who] = playerRpgDataCmd;
		}
		else
		{
			_dictPlayerRPGBuffer.Add(who, playerRpgDataCmd);
		}
		if (_dictPlayerPRGList.ContainsKey(who))
		{
			foreach (Action<PlayerRpgDataCmd> item in _dictPlayerPRGList[who])
			{
				item(playerRpgDataCmd);
			}
			_dictPlayerPRGList[who].Clear();
			_dictPlayerPRGList.Remove(who);
		}
		return true;
	}

	public bool IsPlayerRpgDataValid(PlayerRpgDataCmd data)
	{
		return (data.m_equip_capacity != 1000000) ? true : false;
	}

	private bool RemoteRPGPlayerDataErrorArrived(TUITelegram msg)
	{
		DragPlayerRpgDataErrorCmd dragPlayerRpgDataErrorCmd = msg._pExtraInfo as DragPlayerRpgDataErrorCmd;
		PlayerRpgDataCmd playerRpgDataCmd = new PlayerRpgDataCmd();
		playerRpgDataCmd.m_medal = 0u;
		playerRpgDataCmd.m_rpg_level = 0u;
		playerRpgDataCmd.who = dragPlayerRpgDataErrorCmd.m_role_id;
		playerRpgDataCmd.m_equip_capacity = 1000000u;
		uint role_id = dragPlayerRpgDataErrorCmd.m_role_id;
		if (_dictPlayerProfileBuffer.ContainsKey(role_id))
		{
			_dictPlayerRPGBuffer[role_id] = playerRpgDataCmd;
		}
		else
		{
			_dictPlayerRPGBuffer.Add(role_id, playerRpgDataCmd);
		}
		if (_dictPlayerPRGList.ContainsKey(role_id))
		{
			foreach (Action<PlayerRpgDataCmd> item in _dictPlayerPRGList[role_id])
			{
				item(playerRpgDataCmd);
			}
			_dictPlayerPRGList[role_id].Clear();
			_dictPlayerPRGList.Remove(role_id);
		}
		return true;
	}

	private bool RemoteMailInfoArrived(TUITelegram msg)
	{
		_mailBufferInfo = (DragMailListResultCmd)msg._pExtraInfo;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_MailDataChanged, this, _mailBufferInfo);
		return true;
	}

	private bool RemotePlayerExtInfoArrived(TUITelegram msg)
	{
		GetExtInfoResultCmd getExtInfoResultCmd = (GetExtInfoResultCmd)msg._pExtraInfo;
		if (_dictPlayerExtInfoList.ContainsKey(getExtInfoResultCmd.m_Who))
		{
			for (int i = 0; i < _dictPlayerExtInfoList[getExtInfoResultCmd.m_Who].Count; i++)
			{
				_dictPlayerExtInfoList[getExtInfoResultCmd.m_Who][i](getExtInfoResultCmd.m_extInfo);
			}
			_dictPlayerExtInfoList[getExtInfoResultCmd.m_Who].Clear();
			_dictPlayerExtInfoList.Remove(getExtInfoResultCmd.m_Who);
			if (_dicExtInfoBuffer.ContainsKey(getExtInfoResultCmd.m_Who))
			{
				_dicExtInfoBuffer[getExtInfoResultCmd.m_Who] = getExtInfoResultCmd.m_extInfo;
			}
			else
			{
				_dicExtInfoBuffer.Add(getExtInfoResultCmd.m_Who, getExtInfoResultCmd.m_extInfo);
			}
		}
		return true;
	}

	private bool CollectMarketShopListArrived(TUITelegram msg)
	{
		List<ShopItem> obj = (List<ShopItem>)msg._pExtraInfo;
		if (collectAvatarLstFun != null)
		{
			collectAvatarLstFun(obj);
		}
		return true;
	}

	private bool UploadFileArrived(TUITelegram msg)
	{
		if (msg._pExtraInfo == null)
		{
			return true;
		}
		Debug.Log("文件上传成功");
		SetFileDataResultCmd setFileDataResultCmd = (SetFileDataResultCmd)msg._pExtraInfo;
		if (_dictUploadFile[setFileDataResultCmd.m_mark] != null)
		{
			_dictUploadFile[setFileDataResultCmd.m_mark](setFileDataResultCmd.m_md5);
		}
		return true;
	}

	private bool SyncServerTimeArrived(TUITelegram msg)
	{
		uint obj = (uint)msg._pExtraInfo;
		for (int i = 0; i < lstSyncServerTimeFinish.Count; i++)
		{
			lstSyncServerTimeFinish[i](obj);
		}
		lstSyncServerTimeFinish.Clear();
		return true;
	}

	public void FetchLevelAward(ReportMapBattleCmd cmd, Action<ReportMapBattleResultCmd> completionHandler)
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, cmd);
		_battleResult = completionHandler;
	}

	private bool NotifyMapBattleResult(TUITelegram msg)
	{
		ReportMapBattleResultCmd obj = msg._pExtraInfo as ReportMapBattleResultCmd;
		if (_battleResult != null)
		{
			_battleResult(obj);
			_battleResult = null;
		}
		return true;
	}

	private bool NotifyReqBattleResult_Debug(TUITelegram msg)
	{
		if (!_active_Debug_nofog)
		{
			return true;
		}
		ReqBattleResultCmd obj = msg._pExtraInfo as ReqBattleResultCmd;
		if (_dictReqBattleResult.ContainsKey(cur_debug_nofog))
		{
			_dictReqBattleResult[cur_debug_nofog](obj);
			cur_debug_nofog++;
			if (cur_debug_nofog >= 100)
			{
				_active_Debug_nofog = false;
			}
		}
		return true;
	}

	public void FetchDebug_NOFOG(int index, Action<ReqBattleResultCmd> completionHandler)
	{
		if (Instance.RPGData.m_mapPoint[index].m_status == 1 || Instance.RPGData.m_mapPoint[index].m_status == 3)
		{
			_active_Debug_nofog = true;
			if (_dictReqBattleResult.ContainsKey(index))
			{
				_dictReqBattleResult[index] = completionHandler;
			}
			else
			{
				_dictReqBattleResult.Add(index, completionHandler);
			}
			ReqBattleCmd reqBattleCmd = new ReqBattleCmd();
			reqBattleCmd.m_map_point = (byte)index;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, reqBattleCmd);
		}
		else
		{
			if (Instance.RPGData.m_mapPoint[index].m_status == 2)
			{
				ReqGainGoldCmd reqGainGoldCmd = new ReqGainGoldCmd();
				reqGainGoldCmd.m_map_point = (byte)cur_debug_nofog;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, reqGainGoldCmd);
			}
			completionHandler(null);
			cur_debug_nofog++;
		}
	}

	public void TryToAddFriend(uint id)
	{
		Debug.Log(id);
		if (Instance.GetFriendsCount() >= COMA_DataConfig.Instance._sysConfig.Friend.max_size)
		{
			UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("haoyoujiemian_desc12"));
			UIGolbalStaticFun.PopCommonMessageBox(data);
			return;
		}
		Debug.Log(id);
		UIMessage_CommonBoxData data2 = new UIMessage_CommonBoxData(1, Localization.instance.Get("haoyoujiemian_desc7"));
		UIGolbalStaticFun.PopCommonMessageBox(data2);
		if (!addedFriendIDs.Contains(id))
		{
			Debug.Log(id);
			addedFriendIDs.Add(id);
			RequestFriendCmd requestFriendCmd = new RequestFriendCmd();
			requestFriendCmd.m_who = id;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, requestFriendCmd);
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	protected override void Tick()
	{
		if (ReVerifyStep == EReVerifyStep.Sending && Time.time - _fSendingTime > 10f)
		{
			ReVerifyStep = EReVerifyStep.Confirmed;
			Debug.Log("Restart---------------Verify false!-------------------");
			Application.LoadLevel("COMA_Start");
		}
	}

	public object GetData(EDataType type)
	{
		if (type == EDataType.Role)
		{
			return _roleData;
		}
		return null;
	}

	public int GetFriendsCount()
	{
		if (_roleData == null)
		{
			return 0;
		}
		return _roleData.m_friend_list.Count;
	}

	public void ParseSaveData()
	{
		ParseSaveData(_roleData.m_info, _roleData.m_save_data, _roleData.m_bag_data);
	}

	private void ParseSaveData(RoleInfo info, byte[] data, BagData bagData)
	{
		for (int i = 0; i < COMA_Pref.Instance.TInPack.Length; i++)
		{
			COMA_Pref.Instance.TInPack[i] = -1;
		}
		for (int j = 0; j < COMA_Pref.Instance.AInPack.Length; j++)
		{
			COMA_Pref.Instance.AInPack[j] = -1;
		}
		Debug.Log("数据转换");
		COMA_Pref.Instance.playerID = info.m_player_id.ToString();
		COMA_Pref.Instance.nickname = info.m_name;
		COMA_Pref.Instance.lv = (int)info.m_level;
		COMA_Pref.Instance.SetGold((int)info.m_gold);
		COMA_Pref.Instance.SetCrystal((int)info.m_crystal);
		if (data != null)
		{
			COMA_Pref.Instance.data = Encoding.UTF8.GetString(data);
		}
		if (UIGolbalStaticFun.GetBagItemByID(info.m_head) == null)
		{
			info.m_head = 0uL;
		}
		if (UIGolbalStaticFun.GetBagItemByID(info.m_body) == null)
		{
			info.m_body = 0uL;
		}
		if (UIGolbalStaticFun.GetBagItemByID(info.m_leg) == null)
		{
			info.m_leg = 0uL;
		}
		int k = 0;
		for (int count = bagData.m_bag_list.Count; k < count; k++)
		{
			BagItem bagItem = bagData.m_bag_list[k];
			if (bagItem.m_unique_id == info.m_head || bagItem.m_unique_id == info.m_body || bagItem.m_unique_id == info.m_leg)
			{
				if (bagItem.m_unique_id == info.m_head)
				{
					COMA_Pref.Instance.TInPack[0] = k;
				}
				if (bagItem.m_unique_id == info.m_body)
				{
					COMA_Pref.Instance.TInPack[1] = k;
				}
				if (bagItem.m_unique_id == info.m_leg)
				{
					COMA_Pref.Instance.TInPack[2] = k;
				}
				COMA_PackageItem itemCom = new COMA_PackageItem();
				itemCom.serialName = bagItem.m_unit;
				FetchFileByMD5(itemCom.serialName, delegate(byte[] buffer)
				{
					if (buffer != null)
					{
						Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
						texture2D.LoadImage(buffer);
						texture2D.filterMode = FilterMode.Point;
						texture2D.Apply(false);
						itemCom.texture = texture2D;
					}
					else
					{
						itemCom.texture = UIGolbalStaticFun.CreateWhiteTexture();
					}
				});
				COMA_Pref.Instance.package.pack[k] = itemCom;
			}
			else
			{
				if (bagItem.m_unique_id == info.m_head_top)
				{
					COMA_Pref.Instance.AInPack[0] = k;
				}
				else if (bagItem.m_unique_id == info.m_head_front)
				{
					COMA_Pref.Instance.AInPack[1] = k;
				}
				else if (bagItem.m_unique_id == info.m_head_back)
				{
					COMA_Pref.Instance.AInPack[2] = k;
				}
				else if (bagItem.m_unique_id == info.m_head_left)
				{
					COMA_Pref.Instance.AInPack[3] = k;
				}
				else if (bagItem.m_unique_id == info.m_head_right)
				{
					COMA_Pref.Instance.AInPack[4] = k;
				}
				else if (bagItem.m_unique_id == info.m_chest_front)
				{
					COMA_Pref.Instance.AInPack[5] = k;
				}
				else if (bagItem.m_unique_id == info.m_chest_back)
				{
					COMA_Pref.Instance.AInPack[6] = k;
				}
				COMA_PackageItem cOMA_PackageItem = new COMA_PackageItem();
				cOMA_PackageItem.serialName = bagItem.m_unit;
				COMA_Pref.Instance.package.pack[k] = cOMA_PackageItem;
			}
		}
		if (info.m_head == 0L)
		{
			COMA_Pref.Instance.TInPack[0] = COMA_Package.maxCount;
			COMA_PackageItem cOMA_PackageItem2 = new COMA_PackageItem();
			cOMA_PackageItem2.serialName = "Head01";
			cOMA_PackageItem2.texture = UIGolbalStaticFun.CreateWhiteTexture();
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]] = cOMA_PackageItem2;
		}
		if (info.m_body == 0L)
		{
			COMA_Pref.Instance.TInPack[1] = COMA_Package.maxCount + 1;
			COMA_PackageItem cOMA_PackageItem3 = new COMA_PackageItem();
			cOMA_PackageItem3.serialName = "Body01";
			cOMA_PackageItem3.texture = UIGolbalStaticFun.CreateWhiteTexture();
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]] = cOMA_PackageItem3;
		}
		if (info.m_leg == 0L)
		{
			COMA_Pref.Instance.TInPack[2] = COMA_Package.maxCount + 2;
			COMA_PackageItem cOMA_PackageItem4 = new COMA_PackageItem();
			cOMA_PackageItem4.serialName = "Leg01";
			cOMA_PackageItem4.texture = UIGolbalStaticFun.CreateWhiteTexture();
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]] = cOMA_PackageItem4;
		}
		if (COMA_Pref.Instance.TInPack[0] == -1 || COMA_Pref.Instance.TInPack[1] == -1 || COMA_Pref.Instance.TInPack[2] == -1)
		{
			Debug.LogError("No Head, Body or Leg!!");
		}
	}

	public void InverseSaveData(string data)
	{
		_roleData.m_save_data = Encoding.UTF8.GetBytes(data);
	}

	public GetRankListResultCmd GetRank_World(string rankID)
	{
		if (_rankWorldData.ContainsKey(rankID))
		{
			return _rankWorldData[rankID];
		}
		return null;
	}

	public List<WorldRankAward> GetRankAward_World(string rankID)
	{
		if (_rankWorldAward.ContainsKey(rankID))
		{
			return _rankWorldAward[rankID];
		}
		return null;
	}

	public void FetchFileByMD5(string md5, Action<byte[]> completionHandler)
	{
		byte[] array = null;
		if (md5 == string.Empty)
		{
			Debug.Log("FetchFileByMD5 : NUll!!");
			completionHandler(null);
			return;
		}
		array = UITexCacherMgr.Instance.LoadTexBufferFromCache(md5);
		if (array != null)
		{
			completionHandler(array);
			return;
		}
		if (_dictFileData.ContainsKey(md5))
		{
			if (_dictFileData[md5] != null)
			{
				_dictFileData[md5].Add(completionHandler);
			}
			else
			{
				_dictFileData[md5] = new List<Action<byte[]>>();
				_dictFileData[md5].Add(completionHandler);
			}
		}
		else
		{
			List<Action<byte[]>> list = new List<Action<byte[]>>();
			list.Add(completionHandler);
			_dictFileData.Add(md5, list);
		}
		if (!_dicFetchMap.ContainsKey(md5))
		{
			GetFileDataCmd getFileDataCmd = new GetFileDataCmd();
			getFileDataCmd.m_md5 = md5;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, getFileDataCmd);
			_dicFetchMap.Add(md5, true);
		}
	}

	public void FetchTexture2DByMD5(string md5, Action<Texture2D> completionHandler)
	{
		FetchFileByMD5(md5, delegate(byte[] buffer)
		{
			if (buffer == null)
			{
				Debug.Log("--------FetchTexture2DByMD5----------");
				Texture2D obj = UIGolbalStaticFun.CreateWhiteTexture();
				completionHandler(obj);
			}
			else
			{
				Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
				texture2D.LoadImage(buffer);
				texture2D.filterMode = FilterMode.Point;
				texture2D.wrapMode = TextureWrapMode.Clamp;
				texture2D.Apply(false);
				completionHandler(texture2D);
			}
		});
	}

	public void FetchFacebookIconByTID(uint playerID, Action<Texture2D> completionHandler)
	{
		if (_playerIDToPlayerIconMD5.ContainsKey(playerID) && _playerIDToPlayerIconMD5[playerID] != null)
		{
			completionHandler(_playerIDToPlayerIconMD5[playerID]);
			return;
		}
		Debug.Log("Get Player Info : " + playerID);
		Instance.FetchPlayerProfile(playerID, delegate(WatchRoleInfo roleInfo)
		{
			if (roleInfo != null)
			{
				Debug.Log("Get Player Icon : " + roleInfo.m_face_image_md5);
				Instance.FetchFileByMD5(roleInfo.m_face_image_md5, delegate(byte[] buffer)
				{
					if (buffer != null)
					{
						Debug.Log("Get Player Icon Success!!");
						Texture2D texture2D = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
						texture2D.LoadImage(buffer);
						texture2D.filterMode = FilterMode.Point;
						texture2D.Apply(false);
						if (!_playerIDToPlayerIconMD5.ContainsKey(playerID))
						{
							_playerIDToPlayerIconMD5.Add(playerID, texture2D);
						}
						completionHandler(_playerIDToPlayerIconMD5[playerID]);
					}
					else
					{
						Debug.Log("Get Player Icon Fail!!");
						if (texDefault == null)
						{
							Debug.Log("null");
						}
						if (texDefault == null)
						{
							texDefault = Resources.Load("FBX/Player/FacebookIcon") as Texture2D;
						}
						completionHandler(texDefault);
					}
				});
			}
		});
	}

	public void FetchSuitByMD5(CSuitMD5 suitMd5, Action<List<byte[]>> completionHandler)
	{
		if (_dictSuitFun.ContainsKey(suitMd5))
		{
			_dictSuitFun[suitMd5] = completionHandler;
		}
		else
		{
			_dictSuitFun.Add(suitMd5, completionHandler);
		}
		if (_dictSuitData.ContainsKey(suitMd5))
		{
			_dictSuitData[suitMd5] = new List<byte[]>();
		}
		else
		{
			_dictSuitData.Add(suitMd5, new List<byte[]>());
		}
		_dictSuitData[suitMd5].Add(null);
		_dictSuitData[suitMd5].Add(null);
		_dictSuitData[suitMd5].Add(null);
		FetchFileByMD5(suitMd5._unit[0], delegate(byte[] data)
		{
			_dictSuitData[suitMd5][0] = data;
			if (_dictSuitData[suitMd5].Count == 3)
			{
				completionHandler(_dictSuitData[suitMd5]);
			}
		});
		FetchFileByMD5(suitMd5._unit[1], delegate(byte[] data)
		{
			_dictSuitData[suitMd5][1] = data;
			if (_dictSuitData[suitMd5].Count == 3)
			{
				completionHandler(_dictSuitData[suitMd5]);
			}
		});
		FetchFileByMD5(suitMd5._unit[2], delegate(byte[] data)
		{
			_dictSuitData[suitMd5][2] = data;
			if (_dictSuitData[suitMd5].Count == 3)
			{
				completionHandler(_dictSuitData[suitMd5]);
			}
		});
	}

	public void FetchPlayerSellList(uint id, Action<List<ShopItem>> completionHandler)
	{
		if (_dictPlayerSellList.ContainsKey(id))
		{
			_dictPlayerSellList[id] = completionHandler;
		}
		else
		{
			_dictPlayerSellList.Add(id, completionHandler);
		}
		GetRoleShopListCmd getRoleShopListCmd = new GetRoleShopListCmd();
		getRoleShopListCmd.m_who = id;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, getRoleShopListCmd);
	}

	public void FetchShopList(byte type, byte size, Action<List<ShopItem>> completionHandler)
	{
		if (_dictShopList.ContainsKey(type))
		{
			_dictShopList[type] = completionHandler;
		}
		else
		{
			Debug.Log("==============================Add FetchShopList Type=" + type);
			_dictShopList.Add(type, completionHandler);
		}
		Debug.Log("====FetchShopList Type=" + type);
		GetShopListCmd getShopListCmd = new GetShopListCmd();
		getShopListCmd.m_type = type;
		getShopListCmd.m_size = size;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, getShopListCmd);
	}

	public void FetchPlayerProfile(uint id, Action<WatchRoleInfo> completionHandler)
	{
		if (_dictPlayerProfileBuffer.ContainsKey(id))
		{
			Debug.Log("---------_dictPlayerProfileBuffer--------------");
			completionHandler(_dictPlayerProfileBuffer[id]);
			return;
		}
		if (_dictPlayerProfileList.ContainsKey(id))
		{
			Debug.Log("---------_dictPlayerProfileList--------------");
			_dictPlayerProfileList[id].Add(completionHandler);
			return;
		}
		Debug.Log("---------FetchPlayerProfile---else--------------");
		List<Action<WatchRoleInfo>> list = new List<Action<WatchRoleInfo>>();
		list.Add(completionHandler);
		_dictPlayerProfileList.Add(id, list);
		GetWatchInfoCmd getWatchInfoCmd = new GetWatchInfoCmd();
		getWatchInfoCmd.m_who = id;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, getWatchInfoCmd);
	}

	public void FetchPlayerRPGData(uint id, Action<PlayerRpgDataCmd> completionHandler)
	{
		if (_dictPlayerRPGBuffer.ContainsKey(id))
		{
			Debug.Log("FetchPlayerRPGData---Loc");
			completionHandler(_dictPlayerRPGBuffer[id]);
			return;
		}
		if (_dictPlayerPRGList.ContainsKey(id))
		{
			_dictPlayerPRGList[id].Add(completionHandler);
			return;
		}
		List<Action<PlayerRpgDataCmd>> list = new List<Action<PlayerRpgDataCmd>>();
		list.Add(completionHandler);
		_dictPlayerPRGList.Add(id, list);
		DragPlayerRpgDataCmd dragPlayerRpgDataCmd = new DragPlayerRpgDataCmd();
		dragPlayerRpgDataCmd.m_role_id = id;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, dragPlayerRpgDataCmd);
	}

	public void FollowSuccess(uint id)
	{
		if (_dicExtInfoBuffer.ContainsKey(id))
		{
			_dicExtInfoBuffer[id].m_fans_num++;
		}
	}

	public void UnFollowSuccess(uint id)
	{
		if (_dicExtInfoBuffer.ContainsKey(id))
		{
			_dicExtInfoBuffer[id].m_fans_num--;
		}
	}

	public void FetchPlayerExtInfoByID(uint id, Action<ExtInfo> completionHandler)
	{
		if (_dicExtInfoBuffer.ContainsKey(id))
		{
			completionHandler(_dicExtInfoBuffer[id]);
			return;
		}
		if (_dictPlayerExtInfoList.ContainsKey(id))
		{
			_dictPlayerExtInfoList[id].Add(completionHandler);
			return;
		}
		List<Action<ExtInfo>> list = new List<Action<ExtInfo>>();
		list.Add(completionHandler);
		_dictPlayerExtInfoList.Add(id, list);
		GetExtInfoCmd getExtInfoCmd = new GetExtInfoCmd();
		getExtInfoCmd.m_who = id;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, getExtInfoCmd);
	}

	public void FetchSelfExtInfoByID(Action<ExtInfo> completionHandler)
	{
		uint selfTID = UIGolbalStaticFun.GetSelfTID();
		if (_dicExtInfoBuffer.ContainsKey(selfTID))
		{
			_dicExtInfoBuffer.Remove(selfTID);
		}
		FetchPlayerExtInfoByID(selfTID, delegate(ExtInfo info)
		{
			completionHandler(info);
		});
	}

	public void FetchSelfCollectAvatarLst(Action<List<ShopItem>> completionHandler)
	{
		collectAvatarLstFun = completionHandler;
		GetRoleCollectListCmd extraInfo = new GetRoleCollectListCmd();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
	}

	public void UploadFile(ulong mark, byte[] buffer, Action<string> completionHandler)
	{
		if (_dictUploadFile.ContainsKey(mark))
		{
			_dictUploadFile[mark] = completionHandler;
		}
		else
		{
			_dictUploadFile.Add(mark, completionHandler);
		}
		SetFileDataCmd setFileDataCmd = new SetFileDataCmd();
		setFileDataCmd.m_data = buffer;
		setFileDataCmd.m_mark = mark;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, setFileDataCmd);
	}

	public bool IsMyFriend(uint otherPlayerID)
	{
		if (_roleData == null || _roleData.m_friend_list == null)
		{
			Debug.LogError("No data to search!!");
		}
		return _roleData.m_friend_list.Contains(otherPlayerID);
	}

	public void AddLobbySrvCloseReason(ELobbySrvColseReason reason)
	{
		lobbySrvCloseReason |= (int)reason;
	}

	public bool LobbySrvIsNoReasonClosed()
	{
		return LobbySrvCloseReason == 0;
	}

	public void FetchServerTime(Action<uint> completionHandler)
	{
		if (lstSyncServerTimeFinish.Count <= 0)
		{
			SyncServerTimeCmd syncServerTimeCmd = new SyncServerTimeCmd();
			syncServerTimeCmd.m_local_time = 0u;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, syncServerTimeCmd);
		}
		lstSyncServerTimeFinish.Add(completionHandler);
	}

	public void AddPlayerIDToBlockMap(uint id)
	{
		if (!_dictChatBlockMap.ContainsKey(id))
		{
			_dictChatBlockMap.Add(id, true);
		}
	}

	public void RemovePlayerIDFromBlockMap(uint id)
	{
		if (_dictChatBlockMap.ContainsKey(id))
		{
			_dictChatBlockMap.Remove(id);
		}
	}

	public bool IsPlayerIDInBlockMap(uint id)
	{
		return _dictChatBlockMap.ContainsKey(id);
	}

	public void GetGameModeNum()
	{
		GetAllModelNumCmd extraInfo = new GetAllModelNumCmd();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
	}

	public void GetAcceptFriendRequest()
	{
		GetForbidFriendRequestCmd extraInfo = new GetForbidFriendRequestCmd();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
	}

	public bool HasFirstLoginAwardTips()
	{
		return RPGFirstLoginAward_PerDay >= 0;
	}

	public bool IsExistTeamMem()
	{
		MemberSlot[] member_slot = Instance.RPGData.m_member_slot;
		int num = 0;
		for (int i = 0; i < member_slot.Length; i++)
		{
			if (member_slot[i].m_member != 0)
			{
				num++;
				break;
			}
		}
		if (num == 0)
		{
			return false;
		}
		return true;
	}

	public int GetTeamMem()
	{
		MemberSlot[] member_slot = Instance.RPGData.m_member_slot;
		int num = 0;
		for (int i = 0; i < member_slot.Length; i++)
		{
			if (member_slot[i].m_member != 0)
			{
				num++;
			}
		}
		return num;
	}
}
