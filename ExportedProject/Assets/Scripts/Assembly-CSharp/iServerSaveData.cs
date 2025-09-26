using System.Collections;
using LitJson;
using UnityEngine;

public class iServerSaveData : MonoBehaviour
{
	protected enum kState
	{
		None = 0,
		Fetch = 1,
		Successed = 2,
		Failed = 3
	}

	protected enum kStateBindSave
	{
		None = 0,
		LocalData = 1,
		BindGCData = 2,
		LoginGCData = 3,
		MACData = 4
	}

	protected enum kStateUpload
	{
		None = 0,
		Upload = 1,
		Success = 2,
		Failed = 3
	}

	public delegate void OnEvent();

	public delegate string OnPackData();

	public delegate void OnDialog(string msg, OnEvent onok, OnEvent oncancel);

	public delegate bool OnUnPackData(string sData);

	protected static iServerSaveData m_Instance;

	protected OnEvent m_OnFetchSuccess;

	protected OnEvent m_OnFetchFailed;

	protected OnEvent m_OnUploadSuccess;

	protected OnEvent m_OnUploadFailed;

	protected OnPackData m_OnPackData;

	protected OnDialog m_OnDialog;

	protected OnUnPackData m_OnUnPackData;

	protected kState m_State;

	protected kStateBindSave m_StateBindSave;

	protected kStateUpload m_StateUpload;

	protected string m_sLoginGCAccount;

	protected string m_sLoginGCData;

	protected string m_sBindGCAccount;

	protected string m_sMACAddress;

	protected string m_sSaveData;

	protected bool m_bNeedUpload;

	protected float m_fCheckUploadTime = 60f;

	protected float m_fCheckUploadTimeCount;

	protected int m_nUploadFailedCur;

	protected int m_nUploadFailedMax = 3;

	protected bool m_bNeedNextUpload;

	protected bool m_bAtMainScene;

	protected bool m_bStep_LoginGC_FirstBind_YesResult;

	public bool IsMainScene
	{
		get
		{
			return m_bAtMainScene;
		}
		set
		{
			m_bAtMainScene = value;
		}
	}

	public static iServerSaveData GetInstance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject("_ServerSaveData");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			m_Instance = gameObject.AddComponent<iServerSaveData>();
		}
		return m_Instance;
	}

	public static void ResetInstance()
	{
		m_Instance = null;
	}

	public bool IsFetchSuccess()
	{
		return m_State == kState.Successed;
	}

	public bool IsFetchFailed()
	{
		return m_State == kState.Failed;
	}

	public bool IsUploadSuccess()
	{
		return m_StateUpload == kStateUpload.Success;
	}

	public bool IsUploadFailed()
	{
		return m_StateUpload == kStateUpload.Failed;
	}

	public string GetSaveDataDesc()
	{
		string empty = string.Empty;
		switch (m_StateBindSave)
		{
		case kStateBindSave.BindGCData:
			return m_StateBindSave.ToString() + " " + m_sBindGCAccount;
		case kStateBindSave.LoginGCData:
			return m_StateBindSave.ToString() + " " + m_sLoginGCAccount;
		case kStateBindSave.MACData:
			return m_StateBindSave.ToString() + " " + m_sMACAddress;
		default:
			return "no data";
		}
	}

	protected void OnFetchDataSuccess(kStateBindSave statebindsave)
	{
		if (m_OnFetchSuccess != null)
		{
			m_OnFetchSuccess();
		}
		m_State = kState.Successed;
		m_StateBindSave = statebindsave;
		Debug.Log("fetch success, save type is " + m_StateBindSave);
	}

	protected void OnFetchDataFailed()
	{
		if (m_OnFetchFailed != null)
		{
			m_OnFetchFailed();
		}
		m_State = kState.Failed;
	}

	protected void OnUploadDataSuccess(string sData)
	{
		if (m_OnUploadSuccess != null)
		{
			m_OnUploadSuccess();
		}
		m_StateUpload = kStateUpload.Success;
		Debug.Log("upload success");
	}

	protected void OnUploadDataFailed()
	{
		if (m_OnUploadFailed != null)
		{
			m_OnUploadFailed();
		}
		m_StateUpload = kStateUpload.Failed;
		Debug.Log("upload failed");
	}

	protected bool PackData(ref string sData)
	{
		if (m_OnPackData == null)
		{
			return false;
		}
		try
		{
			sData = m_OnPackData();
			return true;
		}
		catch
		{
			return false;
		}
	}

	protected bool UnPackData(string sData)
	{
		if (m_OnUnPackData == null)
		{
			return false;
		}
		try
		{
			return m_OnUnPackData(sData);
		}
		catch
		{
			return false;
		}
	}

	public void SetCurGCAccount(string sAccount)
	{
		m_sLoginGCAccount = sAccount;
	}

	public bool CheckGCAccountIsValid(string sAccount)
	{
		switch (m_StateBindSave)
		{
		case kStateBindSave.BindGCData:
			if (m_sBindGCAccount != sAccount)
			{
				return false;
			}
			break;
		case kStateBindSave.LoginGCData:
			if (m_sLoginGCAccount != sAccount)
			{
				return false;
			}
			break;
		case kStateBindSave.MACData:
			if (sAccount.Length > 0)
			{
				return false;
			}
			break;
		default:
			if (sAccount.Length > 0)
			{
				return false;
			}
			break;
		}
		return true;
	}

	public void Fetch(OnEvent onsuccess, OnEvent onfailed, OnPackData onpack, OnUnPackData onunpack)
	{
		Debug.Log("start to fetch");
		m_OnFetchSuccess = onsuccess;
		m_OnFetchFailed = onfailed;
		m_OnPackData = onpack;
		m_OnUnPackData = onunpack;
		m_State = kState.Fetch;
		iServerDataManager.GetInstance().SendFetchSaveData(m_sMACAddress, Step_FetchMACData_S, Step_FetchMACData_F);
	}

	public void Upload(OnEvent onsuccess, OnEvent onfailed, OnPackData onpack)
	{
		if (m_StateBindSave == kStateBindSave.None || m_StateBindSave == kStateBindSave.LocalData)
		{
			return;
		}
		m_OnUploadSuccess = onsuccess;
		m_OnUploadFailed = onfailed;
		m_OnPackData = onpack;
		m_OnUnPackData = null;
		if (PackData(ref m_sSaveData))
		{
			switch (m_StateBindSave)
			{
			case kStateBindSave.BindGCData:
				iServerDataManager.GetInstance().SendUploadSaveData(m_sBindGCAccount, m_sSaveData, OnUploadDataSuccess, OnUploadDataFailed);
				break;
			case kStateBindSave.LoginGCData:
				iServerDataManager.GetInstance().SendUploadSaveData(m_sLoginGCAccount, m_sSaveData, OnUploadDataSuccess, OnUploadDataFailed);
				break;
			case kStateBindSave.MACData:
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("data", m_sSaveData);
				iServerDataManager.GetInstance().SendUploadSaveData(m_sMACAddress, JsonMapper.ToJson(hashtable), OnUploadDataSuccess, OnUploadDataFailed);
				break;
			}
			}
			m_StateUpload = kStateUpload.Upload;
			m_fCheckUploadTimeCount = 0f;
			Debug.Log("start upload savedata, save type is " + m_StateBindSave);
		}
	}

	public void ClearServerData()
	{
		Debug.Log("ClearServerData");
		switch (m_StateBindSave)
		{
		case kStateBindSave.BindGCData:
			iServerDataManager.GetInstance().SendUploadSaveData(m_sBindGCAccount, "clear", null, null);
			break;
		case kStateBindSave.LoginGCData:
			iServerDataManager.GetInstance().SendUploadSaveData(m_sLoginGCAccount, "clear", null, null);
			break;
		case kStateBindSave.MACData:
			iServerDataManager.GetInstance().SendUploadSaveData(m_sMACAddress, "clear", null, null);
			break;
		default:
			iServerDataManager.GetInstance().SendUploadSaveData(m_sMACAddress, "clear", null, null);
			break;
		}
	}

	public void SetUploadFlag(bool bNeedUpload, bool bImmidately = false)
	{
		m_bNeedUpload = bNeedUpload;
		if (m_bNeedUpload && m_StateUpload != kStateUpload.Upload)
		{
			if (bImmidately)
			{
				Upload(null, null, m_OnPackData);
			}
			else
			{
				m_bNeedNextUpload = true;
			}
		}
	}

	private void Awake()
	{
		m_sLoginGCAccount = string.Empty;
		m_sBindGCAccount = string.Empty;
		m_State = kState.None;
		m_StateBindSave = kStateBindSave.None;
	}

	private void Start()
	{
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		Update(deltaTime);
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			ClearServerData();
		}
	}

	private void OnApplicationPause(bool bPause)
	{
		Debug.Log("iServerSaveData:" + bPause);
		if (bPause)
		{
			if (!m_bAtMainScene)
			{
				Upload(null, null, m_OnPackData);
			}
		}
		else if (!m_bAtMainScene)
		{
			CLoginManager.GetInstance().BackToApp(OnBackToAppSuccess, OnBackToAppFailed, OnBackToAppNetError);
		}
	}

	protected void OnBackToAppSuccess()
	{
	}

	protected void OnBackToAppFailed(CLoginManager.kFailedType type)
	{
		CMessageBoxScript.GetInstance().MessageBox(string.Empty, "something is error " + type, OnOK, null, "OK");
	}

	protected void OnBackToAppNetError()
	{
		CMessageBoxScript.GetInstance().MessageBox(string.Empty, "net is disconnect", OnOK, null, "OK");
	}

	protected void OnOK()
	{
	}

	protected void Update(float deltaTime)
	{
		if (m_StateUpload != kStateUpload.Upload && m_bNeedNextUpload)
		{
			m_bNeedNextUpload = false;
			Upload(OnUploadSuccess, OnUploadFailed, m_OnPackData);
		}
		if (!m_bNeedUpload)
		{
			return;
		}
		m_fCheckUploadTimeCount += deltaTime;
		if (m_fCheckUploadTimeCount >= m_fCheckUploadTime)
		{
			m_fCheckUploadTimeCount = 0f;
			if (m_StateUpload != kStateUpload.Upload)
			{
				Upload(OnUploadSuccess, OnUploadFailed, m_OnPackData);
			}
		}
	}

	protected void OnUploadSuccess()
	{
		m_nUploadFailedCur = 0;
	}

	protected void OnUploadFailed()
	{
		m_nUploadFailedCur++;
		if (m_nUploadFailedCur >= m_nUploadFailedMax)
		{
			m_nUploadFailedCur = 0;
			return;
		}
		m_fCheckUploadTimeCount = 0f;
		Upload(null, null, m_OnPackData);
	}

	protected void Step_FetchMACData_S(string sData)
	{
		Debug.Log("fetch macaddress information success " + sData);
		if (sData.Length < 1)
		{
			Debug.Log("no data on server, upload local savedata");
			if (!PackData(ref m_sSaveData))
			{
				OnFetchDataFailed();
				return;
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add("data", sData);
			iServerDataManager.GetInstance().SendUploadSaveData(m_sMACAddress, JsonMapper.ToJson(hashtable), Step_FristUpLoad_S, Step_FristUpLoad_F);
			return;
		}
		try
		{
			JsonReader reader = new JsonReader(sData);
			Hashtable hashtable2 = JsonMapper.ToObject<Hashtable>(reader);
			if (hashtable2 == null)
			{
				return;
			}
			if (hashtable2["gamecenter"] != null)
			{
				Debug.Log("your macaddress bind a gc account, fetch the gc savedata");
				m_sBindGCAccount = hashtable2["gamecenter"].ToString();
				Debug.Log("bind gc account = " + m_sBindGCAccount);
				if (m_sLoginGCAccount.Length < 1)
				{
					Step_BindGC_Fetch();
				}
				else if (m_sBindGCAccount == m_sLoginGCAccount)
				{
					Step_BindGC_Fetch();
				}
				else
				{
					Step_LoginGC_Fetch();
				}
			}
			else if (hashtable2["data"] != null)
			{
				string text = hashtable2["data"].ToString();
				if (!UnPackData(text))
				{
					OnFetchDataFailed();
					return;
				}
				Debug.Log("macaddress save data is ok");
				m_sSaveData = text;
				OnFetchDataSuccess(kStateBindSave.MACData);
			}
		}
		catch
		{
			Debug.Log("error data on server, upload local savedata");
			if (!PackData(ref m_sSaveData))
			{
				OnFetchDataFailed();
				return;
			}
			Hashtable hashtable3 = new Hashtable();
			hashtable3.Add("data", m_sSaveData);
			iServerDataManager.GetInstance().SendUploadSaveData(m_sMACAddress, JsonMapper.ToJson(hashtable3), Step_FristUpLoad_S, Step_FristUpLoad_F);
		}
	}

	protected void Step_FetchMACData_F()
	{
		Debug.Log("fetch macaddress information failed");
		OnFetchDataFailed();
	}

	protected void Step_FristUpLoad_S(string sData)
	{
		Debug.Log("first upload success, your savedata is online");
		if (m_sLoginGCAccount.Length < 1)
		{
			Debug.Log("no gc account login , so play macaddress savedata");
			if (!UnPackData(sData))
			{
				OnFetchDataFailed();
				return;
			}
			m_sSaveData = sData;
			OnFetchDataSuccess(kStateBindSave.MACData);
		}
		else
		{
			Debug.Log("gc account login, fetch gc account savedata");
			iServerDataManager.GetInstance().SendFetchSaveData(m_sLoginGCAccount, Step_LoginGC_FetchSaveData_S, Step_LoginGC_FetchSaveData_F);
		}
	}

	protected void Step_FristUpLoad_F()
	{
		Debug.Log("first upload failed, play localdata");
		OnFetchDataFailed();
	}

	protected void Step_LoginGC_FetchSaveData_S(string sData)
	{
		Debug.Log("fetch logined gc account data success");
		try
		{
			if (sData.Length < 1)
			{
				Debug.Log("there is no logined gc account data online. you can upload it");
				return;
			}
			Debug.Log("logined gc account data is ready");
			if (!UnPackData(sData))
			{
				OnFetchDataFailed();
			}
			else
			{
				m_sLoginGCData = sData;
			}
		}
		catch
		{
			Debug.Log("logined gc account data is error. you can upload it");
		}
	}

	protected void Step_LoginGC_FetchSaveData_F()
	{
		Debug.Log("fetch logined gc account data failed, play macaddress savedata");
		if (UnPackData(m_sSaveData))
		{
			OnFetchDataSuccess(kStateBindSave.MACData);
		}
		else
		{
			OnFetchDataFailed();
		}
	}

	protected void Step_LoginGC_FirstBind_Yes()
	{
		Debug.Log("bind to logined gc account which has no data online");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamecenter", m_sLoginGCAccount);
		iServerDataManager.GetInstance().SendUploadSaveData(m_sMACAddress, JsonMapper.ToJson(hashtable), Step_LoginGC_FirstBind_Yes_S, Step_LoginGC_FirstBind_Yes_F);
		iServerDataManager.GetInstance().SendUploadSaveData(m_sLoginGCAccount, m_sSaveData, Step_LoginGC_FirstBind_Yes_S, Step_LoginGC_FirstBind_Yes_F);
		m_bStep_LoginGC_FirstBind_YesResult = false;
	}

	protected void Step_LoginGC_FirstBind_No()
	{
		Debug.Log("dont bind to logined gc account, play macaddress savedata");
		if (UnPackData(m_sSaveData))
		{
			OnFetchDataSuccess(kStateBindSave.MACData);
		}
		else
		{
			OnFetchDataFailed();
		}
	}

	protected void Step_LoginGC_FirstBind_Yes_S(string sData)
	{
		if (!m_bStep_LoginGC_FirstBind_YesResult)
		{
			Debug.Log("bind to logined gc account ok no.1");
			m_bStep_LoginGC_FirstBind_YesResult = true;
		}
		else if (m_bStep_LoginGC_FirstBind_YesResult)
		{
			Debug.Log("bind to logined gc account ok no.2, binded ok");
			if (UnPackData(m_sSaveData))
			{
				OnFetchDataSuccess(kStateBindSave.BindGCData);
				m_sBindGCAccount = m_sLoginGCAccount;
			}
			else
			{
				OnFetchDataFailed();
			}
		}
	}

	protected void Step_LoginGC_FirstBind_Yes_F()
	{
		Debug.Log("bind to logined gc account failed");
	}

	protected void Step_LoginGC_UseData_Yes()
	{
		Debug.Log("use logined gc account savedata");
	}

	protected void Step_LoginGC_UseData_No()
	{
		Debug.Log("dont use logined gc account savedata, play mac savedata");
		if (UnPackData(m_sSaveData))
		{
			OnFetchDataSuccess(kStateBindSave.MACData);
		}
		else
		{
			OnFetchDataFailed();
		}
	}

	protected void Step_LoginGC_UseData_Bind_Yes()
	{
		Debug.Log("bind to logined gc account, new relationship");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamecenter", m_sLoginGCAccount);
		iServerDataManager.GetInstance().SendUploadSaveData(m_sMACAddress, JsonMapper.ToJson(hashtable), Step_LoginGC_UseData_Bind_Yes_S, Step_LoginGC_UseData_Bind_Yes_F);
	}

	protected void Step_LoginGC_UseData_Bind_No()
	{
		Debug.Log("dont bind to logined gc account, play logined gc account save data");
		if (UnPackData(m_sLoginGCData))
		{
			OnFetchDataSuccess(kStateBindSave.LoginGCData);
		}
		else
		{
			OnFetchDataFailed();
		}
	}

	protected void Step_LoginGC_UseData_Bind_Yes_S(string sData)
	{
		Debug.Log("new relation ship success");
		if (UnPackData(m_sLoginGCData))
		{
			OnFetchDataSuccess(kStateBindSave.BindGCData);
			m_sBindGCAccount = m_sLoginGCAccount;
		}
		else
		{
			OnFetchDataFailed();
		}
	}

	protected void Step_LoginGC_UseData_Bind_Yes_F()
	{
		Debug.Log("new relation ship failed");
	}

	protected void Step_BindGC_Fetch()
	{
		Debug.Log("fetch binded gc account savedata");
		iServerDataManager.GetInstance().SendFetchSaveData(m_sBindGCAccount, Step_BindGC_Fetch_Yes_S, Step_BindGC_Fetch_Yes_F);
	}

	protected void Step_BindGC_Fetch_Yes_S(string sData)
	{
		Debug.Log("fetch binded gc account savedata success");
		if (UnPackData(sData))
		{
			OnFetchDataSuccess(kStateBindSave.BindGCData);
		}
		else
		{
			OnFetchDataFailed();
		}
	}

	protected void Step_BindGC_Fetch_Yes_F()
	{
		Debug.Log("fetch binded gc account savedata failed");
		OnFetchDataFailed();
	}

	protected void Step_LoginGC_Fetch()
	{
		Debug.Log("fetch logined gc account savedata");
		iServerDataManager.GetInstance().SendFetchSaveData(m_sLoginGCAccount, Step_LoginGC_Fetch_Yes_S, Step_LoginGC_Fetch_Yes_F);
	}

	protected void Step_LoginGC_Fetch_Yes_S(string sData)
	{
		Debug.Log("fetch logined gc account savedata success");
		m_sLoginGCData = sData;
	}

	protected void Step_LoginGC_Fetch_Yes_F()
	{
		Debug.Log("fetch logined gc account savedata failed");
		Step_BindGC_Fetch();
	}

	protected void Step_LoginGC_BindNew_Yes()
	{
		Debug.Log("bind to login gc account, new relationship");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("gamecenter", m_sLoginGCAccount);
		iServerDataManager.GetInstance().SendUploadSaveData(m_sMACAddress, JsonMapper.ToJson(hashtable), Step_LoginGC_BindNew_Yes_Upload_S, Step_LoginGC_BindNew_Yes_Upload_F);
	}

	protected void Step_LoginGC_BindNew_No()
	{
		Debug.Log("dont bind to login gc account");
		Step_LoginGC_BindNew_Yes_Upload_F();
	}

	protected void Step_LoginGC_BindNew_Yes_Upload_S(string sData)
	{
		Debug.Log("new relationship success");
		if (UnPackData(m_sLoginGCData))
		{
			OnFetchDataSuccess(kStateBindSave.BindGCData);
			m_sBindGCAccount = m_sLoginGCAccount;
		}
		else
		{
			OnFetchDataFailed();
		}
	}

	protected void Step_LoginGC_BindNew_Yes_Upload_F()
	{
		Debug.Log("new relationship failed");
		if (UnPackData(m_sLoginGCData))
		{
			OnFetchDataSuccess(kStateBindSave.LoginGCData);
		}
		else
		{
			OnFetchDataFailed();
		}
	}
}
