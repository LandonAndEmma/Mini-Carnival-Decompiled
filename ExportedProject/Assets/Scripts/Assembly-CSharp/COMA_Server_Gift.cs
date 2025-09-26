using System.Collections;
using LitJson;
using UnityEngine;

public class COMA_Server_Gift : MonoBehaviour
{
	private static COMA_Server_Gift _instance;

	protected string serverName_ID = "svr_id";

	protected string serverName_Bind = "svr_bind";

	protected string actionName_register = "multiCsHandler.enter";

	public static COMA_Server_Gift Instance
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
		Object.DontDestroyOnLoad(base.gameObject);
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
		HttpClient.Instance().SendRequest(serverName_ID, actionName_register, text, base.gameObject.name, "COMA_Server_Gift", "ReceiveFunction", string.Empty);
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
		HttpClient.Instance().SendRequest(serverName_Bind, actionName_register, text, base.gameObject.name, "COMA_Server_Gift", "ReceiveFunction", string.Empty);
	}

	public void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			Debug.LogError("result : " + result + " : " + server + " " + action);
		}
		else if (server == serverName_ID)
		{
			if (!(action == actionName_register))
			{
			}
		}
		else if (server == serverName_Bind)
		{
			Debug.Log("绑定GC返回 : " + response);
		}
	}
}
