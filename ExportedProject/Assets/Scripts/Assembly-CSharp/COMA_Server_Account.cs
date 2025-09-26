using System;
using System.Collections;
using LitJson;
using UnityEngine;

public class COMA_Server_Account : MonoBehaviour
{
	private static COMA_Server_Account _instance;

	public OnServerAccountResponse OnResponse;

	public double svrTime;

	protected double fClearPreTime;

	protected string serverName_Account = "svr_account";

	protected string actionName_Get = "comavataraccount/GetAccount";

	protected string actionName_SetName = "comavataraccount/SetName";

	public static COMA_Server_Account Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
	}

	private void OnEnable()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDisable()
	{
		_instance = null;
	}

	public double GetServerTime()
	{
		return svrTime + (double)Time.realtimeSinceStartup;
	}

	public int GetCurMonth()
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(GetServerTime());
		Debug.Log(dateTime);
		return dateTime.Month;
	}

	public void ClearSrvtime()
	{
		fClearPreTime = svrTime;
		svrTime = 0.0;
	}

	public void RecoverSrvtime()
	{
		svrTime = fClearPreTime;
	}

	private void Start()
	{
	}

	public void InitServer(OnServerAccountResponse dele, string addr, float timeout, string key)
	{
		OnResponse = dele;
		HttpClient.Instance().AddServer(serverName_Account, addr, timeout, key);
	}

	public void DeliverGame(string GID, string GCID, string NID, string uName)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		hashtable.Add("nid", NID);
		hashtable.Add("gid", GCID);
		hashtable.Add("uname", uName);
		string text = JsonMapper.ToJson(hashtable);
		Debug.LogWarning(text);
		HttpClient.Instance().SendRequest(serverName_Account, actionName_Get, text, base.gameObject.name, "COMA_Server_Account", "ReceiveFunction", string.Empty);
	}

	public void SetName(string GID, string uName)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", GID);
		hashtable.Add("uname", uName);
		string text = JsonMapper.ToJson(hashtable);
		Debug.LogWarning(text);
		HttpClient.Instance().SendRequest(serverName_Account, actionName_SetName, text, base.gameObject.name, "COMA_Server_Account", "ReceiveFunction", string.Empty);
	}

	public void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			Debug.LogError("result : " + result + " : " + server + " " + action);
			if (OnResponse != null)
			{
				OnResponse(true, true);
			}
			return;
		}
		Debug.Log("分服服务器返回：" + response);
		if (server == serverName_Account && action == actionName_Get)
		{
			Debug.Log("AccountSvr:" + response);
			JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
			COMA_ServerManager.Instance.saverSvrAddr = jsonData["dataServer"].ToString();
			COMA_ServerManager.Instance.serverAddr_Save = jsonData["fileServer"].ToString();
			bool bNoPref = bool.Parse(jsonData["isNew"].ToString());
			if (OnResponse != null)
			{
				OnResponse(false, bNoPref);
			}
			svrTime = int.Parse(jsonData["nowTime"].ToString());
			Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++ Server Time : " + svrTime);
			Debug.Log(GetCurMonth());
		}
	}
}
