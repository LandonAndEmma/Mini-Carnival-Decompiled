using System.Collections;
using System.Collections.Generic;
using LitJson;
using MC_UIToolKit;
using MessageID;
using Prime31;
using UnityEngine;

public class iIAPManager : MonoBehaviour
{
	protected enum kPingState
	{
		None = 0,
		Pinging = 1,
		Success = 2,
		Fail = 3
	}

	protected enum kPurchaseState
	{
		None = 0,
		Ping = 1,
		Purchase = 2
	}

	public delegate void OnIAPPurchaseSuccess(string sIAPKey, string sIdentifier, string sReceipt);

	public delegate void OnEvent();

	protected static iIAPManager m_Instance;

	private UIEntity entity = new UIEntity();

	public static readonly string[] IAPKeys = new string[5] { "com.trinitigame.isniperworld.099cents", "com.trinitigame.isniperworld.199cents2", "com.trinitigame.isniperworld.499cents2", "com.trinitigame.isniperworld.999cents2", "com.trinitigame.isniperworld.1999cents2" };

	protected OnIAPPurchaseSuccess m_OnIAPPurchaseSuccess;

	protected OnEvent m_OnIAPPurchaseFailed;

	protected OnEvent m_OnIAPPurchaseCancel;

	protected OnEvent m_OnIAPPurchaseNetError;

	protected string m_sCurIAPKey = string.Empty;

	private string m_amazon_userid = string.Empty;

	private bool itemOwned;

	protected kPingState m_PingState;

	protected kPurchaseState m_PurchaseState;

	protected string m_sServerName = string.Empty;

	protected string m_sServerUrl = string.Empty;

	protected string m_sKey = string.Empty;

	protected float m_fTimeOut = -1f;

	public static iIAPManager GetInstance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject("_IAPManager");
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			Object.DontDestroyOnLoad(gameObject);
			m_Instance = gameObject.AddComponent<iIAPManager>();
		}
		return m_Instance;
	}

	public static void ResetInstance()
	{
		m_Instance = null;
	}

	private void Awake()
	{
		m_PingState = kPingState.None;
		m_PurchaseState = kPurchaseState.None;
		if (GameObject.Find("_AndroidPlatform") == null)
		{
			GameObject gameObject = new GameObject("_AndroidPlatform");
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			Object.DontDestroyOnLoad(gameObject);
			DevicePlugin.InitAndroidPlatform();
			gameObject.AddComponent<TrinitiAdAndroidPlugin>();
			gameObject.AddComponent<AndroidQuit>();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		Update(Time.deltaTime);
	}

	private void OnEnable()
	{
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
		string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjRS+7QfZkSVgazVue/gL3XaqG9gsoUmHTEgT/P62YV7NQs6T9rbe+I31NQjcGDSZ9bFSUks028RdAJiutT5oh3ReoWyh35crerbK+4hOU8nng4eBFAF6+meH86rEeEXk8zXumvoBkF8i/P1J3AeBdOOCZumbLUKMSZovCgsSodMCGR8+a2zd43Yg5USaSbxx25ZoFm+LQ6gKDjK1OkCBzGV5a3PqlBUgqFjMOzjS8TMtyggKkMRXn2vi2Bjh/9vK703t9g95YuBrNnTPbeHKp91aCDEUgPPVtx0N8RD4L2oWSkCeTl59E/bwl8cnxb1NLPcmGdQZ7zSznsAaZ8sgXQIDAQAB";
		GoogleIAB.init(publicKey);
		GoogleIAB.setAutoVerifySignatures(true);
	}

	private void OnDisable()
	{
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
		GoogleIAB.unbindService();
	}

	private void billingSupportedEvent()
	{
	}

	private void billingNotSupportedEvent(string error)
	{
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		if (itemOwned)
		{
			GoogleIAB.consumeProduct(m_sCurIAPKey);
			itemOwned = false;
		}
		Prime31.Utils.logObject(purchases);
		Prime31.Utils.logObject(skus);
	}

	private void queryInventoryFailedEvent(string error)
	{
		if (itemOwned)
		{
			itemOwned = false;
		}
	}

	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
		GoogleIAB.consumeProduct(m_sCurIAPKey);
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		string productId = purchase.productId;
		string orderId = purchase.orderId;
		string originalJson = purchase.originalJson;
		string signature = purchase.signature;
		COMA_IAPCheck.Instance.AddToLocal(productId, orderId, originalJson, signature);
		if (iServerIAPVerify.GetInstance().IsCanVerify())
		{
			Debug.Log(" 正常流程iap send cmp srv...");
			iServerIAPVerify.GetInstance().VerifyIAP(productId, orderId, originalJson, signature, OnVerifySuccess, OnVerifyFailed, OnNetError, OnIAPVerifySubmitSuccess);
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ChangeTextIAPBlockBox, entity, Localization.instance.Get("beibaojiemian_desc15"));
		}
	}

	private void consumePurchaseFailedEvent(string error)
	{
		if (error.Substring(0, 14) == "Item not owned" || error.Substring(0, 14) == "Unable to buy ")
		{
			GoogleIAB.consumeProduct(m_sCurIAPKey);
		}
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
	}

	private void purchaseFailedEvent(string reason)
	{
		int num = reason.IndexOf("response: ");
		string text = reason.Substring(num + 10);
		num = text.IndexOf(':');
		switch (text.Substring(0, num))
		{
		case "7":
			itemOwned = true;
			GoogleIAB.queryInventory(new string[1] { m_sCurIAPKey });
			break;
		}
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
	}

	public bool IsCanPurchase()
	{
		return m_PurchaseState == kPurchaseState.None;
	}

	public void Initialize(string sServerName, string sServerUrl, string sKey, float fTimeOut)
	{
		m_sServerName = sServerName;
		m_sServerUrl = sServerUrl;
		m_sKey = sKey;
		m_fTimeOut = fTimeOut;
		HttpClient.Instance().AddServer(m_sServerName, m_sServerUrl, m_fTimeOut, (m_sKey.Length >= 1) ? m_sKey : null);
	}

	public void Purchase(string sIAPKey, OnIAPPurchaseSuccess onsuccess, OnEvent onfailed, OnEvent oncancel, OnEvent onneterror)
	{
		if (m_PurchaseState == kPurchaseState.None)
		{
			m_PurchaseState = kPurchaseState.Ping;
			m_sCurIAPKey = sIAPKey;
			m_OnIAPPurchaseSuccess = onsuccess;
			m_OnIAPPurchaseFailed = onfailed;
			m_OnIAPPurchaseCancel = oncancel;
			m_OnIAPPurchaseNetError = onneterror;
			StartCoroutine(TestPingApple());
		}
	}

	public void AmazonPurchase(string sIAPKey)
	{
		m_sCurIAPKey = sIAPKey;
	}

	public void GooglePurchase(string sIAPKey)
	{
		m_sCurIAPKey = sIAPKey;
		GoogleIAB.purchaseProduct(sIAPKey);
	}

	protected IEnumerator TestPingApple()
	{
		m_PingState = kPingState.Pinging;
		WWW www = new WWW("http://www.apple.com/?rand=" + Random.Range(10, 99999));
		yield return www;
		if (www.error != null)
		{
			Debug.Log("test ping failed " + www.error);
			m_PingState = kPingState.Fail;
		}
		else
		{
			Debug.Log("test ping successed ");
			m_PingState = kPingState.Success;
		}
	}

	protected void Update(float deltaTime)
	{
		if (m_PurchaseState == kPurchaseState.None)
		{
			return;
		}
		if (m_PurchaseState == kPurchaseState.Ping)
		{
			if (m_PingState != kPingState.Pinging)
			{
				if (m_PingState == kPingState.Success)
				{
					m_PingState = kPingState.None;
					IAPPlugin.NowPurchaseProduct(m_sCurIAPKey, "1");
					m_PurchaseState = kPurchaseState.Purchase;
				}
				else if (m_PingState == kPingState.Fail)
				{
					m_PingState = kPingState.None;
					OnPurchaseNetError();
				}
			}
		}
		else
		{
			if (m_PurchaseState != kPurchaseState.Purchase)
			{
				return;
			}
			int purchaseStatus = IAPPlugin.GetPurchaseStatus();
			if (purchaseStatus != 0)
			{
				if (purchaseStatus == 1)
				{
					OnPurchaseSuccess(m_sCurIAPKey);
				}
				else if (purchaseStatus == -2)
				{
					OnPurchaseCancel();
				}
				else if (purchaseStatus < 0)
				{
					OnPurchaseFailed();
				}
			}
		}
	}

	protected void SendServerVerify()
	{
		Debug.Log("SendServerVerify");
		m_PingState = kPingState.Pinging;
		Hashtable hashtable = new Hashtable();
		hashtable["cmd"] = "GetServerTime";
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text);
		HttpClient.Instance().SendRequest(m_sServerName, "groovy", text, "_IAPManager", "iIAPManager", "OnServerVerify", null);
	}

	protected void OnServerVerify(int taskId, int result, string server, string action, string response, string param)
	{
		Debug.Log("OnServerVerify " + action + " " + response);
		try
		{
			JsonData jsonData = JsonMapper.ToObject<JsonData>(response);
			if (int.Parse(jsonData["code"].ToString()) == 0)
			{
				m_PingState = kPingState.Success;
			}
			else
			{
				m_PingState = kPingState.Fail;
			}
		}
		catch
		{
			m_PingState = kPingState.Fail;
		}
	}

	public void OnPurchaseSuccess(string sIAPKey)
	{
		if (m_OnIAPPurchaseSuccess != null)
		{
			m_OnIAPPurchaseSuccess(sIAPKey, IAPPlugin.GetTransactionIdentifier(), IAPPlugin.GetTransactionReceipt());
		}
		m_PurchaseState = kPurchaseState.None;
	}

	public void OnPurchaseFailed()
	{
		if (m_OnIAPPurchaseFailed != null)
		{
			m_OnIAPPurchaseFailed();
		}
		m_PurchaseState = kPurchaseState.None;
	}

	public void OnPurchaseCancel()
	{
		if (m_OnIAPPurchaseCancel != null)
		{
			m_OnIAPPurchaseCancel();
		}
		m_PurchaseState = kPurchaseState.None;
	}

	public void OnPurchaseNetError()
	{
		if (m_OnIAPPurchaseNetError != null)
		{
			m_OnIAPPurchaseNetError();
		}
		m_PurchaseState = kPurchaseState.None;
	}

	public void OnVerifySuccess(bool bS, string sKey, string sIdentifier, string sReceipt, string sSignature)
	{
		COMA_IAPCheck.Instance.RemoveToLocal(sKey, sIdentifier, sReceipt, sSignature);
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
		if (bS)
		{
			COMA_HTTP_DataCollect.Instance.SendIAPInfo(sKey, sReceipt);
			return;
		}
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("beibaojiemian_desc12"));
		UIGolbalStaticFun.PopCommonMessageBox(data);
	}

	public void OnIAPVerifySubmitSuccess(string sKey, string sIdentifier, string sReceipt, string sSignature, string sRandom, int nRat, int nRatA, int nRatB)
	{
		COMA_IAPCheck.Instance.RemoveToLocal(sKey, sIdentifier, sReceipt, sSignature);
		COMA_IAPCheck.Instance.AddToLocal(sKey, sIdentifier, sReceipt, sSignature, sRandom, nRat.ToString(), nRatA.ToString(), nRatB.ToString());
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ChangeTextIAPBlockBox, entity, Localization.instance.Get("beibaojiemian_desc16"));
	}

	public void OnVerifyFailed(string sKey, string sIdentifier, string sReceipt, string sSignature)
	{
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("beibaojiemian_desc13"));
		UIGolbalStaticFun.PopCommonMessageBox(data);
	}

	public void OnAddMoney(string sIAPKey)
	{
		if (sIAPKey == IAPKeys[0])
		{
			Debug.Log("can add money...25");
			COMA_Pref.Instance.AddCrystal(25);
		}
		else if (sIAPKey == IAPKeys[1])
		{
			Debug.Log("can add money...150");
			COMA_Pref.Instance.AddCrystal(150);
		}
		else if (sIAPKey == IAPKeys[2])
		{
			Debug.Log("can add money...350");
			COMA_Pref.Instance.AddCrystal(350);
		}
		else if (sIAPKey == IAPKeys[3])
		{
			Debug.Log("can add money...850");
			COMA_Pref.Instance.AddCrystal(850);
		}
		else if (sIAPKey == IAPKeys[4])
		{
			Debug.Log("can add money...2500");
			COMA_Pref.Instance.AddCrystal(2500);
		}
		COMA_Pref.Instance.Save(true);
	}

	public void OnNetError(string sIAPKey)
	{
		UIGolbalStaticFun.CloseIAPBlockMessageBox();
		UIMessage_CommonBoxData data = new UIMessage_CommonBoxData(1, Localization.instance.Get("fengmian_lianjietishi1"));
		UIGolbalStaticFun.PopCommonMessageBox(data);
	}
}
