using System.Collections;
using LitJson;
using UnityEngine;

public class iServerIAPVerify : MonoBehaviour
{
	protected enum kVerifyState
	{
		None = 0,
		Send = 1,
		WaitResult = 2
	}

	public delegate void OnVerifySuccess(bool bSuccess, string sKey, string sIdentifier, string sReceipt, string sSignature);

	public delegate void OnVerifyFailed(string sKey, string sIdentifier, string sReceipt, string sSignature);

	public delegate void OnVerifySubmitSuccess(string sKey, string sIdentifier, string sReceipt, string sSignature, string sRandom, int nRat, int nRatA, int nRatB);

	public delegate void OnEvent(string sIAPKey);

	protected static iServerIAPVerify m_Instance;

	protected OnVerifySuccess m_OnVerifyIAP_S;

	protected OnVerifyFailed m_OnVerifyIAP_F;

	protected OnVerifySubmitSuccess m_OnVerifySubmit_S;

	protected OnEvent m_OnPurchaseIAP;

	protected OnEvent m_OnNetError;

	protected kVerifyState m_VerifyState;

	protected string m_sCurIAPKey = string.Empty;

	protected string m_sCurTID = string.Empty;

	protected string m_sCurReceipt = string.Empty;

	protected string m_sCurSignature = string.Empty;

	protected string m_sRandom = string.Empty;

	protected int m_nRat;

	protected int m_nRatA;

	protected int m_nRatB;

	protected float m_fVerifyTime = 1f;

	protected float m_fVerifyTimeCount;

	protected string m_sServerName = string.Empty;

	protected string m_sServerUrl = string.Empty;

	protected string m_sKey = string.Empty;

	protected float m_fTimeOut = -1f;

	public static iServerIAPVerify GetInstance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject("_ServerIAPVerify");
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			Object.DontDestroyOnLoad(gameObject);
			m_Instance = gameObject.AddComponent<iServerIAPVerify>();
		}
		return m_Instance;
	}

	public static void ResetInstance()
	{
		m_Instance = null;
	}

	private void Awake()
	{
		m_VerifyState = kVerifyState.None;
	}

	private void Start()
	{
	}

	private void Update()
	{
		Update(Time.deltaTime);
	}

	public bool IsCanVerify()
	{
		return m_VerifyState == kVerifyState.None;
	}

	public void Initialize(string sServerName, string sServerUrl, string sKey, float fTimeOut)
	{
		m_sServerName = sServerName;
		m_sServerUrl = sServerUrl;
		m_sKey = sKey;
		m_fTimeOut = fTimeOut;
		HttpClient.Instance().AddServer(m_sServerName, m_sServerUrl, m_fTimeOut, (m_sKey.Length >= 1) ? m_sKey : null);
	}

	public void SetPurchaseCallBack(OnEvent onpurchaseiap)
	{
		m_OnPurchaseIAP = onpurchaseiap;
	}

	public void VerifyIAP(string iapkey, string identifier, string receipt, string signature, OnVerifySuccess onsuccess, OnVerifyFailed onfailed, OnEvent onneterror, OnVerifySubmitSuccess onsubmitsuccess)
	{
		if (m_VerifyState == kVerifyState.None && iapkey.Length >= 1)
		{
			m_sCurIAPKey = iapkey;
			m_sCurTID = identifier;
			m_sCurReceipt = receipt;
			m_sCurSignature = signature;
			m_OnVerifyIAP_S = onsuccess;
			m_OnVerifyIAP_F = onfailed;
			m_OnNetError = onneterror;
			m_OnVerifySubmit_S = onsubmitsuccess;
			SendIAPVerifyRequest(m_sCurIAPKey, m_sCurTID, m_sCurReceipt);
		}
	}

	public void VerifyIAPCheck(string iapkey, string identifier, string receipt, string signature, string sRandom, int nRat, int nRatA, int nRatB, OnVerifySuccess onsuccess, OnVerifyFailed onfailed, OnEvent onneterror)
	{
		if (m_VerifyState == kVerifyState.None && iapkey.Length >= 1)
		{
			m_sCurIAPKey = iapkey;
			m_sCurTID = identifier;
			m_sCurReceipt = receipt;
			m_sCurSignature = signature;
			m_sRandom = sRandom;
			m_nRat = nRat;
			m_nRatA = nRatA;
			m_nRatB = nRatB;
			m_OnVerifyIAP_S = onsuccess;
			m_OnVerifyIAP_F = onfailed;
			m_OnNetError = onneterror;
			SendIAPVerifyResultRequest(m_sCurTID, m_sCurReceipt);
		}
	}

	protected void Update(float deltaTime)
	{
		if (m_VerifyState == kVerifyState.WaitResult)
		{
			m_fVerifyTimeCount += deltaTime;
			if (!(m_fVerifyTimeCount < m_fVerifyTime))
			{
				m_fVerifyTimeCount = 0f;
				SendIAPVerifyResultRequest(m_sCurTID, m_sCurReceipt);
			}
		}
	}

	protected void OnSuccess(bool bSuccess, string sKey, string sIdentifier, string sReceipt, string sSignature)
	{
		if (bSuccess && m_OnPurchaseIAP != null)
		{
			m_OnPurchaseIAP(sKey);
		}
		if (m_OnVerifyIAP_S != null)
		{
			m_OnVerifyIAP_S(bSuccess, sKey, sIdentifier, sReceipt, sSignature);
		}
		m_VerifyState = kVerifyState.None;
	}

	protected void OnFailed(string sKey, string sIdentifier, string sReceipt, string sSignature)
	{
		if (m_OnVerifyIAP_F != null)
		{
			m_OnVerifyIAP_F(sKey, sIdentifier, sReceipt, sSignature);
		}
		m_VerifyState = kVerifyState.None;
	}

	protected void OnNetError(string sKey)
	{
		if (m_OnNetError != null)
		{
			m_OnNetError(sKey);
		}
		m_VerifyState = kVerifyState.None;
	}

	protected void OnSubmitSuccess(string sKey, string sIdentifier, string sReceipt, string sSignature, string sRandom, int nRat, int nRatA, int nRatB)
	{
		if (m_OnVerifySubmit_S != null)
		{
			m_OnVerifySubmit_S(sKey, sIdentifier, sReceipt, sSignature, sRandom, nRat, nRatA, nRatB);
		}
	}

	protected void SendIAPVerifyRequest(string iapkey, string tid, string receipt)
	{
		m_VerifyState = kVerifyState.Send;
		Hashtable hashtable = new Hashtable();
		hashtable["cmd"] = "purchase/android/UserPurchaseBuy";
		hashtable["aid"] = iMacroDefine.BundleID;
		hashtable["uuid"] = COMA_Server_ID.Instance.GID;
		hashtable["pid"] = iapkey;
		m_sCurTID = tid;
		hashtable["tid"] = m_sCurTID;
		hashtable["info"] = receipt;
		hashtable["signature"] = m_sCurSignature;
		m_sRandom = Random.Range(1, 10).ToString();
		hashtable["rand"] = m_sRandom;
		m_nRat = Random.Range(1, 10);
		hashtable["rat"] = m_nRat;
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(m_sServerName, "groovy", data, "_ServerIAPVerify", "iServerIAPVerify", "OnIAPVerifyRequest", null);
	}

	protected void OnIAPVerifyRequest(int taskId, int result, string server, string action, string response, string param)
	{
		if (m_VerifyState != kVerifyState.Send)
		{
			return;
		}
		if (result != 0)
		{
			OnNetError(m_sCurIAPKey);
			return;
		}
		try
		{
			JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
			if (int.Parse(jsonData["code"].ToString()) != 0)
			{
				OnFailed(m_sCurIAPKey, m_sCurTID, m_sCurReceipt, m_sCurSignature);
				return;
			}
			m_nRatA = int.Parse(jsonData["rata"].ToString());
			m_nRatB = int.Parse(jsonData["ratb"].ToString());
			OnSubmitSuccess(m_sCurIAPKey, m_sCurTID, m_sCurReceipt, m_sCurSignature, m_sRandom, m_nRat, m_nRatA, m_nRatB);
			SendIAPVerifyResultRequest(m_sCurTID, m_sCurReceipt);
		}
		catch
		{
			OnFailed(m_sCurIAPKey, m_sCurTID, m_sCurReceipt, m_sCurSignature);
		}
	}

	protected void SendIAPVerifyResultRequest(string tid, string receipt)
	{
		m_VerifyState = kVerifyState.WaitResult;
		m_fVerifyTimeCount = 0f;
		Hashtable hashtable = new Hashtable();
		hashtable["cmd"] = "purchase/android/GetPurchaseVerify";
		hashtable["transactionId"] = m_sCurTID;
		hashtable["randPara"] = m_sRandom;
		string data = JsonMapper.ToJson(hashtable);
		HttpClient.Instance().SendRequest(m_sServerName, "groovy", data, "_ServerIAPVerify", "iServerIAPVerify", "OnIAPVerifyResultRequest", null);
	}

	protected void OnIAPVerifyResultRequest(int taskId, int result, string server, string action, string response, string param)
	{
		if (m_VerifyState != kVerifyState.WaitResult)
		{
			return;
		}
		if (result != 0)
		{
			OnNetError(m_sCurIAPKey);
			return;
		}
		try
		{
			JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
			if (int.Parse(jsonData["code"].ToString()) != 0)
			{
				OnFailed(m_sCurIAPKey, m_sCurTID, m_sCurReceipt, m_sCurSignature);
				return;
			}
			switch (int.Parse(jsonData["sta"].ToString()))
			{
			case -1:
				break;
			case 0:
			{
				string text = jsonData["pid"].ToString();
				int num = int.Parse(jsonData["ratresult"].ToString());
				string text2 = jsonData["aid"].ToString();
				if (text2 != iMacroDefine.BundleID)
				{
					OnFailed(m_sCurIAPKey, m_sCurTID, m_sCurReceipt, m_sCurSignature);
				}
				else if (num != m_nRat * m_nRatA / 9 + m_nRatB - 3)
				{
					OnFailed(m_sCurIAPKey, m_sCurTID, m_sCurReceipt, m_sCurSignature);
				}
				else
				{
					OnSuccess(true, m_sCurIAPKey, m_sCurTID, m_sCurReceipt, m_sCurSignature);
				}
				break;
			}
			default:
				OnSuccess(false, m_sCurIAPKey, m_sCurTID, m_sCurReceipt, m_sCurSignature);
				break;
			}
		}
		catch
		{
			OnFailed(m_sCurIAPKey, m_sCurTID, m_sCurReceipt, m_sCurSignature);
		}
	}
}
