using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using MessageID;
using Protocol.Role.C2S;
using Protocol.Role.S2C;
using UnityEngine;

public class COMA_Pref
{
	private static COMA_Pref _instance;

	private string defaultFileName = COMA_FileNameManager.Instance.GetFileName("Pref");

	private char sepSign = '|';

	public string playerID = string.Empty;

	private int[] bought = new int[3];

	public string[] TID = new string[3]
	{
		string.Empty,
		string.Empty,
		string.Empty
	};

	public int[] TInPack = new int[3] { 0, 1, 2 };

	public int[] AInPack = new int[7] { -1, -1, -1, -1, -1, -1, -1 };

	private string _nickname = string.Empty;

	private int[] _rankScore = new int[11];

	private int key1 = 19119322;

	private int key2 = 1164404561;

	public int lvMax = 50;

	private List<int> _lv = new List<int>();

	private int _lvID;

	public int[] expLv = new int[50]
	{
		2200, 2420, 2662, 2928, 3221, 3543, 3897, 4287, 4715, 5187,
		5706, 6276, 6904, 7594, 8354, 9189, 10108, 11119, 12231, 13454,
		14800, 16280, 17908, 19699, 21669, 23836, 26219, 28841, 31726, 34898,
		38388, 42227, 46450, 51095, 56204, 61825, 68007, 74808, 82289, 90518,
		99570, 109527, 120480, 132528, 145780, 160359, 176394, 194034, 213437, 99999999
	};

	private List<int> _exp = new List<int>();

	private int _expID;

	private List<int> _gold = new List<int>();

	private int _goldID;

	private List<int> _crystal = new List<int>();

	private int _crystalID;

	public COMA_Package package = new COMA_Package();

	public int priceToUnlockSellSlot = 1000;

	private List<int> _sellSlotCount = new List<int>();

	private int _sellSlotCountID;

	public int bNew_guidelinesId;

	public bool bNew_MainMenu = true;

	public bool bNew_Package = true;

	public bool bNew_BuyAvatar = true;

	public bool bNew_BuyModel = true;

	public bool bNew_SellSingle = true;

	public bool bNew_SellSuit = true;

	public bool NG2_FirstEnterSquare = true;

	public bool NG2_FirstEnterFriends = true;

	public bool NG2_FirstEnterMarket = true;

	public bool NG2_FirstEnterMarketTmp = true;

	public bool NG2_FirstEnterBackpackEdit = true;

	public bool NG2_FirstEnterSellItem = true;

	public bool NG2_1_FirstEnterRPGGame = true;

	public bool NG2_1_FirstEnterSmallGame = true;

	public bool NG2_1_FirstEnterGemCombine = true;

	public bool NG2_1_FirstEnterEnhance = true;

	public bool NG2_1_FirstBackToMap = true;

	public bool NG2_1_FirstEnterRPGBag = true;

	public bool NG2_1_FirstRPGBag_Click = true;

	public bool NG2_1_FirstEnterSquare = true;

	public bool NG2_1_FirstEnterTeam = true;

	public bool NG2_1_FirstEnterMap = true;

	public bool NG2_1_FirstInTeam;

	public int controlMode_Run = -1;

	public int maxFriends;

	public int mode_flappy_bestScore;

	public int mode_flappy_weekScore;

	public string versionRated = "0.0.0";

	private char dataSign = '^';

	public static COMA_Pref Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_Pref();
				_instance.InitData();
			}
			return _instance;
		}
	}

	public string contentForClientSave
	{
		get
		{
			string text = playerID;
			text = text + sepSign + TID[0];
			text = text + sepSign + TID[1];
			text = text + sepSign + TID[2];
			text = text + sepSign + nickname;
			text = text + sepSign + lv.ToString();
			text = text + sepSign + exp.ToString();
			text = text + sepSign + gold.ToString();
			text = text + sepSign + crystal.ToString();
			text = text + sepSign + package.content;
			return text + sepSign + data;
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				string[] array = value.Split(sepSign);
				int num = 0;
				playerID = array[num++];
				TID[0] = array[num++];
				TID[1] = array[num++];
				TID[2] = array[num++];
				nickname = array[num++];
				lv = int.Parse(array[num++]);
				exp = int.Parse(array[num++]);
				gold = int.Parse(array[num++]);
				crystal = int.Parse(array[num++]);
				package.content = array[num++];
				data = array[num++];
			}
		}
	}

	public string contentForServerSave
	{
		get
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("token", COMA_Server_Archive.Instance.TK);
			hashtable.Add("uuid", playerID);
			hashtable.Add("TID0", TID[0]);
			hashtable.Add("TID1", TID[1]);
			hashtable.Add("TID2", TID[2]);
			hashtable.Add("Name", nickname);
			hashtable.Add("Lv", lv);
			hashtable.Add("Exp", exp);
			hashtable.Add("Gold", gold);
			hashtable.Add("Crystal", crystal);
			hashtable.Add("Package", package.content);
			hashtable.Add("Data", data);
			return JsonMapper.ToJson(hashtable);
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				JsonData jsonData = JsonMapper.ToObject<JsonData>(value);
				TID[0] = jsonData["TID0"].ToString();
				TID[1] = jsonData["TID1"].ToString();
				TID[2] = jsonData["TID2"].ToString();
				nickname = jsonData["Name"].ToString();
				lv = int.Parse(jsonData["Lv"].ToString());
				exp = int.Parse(jsonData["Exp"].ToString());
				gold = int.Parse(jsonData["Gold"].ToString());
				crystal = int.Parse(jsonData["Crystal"].ToString());
				package.content = jsonData["Package"].ToString();
				data = jsonData["Data"].ToString();
			}
		}
	}

	public string nickname
	{
		get
		{
			return _nickname;
		}
		set
		{
			_nickname = value;
		}
	}

	public int lv
	{
		get
		{
			return Dec(_lv[_lvID]);
		}
		set
		{
			if (value > lvMax)
			{
				value = lvMax;
			}
			_lvID = Random.Range(0, _lv.Count);
			_lv[_lvID] = Enc(value);
		}
	}

	public int exp
	{
		get
		{
			return Dec(_exp[_expID]);
		}
		set
		{
			if (lv >= lvMax)
			{
				_expID = Random.Range(0, _exp.Count);
				_exp[_expID] = Enc(0);
				return;
			}
			int num = lv;
			while (value >= expLv[lv - 1])
			{
				value -= expLv[lv - 1];
				lv++;
			}
			_expID = Random.Range(0, _exp.Count);
			_exp[_expID] = Enc(value);
			if (UIDataBufferCenter.Instance != null && UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role) != null && num != lv)
			{
				ModifyLevelCmd modifyLevelCmd = new ModifyLevelCmd();
				modifyLevelCmd.m_level = (uint)lv;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, modifyLevelCmd);
				UIDataBufferCenter.Instance.playerInfo.m_level = (uint)lv;
			}
			if (COMA_HTTP_DataCollect.Instance != null)
			{
				COMA_HTTP_DataCollect.Instance.SendLevel(num.ToString(), lv.ToString());
			}
		}
	}

	private int gold
	{
		get
		{
			return Dec(_gold[_goldID]);
		}
		set
		{
			_goldID = Random.Range(0, _gold.Count);
			_gold[_goldID] = Enc(value);
		}
	}

	private int crystal
	{
		get
		{
			return Dec(_crystal[_crystalID]);
		}
		set
		{
			_crystalID = Random.Range(0, _crystal.Count);
			_crystal[_crystalID] = Enc(value);
		}
	}

	public int sellSlotCount
	{
		get
		{
			return Dec(_sellSlotCount[_sellSlotCountID]);
		}
		set
		{
			_sellSlotCountID = Random.Range(0, _sellSlotCount.Count);
			_sellSlotCount[_sellSlotCountID] = Enc(value);
		}
	}

	public string data
	{
		get
		{
			string text = sellSlotCount.ToString();
			text = text + dataSign + TInPack[0].ToString();
			text = text + dataSign + TInPack[1].ToString();
			text = text + dataSign + TInPack[2].ToString();
			text = text + dataSign + AInPack[0].ToString();
			text = text + dataSign + AInPack[1].ToString();
			text = text + dataSign + AInPack[2].ToString();
			text = text + dataSign + AInPack[3].ToString();
			text = text + dataSign + AInPack[4].ToString();
			text = text + dataSign + AInPack[5].ToString();
			text = text + dataSign + AInPack[6].ToString();
			text = text + dataSign + COMA_Achievement.Instance.content;
			text = text + dataSign + COMA_TexOnSale.Instance.content;
			text = text + dataSign + COMA_Sys.Instance.bFirstGame.ToString();
			text = text + dataSign + COMA_Sys.Instance.playTimes.ToString();
			text = text + dataSign + COMA_PaintBase.Instance.content;
			text = text + dataSign + COMA_AudioManager.Instance.content;
			text = text + dataSign + bought[0].ToString();
			text = text + dataSign + bought[1].ToString();
			text = text + dataSign + bought[2].ToString();
			text = text + dataSign + _rankScore[0].ToString();
			text = text + dataSign + _rankScore[1].ToString();
			text = text + dataSign + _rankScore[2].ToString();
			text = text + dataSign + _rankScore[3].ToString();
			text = text + dataSign + _rankScore[4].ToString();
			text = text + dataSign + _rankScore[5].ToString();
			text = text + dataSign + bNew_MainMenu.ToString();
			text = text + dataSign + bNew_Package.ToString();
			text = text + dataSign + bNew_BuyAvatar.ToString();
			text = text + dataSign + bNew_BuyModel.ToString();
			text = text + dataSign + bNew_SellSingle.ToString();
			text = text + dataSign + bNew_SellSuit.ToString();
			text = text + dataSign + maxFriends.ToString();
			text = text + dataSign + controlMode_Run.ToString();
			text = text + dataSign + COMA_Fishing.Instance.content;
			text = text + dataSign + COMA_FishCatalog.Instance.content;
			text = text + dataSign + COMA_Gift_Heart.Instance.content;
			text = text + dataSign + COMA_Fishing.Instance.bFished0.ToString();
			text = text + dataSign + COMA_Fishing.Instance.bFished1.ToString();
			text = text + dataSign + COMA_Fishing.Instance.bFished2.ToString();
			text = text + dataSign + COMA_Gift_Appreciation.Instance.Count.ToString();
			text = text + dataSign + mode_flappy_bestScore.ToString();
			text = text + dataSign + exp.ToString();
			text = text + dataSign + ((!NG2_FirstEnterSquare) ? "0" : "1");
			text = text + dataSign + ((!NG2_FirstEnterFriends) ? "0" : "1");
			text = text + dataSign + ((!NG2_FirstEnterMarket) ? "0" : "1");
			text = text + dataSign + ((!NG2_FirstEnterMarketTmp) ? "0" : "1");
			text = text + dataSign + ((!NG2_FirstEnterBackpackEdit) ? "0" : "1");
			text = text + dataSign + ((!NG2_FirstEnterSellItem) ? "0" : "1");
			text = text + dataSign + versionRated;
			text = text + dataSign + ((!NG2_1_FirstEnterRPGGame) ? "0" : "1");
			text = text + dataSign + ((!NG2_1_FirstEnterSmallGame) ? "0" : "1");
			text = text + dataSign + ((!NG2_1_FirstEnterGemCombine) ? "0" : "1");
			text = text + dataSign + ((!NG2_1_FirstBackToMap) ? "0" : "1");
			text = text + dataSign + ((!NG2_1_FirstEnterEnhance) ? "0" : "1");
			text = text + dataSign + ((!NG2_1_FirstEnterRPGBag) ? "0" : "1");
			text = text + dataSign + ((!NG2_1_FirstRPGBag_Click) ? "0" : "1");
			text = text + dataSign + ((!NG2_1_FirstEnterSquare) ? "0" : "1");
			text = text + dataSign + ((!NG2_1_FirstEnterTeam) ? "0" : "1");
			return text + dataSign + ((!NG2_1_FirstEnterMap) ? "0" : "1");
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				string[] array = value.Split(dataSign);
				int num = 0;
				sellSlotCount = int.Parse(array[num++]);
				num++;
				num++;
				num++;
				num++;
				num++;
				num++;
				num++;
				num++;
				num++;
				num++;
				COMA_Achievement.Instance.content = array[num++];
				COMA_TexOnSale.Instance.content = array[num++];
				COMA_Sys.Instance.bFirstGame = bool.Parse(array[num++]);
				COMA_Sys.Instance.playTimes = int.Parse(array[num++]);
				COMA_PaintBase.Instance.content = array[num++];
				COMA_AudioManager.Instance.content = array[num++];
				bought[0] = int.Parse(array[num++]);
				bought[1] = int.Parse(array[num++]);
				bought[2] = int.Parse(array[num++]);
				num++;
				num++;
				num++;
				num++;
				num++;
				num++;
				if (array.Length > num)
				{
					bNew_MainMenu = bool.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					bNew_Package = bool.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					bNew_BuyAvatar = bool.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					bNew_BuyModel = bool.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					bNew_SellSingle = bool.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					bNew_SellSuit = bool.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					maxFriends = int.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					controlMode_Run = int.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					COMA_Fishing.Instance.content = array[num++];
				}
				if (array.Length > num)
				{
					COMA_FishCatalog.Instance.content = array[num++];
				}
				if (array.Length > num)
				{
					COMA_Gift_Heart.Instance.content = array[num++];
				}
				if (array.Length > num)
				{
					COMA_Fishing.Instance.bFished0 = int.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					COMA_Fishing.Instance.bFished1 = int.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					COMA_Fishing.Instance.bFished2 = int.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					COMA_Gift_Appreciation.Instance.Count = int.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					mode_flappy_bestScore = int.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					exp = int.Parse(array[num++]);
				}
				if (array.Length > num)
				{
					NG2_FirstEnterSquare = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_FirstEnterFriends = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_FirstEnterMarket = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_FirstEnterMarketTmp = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_FirstEnterBackpackEdit = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_FirstEnterSellItem = array[num++] == "1";
				}
				if (array.Length > num)
				{
					versionRated = array[num++];
				}
				if (array.Length > num)
				{
					NG2_1_FirstEnterRPGGame = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_1_FirstEnterSmallGame = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_1_FirstEnterGemCombine = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_1_FirstBackToMap = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_1_FirstEnterEnhance = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_1_FirstEnterRPGBag = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_1_FirstRPGBag_Click = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_1_FirstEnterSquare = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_1_FirstEnterTeam = array[num++] == "1";
				}
				if (array.Length > num)
				{
					NG2_1_FirstEnterMap = array[num++] == "1";
				}
			}
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public string LoadPlayerIDFromLocal()
	{
		string text = COMA_FileIO.LoadFile(defaultFileName);
		if (text == string.Empty)
		{
			return string.Empty;
		}
		string[] array = text.Split(sepSign);
		return array[0];
	}

	public string LoadNickNameFromLocal()
	{
		string text = COMA_FileIO.LoadFile(defaultFileName);
		if (text == string.Empty)
		{
			return string.Empty;
		}
		string[] array = text.Split(sepSign);
		return array[4];
	}

	public void Load()
	{
		Debug.LogWarning("Load Pref");
		string text = COMA_FileIO.LoadFile(defaultFileName);
		if (text == string.Empty)
		{
			playerID = string.Empty;
			contentForClientSave = string.Empty;
			COMA_IAPCheck.Instance.content = string.Empty;
			return;
		}
		int num = text.IndexOf('&');
		if (num < 0)
		{
			Debug.LogError("需要有&符号来分割iap验证信息");
		}
		COMA_IAPCheck.Instance.content = text.Substring(num + 1);
	}

	public void SaveLocal()
	{
		Save(false);
	}

	public void Save(bool bNeedUpload)
	{
		Debug.LogWarning("Save Pref : " + bNeedUpload);
		COMA_FileIO.SaveFile(defaultFileName, "&" + COMA_IAPCheck.Instance.content);
		ReplaceSaveDataCmd replaceSaveDataCmd = new ReplaceSaveDataCmd();
		replaceSaveDataCmd.m_save_data = Encoding.UTF8.GetBytes(data);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, replaceSaveDataCmd);
		UIDataBufferCenter.Instance.InverseSaveData(data);
		Debug.LogWarning("发送data");
	}

	public void ServerSetPackageTexture(int i)
	{
		if (package.pack[i] == null)
		{
			Debug.LogError("Current Item is Not Exist!!");
		}
		else if (package.pack[i].part <= 0)
		{
			package.pack[i].tid = playerID + "_" + i;
			string content = COMA_TexBase.Instance.TextureBytesToString(package.pack[i].texture.EncodeToPNG());
			COMA_Server_Texture.Instance.TexturePackage_Set(package.pack[i].tid, content);
		}
	}

	public void DownLoadAPackageTexture(string id, int i)
	{
		COMA_Server_Texture.Instance.TexturePackage_Get(id, i);
	}

	public void UpPlayerTexturesInit()
	{
		if (package.pack[0].texture == null)
		{
			package.pack[0].LoadPNGDefault(0);
		}
		if (package.pack[1].texture == null)
		{
			package.pack[1].LoadPNGDefault(1);
		}
		if (package.pack[2].texture == null)
		{
			package.pack[2].LoadPNGDefault(2);
		}
	}

	public void UpPlayerTextureUpdate()
	{
		if (COMA_Sys.Instance.IsOfficialEditor)
		{
			COMA_Server_Texture.Instance.TextureOfficial_UpdateTextureToServer(COMA_TexBase.Instance.TextureBytesToString(package.pack[TInPack[0]].texture.EncodeToPNG()), COMA_TexBase.Instance.TextureBytesToString(package.pack[TInPack[1]].texture.EncodeToPNG()), COMA_TexBase.Instance.TextureBytesToString(package.pack[TInPack[2]].texture.EncodeToPNG()), 5000);
		}
	}

	public void BoughtChanged(int i, UIMarket_AvatarShopData data)
	{
		if (!HasBought(i))
		{
			bought[i] = 1;
			data.AvatarPrice *= 4;
		}
	}

	public bool HasBought(int i)
	{
		return bought[i] > 0;
	}

	public void SetRankScore(int score, int id)
	{
		if (id == 6)
		{
			_rankScore[id] = 0;
		}
		else
		{
			_rankScore[id] = score;
		}
	}

	public void AddRankScoreOfCurrentScene(int score)
	{
		Debug.Log("AddRankScoreOfCurrentScene:" + score);
		int num = COMA_CommonOperation.Instance.SceneNameToID(Application.loadedLevelName);
		if (num >= 0)
		{
			_rankScore[num] += score;
			if (_rankScore[num] < 0)
			{
				_rankScore[num] = 0;
			}
			COMA_Server_Rank.Instance.SubmitScore(COMA_Server_ID.Instance.GID, score, COMA_CommonOperation.Instance.SceneNameToRankID(), nickname);
		}
	}

	public int GetRankScoreOfCurrentScene()
	{
		int num = COMA_CommonOperation.Instance.SceneNameToID(Application.loadedLevelName);
		if (num < 0)
		{
			return 0;
		}
		return _rankScore[num];
	}

	public int GetRankScoreOfSceneID(int i)
	{
		if (i < 0)
		{
			return 0;
		}
		return _rankScore[i];
	}

	private int Enc(int c)
	{
		int num = c;
		num *= 2;
		num ^= key1;
		return num ^ key2;
	}

	private int Dec(int c)
	{
		int num = c;
		num ^= key2;
		num ^= key1;
		return num / 2;
	}

	public void SetGold(int value)
	{
		gold = value;
	}

	public int GetGold()
	{
		return gold;
	}

	public void AddGold(int div)
	{
		gold += div;
		ModifyGoldCmd modifyGoldCmd = new ModifyGoldCmd();
		modifyGoldCmd.m_op = 1;
		modifyGoldCmd.m_val = div;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, modifyGoldCmd);
	}

	public void SetCrystal(int value)
	{
		crystal = value;
	}

	public int GetCrystal()
	{
		return crystal;
	}

	public void AddCrystal(int div)
	{
		crystal += div;
		ModifyCrystalCmd modifyCrystalCmd = new ModifyCrystalCmd();
		modifyCrystalCmd.m_op = 1;
		modifyCrystalCmd.m_val = div;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, modifyCrystalCmd);
	}

	public void LoadTexturesOfPackageItem()
	{
	}

	public bool IsNeedNewGuideMask()
	{
		return bNew_MainMenu || bNew_Package || bNew_BuyAvatar || bNew_BuyModel || bNew_SellSingle;
	}

	public bool BShowRateButton()
	{
		if (!COMA_Sys.Instance.bRateActive)
		{
			return false;
		}
		return COMA_Login.Instance.NeedUpdateVersion(versionRated, MiscPlugin.GetAppVersion());
	}

	public void SetRated()
	{
		versionRated = MiscPlugin.GetAppVersion();
		Save(true);
	}

	public void InitData()
	{
		for (int i = 0; i < 16; i++)
		{
			int item = 1;
			_lv.Add(item);
			int item2 = 0;
			_exp.Add(item2);
			int item3 = 0;
			_gold.Add(item3);
			int item4 = 0;
			_crystal.Add(item4);
			int item5 = 0;
			_sellSlotCount.Add(item5);
			int num = Random.Range(0, 5);
			for (int j = 0; j < num; j++)
			{
				int num2 = 52013;
			}
		}
		lv = 1;
		exp = 0;
		gold = 0;
		crystal = 0;
		sellSlotCount = 1;
		package.pack[0] = new COMA_PackageItem();
		package.pack[0].serialName = "Head01";
		package.pack[0].itemName = "Head";
		package.pack[0].num = -1;
		package.pack[0].tid = string.Empty;
		package.pack[0].state = COMA_PackageItem.PackageItemStatus.Equiped;
		package.pack[0].textureName = COMA_FileNameManager.Instance.GetFileName("Head01");
		package.pack[1] = new COMA_PackageItem();
		package.pack[1].serialName = "Body01";
		package.pack[1].itemName = "Body";
		package.pack[1].num = -1;
		package.pack[1].tid = string.Empty;
		package.pack[1].state = COMA_PackageItem.PackageItemStatus.Equiped;
		package.pack[1].textureName = COMA_FileNameManager.Instance.GetFileName("Body01");
		package.pack[2] = new COMA_PackageItem();
		package.pack[2].serialName = "Leg01";
		package.pack[2].itemName = "Feet";
		package.pack[2].num = -1;
		package.pack[2].tid = string.Empty;
		package.pack[2].state = COMA_PackageItem.PackageItemStatus.Equiped;
		package.pack[2].textureName = COMA_FileNameManager.Instance.GetFileName("Leg01");
	}

	public void ResetData_Partful()
	{
		COMA_PackageItem[] array = new COMA_PackageItem[3]
		{
			new COMA_PackageItem(),
			null,
			null
		};
		array[0].serialName = "Head01";
		array[0].itemName = "Head";
		array[0].num = -1;
		array[0].tid = string.Empty;
		array[0].state = COMA_PackageItem.PackageItemStatus.Equiped;
		array[0].textureName = COMA_FileNameManager.Instance.GetFileName("Head01");
		array[0].LoadPNGDefault(0);
		array[1] = new COMA_PackageItem();
		array[1].serialName = "Body01";
		array[1].itemName = "Body";
		array[1].num = -1;
		array[1].tid = string.Empty;
		array[1].state = COMA_PackageItem.PackageItemStatus.Equiped;
		array[1].textureName = COMA_FileNameManager.Instance.GetFileName("Body01");
		array[1].LoadPNGDefault(1);
		array[2] = new COMA_PackageItem();
		array[2].serialName = "Leg01";
		array[2].itemName = "Feet";
		array[2].num = -1;
		array[2].tid = string.Empty;
		array[2].state = COMA_PackageItem.PackageItemStatus.Equiped;
		array[2].textureName = COMA_FileNameManager.Instance.GetFileName("Leg01");
		array[2].LoadPNGDefault(2);
		package.Clear();
		for (int i = 0; i < 3; i++)
		{
			package.pack[i] = array[i];
		}
		for (int j = 0; j < 3; j++)
		{
			TInPack[j] = j;
		}
		for (int k = 0; k < 7; k++)
		{
			AInPack[k] = -1;
		}
		Save(true);
	}

	public void ResetData()
	{
		lv = 1;
		exp = 0;
		gold = 0;
		sellSlotCount = 1;
		COMA_PackageItem[] array = new COMA_PackageItem[3]
		{
			new COMA_PackageItem(),
			null,
			null
		};
		array[0].serialName = "Head01";
		array[0].itemName = "Head";
		array[0].num = -1;
		array[0].tid = string.Empty;
		array[0].state = COMA_PackageItem.PackageItemStatus.Equiped;
		array[0].textureName = COMA_FileNameManager.Instance.GetFileName("Head01");
		array[0].LoadPNGDefault(0);
		array[1] = new COMA_PackageItem();
		array[1].serialName = "Body01";
		array[1].itemName = "Body";
		array[1].num = -1;
		array[1].tid = string.Empty;
		array[1].state = COMA_PackageItem.PackageItemStatus.Equiped;
		array[1].textureName = COMA_FileNameManager.Instance.GetFileName("Body01");
		array[1].LoadPNGDefault(1);
		array[2] = new COMA_PackageItem();
		array[2].serialName = "Leg01";
		array[2].itemName = "Feet";
		array[2].num = -1;
		array[2].tid = string.Empty;
		array[2].state = COMA_PackageItem.PackageItemStatus.Equiped;
		array[2].textureName = COMA_FileNameManager.Instance.GetFileName("Leg01");
		array[2].LoadPNGDefault(2);
		package.Clear();
		for (int i = 0; i < 3; i++)
		{
			package.pack[i] = array[i];
		}
		for (int j = 0; j < 3; j++)
		{
			TInPack[j] = j;
		}
		for (int k = 0; k < 7; k++)
		{
			AInPack[k] = -1;
		}
		COMA_TexOnSale.Instance.ResetData();
		COMA_Achievement.Instance.ResetData();
		Save(true);
	}

	public bool IsPackageFull()
	{
		if (PackageNullCount() > 0)
		{
			return false;
		}
		return true;
	}

	public int PackageNullCount()
	{
		int num = 0;
		NotifyRoleDataCmd notifyRoleDataCmd = (NotifyRoleDataCmd)UIDataBufferCenter.Instance.GetData(UIDataBufferCenter.EDataType.Role);
		return notifyRoleDataCmd.m_bag_data.m_bag_capacity - notifyRoleDataCmd.m_bag_data.m_bag_list.Count;
	}

	public int PackageNullCountWithLocked()
	{
		int num = 0;
		for (int i = 0; i < COMA_Package.maxCount; i++)
		{
			if (package.pack[i] != null)
			{
				num++;
			}
		}
		return COMA_Package.maxCount - num;
	}

	public int GetAnItem(COMA_PackageItem item)
	{
		if (item != null && item.serialName != string.Empty && COMA_PackageItem.NameToPart(item.serialName) != 0)
		{
			Debug.Log("----------get a accessory:" + item.serialName);
			AddBagItemCmd addBagItemCmd = new AddBagItemCmd();
			addBagItemCmd.m_part = 4;
			addBagItemCmd.m_state = 0;
			addBagItemCmd.m_unit = item.serialName;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, addBagItemCmd);
		}
		for (int i = 0; i < COMA_Package.slotUnlocked; i++)
		{
			if (package.pack[i] == null)
			{
				package.pack[i] = item;
				if (item.part <= 0)
				{
					ServerSetPackageTexture(i);
				}
				return i;
			}
		}
		Debug.LogError("Package is Full!!");
		return -1;
	}
}
