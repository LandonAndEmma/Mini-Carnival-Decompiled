using MC_UIToolKit;
using MessageID;
using Protocol.Binary;
using UnityEngine;

public class UILobby_ComProtocolProcessor : UILobbyMessageHandler
{
	protected override void Load()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.RegisterHanlder(0, this);
			OnMessage(2, OnMaintain);
			OnMessage(3, GatewayBusy);
			OnMessage(5, VerifySessionResult);
		}
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	protected override void UnLoad()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.UnregisterHanlder(0, this);
		}
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	public bool OnMaintain(UnPacker unpacker)
	{
		Debug.Log(" ===========OnMaintain");
		UIDataBufferCenter.Instance.AddLobbySrvCloseReason(UIDataBufferCenter.ELobbySrvColseReason.Maintain);
		UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("fengmian_lianjietishi21"));
		uIMessage_CommonBoxData.Mark = "SrvMaintain";
		UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		return true;
	}

	public bool GatewayBusy(UnPacker unpacker)
	{
		Debug.Log(" ===========GatewayBusy");
		UIDataBufferCenter.Instance.AddLobbySrvCloseReason(UIDataBufferCenter.ELobbySrvColseReason.Busy);
		if (COMA_Login.Instance.ChangeLobbyGate())
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ReconnectingLobby, this, COMA_Login.Instance.lstIPAndPort[COMA_Login.Instance.lobbySrvIndex].Key, COMA_Login.Instance.lstIPAndPort[COMA_Login.Instance.lobbySrvIndex].Value);
		}
		else
		{
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(0, Localization.instance.Get("fengmian_lianjietishi22"));
			uIMessage_CommonBoxData.Mark = "SrvBusy";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		}
		return true;
	}

	public bool VerifySessionResult(UnPacker unpacker)
	{
		Debug.Log(" ===========VerifySessionResult");
		UIDataBufferCenter.Instance.ReVerifyStep = UIDataBufferCenter.EReVerifyStep.Confirmed;
		return true;
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "SrvMaintain":
			Application.LoadLevel("COMA_Start");
			break;
		case "SrvBusy":
			Application.LoadLevel("COMA_Start");
			break;
		case "SrvCloseSuddenly":
			Application.LoadLevel("COMA_Start");
			break;
		}
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
