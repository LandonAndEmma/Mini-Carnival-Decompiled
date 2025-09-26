using System.Collections.Generic;
using System.Text;
using LitJson;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Binary;
using Protocol.Mail.S2C;
using Protocol.Role;
using Protocol.Role.C2S;
using Protocol.Role.S2C;
using UnityEngine;

public class UILobby_RoleProtocolProcessor : UILobbyMessageHandler
{
	[SerializeField]
	private COMA_TexTransfer transferCom;

	protected override void Load()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.RegisterHanlder(1, this);
			OnMessage(71, OnSyncServerTime);
			OnMessage(1, OnLoginResult);
			OnMessage(12, OnRegisterResult);
			OnMessage(14, OnUploadRoleDataResult);
			OnMessage(4, OnDragConfigListResult);
			OnMessage(6, OnDragConfigFileResult);
			OnMessage(15, OnRoleData);
			OnMessage(32, OnAddItemToBagResult);
			OnMessage(33, OnNewItemInBag);
			OnMessage(37, OnDelItemResult);
			OnMessage(27, OnWatchInfoError);
			OnMessage(28, OnWatchInfoResult);
			OnMessage(23, OnNotifyHeartChanged);
			OnMessage(24, OnNotifyGoldChanged);
			OnMessage(25, OnNotifyCrystalChanged);
			OnMessage(53, OnSearchPlayerResult);
			OnMessage(63, OnPlayerExtInfoResult);
			OnMessage(48, OnResponseMakeFriendResult);
			OnMessage(50, OnNotifyFriendAddResult);
			OnMessage(51, OnNotifyFriendDelResult);
			OnMessage(30, OnNotifyChat);
			OnMessage(35, OnModifyBagItemResult);
			OnMessage(39, OnEquipedBagItemResult);
			OnMessage(2, OnKicked);
			OnMessage(41, OnPurchaseBagCellResult);
			OnMessage(42, OnNotifyBagCapacityChanged);
			OnMessage(9, OnFriendOnline);
			OnMessage(10, OnFriendOffline);
			OnMessage(67, OnNotifyRoleInvite);
			OnMessage(59, OnSearchRoleByFBIDResult);
			OnMessage(61, OnFacebookFriendsImport);
			OnMessage(44, OnEstablishFriendResult);
			OnMessage(72, NotifyGlobalMessage);
			OnMessage(77, OnGetGameModeNum);
			OnMessage(80, OnGetForbidFriendRequire);
		}
		RegisterMessage(EUIMessageID.UI_InputNameEnd_Register, this, InputNameEnd);
		RegisterMessage(EUIMessageID.UI_ArchiveCreate, this, CreateArchive);
		RegisterMessage(EUIMessageID.UI_ArchiveUpload, this, UploadArchive);
		RegisterMessage(EUIMessageID.UIRankings_LogFacebook, this, LogFacebook);
	}

	protected override void UnLoad()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.UnregisterHanlder(1, this);
		}
		UnregisterMessage(EUIMessageID.UI_InputNameEnd_Register, this);
		UnregisterMessage(EUIMessageID.UI_ArchiveCreate, this);
		UnregisterMessage(EUIMessageID.UI_ArchiveUpload, this);
		UnregisterMessage(EUIMessageID.UIRankings_LogFacebook, this);
	}

	public bool OnSyncServerTime(UnPacker unpacker)
	{
		Debug.Log(" ------OnSyncServerTime");
		SyncServerTimeResultCmd syncServerTimeResultCmd = new SyncServerTimeResultCmd();
		if (!syncServerTimeResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log(syncServerTimeResultCmd.m_server_time);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Role_ServerTimeSync, this, syncServerTimeResultCmd.m_server_time);
		return true;
	}

	public bool OnLoginResult(UnPacker unpacker)
	{
		COMA_Login.Instance.TipPercent(COMA_Login.PercentTips.ConnectLobbyServer_End);
		Debug.Log(" ------OnLoginResult");
		LoginResultCmd loginResultCmd = new LoginResultCmd();
		if (!loginResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (loginResultCmd.m_result == 0)
		{
			Debug.Log("Login success");
			COMA_Login.Instance.TipPercent(COMA_Login.PercentTips.Finish);
		}
		else if (loginResultCmd.m_result == 1)
		{
			if (COMA_Sys.Instance.version.StartsWith("999.997"))
			{
				CreateArchive(null);
			}
			else
			{
				GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Common/TexTransfer")) as GameObject;
			}
		}
		else if (loginResultCmd.m_result == 2)
		{
			Debug.Log("SRV FULL!!!");
		}
		DragConfigListCmd extraInfo = new DragConfigListCmd();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, extraInfo);
		UIDataBufferCenter.Instance.FetchServerTime(delegate(uint time)
		{
			Debug.Log("COMA_Server_Account.Instance.svrTime = " + time);
			COMA_Server_Account.Instance.svrTime = time;
			RPGGlobalClock.Instance.InitClock(time);
		});
		COMA_Login.Instance.bGameLoadFinishedForPauseLock = true;
		return true;
	}

	private bool CreateArchive(TUITelegram msg)
	{
		if (COMA_Carnival_Camera.Instance != null)
		{
			COMA_Carnival_Camera.Instance.HideTitle();
		}
		COMA_CommonOperation.Instance.defaultInput = COMA_Sys.Instance.GetDeviceName();
		COMA_CommonOperation.Instance.inputKind = InputBoardCategory.Register;
		Application.LoadLevelAdditive("UI.InputName");
		return true;
	}

	private bool UploadArchive(TUITelegram msg)
	{
		COMA_Login.Instance.ChangeConnectingTip(COMA_Login.Instance.tip_uploadContent);
		Debug.Log(" ------------------------------------------------------------------------------- ");
		Debug.Log(COMA_Pref.Instance.nickname);
		UploadRoleDataCmd uploadRoleDataCmd = new UploadRoleDataCmd();
		uploadRoleDataCmd.m_name = COMA_Pref.Instance.nickname;
		uploadRoleDataCmd.m_level = (uint)COMA_Pref.Instance.lv;
		uploadRoleDataCmd.m_gold = (uint)COMA_Pref.Instance.GetGold();
		uploadRoleDataCmd.m_crystal = (uint)COMA_Pref.Instance.GetCrystal();
		if (!COMA_Pref.Instance.HasBought(0))
		{
			uploadRoleDataCmd.m_first_buy_head = 1;
		}
		if (!COMA_Pref.Instance.HasBought(1))
		{
			uploadRoleDataCmd.m_first_buy_body = 1;
		}
		if (!COMA_Pref.Instance.HasBought(2))
		{
			uploadRoleDataCmd.m_first_buy_leg = 1;
		}
		uploadRoleDataCmd.m_head = (byte)COMA_Pref.Instance.TInPack[0];
		uploadRoleDataCmd.m_body = (byte)COMA_Pref.Instance.TInPack[1];
		uploadRoleDataCmd.m_leg = (byte)COMA_Pref.Instance.TInPack[2];
		uploadRoleDataCmd.m_head_top = (byte)COMA_Pref.Instance.AInPack[0];
		uploadRoleDataCmd.m_head_front = (byte)COMA_Pref.Instance.AInPack[1];
		uploadRoleDataCmd.m_head_back = (byte)COMA_Pref.Instance.AInPack[2];
		uploadRoleDataCmd.m_head_left = (byte)COMA_Pref.Instance.AInPack[3];
		uploadRoleDataCmd.m_head_right = (byte)COMA_Pref.Instance.AInPack[4];
		uploadRoleDataCmd.m_chest_front = (byte)COMA_Pref.Instance.AInPack[5];
		uploadRoleDataCmd.m_chest_back = (byte)COMA_Pref.Instance.AInPack[6];
		uploadRoleDataCmd.m_bag_capacity = (byte)COMA_Package.slotUnlocked;
		for (int i = 0; i < 72; i++)
		{
			COMA_PackageItem cOMA_PackageItem = COMA_Pref.Instance.package.pack[i];
			BagItem bagItem = new BagItem();
			if (cOMA_PackageItem != null)
			{
				bagItem.m_unique_id = 1uL;
				if (cOMA_PackageItem.serialName == "Head01")
				{
					bagItem.m_part = 1;
				}
				else if (cOMA_PackageItem.serialName == "Body01")
				{
					bagItem.m_part = 2;
				}
				else if (cOMA_PackageItem.serialName == "Leg01")
				{
					bagItem.m_part = 3;
				}
				else
				{
					bagItem.m_part = 4;
				}
				if (cOMA_PackageItem.num >= 100)
				{
					bagItem.m_state = 2;
				}
				else if (cOMA_PackageItem.num >= 99)
				{
					bagItem.m_state = 1;
				}
				else
				{
					bagItem.m_state = 0;
				}
				if (cOMA_PackageItem.textureName != string.Empty)
				{
					COMA_TexTransfer.TexNode texNodeByTextureName = COMA_TexTransfer.Instance.GetTexNodeByTextureName(cOMA_PackageItem.textureName);
					bagItem.m_unit = texNodeByTextureName.md5;
				}
				else
				{
					bagItem.m_unit = cOMA_PackageItem.serialName;
				}
			}
			uploadRoleDataCmd.m_bag[i] = bagItem;
		}
		List<string> friends = COMA_TexTransfer.Instance.GetFriends();
		for (int j = 0; j < 30; j++)
		{
			if (j < friends.Count)
			{
				uploadRoleDataCmd.m_friend_list[j] = uint.Parse(friends[j]);
			}
			else
			{
				uploadRoleDataCmd.m_friend_list[j] = 0u;
			}
		}
		uploadRoleDataCmd.m_save = Encoding.UTF8.GetBytes(COMA_Pref.Instance.data);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, uploadRoleDataCmd);
		return true;
	}

	public bool OnUploadRoleDataResult(UnPacker unpacker)
	{
		UploadRoleDataResultCmd uploadRoleDataResultCmd = new UploadRoleDataResultCmd();
		if (!uploadRoleDataResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (uploadRoleDataResultCmd.m_result == 0)
		{
			COMA_TexTransfer.Instance.ClearTempFiles();
			Debug.Log("upload success!");
			Application.LoadLevel("COMA_Start");
		}
		else
		{
			Debug.Log("upload failure!");
		}
		return true;
	}

	public bool OnSearchRoleByFBIDResult(UnPacker unpacker)
	{
		SearchRoleByFBInfoResultCmd searchRoleByFBInfoResultCmd = new SearchRoleByFBInfoResultCmd();
		if (!searchRoleByFBInfoResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log(searchRoleByFBInfoResultCmd.m_result);
		if (searchRoleByFBInfoResultCmd.m_result != string.Empty)
		{
			JsonData[] array = JsonMapper.ToObject<JsonData[]>(searchRoleByFBInfoResultCmd.m_result);
			foreach (JsonData jsonData in array)
			{
				Debug.LogWarning(string.Concat("fb:", jsonData["fb"], " tid:", jsonData["tid"]));
				string s = jsonData["tid"].ToString();
				uint num = uint.Parse(s);
				if (UIGolbalStaticFun.GetSelfTID() != num && !UIDataBufferCenter.Instance.IsMyFriend(num))
				{
					EstablishFriendCmd establishFriendCmd = new EstablishFriendCmd();
					establishFriendCmd.m_who = num;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, establishFriendCmd);
				}
			}
		}
		return true;
	}

	private bool InputNameEnd(TUITelegram msg)
	{
		Debug.Log("register now: " + (string)msg._pExtraInfo);
		RegisterCmd registerCmd = new RegisterCmd();
		registerCmd.m_name = (string)msg._pExtraInfo;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, registerCmd);
		return true;
	}

	public bool OnRegisterResult(UnPacker unpacker)
	{
		RegisterResultCmd registerResultCmd = new RegisterResultCmd();
		if (!registerResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (registerResultCmd.m_result == 0)
		{
			Debug.Log("register success!");
		}
		else
		{
			Debug.Log("register failure!");
		}
		return true;
	}

	private bool IsOldConfig(string name)
	{
		if (name == "ShopAtlas.xml" || name == "params.xml")
		{
			return true;
		}
		return false;
	}

	public bool OnDragConfigListResult(UnPacker unpacker)
	{
		Debug.Log(" ------Drag Config List Success");
		DragConfigListResultCmd dragConfigListResultCmd = new DragConfigListResultCmd();
		if (!dragConfigListResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("__Drag Success");
		foreach (FileDigest item in dragConfigListResultCmd.lstFileDigest)
		{
			string text = string.Empty;
			if (IsOldConfig(item._fileName))
			{
				text = COMA_FileIO.LoadFile(item._fileName);
			}
			else if (UIGolbalStaticFun.IsLevelConfig(item._fileName))
			{
				int levelIDByFileName = UIGolbalStaticFun.GetLevelIDByFileName(item._fileName);
				if (levelIDByFileName >= 1 && levelIDByFileName <= 100)
				{
					if (levelIDByFileName == 1)
					{
						text = COMA_FileIO.LoadFile("Levels", item._fileName);
					}
					UIDataBufferCenter.Instance.RpgConfig_LevelValid[levelIDByFileName - 1] = item._md5;
					continue;
				}
			}
			else
			{
				text = COMA_FileIO.LoadFile("Configs", item._fileName);
			}
			if (text == string.Empty || COMA_FileIO.GetFileMD5(text) != item._md5)
			{
				DragConfigFileCmd dragConfigFileCmd = new DragConfigFileCmd();
				dragConfigFileCmd.fileName = item._fileName;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, dragConfigFileCmd);
			}
			else
			{
				ParseConfigFile(item._fileName, text);
			}
		}
		return true;
	}

	public bool OnDragConfigFileResult(UnPacker unpacker)
	{
		Debug.Log(" ------Drag Config File Success");
		DragConfigFileResultCmd dragConfigFileResultCmd = new DragConfigFileResultCmd();
		if (!dragConfigFileResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("__Drag Success:" + dragConfigFileResultCmd.fileName);
		string content = Encoding.UTF8.GetString(dragConfigFileResultCmd.data);
		if (IsOldConfig(dragConfigFileResultCmd.fileName))
		{
			COMA_FileIO.SaveFile(dragConfigFileResultCmd.fileName, content);
		}
		else if (UIGolbalStaticFun.IsLevelConfig(dragConfigFileResultCmd.fileName))
		{
			COMA_FileIO.SaveFile("Levels", dragConfigFileResultCmd.fileName, content);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRPG_LevelConfigReady, this, null);
		}
		else
		{
			COMA_FileIO.SaveFile("Configs", dragConfigFileResultCmd.fileName, content);
		}
		ParseConfigFile(dragConfigFileResultCmd.fileName, content);
		return true;
	}

	private void ParseConfigFile(string fileName, string content)
	{
		if (COMA_Carnival_Camera.Instance == null)
		{
			return;
		}
		int totleNeed = 21;
		switch (fileName)
		{
		case "ShopAtlas.xml":
			COMA_DataConfig.Instance.ParseSysShop(content);
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		case "params.xml":
			COMA_DataConfig.Instance.ParseSysConfig(content);
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		case "AttackEffect.xml":
		{
			string pXmlizedString8 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.AttackEffectPool = COMA_Tools.DeserializeObject<RPGAttackEffectUnitPool>(pXmlizedString8) as RPGAttackEffectUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "BeAttackEffect.xml":
		{
			string pXmlizedString7 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.BeAttackEffectPool = COMA_Tools.DeserializeObject<RPGBeAttackEffectUnitPool>(pXmlizedString7) as RPGBeAttackEffectUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "BuffEffect.xml":
		{
			string pXmlizedString5 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.BuffEffectPool = COMA_Tools.DeserializeObject<RPGBuffEffectUnitPool>(pXmlizedString5) as RPGBuffEffectUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "CareerAnis.xml":
		{
			string pXmlizedString4 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.CareerAniPool = COMA_Tools.DeserializeObject<RPGCareerAnimationUnitPool>(pXmlizedString4) as RPGCareerAnimationUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "Careers.xml":
		{
			string pXmlizedString16 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.CareerUnitPool = COMA_Tools.DeserializeObject<RPGCareerUnitPool>(pXmlizedString16) as RPGCareerUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "CompoundFee.xml":
		{
			string pXmlizedString15 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.CompoundFeePool = COMA_Tools.DeserializeObject<RPGCompoundFeeUnit>(pXmlizedString15) as RPGCompoundFeeUnit;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "GemCompoundTable.xml":
		{
			string pXmlizedString14 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.CompoundTableUnitPool = COMA_Tools.DeserializeObject<RPGCompoundTableUnitPool>(pXmlizedString14) as RPGCompoundTableUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "GemDefine.xml":
		{
			string pXmlizedString13 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.GemDefineUnitPool = COMA_Tools.DeserializeObject<RPGGemDefineUnitPool>(pXmlizedString13) as RPGGemDefineUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "GemDrop.xml":
		{
			string pXmlizedString12 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.GemDropPool = COMA_Tools.DeserializeObject<RPGGemDropUnitPool>(pXmlizedString12) as RPGGemDropUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "GemShop.xml":
		{
			string pXmlizedString11 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.RpgGemShop = COMA_Tools.DeserializeObject<RPGGemShopUnitPool>(pXmlizedString11) as RPGGemShopUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "Level_Scene.xml":
		{
			string pXmlizedString10 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.Level_Scene = COMA_Tools.DeserializeObject<RPGLevel_SceneUnitPool>(pXmlizedString10) as RPGLevel_SceneUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "LevelIncome.csv":
		{
			string text3 = COMA_FileIO.LoadFile("Configs", fileName);
			string[] array5 = text3.Split(';');
			for (int k = 0; k < array5.Length; k++)
			{
				string[] array6 = array5[k].Split(',');
				LevelIncome levelIncome = new LevelIncome();
				levelIncome._lv = int.Parse(array6[0]);
				levelIncome._gold = int.Parse(array6[1]);
				levelIncome._exp = int.Parse(array6[2]);
				RPGGlobalData.Instance.LstLevelIncome.Add(levelIncome);
			}
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "MapLayout.xml":
		{
			string pXmlizedString9 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.MapLayout = COMA_Tools.DeserializeObject<RPGMapLayoutUnitPool>(pXmlizedString9) as RPGMapLayoutUnitPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "MAXEXP.csv":
		{
			string text2 = COMA_FileIO.LoadFile("Configs", fileName);
			string[] array3 = text2.Split(';');
			for (int j = 0; j < array3.Length; j++)
			{
				string[] array4 = array3[j].Split(',');
				RPGMaxExp rPGMaxExp = new RPGMaxExp();
				rPGMaxExp._lv_min = int.Parse(array4[0]);
				rPGMaxExp._lv_max = int.Parse(array4[1]);
				rPGMaxExp._exp = int.Parse(array4[2]);
				RPGGlobalData.Instance.LstRPGMaxExp.Add(rPGMaxExp);
			}
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "MiscUnit.xml":
		{
			string pXmlizedString6 = COMA_FileIO.LoadFile("Configs", fileName);
			Debug.Log("--------Before MiscUnit!!");
			RPGGlobalData.Instance.RpgMiscUnit = COMA_Tools.DeserializeObject<RPGMiscUnit>(pXmlizedString6) as RPGMiscUnit;
			Debug.Log("--------End MiscUnit!!");
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "pointincome.csv":
		{
			string text = COMA_FileIO.LoadFile("Configs", fileName);
			string[] array = text.Split(';');
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(',');
				OccupyPointIncome occupyPointIncome = new OccupyPointIncome();
				occupyPointIncome._lv = int.Parse(array2[0]);
				occupyPointIncome._income = int.Parse(array2[1]);
				RPGGlobalData.Instance.LstOccupyPointIncome.Add(occupyPointIncome);
			}
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "RPGSkills.xml":
		{
			string pXmlizedString3 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.SkillUnitPool = COMA_Tools.DeserializeObject<RPGSkillUnitPool>(pXmlizedString3) as RPGSkillUnitPool;
			Debug.Log("Skill Count=" + RPGGlobalData.Instance.SkillUnitPool._dict.Count);
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "SpecialLevelDrop.xml":
		{
			string pXmlizedString2 = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.SpecLevelDrop = COMA_Tools.DeserializeObject<RPGSpeciaLevelDropPool>(pXmlizedString2) as RPGSpeciaLevelDropPool;
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		case "Tactics.xml":
		{
			string pXmlizedString = COMA_FileIO.LoadFile("Configs", fileName);
			RPGGlobalData.Instance.TacticUnitPool = COMA_Tools.DeserializeObject<RPGTacticUnitPool>(pXmlizedString) as RPGTacticUnitPool;
			Debug.Log("Tactic Count=" + RPGGlobalData.Instance.TacticUnitPool._dict.Count);
			COMA_Carnival_Camera.Instance.ConfigReady(totleNeed);
			break;
		}
		}
	}

	public bool OnRoleData(UnPacker unpacker)
	{
		Debug.Log("---Receive data!");
		NotifyRoleDataCmd notifyRoleDataCmd = new NotifyRoleDataCmd();
		if (!notifyRoleDataCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		RoleInfo info = notifyRoleDataCmd.m_info;
		BagData bag_data = notifyRoleDataCmd.m_bag_data;
		Debug.Log("Bag capacity=" + bag_data.m_bag_capacity);
		List<BagItem> bag_list = bag_data.m_bag_list;
		for (int i = 0; i < notifyRoleDataCmd.m_friend_list.Count; i++)
		{
			Debug.Log("Friend ID:" + notifyRoleDataCmd.m_friend_list[i]);
		}
		Debug.Log(info.m_head + " " + info.m_body + " " + info.m_leg);
		foreach (BagItem item in notifyRoleDataCmd.m_bag_data.m_bag_list)
		{
			if (item.m_part == 1 && item.m_unit == string.Empty && item.m_state == 0)
			{
				Texture2D texture2D = Resources.Load<Texture2D>("FBX/Player/Character/Texture/T_head");
				item.m_unit = "6ba2377776d6c137ee29551baff81bb5";
				UITexCacherMgr.Instance.InsertTexToCache(item.m_unit, texture2D.EncodeToPNG());
			}
			else if (item.m_part == 2 && item.m_unit == string.Empty && item.m_state == 0)
			{
				Texture2D texture2D2 = Resources.Load<Texture2D>("FBX/Player/Character/Texture/T_body");
				item.m_unit = "54245d0a0b0c5c8305976247da71f59f";
				UITexCacherMgr.Instance.InsertTexToCache(item.m_unit, texture2D2.EncodeToPNG());
			}
			else if (item.m_part == 3 && item.m_unit == string.Empty && item.m_state == 0)
			{
				Texture2D texture2D3 = Resources.Load<Texture2D>("FBX/Player/Character/Texture/T_Leg");
				item.m_unit = "9a53aef61db65e1ed1298fca0cc15a3d";
				UITexCacherMgr.Instance.InsertTexToCache(item.m_unit, texture2D3.EncodeToPNG());
			}
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_SetData, this, notifyRoleDataCmd);
		COMA_Achievement.Instance.CheckFriendsAchievement();
		COMA_Carnival_Camera.Instance.VersionReady();
		COMA_Login.Instance.ConnectGameServer();
		UIDataBufferCenter.Instance.GetGameModeNum();
		UIDataBufferCenter.Instance.GetAcceptFriendRequest();
		return true;
	}

	private bool LogFacebook(TUITelegram msg)
	{
		Debug.Log("Start    ===LogFaceBook");
		UIFacebookFeedback.Instance.LoginFacebook();
		return true;
	}

	public bool OnFacebookFriendsImport(UnPacker unpacker)
	{
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		notifyRoleDataCmd.m_hasInportFB = 1;
		return true;
	}

	public bool OnAddItemToBagResult(UnPacker unpacker)
	{
		Debug.Log("---OnAddItemToBagResult!");
		AddBagItemResultCmd addBagItemResultCmd = new AddBagItemResultCmd();
		if (!addBagItemResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (addBagItemResultCmd.m_result == 0)
		{
			Debug.Log("Add Item to bag success!");
		}
		else
		{
			string str = Localization.instance.Get("youxiang_desc4");
			UIGolbalStaticFun.PopupTipsBox(str);
			Debug.Log("Add Item to bag failure!");
		}
		return true;
	}

	public bool OnNewItemInBag(UnPacker unpacker)
	{
		Debug.Log("---OnNewItemInBag!");
		NotifyBagAddCmd notifyBagAddCmd = new NotifyBagAddCmd();
		if (!notifyBagAddCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("-------------------------New Item in bag!");
		BagItem item = notifyBagAddCmd.m_item;
		Debug.Log("id=" + item.m_unique_id);
		Debug.Log("part=" + (BagItem.Part)item.m_part);
		Debug.Log("m_state=" + (BagItem.State)item.m_state);
		Debug.Log("m_unit=" + item.m_unit);
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		BagData bag_data = notifyRoleDataCmd.m_bag_data;
		if (bag_data != null)
		{
			if (bag_data.m_bag_list == null)
			{
				bag_data.m_bag_list = new List<BagItem>();
			}
			bag_data.m_bag_list.Add(item);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyBagAddCmd, UIDataBufferCenter.ERoleDataType.BagData);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_NewItemInBag, this, item);
			UIGolbalStaticFun.PopupTipsBox(Localization.instance.Get("youxiang_desc3"));
		}
		else
		{
			Debug.LogError("bagData is null!");
		}
		return true;
	}

	public bool OnDelItemResult(UnPacker unpacker)
	{
		Debug.Log("---OnDelItemResult!");
		DelBagItemResultCmd delBagItemResultCmd = new DelBagItemResultCmd();
		if (!delBagItemResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("id=" + delBagItemResultCmd.m_unique_id);
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_DiscardItem);
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		BagData bag_data = notifyRoleDataCmd.m_bag_data;
		if (bag_data != null)
		{
			bool flag = false;
			if (bag_data.m_bag_list == null)
			{
				bag_data.m_bag_list = new List<BagItem>();
			}
			for (int i = 0; i < bag_data.m_bag_list.Count; i++)
			{
				if (delBagItemResultCmd.m_unique_id == bag_data.m_bag_list[i].m_unique_id)
				{
					bag_data.m_bag_list.RemoveAt(i);
					RoleInfo info = notifyRoleDataCmd.m_info;
					if (info.m_head == delBagItemResultCmd.m_unique_id)
					{
						info.m_head = 0uL;
					}
					if (info.m_body == delBagItemResultCmd.m_unique_id)
					{
						info.m_body = 0uL;
					}
					if (info.m_leg == delBagItemResultCmd.m_unique_id)
					{
						info.m_leg = 0uL;
					}
					if (info.m_head_top == delBagItemResultCmd.m_unique_id)
					{
						info.m_head_top = 0uL;
					}
					if (info.m_head_front == delBagItemResultCmd.m_unique_id)
					{
						info.m_head_front = 0uL;
					}
					if (info.m_head_back == delBagItemResultCmd.m_unique_id)
					{
						info.m_head_back = 0uL;
					}
					if (info.m_head_left == delBagItemResultCmd.m_unique_id)
					{
						info.m_head_left = 0uL;
					}
					if (info.m_head_right == delBagItemResultCmd.m_unique_id)
					{
						info.m_head_right = 0uL;
					}
					if (info.m_chest_front == delBagItemResultCmd.m_unique_id)
					{
						info.m_chest_front = 0uL;
					}
					if (info.m_chest_back == delBagItemResultCmd.m_unique_id)
					{
						info.m_chest_back = 0uL;
					}
					flag = true;
					break;
				}
			}
			if (flag)
			{
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.BagData);
			}
		}
		else
		{
			Debug.LogError("bagData is null!");
		}
		return true;
	}

	public bool OnWatchInfoError(UnPacker unpacker)
	{
		GetWatchInfoErrorCmd getWatchInfoErrorCmd = new GetWatchInfoErrorCmd();
		if (!getWatchInfoErrorCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("---OnWatchInfoError! ID=" + getWatchInfoErrorCmd.m_who);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_NotifyCurSelShopItemAttribute, this, null);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RemoteWatchInfoArrived, this, null, getWatchInfoErrorCmd);
		return true;
	}

	public bool OnWatchInfoResult(UnPacker unpacker)
	{
		Debug.Log("---OnWatchInfoResult!");
		NotifyWatchInfoCmd notifyWatchInfoCmd = new NotifyWatchInfoCmd();
		if (!notifyWatchInfoCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RemoteWatchInfoArrived, this, notifyWatchInfoCmd.m_info);
		return true;
	}

	public bool OnNotifyHeartChanged(UnPacker unpacker)
	{
		Debug.Log("---OnNotifyHeartChanged!");
		NotifyHeartCmd notifyHeartCmd = new NotifyHeartCmd();
		if (!notifyHeartCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		RoleInfo info = notifyRoleDataCmd.m_info;
		info.m_heart = notifyHeartCmd.m_heart;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
		return true;
	}

	public bool OnNotifyGoldChanged(UnPacker unpacker)
	{
		Debug.Log("---OnNotifyGoldChanged!");
		NotifyGoldCmd notifyGoldCmd = new NotifyGoldCmd();
		if (!notifyGoldCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		RoleInfo info = notifyRoleDataCmd.m_info;
		int num = (int)(notifyGoldCmd.m_gold - info.m_gold);
		if (num > 0 && !Application.loadedLevelName.StartsWith("COMA_Scene_RPG"))
		{
			UIGetItemBoxData data = new UIGetItemBoxData(UIGetItemBoxData.EGetItemType.Gold, num);
			UIGolbalStaticFun.PopGetItemBox(data);
		}
		info.m_gold = notifyGoldCmd.m_gold;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
		return true;
	}

	public bool OnNotifyCrystalChanged(UnPacker unpacker)
	{
		Debug.Log("---OnNotifyCrystalChanged!");
		NotifyCrystalCmd notifyCrystalCmd = new NotifyCrystalCmd();
		if (!notifyCrystalCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		RoleInfo info = notifyRoleDataCmd.m_info;
		int num = (int)(notifyCrystalCmd.m_crystal - info.m_crystal);
		if (num > 0)
		{
			UIGetItemBoxData data = new UIGetItemBoxData(UIGetItemBoxData.EGetItemType.Crystal, num);
			UIGolbalStaticFun.PopGetItemBox(data);
		}
		info.m_crystal = notifyCrystalCmd.m_crystal;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
		return true;
	}

	public bool OnSearchPlayerResult(UnPacker unpacker)
	{
		Debug.Log("---OnSearchPlayerResult!");
		SearchPlayerResultCmd searchPlayerResultCmd = new SearchPlayerResultCmd();
		if (!searchPlayerResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_SearchFriendFinish, this, searchPlayerResultCmd);
		return true;
	}

	public bool OnPlayerExtInfoResult(UnPacker unpacker)
	{
		Debug.Log("---OnPlayerExtInfoResult!");
		GetExtInfoResultCmd getExtInfoResultCmd = new GetExtInfoResultCmd();
		if (!getExtInfoResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_RemotePlayerExtInfoArrived, this, getExtInfoResultCmd);
		return true;
	}

	public bool OnResponseMakeFriendResult(UnPacker unpacker)
	{
		Debug.Log("---OnResponseMakeFriendResult!");
		ResponseFriendResultCmd responseFriendResultCmd = new ResponseFriendResultCmd();
		if (!responseFriendResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("**********OnResponseMakeFriendResult = " + (ResponseFriendResultCmd.Code)responseFriendResultCmd.m_result);
		if (responseFriendResultCmd.m_result == 0)
		{
			string str = Localization.instance.Get("haoyoujiemian_desc17");
			UIGolbalStaticFun.PopupTipsBox(str);
			DragMailListResultCmd mailBufferInfo = UIDataBufferCenter.Instance.MailBufferInfo;
			for (int i = 0; i < mailBufferInfo.m_mail_list.Count; i++)
			{
				if (mailBufferInfo.m_mail_list[i].m_id == responseFriendResultCmd.m_mail_id)
				{
					mailBufferInfo.m_mail_list[i].m_status = 2;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, mailBufferInfo);
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_AcceptMailResult, this, responseFriendResultCmd.m_mail_id);
					break;
				}
			}
			COMA_Achievement.Instance.CheckFriendsAchievement();
		}
		else if (responseFriendResultCmd.m_result == 1)
		{
			string str2 = Localization.instance.Get("haoyoujiemian_desc12");
			UIGolbalStaticFun.PopupTipsBox(str2);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_AcceptMailResult, this, responseFriendResultCmd.m_mail_id);
		}
		else if (responseFriendResultCmd.m_result == 2)
		{
			string str3 = Localization.instance.Get("haoyoujiemian_desc16");
			UIGolbalStaticFun.PopupTipsBox(str3);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_AcceptMailResult, this, responseFriendResultCmd.m_mail_id);
		}
		else if (responseFriendResultCmd.m_result == 3)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_AcceptMailResult, this, responseFriendResultCmd.m_mail_id);
		}
		return true;
	}

	public bool OnNotifyFriendAddResult(UnPacker unpacker)
	{
		Debug.Log("---OnNotifyFriendAddResult!");
		NotifyFriendAddCmd notifyFriendAddCmd = new NotifyFriendAddCmd();
		if (!notifyFriendAddCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log(notifyFriendAddCmd.m_who);
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		if (!notifyRoleDataCmd.m_friend_list.Contains(notifyFriendAddCmd.m_who))
		{
			notifyRoleDataCmd.m_friend_list.Add(notifyFriendAddCmd.m_who);
		}
		COMA_Achievement.Instance.CheckFriendsAchievement();
		return true;
	}

	public bool OnNotifyFriendDelResult(UnPacker unpacker)
	{
		Debug.Log("---OnNotifyFriendDelResult!");
		NotifyFriendDelCmd notifyFriendDelCmd = new NotifyFriendDelCmd();
		if (!notifyFriendDelCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log(notifyFriendDelCmd.m_who);
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		if (notifyRoleDataCmd.m_friend_list.Contains(notifyFriendDelCmd.m_who))
		{
			notifyRoleDataCmd.m_friend_list.Remove(notifyFriendDelCmd.m_who);
		}
		if (UIDataBufferCenter.Instance.Online_friend_list.Contains(notifyFriendDelCmd.m_who))
		{
			UIDataBufferCenter.Instance.Online_friend_list.Remove(notifyFriendDelCmd.m_who);
		}
		return true;
	}

	public bool OnNotifyChat(UnPacker unpacker)
	{
		Debug.Log("---OnNotifyChat!");
		NotifyChatCmd notifyChatCmd = new NotifyChatCmd();
		if (!notifyChatCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UISquare_SpawnNewChatRecord, this, notifyChatCmd);
		return true;
	}

	public bool OnModifyBagItemResult(UnPacker unpacker)
	{
		Debug.Log("---OnModifyBagItemResult!");
		ModifyBagItemResultCmd modifyBagItemResultCmd = new ModifyBagItemResultCmd();
		if (!modifyBagItemResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (modifyBagItemResultCmd.m_result == 0)
		{
			Debug.Log("Modify Bag data success.");
			NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
			int i = 0;
			for (int count = notifyRoleDataCmd.m_bag_data.m_bag_list.Count; i < count; i++)
			{
				if (notifyRoleDataCmd.m_bag_data.m_bag_list[i].m_unique_id == UIDataBufferCenter.Instance.SelectBoxDataForDesign.ItemId)
				{
					notifyRoleDataCmd.m_bag_data.m_bag_list[i].m_unit = UIDataBufferCenter.Instance.SelectBoxDataForDesign.Unit;
					notifyRoleDataCmd.m_bag_data.m_bag_list[i].m_state = 2;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.BagData);
					break;
				}
			}
		}
		else
		{
			Debug.LogError("Modify Bag data fail!");
		}
		return true;
	}

	public bool OnEquipedBagItemResult(UnPacker unpacker)
	{
		Debug.Log("---OnEquipedBagItemResult!");
		MountBagItemResultCmd mountBagItemResultCmd = new MountBagItemResultCmd();
		if (!mountBagItemResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("OnEquipedBagItemResult=" + (MountBagItemResultCmd.Code)mountBagItemResultCmd.m_result);
		UIAutoDelBlockOnlyMessageBoxMgr.Instance.ReleaseAutoDelBlockOnlyMessageBox();
		if (mountBagItemResultCmd.m_result == 1)
		{
			Debug.LogError("OnEquipedBagItemResult:Error!");
		}
		ulong mount_id = mountBagItemResultCmd.m_mount_id;
		int num = -1;
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		RoleInfo info = notifyRoleDataCmd.m_info;
		if (mount_id == 0L)
		{
			switch (mountBagItemResultCmd.m_mount_part)
			{
			case 7:
				num = 1;
				break;
			case 8:
				num = 2;
				break;
			case 9:
				num = 3;
				break;
			default:
				num = 4;
				break;
			}
			Debug.Log("UnEquiped: Part=" + num);
		}
		else
		{
			Debug.Log("Confirm Equiped:" + mount_id);
			BagData bag_data = notifyRoleDataCmd.m_bag_data;
			for (int i = 0; i < bag_data.m_bag_list.Count; i++)
			{
				if (mount_id == bag_data.m_bag_list[i].m_unique_id)
				{
					num = bag_data.m_bag_list[i].m_part;
					break;
				}
			}
		}
		switch (num)
		{
		case 1:
			if (info.m_head != mount_id)
			{
				info.m_head = mount_id;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
			}
			break;
		case 2:
			if (info.m_body != mount_id)
			{
				info.m_body = mount_id;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
			}
			break;
		case 3:
			if (info.m_leg != mount_id)
			{
				info.m_leg = mount_id;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
			}
			break;
		case 4:
			switch ((MountBagItemCmd.Part)mountBagItemResultCmd.m_mount_part)
			{
			case MountBagItemCmd.Part.head_top:
				if (info.m_head_top != mount_id)
				{
					info.m_head_top = mount_id;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
				}
				break;
			case MountBagItemCmd.Part.head_front:
				if (info.m_head_front != mount_id)
				{
					info.m_head_front = mount_id;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
				}
				break;
			case MountBagItemCmd.Part.head_back:
				if (info.m_head_back != mount_id)
				{
					info.m_head_back = mount_id;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
				}
				break;
			case MountBagItemCmd.Part.head_left:
				if (info.m_head_left != mount_id)
				{
					info.m_head_left = mount_id;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
				}
				break;
			case MountBagItemCmd.Part.head_right:
				if (info.m_head_right != mount_id)
				{
					info.m_head_right = mount_id;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
				}
				break;
			case MountBagItemCmd.Part.chest_front:
				if (info.m_chest_front != mount_id)
				{
					info.m_chest_front = mount_id;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
				}
				break;
			case MountBagItemCmd.Part.chest_back:
				if (info.m_chest_back != mount_id)
				{
					info.m_chest_back = mount_id;
					UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIDataBuffer_DataDirty, this, notifyRoleDataCmd, UIDataBufferCenter.ERoleDataType.RoleInfo);
				}
				break;
			}
			break;
		}
		return true;
	}

	public bool OnKicked(UnPacker unpacker)
	{
		Debug.Log("---OnKicked!");
		string str = Localization.instance.Get("fengmian_lianjietishi23");
		UIGolbalStaticFun.PopupTipsBox(str);
		return true;
	}

	public bool OnPurchaseBagCellResult(UnPacker unpacker)
	{
		Debug.Log("---OnPurchaseBagCellResult!");
		BuyBagCellResultCmd buyBagCellResultCmd = new BuyBagCellResultCmd();
		if (!buyBagCellResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIGolbalStaticFun.CloseBlockOnlyMessageBox();
		Debug.Log("OnPurchaseBagCellResult:" + (BuyBagCellResultCmd.Code)buyBagCellResultCmd.m_result);
		if (buyBagCellResultCmd.m_result == 0)
		{
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIBackpack_UnlockBagCellSuccess1, this, null);
			Debug.Log("PurchaseBagCell success.");
		}
		else if (buyBagCellResultCmd.m_result == 1)
		{
			Debug.Log("kNoEnough");
			UIGolbalStaticFun.PopMsgBox_LackMoney();
		}
		else if (buyBagCellResultCmd.m_result == 2)
		{
			Debug.Log("kMax");
		}
		return true;
	}

	public bool OnNotifyBagCapacityChanged(UnPacker unpacker)
	{
		Debug.Log("---OnNotifyBagCapacityChanged!");
		NotifyBagCapacityCmd notifyBagCapacityCmd = new NotifyBagCapacityCmd();
		if (!notifyBagCapacityCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		BagData bag_data = notifyRoleDataCmd.m_bag_data;
		bag_data.m_bag_capacity = notifyBagCapacityCmd.m_capacity;
		Debug.Log("New Capacity = " + bag_data.m_bag_capacity);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_UnlockBagCellSuccess, this, bag_data.m_bag_capacity);
		return true;
	}

	public bool OnFriendOnline(UnPacker unpacker)
	{
		Debug.Log("---OnFriendOnline!");
		NotifyFriendOnlineCmd notifyFriendOnlineCmd = new NotifyFriendOnlineCmd();
		if (!notifyFriendOnlineCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("^^^^^^^   " + notifyFriendOnlineCmd.m_who + " Online!");
		if (!UIDataBufferCenter.Instance.Online_friend_list.Contains(notifyFriendOnlineCmd.m_who))
		{
			UIDataBufferCenter.Instance.Online_friend_list.Add(notifyFriendOnlineCmd.m_who);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OnlineFriendsChanged, this, UIDataBufferCenter.Instance.Online_friend_list);
		return true;
	}

	public bool OnFriendOffline(UnPacker unpacker)
	{
		Debug.Log("---OnFriendOffline!");
		NotifyFriendOfflineCmd notifyFriendOfflineCmd = new NotifyFriendOfflineCmd();
		if (!notifyFriendOfflineCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("^^^^^^^   " + notifyFriendOfflineCmd.m_who + " Offline!");
		if (UIDataBufferCenter.Instance.Online_friend_list.Contains(notifyFriendOfflineCmd.m_who))
		{
			UIDataBufferCenter.Instance.Online_friend_list.Remove(notifyFriendOfflineCmd.m_who);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OnlineFriendsChanged, this, UIDataBufferCenter.Instance.Online_friend_list);
		return true;
	}

	public bool OnNotifyRoleInvite(UnPacker unpacker)
	{
		Debug.Log("---OnNotifyRoleInvite!");
		NotifyRoleInviteCmd notifyRoleInviteCmd = new NotifyRoleInviteCmd();
		if (!notifyRoleInviteCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (COMA_CommonOperation.Instance.CanBeInvited(Application.loadedLevelName))
		{
			string[] array = notifyRoleInviteCmd.m_param.Split('|');
			int roomId = int.Parse(array[0]);
			int sceneID = int.Parse(array[1]);
			UIJoinGameMessageBoxData data = new UIJoinGameMessageBoxData(notifyRoleInviteCmd.m_whoname, sceneID, roomId);
			UIGolbalStaticFun.PopJoinGameMessageBox(data);
		}
		return true;
	}

	public bool OnEstablishFriendResult(UnPacker unpacker)
	{
		Debug.Log("---OnEstablishFriendResult!");
		EstablishFriendResultCmd establishFriendResultCmd = new EstablishFriendResultCmd();
		if (!establishFriendResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (establishFriendResultCmd.m_result == 0)
		{
			Debug.Log("Success Establish Friend:" + establishFriendResultCmd.m_who);
			COMA_Achievement.Instance.CheckFriendsAchievement();
		}
		return true;
	}

	public bool NotifyGlobalMessage(UnPacker unpacker)
	{
		Debug.Log("---NotifyGlobalMessage!");
		NotifyGlobalMessageCmd notifyGlobalMessageCmd = new NotifyGlobalMessageCmd();
		if (!notifyGlobalMessageCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("Global Notify：" + notifyGlobalMessageCmd.m_msg);
		UIGolbalStaticFun.PopupTipsBox(notifyGlobalMessageCmd.m_msg, true);
		return true;
	}

	public bool OnGetGameModeNum(UnPacker unpacker)
	{
		Debug.Log("---OnGetGameModeNum!");
		GetAllModelNumResultCmd getAllModelNumResultCmd = new GetAllModelNumResultCmd();
		if (!getAllModelNumResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIDataBufferCenter.Instance._gameModeNum = getAllModelNumResultCmd._dict;
		return true;
	}

	public bool OnGetForbidFriendRequire(UnPacker unpacker)
	{
		Debug.Log("---OnGetForbidFriendRequire!");
		GetForbidFriendRequestResultCmd getForbidFriendRequestResultCmd = new GetForbidFriendRequestResultCmd();
		if (!getForbidFriendRequestResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Debug.Log("Mode Count：" + getForbidFriendRequestResultCmd.m_val);
		if (getForbidFriendRequestResultCmd.m_val == 0)
		{
			UIDataBufferCenter.Instance._bAcceptFriendRequest = true;
		}
		else
		{
			UIDataBufferCenter.Instance._bAcceptFriendRequest = false;
		}
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
