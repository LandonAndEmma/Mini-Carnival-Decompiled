using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class iServerDataManager : MonoBehaviour
{
	public delegate void OnEvent();

	public delegate void OnSuccess(string str);

	protected static iServerDataManager m_Instance;

	protected OnSuccess m_OnSuccess;

	protected OnEvent m_OnFailed;

	protected string m_sServerName = string.Empty;

	protected string m_sServerUrl = string.Empty;

	protected string m_sKey = string.Empty;

	protected float m_fTimeOut = -1f;

	protected Dictionary<string, string> m_dictRandom;

	public iServerDataManager()
	{
		m_dictRandom = new Dictionary<string, string>();
	}

	public static iServerDataManager GetInstance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject("_ServerDataManager");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			m_Instance = gameObject.AddComponent<iServerDataManager>();
		}
		return m_Instance;
	}

	public static void ResetInstance()
	{
		m_Instance = null;
	}

	public void Initialize(string sServerName, string sServerUrl, string sKey, float fTimeOut)
	{
		m_sServerName = sServerName;
		m_sServerUrl = sServerUrl;
		m_sKey = sKey;
		m_fTimeOut = fTimeOut;
		m_fTimeOut = -1f;
		HttpClient.Instance().AddServer(m_sServerName, m_sServerUrl, m_fTimeOut, (m_sKey.Length >= 1) ? m_sKey : null);
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		Update(Time.deltaTime);
	}

	public void Update(float deltaTime)
	{
		HttpClient.Instance().HandleResponse();
	}

	public void SendFetchSaveData(string userid, OnSuccess successfunc, OnEvent failedfunc)
	{
		m_OnSuccess = successfunc;
		m_OnFailed = failedfunc;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("userId", userid);
		HttpClient.Instance().SendRequest(m_sServerName, "userHandler.loadProfile&json=" + WWW.EscapeURL(JsonMapper.ToJson(hashtable)), "nodata", "_ServerDataManager", "iServerDataManager", "OnFetchSaveDataResult", null);
	}

	protected void OnFetchSaveDataResult(int taskId, int result, string server, string action, string response, string param)
	{
		Debug.Log("OnFetchSaveDataResult " + result + " " + action + " " + response + " " + param);
		try
		{
			JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
			if (int.Parse(jsonData["code"].ToString()) == 0)
			{
				string userid = jsonData["userId"].ToString();
				string text = jsonData["rand"].ToString();
				string text2 = string.Empty;
				if (jsonData["profile"].IsString)
				{
					text2 = jsonData["profile"].ToString();
				}
				else if (jsonData["profile"].IsObject)
				{
					text2 = jsonData["profile"].ToJson();
				}
				Debug.Log(text + " " + text2);
				SetUserRandom(userid, text);
				if (m_OnSuccess != null)
				{
					m_OnSuccess(text2);
				}
			}
			else if (m_OnFailed != null)
			{
				m_OnFailed();
			}
		}
		catch
		{
			if (m_OnFailed != null)
			{
				m_OnFailed();
			}
		}
	}

	public void SendUploadSaveData(string userid, string sData, OnSuccess successfunc, OnEvent failedfunc)
	{
		m_OnSuccess = successfunc;
		m_OnFailed = failedfunc;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("userId", userid);
		hashtable.Add("rand", GetUserRandom(userid));
		HttpClient.Instance().SendRequest(m_sServerName, "userHandler.saveProfile&json=" + WWW.EscapeURL(JsonMapper.ToJson(hashtable)), sData, "_ServerDataManager", "iServerDataManager", "OnUploadSaveDataResult", null);
	}

	protected void OnUploadSaveDataResult(int taskId, int result, string server, string action, string response, string param)
	{
		Debug.Log("OnUploadSaveDataResult " + result + " " + action + " " + response);
		try
		{
			JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
			if (int.Parse(jsonData["code"].ToString()) == 0)
			{
				string text = jsonData["userId"].ToString();
				string text2 = jsonData["rand"].ToString();
				Debug.Log("upload sucess random = " + text2);
				if (m_OnSuccess != null)
				{
					m_OnSuccess(string.Empty);
				}
			}
			else if (m_OnFailed != null)
			{
				m_OnFailed();
			}
		}
		catch
		{
			if (m_OnFailed != null)
			{
				m_OnFailed();
			}
		}
	}

	protected void SetUserRandom(string userid, string random)
	{
		if (!m_dictRandom.ContainsKey(userid))
		{
			m_dictRandom.Add(userid, random);
		}
		else
		{
			m_dictRandom[userid] = random;
		}
	}

	protected string GetUserRandom(string userid)
	{
		if (!m_dictRandom.ContainsKey(userid))
		{
			return "000";
		}
		return m_dictRandom[userid];
	}
}
