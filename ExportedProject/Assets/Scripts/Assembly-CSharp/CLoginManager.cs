using UnityEngine;

public class CLoginManager
{
	public enum kFailedType
	{
		None = 0,
		VersionError = 1,
		GameCenterChanged = 2,
		FetchFailed = 3
	}

	public enum kState
	{
		None = 0,
		StartApp = 1,
		BackToApp = 2,
		Success = 3,
		Failed = 4
	}

	public delegate void OnSuccess();

	public delegate void OnFailed(kFailedType failedtype);

	public delegate void OnNetError();

	protected static CLoginManager m_Instance;

	protected kFailedType m_FailedType;

	protected OnSuccess m_OnSuccess;

	protected OnFailed m_OnFailed;

	protected OnNetError m_OnNetError;

	protected kState m_State;

	public CLoginManager()
	{
		m_State = kState.None;
	}

	public static CLoginManager GetInstance()
	{
		if (m_Instance == null)
		{
			m_Instance = new CLoginManager();
		}
		return m_Instance;
	}

	public static void ResetInstance()
	{
		m_Instance = null;
	}

	public void StartApp(OnSuccess onsuccess, OnFailed onfailed, OnNetError onneterror)
	{
		m_State = kState.StartApp;
		m_OnSuccess = onsuccess;
		m_OnFailed = onfailed;
		m_OnNetError = onneterror;
		iServerVerify.GetInstance().ConnectServer(iMacroDefine.sVersion, OnVerifySuccess, OnVerifyFailed, OnVerifyNetError, 10f, 0f);
	}

	public void BackToApp(OnSuccess onsuccess, OnFailed onfailed, OnNetError onneterror)
	{
		m_State = kState.BackToApp;
		m_OnSuccess = onsuccess;
		m_OnFailed = onfailed;
		m_OnNetError = onneterror;
		iServerVerify.GetInstance().ConnectServer(iMacroDefine.sVersion, OnVerifySuccess, OnVerifyFailed, OnVerifyNetError, 10f, 0f);
	}

	protected void OnLoginSuccess()
	{
		m_State = kState.Success;
		if (m_OnSuccess != null)
		{
			m_OnSuccess();
		}
		CMessageBoxScript.GetInstance().MessageBox(string.Empty, iServerSaveData.GetInstance().GetSaveDataDesc(), null, null, "OK");
		iServerIAPVerifyBackground.GetInstance().SetActive(true);
	}

	protected void OnLoginFailed(kFailedType type)
	{
		m_State = kState.Failed;
		if (m_OnFailed != null)
		{
			m_OnFailed(type);
		}
	}

	protected void OnLoginNetError()
	{
		m_State = kState.Failed;
		if (m_OnNetError != null)
		{
			m_OnNetError();
		}
	}

	protected void OnVerifySuccess()
	{
		iServerVerify.CServerConfigInfo serverConfigInfo = iServerVerify.GetInstance().GetServerConfigInfo();
		if (serverConfigInfo != null)
		{
			iServerVerify.CServerConfigInfo.CServerInfo cServerInfo = null;
			cServerInfo = serverConfigInfo.GetServerInfo("dataserver");
			if (cServerInfo != null)
			{
				iServerDataManager.GetInstance().Initialize(cServerInfo.sName, cServerInfo.sUrl, cServerInfo.sKey, cServerInfo.fTimeOut);
				iGameCenter.GetInstance().Login(OnGameCenterLoginSuccess, OnGameCenterLoginFailed);
			}
			cServerInfo = serverConfigInfo.GetServerInfo("iapserver");
			if (cServerInfo != null)
			{
				iIAPManager.GetInstance().Initialize(cServerInfo.sName, cServerInfo.sUrl, cServerInfo.sKey, cServerInfo.fTimeOut);
				iServerIAPVerify.GetInstance().Initialize(cServerInfo.sName, cServerInfo.sUrl, cServerInfo.sKey, cServerInfo.fTimeOut);
			}
			cServerInfo = serverConfigInfo.GetServerInfo("collectserver");
			if (cServerInfo == null)
			{
			}
		}
	}

	protected void OnVerifyFailed()
	{
		m_FailedType = kFailedType.VersionError;
		OnLoginFailed(m_FailedType);
	}

	protected void OnVerifyNetError()
	{
		OnLoginNetError();
	}

	protected void OnGameCenterLoginSuccess(string sAccount)
	{
		if (m_State == kState.BackToApp && !iServerSaveData.GetInstance().CheckGCAccountIsValid(sAccount))
		{
			m_FailedType = kFailedType.GameCenterChanged;
			OnLoginFailed(m_FailedType);
		}
		else
		{
			iServerSaveData.GetInstance().SetCurGCAccount(sAccount);
		}
	}

	protected void OnGameCenterLoginFailed()
	{
		if (m_State == kState.BackToApp && !iServerSaveData.GetInstance().CheckGCAccountIsValid(string.Empty))
		{
			m_FailedType = kFailedType.GameCenterChanged;
			OnLoginFailed(m_FailedType);
		}
		else
		{
			iServerSaveData.GetInstance().SetCurGCAccount(string.Empty);
		}
	}

	protected void OnFetchSuccess()
	{
		iServerConfigData.GetInstance().Fetch(OnFetchConfigSuccess, OnFetchConfigFailed);
	}

	protected void OnFetchFailed()
	{
		m_FailedType = kFailedType.FetchFailed;
		OnLoginFailed(m_FailedType);
	}

	protected void OnFetchConfigSuccess()
	{
		OnLoginSuccess();
	}

	protected void OnFetchConfigFailed()
	{
		Debug.Log("OnFetchConfigFailed");
		m_FailedType = kFailedType.FetchFailed;
		OnLoginFailed(m_FailedType);
	}
}
