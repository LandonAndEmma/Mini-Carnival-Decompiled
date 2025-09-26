using UnityEngine;

public class iServerIAPVerifyBackground : MonoBehaviour
{
	protected static iServerIAPVerifyBackground m_Instance;

	protected bool m_bAcitve;

	protected iGameData m_GameData;

	protected iDataCenter m_DataCenter;

	protected string m_sCurIAPKey = string.Empty;

	protected string m_sCurTID = string.Empty;

	protected string m_sCurReceipt = string.Empty;

	public static iServerIAPVerifyBackground GetInstance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject("_ServerIAPVerifyBackground");
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			Object.DontDestroyOnLoad(gameObject);
			m_Instance = gameObject.AddComponent<iServerIAPVerifyBackground>();
		}
		return m_Instance;
	}

	public static void ResetInstance()
	{
		m_Instance = null;
	}

	private void Awake()
	{
		m_bAcitve = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (m_bAcitve)
		{
		}
	}

	public void SetActive(bool bActive)
	{
		m_bAcitve = bActive;
	}

	protected void OnIAPVerifySuccess(string sKey, string sIdentifier, string sReceipt)
	{
		if (m_DataCenter != null)
		{
			Debug.Log("verify success at the background");
			iServerSaveData.GetInstance().SetUploadFlag(true, true);
		}
	}

	protected void OnIAPVerifyFailed(string sKey, string sIdentifier, string sReceipt)
	{
		if (m_DataCenter != null)
		{
			Debug.Log("verify failed at the background");
			iServerSaveData.GetInstance().SetUploadFlag(true, true);
		}
	}

	protected void OnIAPVerifySubmitSuccess(string sKey, string sIdentifier, string sReceipt, string sRandom, int nRat, int nRatA, int nRatB)
	{
		if (m_DataCenter != null)
		{
			Debug.Log("verify submit success at the background");
		}
	}
}
