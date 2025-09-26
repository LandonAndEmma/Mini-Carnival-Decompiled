using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMA_Version : MonoBehaviour
{
	private static COMA_Version _instance;

	private string randomNickName = string.Empty;

	private bool bNeedSyncArchive;

	[NonSerialized]
	public List<int> _gameModeCount = new List<int>();

	private bool bNeedUpdate;

	private string ip = string.Empty;

	private List<int> port = new List<int>();

	[NonSerialized]
	public int[] orders;

	[NonSerialized]
	public int[] tickets;

	[NonSerialized]
	public string shopConfig = string.Empty;

	public GameObject[] targetObj;

	public COMA_PlayerCharacter characterCom;

	public GameObject tuiObj;

	private UI_MsgBox tipBox;

	private string m_strGameCenterId = string.Empty;

	private bool m_bLoginGameCenter;

	private bool bBindingGameCenter;

	public Texture2D _gNWMask;

	private bool bNetUnReachable;

	private UI_MsgBox boxNetUnreachable;

	private bool _NeedIAPCertificate;

	private int _CertificateNum;

	private int _curCertiNum;

	private bool _CanIAPCertificate;

	public static COMA_Version Instance
	{
		get
		{
			return _instance;
		}
	}

	private void OnEnable()
	{
		COMA_CommandHandler.Instance.AddHandlerFunction(COMA_Command.RUN_CREATEROAD, ReceiveCreateRoad);
	}

	private void OnDisable()
	{
		COMA_CommandHandler.Instance.RemoveHandlerFunction(COMA_Command.RUN_CREATEROAD, ReceiveCreateRoad);
	}

	private void ReceiveCreateRoad(COMA_CommandDatas commandDatas)
	{
		COMA_CD_CreateRoad cOMA_CD_CreateRoad = commandDatas as COMA_CD_CreateRoad;
		COMA_Sys.Instance.roadIDs = cOMA_CD_CreateRoad.rIDs;
		Debug.Log("-----------> " + COMA_Sys.Instance.roadIDs);
	}

	private void Awake()
	{
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Application.runInBackground = true;
		Application.targetFrameRate = 120;
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			bNetUnReachable = true;
			boxNetUnreachable = TUI_MsgBox.Instance.MessageBox(101);
			return;
		}
		bNetUnReachable = false;
		if (boxNetUnreachable != null)
		{
			UnityEngine.Object.DestroyObject(boxNetUnreachable.gameObject);
		}
		COMA_GC_TID.Instance.content = COMA_FileIO.LoadFile(COMA_GC_TID.Instance.defaultFileName);
		OpenClikPlugin.Initialize("36767EE6-EBD2-4141-92DF-4330146C4DF1");
		GCInit();
		ChartboostPlugin.StartSession("52132e1216ba47932e000003", "d8f9935dda1ef8ec0591efecf9e412727612b0a4");
		SceneTimerInstance.Instance.Add(10f, TimeOut);
		randomNickName = GetDeviceName();
	}

	private string GetDeviceName()
	{
		string empty = string.Empty;
		return "P" + UnityEngine.Random.Range(0, 1000000000).ToString("d9");
	}

	public bool TimeOut()
	{
		TUI_MsgBox.Instance.MessageBox(102);
		return false;
	}

	public bool TimeOut_InputName()
	{
		if (UIInputName.Instance != null)
		{
			UIInputName.Instance.DestroyWaitingBox();
		}
		TUI_MsgBox.Instance.MessageBox(102);
		return false;
	}

	public bool TimeOut_Update()
	{
		COMA_HTTP_TextureManager.Instance.DeliverServerUpdate(COMA_Pref.Instance.playerID, COMA_Pref.Instance.nickname);
		TUI_MsgBox.Instance.MessageBox(102);
		return true;
	}

	public bool TimeOut_PrefInit()
	{
		TUI_MsgBox.Instance.MessageBox(102);
		if (COMA_ServerManager.Instance.saverSvrAddr != string.Empty)
		{
			COMA_HTTP_TextureManager.Instance.PlayerPref_Init();
		}
		return true;
	}

	public bool TimeOut_PrefUpdate()
	{
		TUI_MsgBox.Instance.MessageBox(102);
		if (COMA_ServerManager.Instance.saverSvrAddr != string.Empty)
		{
			COMA_HTTP_TextureManager.Instance.PlayerPref_Update(COMA_Pref.Instance.playerID);
		}
		return true;
	}

	private IEnumerator Start()
	{
		if (bNetUnReachable)
		{
			yield break;
		}
		WWW www = new WWW(COMA_ServerManager.Instance.serverAddr_Config);
		yield return www;
		if (www.error != null)
		{
			Debug.LogWarning("================error: " + www.error);
			yield break;
		}
		SceneTimerInstance.Instance.Remove(TimeOut);
		Debug.Log(www.text);
		string totalContent = www.text.Replace("\r\n", string.Empty);
		string[] content = totalContent.Split('|');
		string v = content[0];
		ip = content[1];
		string[] subPortStr = content[2].Split(',');
		string[] array = subPortStr;
		foreach (string s in array)
		{
			port.Add(int.Parse(s));
		}
		string[] subCnt = content[3].Split(',');
		int count = int.Parse(subCnt[0]);
		orders = new int[count];
		tickets = new int[count];
		for (int j = 0; j < count; j++)
		{
			orders[j] = int.Parse(subCnt[1 + j]);
			tickets[j] = int.Parse(subCnt[1 + count + j]);
		}
		int indexMax = 0;
		for (int k = 0; k < count; k++)
		{
			if (orders[k] > indexMax)
			{
				indexMax = orders[k];
			}
		}
		for (int l = 0; l <= indexMax; l++)
		{
			_gameModeCount.Add(0);
		}
		shopConfig = content[4];
		COMA_Sys.Instance.tax = int.Parse(content[5]);
		COMA_ServerManager.Instance.serverAddr_IAP = content[6];
		Debug.Log("-----1-------------serverAddr_IAP:" + COMA_ServerManager.Instance.serverAddr_IAP);
		COMA_ServerManager.Instance.deliverSvrAddr = content[7];
		Debug.Log("分服服务器 : " + COMA_ServerManager.Instance.deliverSvrAddr);
		COMA_Sys.Instance.tipContent = content[8];
		COMA_Sys.Instance.bCanGame = ((!(content[9] == "0")) ? true : false);
		COMA_Sys.Instance.bLockMarket = content[10] == "1";
		COMA_Sys.Instance.marketRefreshInterval = int.Parse(content[11]);
		COMA_Sys.Instance.curMarketRefreshInterval = COMA_Sys.Instance.marketRefreshInterval;
		COMA_Sys.Instance.marketRefreshCrystal = int.Parse(content[12]);
		COMA_ServerManager.Instance.dataCollectSvrAddr = content[13];
		COMA_HTTP_DataCollect.Instance.InitDataCollectServer();
		COMA_ServerManager.Instance.idSvrAddr = content[14];
		COMA_ServerManager.Instance.idSvrKey = content[15];
		COMA_CommonOperation.Instance.bfriendRequireInverval = content[16] == "1";
		COMA_Fishing.Instance.nTime0 = int.Parse(content[17]);
		COMA_Fishing.Instance.nTime1 = int.Parse(content[18]);
		COMA_Fishing.Instance.nTime2 = int.Parse(content[19]);
		Debug.Log(COMA_Fishing.Instance.nTime0 + " " + COMA_Fishing.Instance.nTime1 + " " + COMA_Fishing.Instance.nTime2);
		COMA_Package.unlockPrice = int.Parse(content[20]);
		COMA_VideoController.Instance.nVideo = int.Parse(content[21]);
		COMA_Sys.Instance.version = MiscPlugin.GetAppVersion();
		bNeedUpdate = NeedUpdateVersion(COMA_Sys.Instance.version, v);
		if (bNeedUpdate)
		{
			if (tipBox == null)
			{
				tipBox = TUI_MsgBox.Instance.MessageBox(113);
				tipBox.AddProceYesHandler(DownloadNewVersion);
			}
		}
		else
		{
			Debug.Log("------------------------------GetUUID()-----------------1");
			COMA_Sys.Instance.VID = COMA_Sys.Instance.PlayerDevice.GetUUID();
			Debug.Log("Current VID : " + COMA_Sys.Instance.VID);
			COMA_Server_ID.Instance.InitServer(OnGetServerID, COMA_ServerManager.Instance.idSvrAddr, COMA_ServerManager.Instance.idSvrOutTime, COMA_ServerManager.Instance.idSvrKey);
			RequestIDServer();
		}
	}

	private bool NeedUpdateVersion(string localVersiong, string v)
	{
		bool result = false;
		Debug.Log("Local Version : " + localVersiong + "   Server Version : " + v);
		string[] array = localVersiong.Split('.');
		string[] array2 = v.Split('.');
		int[] array3 = new int[3];
		int[] array4 = new int[3];
		array3[0] = int.Parse(array[0]);
		array3[1] = int.Parse(array[1]);
		array3[2] = ((array.Length >= 3) ? int.Parse(array[2]) : 0);
		array4[0] = int.Parse(array2[0]);
		array4[1] = int.Parse(array2[1]);
		array4[2] = ((array2.Length >= 3) ? int.Parse(array2[2]) : 0);
		for (int i = 0; i < 3; i++)
		{
			if (array3[i] < array4[i])
			{
				result = true;
				break;
			}
			if (array3[i] > array4[i])
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private void RequestIDServer()
	{
		if (COMA_GC_TID.Instance.gcLocal != string.Empty && (COMA_GC_TID.Instance.gcLocal == m_strGameCenterId || m_strGameCenterId == string.Empty))
		{
			COMA_Server_ID.Instance.GID = COMA_GC_TID.Instance.gidLocal;
			OnGetServerID(false);
		}
		else
		{
			COMA_Server_ID.Instance.RegisterGame("carnival", COMA_Sys.Instance.version, COMA_Sys.Instance.VID, string.Empty, m_strGameCenterId);
			COMA_GC_TID.Instance.gcLocal = m_strGameCenterId;
		}
	}

	private void BindGCtoIDServer()
	{
		if (!(COMA_Pref.Instance.playerID == string.Empty))
		{
			COMA_Server_ID.Instance.BindGameCenter("carnival", COMA_Sys.Instance.version, COMA_Sys.Instance.VID, string.Empty, m_strGameCenterId);
		}
	}

	public void OnGetServerID(bool bTimeOut)
	{
		Debug.Log("OnGetServerID()");
		if (bTimeOut)
		{
			COMA_Server_ID.Instance.RegisterGame("carnival", COMA_Sys.Instance.version, COMA_Sys.Instance.VID, string.Empty, m_strGameCenterId);
			return;
		}
		if (COMA_Sys.Instance.version == "999.999")
		{
			COMA_Pref.Instance.nickname = GetDeviceName();
		}
		else
		{
			COMA_Pref.Instance.Load();
		}
		Debug.Log("LocalID:" + COMA_Pref.Instance.playerID + "  ServerID:" + COMA_Server_ID.Instance.GID + "  nickname:" + COMA_Pref.Instance.nickname);
		if (COMA_Pref.Instance.playerID == string.Empty)
		{
			COMA_Pref.Instance.playerID = COMA_Server_ID.Instance.GID;
		}
		if (COMA_Pref.Instance.playerID != COMA_Server_ID.Instance.GID)
		{
			UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(124);
			uI_MsgBox.AddProceYesHandler(ArchiveUseServer);
			uI_MsgBox.AddProceNoHandler(ArchiveUseLocal);
			uI_MsgBox.param = COMA_Server_ID.Instance.GID;
		}
		else
		{
			COMA_Server_Account.Instance.InitServer(OnGetServerAccount, COMA_ServerManager.Instance.deliverSvrAddr, COMA_ServerManager.Instance.deliverSvrOutTime, COMA_ServerManager.Instance.deliverSvrKey);
			COMA_Server_Friends.Instance.InitServer(COMA_ServerManager.Instance.deliverSvrAddr, COMA_ServerManager.Instance.deliverSvrOutTime, COMA_ServerManager.Instance.deliverSvrKey);
			COMA_Server_Account.Instance.DeliverGame(COMA_Server_ID.Instance.GID, m_strGameCenterId, COMA_Server_ID.Instance.NID, COMA_Pref.Instance.nickname);
		}
	}

	private void ArchiveUseServer(string param)
	{
		Debug.Log("Use Server Archive!!");
		bNeedSyncArchive = true;
		COMA_Pref.Instance.playerID = param;
		COMA_Server_Account.Instance.InitServer(OnGetServerAccount, COMA_ServerManager.Instance.deliverSvrAddr, COMA_ServerManager.Instance.deliverSvrOutTime, COMA_ServerManager.Instance.deliverSvrKey);
		COMA_Server_Friends.Instance.InitServer(COMA_ServerManager.Instance.deliverSvrAddr, COMA_ServerManager.Instance.deliverSvrOutTime, COMA_ServerManager.Instance.deliverSvrKey);
		COMA_Server_Account.Instance.DeliverGame(COMA_Pref.Instance.playerID, m_strGameCenterId, COMA_Server_ID.Instance.NID, COMA_Pref.Instance.nickname);
	}

	private void ArchiveUseLocal(string param)
	{
		Debug.Log("Use Local Archive!!");
		COMA_Server_Account.Instance.InitServer(OnGetServerAccount, COMA_ServerManager.Instance.deliverSvrAddr, COMA_ServerManager.Instance.deliverSvrOutTime, COMA_ServerManager.Instance.deliverSvrKey);
		COMA_Server_Friends.Instance.InitServer(COMA_ServerManager.Instance.deliverSvrAddr, COMA_ServerManager.Instance.deliverSvrOutTime, COMA_ServerManager.Instance.deliverSvrKey);
		COMA_Server_Account.Instance.DeliverGame(COMA_Pref.Instance.playerID, m_strGameCenterId, COMA_Server_ID.Instance.NID, COMA_Pref.Instance.nickname);
	}

	public void OnGetServerAccount(bool bTimeOut, bool bNoPref)
	{
		Debug.Log("OnGetServerAccount() : " + bNoPref);
		if (bTimeOut)
		{
			COMA_Server_Account.Instance.DeliverGame(COMA_Server_ID.Instance.GID, m_strGameCenterId, COMA_Server_ID.Instance.NID, COMA_Pref.Instance.nickname);
			return;
		}
		COMA_Sys.Instance.bNeedInitPref = bNoPref;
		COMA_Server_Archive.Instance.InitServer(OnGetServerArchive_Get, COMA_ServerManager.Instance.saverSvrAddr, COMA_ServerManager.Instance.saverSvrOutTime, COMA_ServerManager.Instance.saverSvrKey);
		COMA_Server_Texture.Instance.InitServer(COMA_ServerManager.Instance.serverAddr_Save, COMA_ServerManager.Instance.serverAddr_Save_OutTime, COMA_ServerManager.Instance.serverAddr_Save_Key);
		if (COMA_Sys.Instance.bNeedInitPref)
		{
			COMA_Pref.Instance.nickname = randomNickName;
			COMA_CommonOperation.Instance.defaultInput = COMA_Pref.Instance.nickname;
			Application.LoadLevelAdditive("UI.InputName");
		}
		else if (COMA_Sys.Instance.version == "999.999")
		{
			OnGetServerArchive_Get(false, string.Empty);
		}
		else
		{
			COMA_Server_Archive.Instance.PlayerPref_Get(COMA_Pref.Instance.playerID);
		}
	}

	public void InputEnd(string input)
	{
		if (COMA_Sys.Instance.bNeedInitPref)
		{
			COMA_Sys.Instance.bNeedInitPref = false;
			COMA_Pref.Instance.nickname = input;
			COMA_Pref.Instance.UpPlayerTexturesInit();
			COMA_Pref.Instance.ServerSetPackageTexture(0);
			COMA_Pref.Instance.ServerSetPackageTexture(1);
			COMA_Pref.Instance.ServerSetPackageTexture(2);
			LoadSaveFinish();
		}
		else
		{
			COMA_Pref.Instance.nickname = input;
			COMA_Pref.Instance.Save(true);
			if (UIOptions.Instance != null)
			{
				UIOptions.Instance.DisableBlock();
			}
		}
		COMA_Server_Account.Instance.SetName(COMA_Server_ID.Instance.GID, COMA_Pref.Instance.nickname);
		UIInputName.Instance.LeaveAnim(string.Empty);
	}

	public void OnGetServerArchive_Get(bool bTimeOut, string content)
	{
		Debug.Log("OnGetServerArchive_Get() : " + bTimeOut + " " + content);
		if (bTimeOut)
		{
			COMA_Server_Archive.Instance.PlayerPref_Get(COMA_Pref.Instance.playerID);
			return;
		}
		if (content == string.Empty)
		{
			Debug.Log("11");
			COMA_Pref.Instance.package.pack[0].LoadPNGDefault(0);
			COMA_Pref.Instance.package.pack[1].LoadPNGDefault(1);
			COMA_Pref.Instance.package.pack[2].LoadPNGDefault(2);
			COMA_Pref.Instance.ServerSetPackageTexture(0);
			COMA_Pref.Instance.ServerSetPackageTexture(1);
			COMA_Pref.Instance.ServerSetPackageTexture(2);
		}
		else
		{
			if (bNeedSyncArchive)
			{
				COMA_Pref.Instance.package.Clear();
			}
			COMA_Pref.Instance.contentForServerSave = content;
		}
		LoadSaveFinish();
	}

	public bool SyncPackageTextureTimeOut()
	{
		LoadSaveFinish();
		return false;
	}

	public void RateItNow(string param)
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.trinitigame.android.google.callofminiavatar");
	}

	public void DownloadNewVersion(string param)
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.trinitigame.android.google.callofminiavatar");
		tipBox = null;
	}

	private void OnApplicationPause(bool pause)
	{
		Debug.Log("COMA_Version:" + pause);
		if (pause)
		{
			if (!(Application.loadedLevelName == "UI.IAP") && !COMA_VideoController.Instance.bVideoPopView)
			{
				Debug.Log("---------->close game");
				COMA_HTTP_DataCollect.Instance.SendCloseGameInfo();
				for (int i = 0; i < _gameModeCount.Count; i++)
				{
					_gameModeCount[i] = 0;
				}
				if (COMA_CommonOperation.Instance.IsPlayingMultiMode())
				{
					COMA_Pref.Instance.AddRankScoreOfCurrentScene(-20);
				}
				COMA_NetworkConnect.Instance.BackFromScene();
			}
			return;
		}
		Debug.Log("---------->start game");
		if (!(Application.loadedLevelName == "UI.IAP"))
		{
			if (COMA_VideoController.Instance.bVideoPopView)
			{
				COMA_VideoController.Instance.bVideoPopView = false;
				return;
			}
			COMA_HTTP_DataCollect.Instance.SendStartGameInfo();
			Application.LoadLevel("COMA_Start");
		}
	}

	private void OnApplicationFocus()
	{
		if (bNeedUpdate && tipBox == null)
		{
			tipBox = TUI_MsgBox.Instance.MessageBox(113);
			tipBox.AddProceYesHandler(DownloadNewVersion);
		}
	}

	public bool TryToConnectGameServer()
	{
		if (port.Count > 0)
		{
			COMA_NetworkConnect.Instance.EndConnect();
			int num = port[UnityEngine.Random.Range(0, port.Count)];
			Debug.Log("Start Connect " + ip + ":" + num);
			COMA_NetworkConnect.Instance.StartConnect(ip, num);
			return true;
		}
		return false;
	}

	public bool LoadSaveFinish()
	{
		return LoadSaveFinish(true);
	}

	public bool LoadSaveFinish(bool bNeedSave)
	{
		Resources.UnloadUnusedAssets();
		Debug.Log("LoadSaveFinish");
		COMA_Pref.Instance.AddCrystal(GotTapPointsMono.Instance.tapjoy_reward);
		GotTapPointsMono.Instance.tapjoy_reward = 0;
		COMA_Sys.Instance.playTimes++;
		COMA_Sys.Instance.bMemFirstGame = COMA_Sys.Instance.bFirstGame;
		Debug.Log("当前第" + COMA_Sys.Instance.playTimes + "次游戏");
		if (COMA_Sys.Instance.playTimes == 3)
		{
			tipBox = TUI_MsgBox.Instance.MessageBox(116);
			tipBox.AddProceNoHandler(RateItNow);
		}
		if (bNeedSave)
		{
			COMA_Pref.Instance.Save(true);
		}
		if (COMA_Pref.Instance.IsNeedNewGuideMask())
		{
			_gNWMask = new Texture2D(1136, 768, TextureFormat.ARGB32, false);
			for (int i = 0; i < 1136; i++)
			{
				for (int j = 0; j < 768; j++)
				{
					_gNWMask.SetPixel(i, j, new Color(0f, 0f, 0f, 1f));
				}
			}
			_gNWMask.Apply(false);
		}
		string text = COMA_FileIO.LoadFile(COMA_FileNameManager.Instance.GetFileName("Board"));
		if (text != COMA_Sys.Instance.tipContent)
		{
			TUI_MsgBox.Instance.MessageBox(1211);
			COMA_FileIO.SaveFile(COMA_FileNameManager.Instance.GetFileName("Board"), COMA_Sys.Instance.tipContent);
		}
		UpdateAvatar();
		COMA_Carnival_Camera.Instance.VersionReady();
		StartCoroutine(CreateIconInPackage());
		if (!TryToConnectGameServer())
		{
			TUI_MsgBox.Instance.MessageBox(101);
		}
		iServerIAPVerify.GetInstance().Initialize(COMA_ServerManager.Instance.serverName_IAP, COMA_ServerManager.Instance.serverAddr_IAP, COMA_ServerManager.Instance.serverKey_IAP, COMA_ServerManager.Instance.serverTimeout_IAP);
		iIAPManager.GetInstance().Initialize(COMA_ServerManager.Instance.serverName_IAP, COMA_ServerManager.Instance.serverAddr_IAP, COMA_ServerManager.Instance.serverKey_IAP, COMA_ServerManager.Instance.serverTimeout_IAP);
		iServerIAPVerify.GetInstance().SetPurchaseCallBack(OnAddMoney);
		int checkIAPCount = COMA_IAPCheck.Instance.GetCheckIAPCount();
		Debug.Log("-----------------GetCheckIAPCount()---------------------  :" + checkIAPCount);
		if (checkIAPCount > 0)
		{
			_CertificateNum = checkIAPCount;
			_curCertiNum = 0;
			_NeedIAPCertificate = true;
			_CanIAPCertificate = true;
		}
		COMA_Server_Rank.Instance.GetRankings();
		Debug.LogError("------------------------------------------------------------------IAP");
		return false;
	}

	public void OnVerifySuccess(bool bS, string sKey, string sIdentifier, string sReceipt, string sSignature)
	{
		COMA_IAPCheck.Instance.RemoveToLocal(sKey, sIdentifier, sReceipt, sSignature);
		Debug.Log("cmp srv return-----purchase success=" + bS);
		_curCertiNum++;
		if (_curCertiNum < _CertificateNum)
		{
			_CanIAPCertificate = true;
		}
		else
		{
			_CanIAPCertificate = false;
		}
		if (bS)
		{
			COMA_HTTP_DataCollect.Instance.SendIAPInfo(sKey, sReceipt);
		}
	}

	public void OnIAPVerifySubmitSuccess(string sKey, string sIdentifier, string sReceipt, string sSignature, string sRandom, int nRat, int nRatA, int nRatB)
	{
		Debug.Log("COMA_Version   verify submit success at the background");
		COMA_IAPCheck.Instance.RemoveToLocal(sKey, sIdentifier, sReceipt, sSignature);
		COMA_IAPCheck.Instance.AddToLocal(sKey, sIdentifier, sReceipt, sSignature, sRandom, nRat.ToString(), nRatA.ToString(), nRatB.ToString());
	}

	public void OnVerifyFailed(string sKey, string sIdentifier, string sReceipt, string sSignature)
	{
		Debug.Log("cmp srv return-----purchase failed");
		COMA_IAPCheck.Instance.RemoveToLocal(sKey, sIdentifier, sReceipt, sSignature);
		_curCertiNum++;
		if (_curCertiNum < _CertificateNum)
		{
			_CanIAPCertificate = true;
		}
		else
		{
			_CanIAPCertificate = false;
		}
	}

	public void OnAddMoney(string sIAPKey)
	{
		if (sIAPKey == UI_IAP.IAPKeys[0])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(25);
			TUI_MsgBox.Instance.TipBox(4, 25, string.Empty, null);
		}
		else if (sIAPKey == UI_IAP.IAPKeys[1])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(150);
			TUI_MsgBox.Instance.TipBox(4, 150, string.Empty, null);
		}
		else if (sIAPKey == UI_IAP.IAPKeys[2])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(350);
			TUI_MsgBox.Instance.TipBox(4, 350, string.Empty, null);
		}
		else if (sIAPKey == UI_IAP.IAPKeys[3])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(850);
			TUI_MsgBox.Instance.TipBox(4, 850, string.Empty, null);
		}
		else if (sIAPKey == UI_IAP.IAPKeys[4])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(2500);
			TUI_MsgBox.Instance.TipBox(4, 2500, string.Empty, null);
		}
		GameObject gameObject = GameObject.Find("TUI");
		if (gameObject != null)
		{
			UIMessageHandler component = gameObject.transform.root.GetComponent<UIMessageHandler>();
			if (component != null)
			{
				component.RefreshGoldAndCrystal();
			}
		}
		COMA_Pref.Instance.Save(true);
	}

	public void OnNetError(string sIAPKey)
	{
		Debug.Log("cmp srv return-----error");
		_curCertiNum++;
		if (_curCertiNum < _CertificateNum)
		{
			_CanIAPCertificate = true;
		}
		else
		{
			_CanIAPCertificate = false;
		}
	}

	private void GCInit()
	{
		GameCenterPlugin.Initialize();
	}

	private void GCLogin()
	{
		if (GameCenterPlugin.IsSupported())
		{
			GameCenterPlugin.Login();
			m_bLoginGameCenter = true;
			Debug.Log("GC -----------> Auto Login!!");
			SceneTimerInstance.Instance.Add(40f, GCLoginTimeOut);
		}
	}

	private void GCCheck()
	{
		Debug.Log("GC -----------> Check & Bind!!");
		if (GameCenterPlugin.IsSupported())
		{
			bBindingGameCenter = true;
		}
	}

	private void GCUpdate()
	{
		if (m_bLoginGameCenter)
		{
			switch (GameCenterPlugin.LoginStatus())
			{
			case GameCenterPlugin.LOGIN_STATUS.LOGIN_STATUS_SUCCESS:
				m_bLoginGameCenter = false;
				m_strGameCenterId = GameCenterPlugin.GetAccount();
				Debug.Log("Login GameCenter ---------------------------------------------- Success:" + m_strGameCenterId);
				COMA_Server_ID.Instance.BindGameCenter("carnival", COMA_Sys.Instance.version, COMA_Pref.Instance.playerID, string.Empty, m_strGameCenterId);
				RequestIDServer();
				SceneTimerInstance.Instance.Remove(GCLoginTimeOut);
				break;
			case GameCenterPlugin.LOGIN_STATUS.LOGIN_STATUS_ERROR:
				m_bLoginGameCenter = false;
				m_strGameCenterId = string.Empty;
				Debug.Log("Login GameCenter ---------------------------------------------- Error!!");
				RequestIDServer();
				SceneTimerInstance.Instance.Remove(GCLoginTimeOut);
				break;
			}
		}
		if (bBindingGameCenter)
		{
			switch (GameCenterPlugin.LoginStatus())
			{
			case GameCenterPlugin.LOGIN_STATUS.LOGIN_STATUS_SUCCESS:
				bBindingGameCenter = false;
				m_strGameCenterId = GameCenterPlugin.GetAccount();
				Debug.Log("Bind GameCenter ---------------------------------------------- Success:" + m_strGameCenterId);
				BindGCtoIDServer();
				break;
			}
		}
	}

	public bool GCLoginTimeOut()
	{
		Debug.Log("Login GameCenter ---------------------------------------------- TimeOut!!");
		return false;
	}

	private void Update()
	{
		GCUpdate();
		if (!_NeedIAPCertificate || !_CanIAPCertificate)
		{
			return;
		}
		Debug.Log("------------IAP 验证-------------------");
		if (iServerIAPVerify.GetInstance().IsCanVerify())
		{
			int index = 0;
			string subInfo = COMA_IAPCheck.Instance.GetSubInfo0(index);
			string subInfo2 = COMA_IAPCheck.Instance.GetSubInfo1(index);
			string subInfo3 = COMA_IAPCheck.Instance.GetSubInfo2(index);
			string subInfo4 = COMA_IAPCheck.Instance.GetSubInfo3(index);
			string subInfo5 = COMA_IAPCheck.Instance.GetSubInfo4(index);
			string subInfo6 = COMA_IAPCheck.Instance.GetSubInfo5(index);
			string subInfo7 = COMA_IAPCheck.Instance.GetSubInfo6(index);
			string subInfo8 = COMA_IAPCheck.Instance.GetSubInfo7(index);
			_CanIAPCertificate = false;
			if ("0" == subInfo5)
			{
				iServerIAPVerify.GetInstance().VerifyIAP(subInfo, subInfo2, subInfo3, subInfo4, OnVerifySuccess, OnVerifyFailed, OnNetError, OnIAPVerifySubmitSuccess);
			}
			else
			{
				iServerIAPVerify.GetInstance().VerifyIAPCheck(subInfo, subInfo2, subInfo3, subInfo4, subInfo5, int.Parse(subInfo6), int.Parse(subInfo7), int.Parse(subInfo8), OnVerifySuccess, OnVerifyFailed, OnNetError);
			}
		}
		else
		{
			_CanIAPCertificate = false;
		}
	}

	public void UpdateAvatar()
	{
		if (targetObj[0] == null || targetObj[1] == null || targetObj[2] == null)
		{
			return;
		}
		if (COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]] == null || COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]] == null || COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]] == null)
		{
			for (int i = 0; i < COMA_Pref.Instance.package.pack.Length; i++)
			{
				if (COMA_Pref.Instance.package.pack[i] != null && COMA_Pref.Instance.package.pack[i].itemName != null)
				{
					string itemName = COMA_Pref.Instance.package.pack[i].itemName;
					if (itemName.StartsWith("Head"))
					{
						COMA_Pref.Instance.TInPack[0] = i;
						Debug.Log(i + "... " + COMA_Pref.Instance.TInPack[0] + " " + COMA_Pref.Instance.TInPack[1] + " " + COMA_Pref.Instance.TInPack[2]);
					}
					else if (itemName.StartsWith("Body"))
					{
						COMA_Pref.Instance.TInPack[1] = i;
						Debug.Log(i + "... " + COMA_Pref.Instance.TInPack[0] + " " + COMA_Pref.Instance.TInPack[1] + " " + COMA_Pref.Instance.TInPack[2]);
					}
					else if (itemName.StartsWith("Leg"))
					{
						COMA_Pref.Instance.TInPack[2] = i;
						Debug.Log(i + "... " + COMA_Pref.Instance.TInPack[0] + " " + COMA_Pref.Instance.TInPack[1] + " " + COMA_Pref.Instance.TInPack[2]);
					}
				}
			}
		}
		if (COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]] == null)
		{
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]] = new COMA_PackageItem();
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].serialName = "Head01";
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].itemName = "Head";
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].num = -1;
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].tid = string.Empty;
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].state = COMA_PackageItem.PackageItemStatus.Equiped;
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]].textureName = COMA_FileNameManager.Instance.GetFileName("Head01");
		}
		if (COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]] == null)
		{
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]] = new COMA_PackageItem();
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].serialName = "Body01";
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].itemName = "Body";
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].num = -1;
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].tid = string.Empty;
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].state = COMA_PackageItem.PackageItemStatus.Equiped;
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]].textureName = COMA_FileNameManager.Instance.GetFileName("Body01");
		}
		if (COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]] == null)
		{
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]] = new COMA_PackageItem();
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].serialName = "Leg01";
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].itemName = "Feet";
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].num = -1;
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].tid = string.Empty;
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].state = COMA_PackageItem.PackageItemStatus.Equiped;
			COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]].textureName = COMA_FileNameManager.Instance.GetFileName("Leg01");
		}
		if (COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[0]] == null || COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[1]] == null || COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[2]] == null)
		{
			Debug.Log("Reset!!!");
			COMA_Pref.Instance.ResetData_Partful();
		}
		Debug.Log(COMA_Pref.Instance.TInPack[0] + " " + COMA_Pref.Instance.TInPack[1] + " " + COMA_Pref.Instance.TInPack[2]);
		for (int j = 0; j < targetObj.Length; j++)
		{
			targetObj[j].renderer.material.mainTexture = COMA_Pref.Instance.package.pack[COMA_Pref.Instance.TInPack[j]].texture;
		}
		for (int k = 0; k < COMA_Pref.Instance.AInPack.Length; k++)
		{
			if (COMA_Pref.Instance.AInPack[k] >= 0)
			{
				characterCom.CreateAccouterment(COMA_Pref.Instance.package.pack[COMA_Pref.Instance.AInPack[k]].serialName);
			}
		}
	}

	private IEnumerator CreateIconInPackage()
	{
		for (int i = 0; i < COMA_Pref.Instance.package.pack.Length; i++)
		{
			if (COMA_Pref.Instance.package.pack[i] != null)
			{
				COMA_Pref.Instance.package.pack[i].CreateIconTexture();
				yield return 0;
			}
		}
	}

	public IEnumerator CreateIconInPackage(int i)
	{
		COMA_Pref.Instance.package.pack[i].CreateIconTexture();
		yield return new WaitForEndOfFrame();
	}

	public void ShowUI()
	{
		tuiObj.SetActive(true);
	}
}
