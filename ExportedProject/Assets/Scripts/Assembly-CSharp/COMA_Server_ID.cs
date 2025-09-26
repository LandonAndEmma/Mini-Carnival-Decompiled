using System;
using System.Collections;
using LitJson;
using UnityEngine;

public class COMA_Server_ID : MonoBehaviour
{
	private static COMA_Server_ID _instance;

	public OnServerIDResponse OnResponse;

	[NonSerialized]
	public string GID = string.Empty;

	[NonSerialized]
	public string NID = string.Empty;

	protected string serverName_ID = "svr_id";

	protected string serverName_Bind = "svr_bind";

	protected string actionName_register = "multiCsHandler.enter";

	public static COMA_Server_ID Instance
	{
		get
		{
			return _instance;
		}
	}

	public uint uintGID
	{
		get
		{
			return uint.Parse(GID);
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

	private void Start()
	{
	}

	public void InitServer(OnServerIDResponse dele, string addr, float timeout, string key)
	{
		OnResponse = dele;
		HttpClient.Instance().AddServer(serverName_ID, addr, timeout, key);
		HttpClient.Instance().AddServer(serverName_Bind, addr, timeout, key);
	}

	public void RegisterGame(string gameName, string version, string VID, string FBID, string GCID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("game", gameName);
		hashtable.Add("version", version);
		hashtable.Add("deviceId", VID);
		hashtable.Add("facebookId", FBID);
		hashtable.Add("gamecenterId", GCID);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_ID, actionName_register, text, base.gameObject.name, "COMA_Server_ID", "ReceiveFunction", string.Empty);
	}

	public void BindGameCenter(string gameName, string version, string VID, string FBID, string GCID)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("game", gameName);
		hashtable.Add("version", version);
		hashtable.Add("deviceId", VID);
		hashtable.Add("facebookId", FBID);
		hashtable.Add("gamecenterId", GCID);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(serverName_Bind, actionName_register, text, base.gameObject.name, "COMA_Server_ID", "ReceiveFunction", string.Empty);
	}

	public void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			Debug.LogError("result : " + result + " : " + server + " " + action);
			if (OnResponse != null)
			{
				OnResponse(true);
			}
		}
		else if (server == serverName_ID)
		{
			if (action == actionName_register)
			{
				JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
				GID = jsonData["tid"].ToString();
				Debug.Log("--------------GID:" + GID);
				NID = jsonData["serverId"].ToString();
				JsonData jsonData2 = jsonData["user"];
				string text = jsonData2["gamecenterId"].ToString();
				string text2 = jsonData2["facebookId"].ToString();
				Debug.Log("ID服务器返回  tid:" + GID + " GCID:" + text + " FBID:" + text2);
				if (OnResponse != null)
				{
					OnResponse(false);
				}
			}
		}
		else if (server == serverName_Bind)
		{
			Debug.Log("绑定GC返回 : " + response);
		}
	}
}
