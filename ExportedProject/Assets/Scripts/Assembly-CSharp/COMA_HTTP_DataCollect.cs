using System;
using System.Collections;
using LitJson;
using UnityEngine;

public class COMA_HTTP_DataCollect : MonoBehaviour
{
	private bool isInit;

	private static COMA_HTTP_DataCollect _instance;

	private string serverName_DataCollect = "svr_dataCollect";

	private string _action = "logAllInfo";

	private string _gameName = "com.trinitigame.avatar";

	[NonSerialized]
	public string strTime1 = string.Empty;

	public static COMA_HTTP_DataCollect Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null)
		{
			UnityEngine.Object.DestroyObject(base.gameObject);
			return;
		}
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public void InitDataCollectServer()
	{
		HttpClient.Instance().AddServer(serverName_DataCollect, COMA_ServerManager.Instance.dataCollectSvrAddr, COMA_ServerManager.Instance.dataCollectSvrOutTime, string.Empty);
		isInit = true;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void SendToSrv(string data)
	{
		if (isInit)
		{
			HttpClient.Instance().SendRequest(serverName_DataCollect, _action, data, base.gameObject.name, "COMA_HTTP_DataCollect", "ReceiveFunction", string.Empty);
		}
	}

	public void SendStartGameInfo()
	{
		string value = DateTime.Now.ToString();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "1");
		hashtable.Add("_T", value);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendCloseGameInfo()
	{
		string value = DateTime.Now.ToString();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "2");
		hashtable.Add("_T", value);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendIAPInfo(string strCNum, string strR)
	{
		string value = DateTime.Now.ToString();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "3");
		hashtable.Add("_T", value);
		hashtable.Add("_CN", strCNum);
		hashtable.Add("_R", strR);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log("SendIAPInfo---------------:" + text);
		SendToSrv(text);
	}

	public void SendCrystalInfo(string strCNum, string strCSpendNum, string strPurpose)
	{
		string value = DateTime.Now.ToString();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "4");
		hashtable.Add("_T", value);
		hashtable.Add("_CC", strCNum);
		hashtable.Add("_SC", strCSpendNum);
		hashtable.Add("_P", strPurpose);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendGoldInfo(string strGNum, string strGSpendNum, string strPurpose)
	{
		string value = DateTime.Now.ToString();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "5");
		hashtable.Add("_T", value);
		hashtable.Add("_CG", strGNum);
		hashtable.Add("_SG", strGSpendNum);
		hashtable.Add("_P", strPurpose);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendGameInfo(int[] counts, int[] counts_create)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "6");
		hashtable.Add("_HC", counts[0] + "," + counts_create[0]);
		hashtable.Add("_RC", counts[1] + "," + counts_create[1]);
		hashtable.Add("_FC", counts[2] + "," + counts_create[2]);
		hashtable.Add("_DC", counts[3] + "," + counts_create[3]);
		hashtable.Add("_TC", counts[4] + "," + counts_create[4]);
		hashtable.Add("_PC", counts[5] + "," + counts_create[5]);
		hashtable.Add("_FHC", counts[6] + "," + counts_create[6]);
		hashtable.Add("_BC", counts[7] + "," + counts_create[7]);
		hashtable.Add("_TKC", counts[8] + "," + counts_create[8]);
		hashtable.Add("_BH", counts[9] + "," + counts_create[9]);
		hashtable.Add("_FLP", counts[10] + "," + counts_create[10]);
		string data = JsonMapper.ToJson(hashtable);
		int num = 0;
		foreach (int num2 in counts)
		{
			num += num2;
		}
		if (num != 0)
		{
			SendToSrv(data);
		}
	}

	public void SendTextureSell(string strType, string tid, string strGold)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "7");
		hashtable.Add("type", strType);
		hashtable.Add("tid", tid);
		hashtable.Add("gold", strGold);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendBuyAvatar(string strSelluuid, string strType, string tid, string strGold)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "8");
		hashtable.Add("suuid", strSelluuid);
		hashtable.Add("type", strType);
		hashtable.Add("tid", tid);
		hashtable.Add("gold", strGold);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendUnlockPackage(string strCrystal, string strNum)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "9");
		hashtable.Add("crys", strCrystal);
		hashtable.Add("num", strNum);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendBuyModel(string strType, string strGold)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "10");
		hashtable.Add("mtype", strType);
		hashtable.Add("gold", strGold);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendRefreshShop(string strCrystal)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "11");
		hashtable.Add("crys", strCrystal);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendLevel(string strLv, string strLvTo)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "12");
		hashtable.Add("level", strLv);
		hashtable.Add("levelup", strLvTo);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendDrawTime(string tid, string strTime2)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "13");
		hashtable.Add("tid", tid);
		hashtable.Add("time1", strTime1);
		hashtable.Add("time2", strTime2);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendBuyAccessory_Gold(string aid, string strGold)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "14");
		hashtable.Add("aid", aid);
		hashtable.Add("gold", strGold);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendBuyAccessory_Crystal(string aid, string strCrystal)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "15");
		hashtable.Add("aid", aid);
		hashtable.Add("crys", strCrystal);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendGetCouponNumFromFriend(string couponNum)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "16");
		hashtable.Add("card", couponNum);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendRPGBattleCount(string count)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "17");
		hashtable.Add("battle", count);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendUnlockRPGBackpackByGemCount(string count)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "18");
		hashtable.Add("open", count);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendBuyCouponByGemCount(string count1, string count2)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "19");
		hashtable.Add("card1", count1);
		hashtable.Add("card2", count2);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendBuyGemCount(string count)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "20");
		hashtable.Add("stone", count);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendCompoundCardCount(string count)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "21");
		hashtable.Add("card", count);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void SendCompoundGemCount(string count)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamename", _gameName);
		hashtable.Add("uuid", COMA_Pref.Instance.playerID);
		hashtable.Add("action", "22");
		hashtable.Add("stone", count);
		string data = JsonMapper.ToJson(hashtable);
		SendToSrv(data);
	}

	public void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			Debug.LogError("COMA_HTTP_DataCollect result : " + result);
		}
	}
}
