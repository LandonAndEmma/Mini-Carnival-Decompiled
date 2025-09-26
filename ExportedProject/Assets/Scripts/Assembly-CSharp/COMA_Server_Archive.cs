using System;
using System.Collections;
using LitJson;
using UnityEngine;

public class COMA_Server_Archive : MonoBehaviour
{
	protected const string serverName_Archive = "svr_pref";

	private static COMA_Server_Archive _instance;

	public OnServerArchiveResponse_Get OnResponse_get;

	[NonSerialized]
	public string TK = string.Empty;

	protected string actionName_Archive_Set = "Callofminiavatar/SetBase";

	protected string actionName_Archive_Get = "Callofminiavatar/GetBase";

	protected string actionName_Archive_Update = "Callofminiavatar/UpdateBase";

	public static COMA_Server_Archive Instance
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

	private void Start()
	{
	}

	public void InitServer(OnServerArchiveResponse_Get dele, string addr, float timeout, string key)
	{
		OnResponse_get = dele;
		HttpClient.Instance().AddServer("svr_pref", addr, timeout, key);
	}

	public void PlayerPref_Set(string json)
	{
		Debug.Log(json);
		HttpClient.Instance().SendRequest("svr_pref", actionName_Archive_Set, json, base.gameObject.name, "COMA_Server_Archive", "ReceiveFunction", string.Empty);
	}

	public void PlayerPref_Get(string pid)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("uuid", pid);
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest("svr_pref", actionName_Archive_Get, text, base.gameObject.name, "COMA_Server_Archive", "ReceiveFunction", string.Empty);
	}

	public void PlayerPref_Upload(string json)
	{
		Debug.Log(json);
		HttpClient.Instance().SendRequest("svr_pref", actionName_Archive_Update, json, base.gameObject.name, "COMA_Server_Archive", "ReceiveFunction", string.Empty);
	}

	public void ReceiveFunction(int taskId, int result, string server, string action, string response, object param)
	{
		if (result != 0)
		{
			Debug.LogError("result : " + result + " : " + server + " " + action);
			if (action == actionName_Archive_Get && OnResponse_get != null)
			{
				OnResponse_get(true, string.Empty);
			}
			return;
		}
		switch (server)
		{
		case "svr_pref":
			if (action == actionName_Archive_Set)
			{
				Debug.Log("Set Pref Success!!");
				Debug.Log(response);
				JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
				if (response.Contains("\"invalid_token\":"))
				{
					int num = int.Parse(jsonData["invalid_token"].ToString());
					if (num != 0)
					{
						Debug.LogError("invalid_token : " + num);
					}
					if (num == 1)
					{
						UI_MsgBox uI_MsgBox = TUI_MsgBox.Instance.MessageBox(126);
						uI_MsgBox.AddProceYesHandler(QuitGame);
					}
				}
			}
			else
			{
				if (!(action == actionName_Archive_Get))
				{
					break;
				}
				Debug.Log("Get Pref Success!!");
				Debug.Log(response);
				if (response.Contains("\"code\":1"))
				{
					if (OnResponse_get != null)
					{
						OnResponse_get(false, string.Empty);
					}
					break;
				}
				JsonData jsonData2 = JsonMapper.ToObject<JsonData>(response);
				TK = jsonData2["token"].ToString();
				Debug.Log("token:" + TK);
				if (OnResponse_get != null)
				{
					OnResponse_get(false, response);
				}
			}
			break;
		default:
			Debug.LogError(server);
			break;
		}
	}

	public void QuitGame(string param)
	{
		Application.Quit();
	}
}
