using UnityEngine;

public class iServerFile : MonoBehaviour
{
	public delegate void OnSuccess(string sFileData);

	public delegate void OnFailed();

	protected static iServerFile m_Instance;

	protected OnSuccess m_OnSuccess;

	protected OnFailed m_OnFailed;

	protected string m_sUrl = string.Empty;

	protected WWW m_www;

	public static iServerFile Instance
	{
		get
		{
			if (m_Instance == null)
			{
				GameObject gameObject = new GameObject("_ServerFile");
				Object.DontDestroyOnLoad(gameObject);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				m_Instance = gameObject.AddComponent<iServerFile>();
			}
			return m_Instance;
		}
	}

	public static void ResetInstance()
	{
		m_Instance = null;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (m_www == null || !m_www.isDone)
		{
			return;
		}
		if (m_www.error != null)
		{
			Debug.Log(m_www.error);
			if (m_OnFailed != null)
			{
				m_OnFailed();
			}
		}
		else if (m_OnSuccess != null)
		{
			m_OnSuccess(m_www.text);
		}
		m_www = null;
	}

	public void Visit(string url, OnSuccess onsuccess, OnFailed onfailed)
	{
		m_sUrl = url;
		m_OnSuccess = onsuccess;
		m_OnFailed = onfailed;
		m_www = new WWW(m_sUrl + "?rand=" + Random.Range(10, 99999));
	}
}
