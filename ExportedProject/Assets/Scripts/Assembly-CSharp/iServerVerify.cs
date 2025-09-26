using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class iServerVerify : MonoBehaviour
{
	public class CServerConfigInfo
	{
		public class CServerInfo
		{
			public string sName = string.Empty;

			public string sUrl = string.Empty;

			public string sKey = string.Empty;

			public float fTimeOut = -1f;
		}

		public string m_sVersion;

		public Dictionary<string, CServerInfo> m_dictServerInfo;

		public CServerConfigInfo()
		{
			m_dictServerInfo = new Dictionary<string, CServerInfo>();
			Clear();
		}

		public void Clear()
		{
			m_sVersion = string.Empty;
			m_dictServerInfo.Clear();
		}

		public CServerInfo GetServerInfo(string servername)
		{
			if (!m_dictServerInfo.ContainsKey(servername))
			{
				return null;
			}
			return m_dictServerInfo[servername];
		}

		public void LoadData(XmlDocument doc)
		{
			XmlNode documentElement = doc.DocumentElement;
			if (documentElement == null)
			{
				return;
			}
			string value = string.Empty;
			foreach (XmlNode item in documentElement)
			{
				if (item.Name == "common")
				{
					if (GetAttribute(item, "version", ref value))
					{
						m_sVersion = value;
					}
				}
				else
				{
					if (!(item.Name == "serverlist"))
					{
						continue;
					}
					Debug.Log(item.Name);
					foreach (XmlNode childNode in item.ChildNodes)
					{
						if (!(childNode.Name != "node") && GetAttribute(childNode, "name", ref value))
						{
							string text = value;
							CServerInfo cServerInfo = null;
							if (!m_dictServerInfo.ContainsKey(text))
							{
								cServerInfo = new CServerInfo();
								cServerInfo.sName = text;
								m_dictServerInfo.Add(text, cServerInfo);
							}
							else
							{
								cServerInfo = m_dictServerInfo[text];
							}
							if (GetAttribute(childNode, "url", ref value))
							{
								cServerInfo.sUrl = value;
							}
							if (GetAttribute(childNode, "key", ref value))
							{
								cServerInfo.sKey = value;
							}
							if (GetAttribute(childNode, "timeout", ref value))
							{
								cServerInfo.fTimeOut = float.Parse(value);
							}
						}
					}
				}
			}
		}

		protected bool GetAttribute(XmlNode node, string name, ref string value)
		{
			if (node == null || node.Attributes[name] == null)
			{
				return false;
			}
			value = node.Attributes[name].Value.Trim();
			if (value.Length < 1)
			{
				return false;
			}
			return true;
		}
	}

	protected enum kPingState
	{
		None = 0,
		Delay = 1,
		Pinging = 2,
		Success = 3,
		Fail = 4
	}

	public delegate void OnEvent();

	protected static iServerVerify m_Instance;

	protected OnEvent m_OnSuccess;

	protected OnEvent m_OnFailed;

	protected OnEvent m_OnNetError;

	protected CServerConfigInfo m_ServerConfigInfo;

	protected kPingState m_PingState;

	protected float m_fTimeOut;

	protected float m_fTimeOutCount;

	protected float m_fTimeDelay;

	protected float m_fTimeDelayCount;

	protected string m_sUrl = "http://account.trinitigame.com/game/CoMDH/CoMDH_ServerConfig.txt";

	protected string m_sVersion = "1.0.1";

	protected string m_sServerInfoFilePathSrc = "D:\\development\\_DinoCapWorld\\src\\_DinoCapWorld\\Documents\\serverconfig\\CoMDH_ServerConfig.xml";

	protected string m_sServerInfoFilePathDst = "D:\\development\\_DinoCapWorld\\src\\_DinoCapWorld\\Documents\\serverconfig\\CoMDH_ServerConfig.txt";

	protected string m_sServerInfoKey = "trinitigame_comdh";

	public string Version
	{
		get
		{
			return m_sVersion;
		}
		set
		{
			m_sVersion = value;
		}
	}

	public static iServerVerify GetInstance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject("_ServerVerify");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			m_Instance = gameObject.AddComponent<iServerVerify>();
		}
		return m_Instance;
	}

	public static void ResetInstance()
	{
		m_Instance = null;
	}

	public bool IsSuccess()
	{
		return m_PingState == kPingState.Success;
	}

	public bool IsFailed()
	{
		return m_PingState == kPingState.Fail;
	}

	public CServerConfigInfo GetServerConfigInfo()
	{
		return m_ServerConfigInfo;
	}

	private void Awake()
	{
		m_ServerConfigInfo = new CServerConfigInfo();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (m_PingState == kPingState.Delay)
		{
			m_fTimeDelayCount += Time.deltaTime;
			if (m_fTimeDelayCount >= m_fTimeDelay)
			{
				m_fTimeDelayCount = 0f;
				StartCoroutine(Connect());
			}
		}
		else
		{
			if (m_PingState != kPingState.Pinging)
			{
				return;
			}
			m_fTimeOutCount += Time.deltaTime;
			if (m_fTimeOutCount >= m_fTimeOut)
			{
				m_fTimeOutCount = 0f;
				Debug.Log("test ping time out ");
				m_PingState = kPingState.Fail;
				if (m_OnNetError != null)
				{
					m_OnNetError();
				}
			}
		}
	}

	public void ConnectServer(string sVersion, OnEvent onsuccess, OnEvent onfailed, OnEvent onneterror, float timeout = 10f, float delaytime = 0f)
	{
		m_sVersion = sVersion;
		m_OnSuccess = onsuccess;
		m_OnFailed = onfailed;
		m_OnNetError = onneterror;
		m_fTimeOut = timeout;
		m_fTimeOutCount = 0f;
		if (delaytime <= 0f)
		{
			StartCoroutine(Connect());
			return;
		}
		m_PingState = kPingState.Delay;
		m_fTimeDelay = delaytime;
		m_fTimeDelayCount = 0f;
	}

	protected IEnumerator Connect()
	{
		m_PingState = kPingState.Pinging;
		WWW www = new WWW(m_sUrl + "?rand=" + UnityEngine.Random.Range(10, 99999));
		Debug.Log(www.url);
		yield return www;
		if (m_PingState != kPingState.Pinging)
		{
			yield break;
		}
		if (www.error != null)
		{
			Debug.Log("net error " + www.error);
			m_PingState = kPingState.Fail;
			if (m_OnNetError != null)
			{
				m_OnNetError();
			}
			yield break;
		}
		if (www.text == null || www.text.Length < 1)
		{
			Debug.Log("text is not exist ");
			m_PingState = kPingState.Fail;
			if (m_OnFailed != null)
			{
				m_OnFailed();
			}
			yield break;
		}
		LoadServerData(www.text);
		if (m_sVersion != m_ServerConfigInfo.m_sVersion)
		{
			Debug.Log("version error " + m_ServerConfigInfo.m_sVersion);
			m_PingState = kPingState.Fail;
			if (m_OnFailed != null)
			{
				m_OnFailed();
			}
		}
		else
		{
			Debug.Log("serverconfig successed ");
			m_PingState = kPingState.Success;
			if (m_OnSuccess != null)
			{
				m_OnSuccess();
			}
		}
	}

	protected void LoadServerData(string input)
	{
		Debug.Log("LoadServerData " + input);
		m_ServerConfigInfo.Clear();
		string empty = string.Empty;
		try
		{
			empty = XXTEAUtils.Decrypt(input, m_sServerInfoKey);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(empty);
			m_ServerConfigInfo.LoadData(xmlDocument);
		}
		catch (Exception ex)
		{
			Debug.LogError("LoadServerData Error " + ex);
		}
	}

	protected void TransformXML2TXT(string srcpath, string dstpath, string key)
	{
		if (srcpath.Length < 1 || dstpath.Length < 1)
		{
			return;
		}
		string text = string.Empty;
		Debug.Log(srcpath);
		if (File.Exists(srcpath))
		{
			StreamReader streamReader = null;
			try
			{
				streamReader = new StreamReader(srcpath);
				text = streamReader.ReadToEnd();
			}
			catch
			{
				Debug.Log("ERROR - Encrypt()!!!");
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
		}
		if (text != null && text.Length > 0)
		{
			string value = XXTEAUtils.Encrypt(text, key);
			StreamWriter streamWriter = new StreamWriter(dstpath, false);
			streamWriter.Write(value);
			streamWriter.Flush();
			streamWriter.Close();
		}
	}
}
