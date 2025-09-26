using System;
using System.Collections;
using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol.Common;
using UnityEngine;

public class COMA_Login : MonoBehaviour
{
	public enum PercentTips
	{
		None = 0,
		Awake_Start = 10,
		Awake_End = 11,
		Start_Start = 20,
		Start_End = 21,
		RequestTID_Start = 30,
		RequestTID_End = 31,
		ConnectLobbyServer_Start = 40,
		ConnectLobbyServer_End = 80,
		Finish = 100
	}

	private static COMA_Login _instance;

	public UILabel connectingTipLabel;

	private bool bNetUnReachable;

	private UI_MsgBox boxNetUnreachable;

	[NonSerialized]
	public List<KeyValuePair<string, int>> lstIPAndPort = new List<KeyValuePair<string, int>>();

	[NonSerialized]
	public int lobbySrvIndex;

	private int fstLobbySrvIndex;

	[NonSerialized]
	public bool bMaintenance;

	[NonSerialized]
	public string ip_game = string.Empty;

	[NonSerialized]
	private List<int> port_game = new List<int>();

	[NonSerialized]
	public int[] orders;

	[NonSerialized]
	public int[] tickets;

	[NonSerialized]
	private List<int> _gameModeCount = new List<int>();

	[NonSerialized]
	private List<int> _gameModeCount_Create = new List<int>();

	public Camera clearCamera;

	public readonly string tip_downloadContent = "Download Content...";

	public readonly string tip_uploadContent = "Transmit Content...";

	public readonly string tip_connectToServer = "Connecting To Server...";

	[SerializeField]
	private UILabel _labelTxt;

	[SerializeField]
	private UISlider _slider;

	private bool _NeedIAPCertificate;

	private int _CertificateNum;

	private int _curCertiNum;

	private bool _CanIAPCertificate;

	[SerializeField]
	private TextAsset _tipsAsset;

	private bool bNeedSaveGCGIDLocal;

	private bool bNeedSaveDeviceGIDLocal;

	private float fpreVerifyTime = -200f;

	public bool bGameLoadFinishedForPauseLock;

	public static COMA_Login Instance
	{
		get
		{
			return _instance;
		}
	}

	public bool ChangeLobbyGate()
	{
		int count = lstIPAndPort.Count;
		int num = lobbySrvIndex;
		num++;
		if (num >= count)
		{
			num = 0;
		}
		if (num == fstLobbySrvIndex)
		{
			return false;
		}
		lobbySrvIndex = num;
		Debug.Log("============================>>>>Change LobbySRV Index to:" + lobbySrvIndex);
		return true;
	}

	public bool IsModeExist(int modeIndex)
	{
		int[] array = orders;
		foreach (int num in array)
		{
			if (num == modeIndex)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsModeRankOn(int modeIndex)
	{
		for (int i = 0; i < orders.Length; i++)
		{
			if (orders[i] == modeIndex && (tickets[i] & 1) == 1)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsModeNew(int modeIndex)
	{
		for (int i = 0; i < orders.Length; i++)
		{
			if (orders[i] == modeIndex && (tickets[i] & 2) == 2)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsModeHot(int modeIndex)
	{
		for (int i = 0; i < orders.Length; i++)
		{
			if (orders[i] == modeIndex && (tickets[i] & 4) == 4)
			{
				return true;
			}
		}
		return false;
	}

	public void AddGameModeCount(int sceneId, bool isCreated)
	{
		List<int> gameModeCount;
		List<int> list = (gameModeCount = _gameModeCount);
		int index2;
		int index = (index2 = sceneId);
		index2 = gameModeCount[index2];
		list[index] = index2 + 1;
		if (isCreated)
		{
			List<int> gameModeCount_Create;
			List<int> list2 = (gameModeCount_Create = _gameModeCount_Create);
			int index3 = (index2 = sceneId);
			index2 = gameModeCount_Create[index2];
			list2[index3] = index2 + 1;
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

	public void ChangeConnectingTip(string content)
	{
		connectingTipLabel.text = content;
	}

	private void Tip_NoNetwork()
	{
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("fengmian_lianjietishi2"));
		UIGolbalStaticFun.PopCommonMessageBox(data);
	}

	public bool Tip_ConnectTimeOut()
	{
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("fengmian_lianjietishi1"));
		UIGolbalStaticFun.PopCommonMessageBox(data);
		return false;
	}

	private void Tip_NeedUpdate()
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(1, Localization.instance.Get("NoviceProcess_22"));
		uIMessage_CommonBoxData.Mark = "Desc_NeedUpdate";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
	}

	private void Tip_SysBillboard(string content)
	{
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, content);
		UIGolbalStaticFun.PopCommonMessageBox(data);
	}

	public bool Tip_NeedDownLoadSupport()
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(1, Localization.instance.Get("fengmian_lianjietishi20"));
		uIMessage_CommonBoxData.Mark = "Desc_NeedDownLoadSupport";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		return false;
	}

	private void Tip_NeedSync()
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("fengmian_lianjietishi13"));
		uIMessage_CommonBoxData.Mark = "Desc_NeedSync";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
	}

	public void OnSliderChange(float val)
	{
		if (_labelTxt != null)
		{
			_labelTxt.text = Mathf.CeilToInt(val * 100f) + "%";
		}
	}

	public void TipPercent(PercentTips step)
	{
		_slider.sliderValue = (float)step / 100f;
		Debug.Log((int)step + "%");
	}

	private void Awake()
	{
		TipPercent(PercentTips.Awake_Start);
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Application.runInBackground = true;
		Application.targetFrameRate = 120;
		TUITextManager.Instance().Parser("UI/language.en", "UI/language.en");
		COMA_GC_TID.Instance.Load();
		OpenClikPlugin.Initialize("36767EE6-EBD2-4141-92DF-4330146C4DF1");
		COMA_Sys.Instance.VID = GetDeviceID();
		Debug.Log("Current VID : " + COMA_Sys.Instance.VID);
		ChangeConnectingTip(tip_connectToServer);
		TipPercent(PercentTips.Awake_End);
	}

	private string GetDeviceID()
	{
		string empty = string.Empty;
		return COMA_Sys.Instance.PlayerDevice.GetUUID();
	}

	private IEnumerator Start()
	{
		TipPercent(PercentTips.Start_Start);
		string fileContent = _tipsAsset.ToString();
		COMA_DataConfig.Instance._helpTips = COMA_Tools.DeserializeObject<CHelpTips>(fileContent) as CHelpTips;
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			Tip_NoNetwork();
			bNetUnReachable = true;
		}
		if (bNetUnReachable)
		{
			yield break;
		}
		Debug.Log("Start download WWW...");
		WWW www = new WWW(COMA_ServerManager.Instance.serverAddr_Config);
		yield return www;
		if (www.error != null)
		{
			Debug.LogWarning("================error: " + www.error);
			Start();
			yield break;
		}
		string totalContent = www.text.Replace("\r\n", string.Empty);
		string[] content = totalContent.Split('|');
		string v = content[0];
		lstIPAndPort.Clear();
		Debug.LogWarning(content[1]);
		string[] ipAndPorts = content[1].Split(',');
		string[] array = ipAndPorts;
		foreach (string str in array)
		{
			string[] sub = str.Split(':');
			KeyValuePair<string, int> pair = new KeyValuePair<string, int>(sub[0], int.Parse(sub[1]));
			lstIPAndPort.Add(pair);
		}
		bMaintenance = content[2] == "1";
		lobbySrvIndex = UnityEngine.Random.Range(0, lstIPAndPort.Count);
		Debug.Log("========================================lobbySrvIndex=" + lobbySrvIndex);
		fstLobbySrvIndex = lobbySrvIndex;
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
			_gameModeCount_Create.Add(0);
		}
		COMA_Sys.Instance.tax = int.Parse(content[5]);
		COMA_ServerManager.Instance.serverAddr_IAP = content[6];
		COMA_ServerManager.Instance.deliverSvrAddr = content[7];
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
		COMA_Package.unlockPrice = int.Parse(content[20]);
		COMA_VideoController.Instance.nVideo = int.Parse(content[21]);
		ip_game = content[22];
		string c = content[23];
		string[] subPortStr = c.Split(',');
		string[] array2 = subPortStr;
		foreach (string s in array2)
		{
			port_game.Add(int.Parse(s));
		}
		COMA_Sys.Instance.bRateActive = content[24] == "1";
		TipPercent(PercentTips.Start_End);
		COMA_Sys.Instance.version = MiscPlugin.GetAppVersion();
		if (NeedUpdateVersion(COMA_Sys.Instance.version, v))
		{
			Tip_NeedUpdate();
			yield break;
		}
		ShowBillboard();
		COMA_Server_ID.Instance.InitServer(OnGetServerID, COMA_ServerManager.Instance.idSvrAddr, COMA_ServerManager.Instance.idSvrOutTime, COMA_ServerManager.Instance.idSvrKey);
		GameCenter.Instance.GCLogin(delegate(string state)
		{
			Debug.Log("GameCenter.Instance.GCLogin((string state) => " + state);
			if (state == "Success")
			{
				COMA_Server_ID.Instance.BindGameCenter("carnival", COMA_Sys.Instance.version, COMA_Sys.Instance.VID, string.Empty, GameCenter.Instance.gameCenterID);
			}
			RequestIDServer();
		});
		COMA_Pref.Instance.Load();
		iServerIAPVerify.GetInstance().Initialize(COMA_ServerManager.Instance.serverName_IAP, COMA_ServerManager.Instance.serverAddr_IAP, COMA_ServerManager.Instance.serverKey_IAP, COMA_ServerManager.Instance.serverTimeout_IAP);
		iIAPManager.GetInstance().Initialize(COMA_ServerManager.Instance.serverName_IAP, COMA_ServerManager.Instance.serverAddr_IAP, COMA_ServerManager.Instance.serverKey_IAP, COMA_ServerManager.Instance.serverTimeout_IAP);
		iServerIAPVerify.GetInstance().SetPurchaseCallBack(OnAddMoney);
		int nCount = COMA_IAPCheck.Instance.GetCheckIAPCount();
		if (nCount > 0)
		{
			_CertificateNum = nCount;
			_curCertiNum = 0;
			_NeedIAPCertificate = true;
			_CanIAPCertificate = true;
		}
	}

	public bool NeedUpdateVersion(string localVersiong, string v)
	{
		bool result = false;
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
		TipPercent(PercentTips.RequestTID_Start);
		Debug.Log("bNeedSaveGCGIDLocal:" + bNeedSaveGCGIDLocal);
		Debug.Log("bNeedSaveDeviceGIDLocal:" + bNeedSaveDeviceGIDLocal);
		Debug.Log("gameCenterID:" + GameCenter.Instance.gameCenterID);
		Debug.Log("device_gidLocal:" + COMA_GC_TID.Instance.device_gidLocal);
		Debug.Log("GID:" + COMA_Server_ID.Instance.GID);
		if (GameCenter.Instance.gameCenterID != string.Empty)
		{
			if (GameCenter.Instance.gameCenterID == COMA_GC_TID.Instance.gcLocal)
			{
				COMA_Server_ID.Instance.GID = COMA_GC_TID.Instance.gidLocal;
				OnGetServerID(false);
			}
			else if (COMA_GC_TID.Instance.device_gidLocal != string.Empty)
			{
				COMA_Server_ID.Instance.GID = COMA_GC_TID.Instance.device_gidLocal;
				OnGetServerID(false);
			}
			else
			{
				bNeedSaveGCGIDLocal = true;
				COMA_Server_ID.Instance.RegisterGame("carnival", COMA_Sys.Instance.version, COMA_Sys.Instance.VID, string.Empty, GameCenter.Instance.gameCenterID);
			}
		}
		else if (COMA_GC_TID.Instance.gcLocal != string.Empty)
		{
			COMA_Server_ID.Instance.GID = COMA_GC_TID.Instance.gidLocal;
			OnGetServerID(false);
		}
		else if (COMA_GC_TID.Instance.device_gidLocal != string.Empty)
		{
			COMA_Server_ID.Instance.GID = COMA_GC_TID.Instance.device_gidLocal;
			OnGetServerID(false);
		}
		else
		{
			bNeedSaveDeviceGIDLocal = true;
			COMA_Server_ID.Instance.RegisterGame("carnival", COMA_Sys.Instance.version, COMA_Sys.Instance.VID, string.Empty, GameCenter.Instance.gameCenterID);
		}
	}

	public void OnGetServerID(bool bTimeOut)
	{
		Debug.Log("ID服务器返回:" + COMA_Server_ID.Instance.GID);
		if (bTimeOut)
		{
			COMA_Server_ID.Instance.RegisterGame("carnival", COMA_Sys.Instance.version, COMA_Sys.Instance.VID, string.Empty, GameCenter.Instance.gameCenterID);
			return;
		}
		if (bNeedSaveGCGIDLocal)
		{
			COMA_GC_TID.Instance.gcLocal = GameCenter.Instance.gameCenterID;
			COMA_GC_TID.Instance.gidLocal = COMA_Server_ID.Instance.GID;
			COMA_GC_TID.Instance.Save();
			Debug.Log("COMA_GC_TID.Instance.Save() gcLocal:" + COMA_GC_TID.Instance.gcLocal);
			Debug.Log("COMA_GC_TID.Instance.Save() gidLocal:" + COMA_GC_TID.Instance.gidLocal);
		}
		if (bNeedSaveDeviceGIDLocal)
		{
			COMA_GC_TID.Instance.device_gidLocal = COMA_Server_ID.Instance.GID;
			COMA_GC_TID.Instance.Save();
			Debug.Log("COMA_GC_TID.Instance.Save() device_gidLocal:" + COMA_GC_TID.Instance.device_gidLocal);
		}
		TipPercent(PercentTips.RequestTID_End);
		if (COMA_Sys.Instance.version.StartsWith("999.998"))
		{
			string[] array = COMA_Sys.Instance.version.Split('.');
			COMA_Server_ID.Instance.GID = COMA_CommonOperation.Instance.GIDFormat(array[2]);
			Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++>" + COMA_Server_ID.Instance.GID);
		}
		string fileName = COMA_FileNameManager.Instance.GetFileName("LocalTID");
		COMA_Pref.Instance.playerID = COMA_FileIO.LoadFile(fileName);
		if (COMA_Pref.Instance.playerID == string.Empty)
		{
			COMA_Pref.Instance.playerID = COMA_Server_ID.Instance.GID;
		}
		if (COMA_Pref.Instance.playerID != COMA_Server_ID.Instance.GID)
		{
			Tip_NeedSync();
		}
		else
		{
			NextStep(COMA_Server_ID.Instance.GID);
		}
	}

	public void NextStep(string tarTID)
	{
		TipPercent(PercentTips.ConnectLobbyServer_Start);
		Debug.LogWarning("本地TID:" + COMA_Pref.Instance.playerID + "  服务器TID:" + COMA_Server_ID.Instance.GID + "  当前使用TID:" + tarTID);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_TIDReady, null, lstIPAndPort[lobbySrvIndex].Key, lstIPAndPort[lobbySrvIndex].Value);
		COMA_Server_ID.Instance.GID = tarTID;
		COMA_Pref.Instance.playerID = COMA_Server_ID.Instance.GID;
		string fileName = COMA_FileNameManager.Instance.GetFileName("LocalTID");
		COMA_FileIO.SaveFile(fileName, COMA_Server_ID.Instance.GID);
	}

	public bool ConnectGameServer()
	{
		if (port_game.Count > 0)
		{
			int num = port_game[UnityEngine.Random.Range(0, port_game.Count)];
			Debug.Log("Start Connect " + ip_game + ":" + num);
			COMA_NetworkConnect.Instance.StartConnect(ip_game, num);
			return true;
		}
		return false;
	}

	public bool ReconnectGameServer()
	{
		COMA_NetworkConnect.Instance.EndConnect();
		return ConnectGameServer();
	}

	private void IAPVerify()
	{
		if (!_NeedIAPCertificate || !_CanIAPCertificate)
		{
			return;
		}
		if (iServerIAPVerify.GetInstance().IsCanVerify())
		{
			Debug.Log("中断后进入游戏 send cmp srv...");
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
			Debug.Log("cmp srv cannot process");
			_CanIAPCertificate = false;
		}
	}

	public void ShowBillboard()
	{
		Debug.LogWarning(COMA_Sys.Instance.tipContent);
		if (COMA_Sys.Instance.tipContent == string.Empty)
		{
			return;
		}
		string text = COMA_FileIO.LoadFile(COMA_FileNameManager.Instance.GetFileName("Board"));
		Debug.Log(text + " " + COMA_Sys.Instance.tipContent);
		if (text != COMA_Sys.Instance.tipContent)
		{
			Debug.LogError(COMA_Sys.Instance.tipContent);
			string tipContent = COMA_Sys.Instance.tipContent;
			tipContent.Trim();
			if (tipContent.Length > 1)
			{
				COMA_FileIO.SaveFile(COMA_FileNameManager.Instance.GetFileName("Board"), COMA_Sys.Instance.tipContent);
				Tip_SysBillboard(COMA_Sys.Instance.tipContent);
			}
		}
	}

	private void Update()
	{
		if (Time.time - fpreVerifyTime >= 3f)
		{
			fpreVerifyTime = Time.time;
			IAPVerify();
		}
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
		Debug.Log("COMA_Lgoin  verify submit success at the background");
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
		}
		else if (sIAPKey == UI_IAP.IAPKeys[1])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(150);
		}
		else if (sIAPKey == UI_IAP.IAPKeys[2])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(350);
		}
		else if (sIAPKey == UI_IAP.IAPKeys[3])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(850);
		}
		else if (sIAPKey == UI_IAP.IAPKeys[4])
		{
			Debug.Log("can add money...");
			COMA_Pref.Instance.AddCrystal(2500);
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

	private void OnApplicationPause(bool pause)
	{
		Debug.Log("COMA_Login:" + pause);
		if (pause)
		{
			if (!(Application.loadedLevelName == "UI.IAP") && !COMA_VideoController.Instance.bVideoPopView)
			{
				Debug.Log("---------->close game");
				COMA_HTTP_DataCollect.Instance.SendCloseGameInfo();
				COMA_HTTP_DataCollect.Instance.SendGameInfo(_gameModeCount.ToArray(), _gameModeCount_Create.ToArray());
				for (int i = 0; i < _gameModeCount.Count; i++)
				{
					_gameModeCount[i] = 0;
					_gameModeCount_Create[i] = 0;
				}
				if (COMA_CommonOperation.Instance.IsPlayingMultiMode())
				{
					COMA_Pref.Instance.AddRankScoreOfCurrentScene(-20);
				}
			}
			return;
		}
		COMA_HTTP_DataCollect.Instance.SendStartGameInfo();
		if (bGameLoadFinishedForPauseLock)
		{
			bool flag = (COMA_VideoController.Instance.bClickKamcord ? true : false);
			if (COMA_CommonOperation.Instance.IsMode_Network(Application.loadedLevelName) || COMA_CommonOperation.Instance.IsWaittingRoom(Application.loadedLevelName))
			{
				Application.LoadLevel("COMA_Start");
				return;
			}
			if (flag)
			{
				COMA_VideoController.Instance.bClickKamcord = false;
				Debug.Log("------------------------------------------Back From Kamcord!!!!Switch Hall!");
				return;
			}
			Debug.Log("Come back!---Send Verify Data---");
			UIGolbalStaticFun.PopBlockOnlyMessageBox();
			VerifySessionCmd extraInfo = new VerifySessionCmd();
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
			UIDataBufferCenter.Instance.ReVerifyStep = UIDataBufferCenter.EReVerifyStep.Sending;
		}
	}
}
