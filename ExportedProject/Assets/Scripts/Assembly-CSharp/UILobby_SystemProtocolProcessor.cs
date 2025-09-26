using MC_UIToolKit;
using MessageID;
using Protocol.Binary;
using Protocol.Role.C2S;
using UnityEngine;

public class UILobby_SystemProtocolProcessor : UILobbyMessageHandler
{
	public enum ESystemEvent
	{
		OnConnected = 0,
		OnConnectError = 1,
		OnConnectTimeout = 2,
		OnClosed = 3
	}

	protected override void Load()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.RegisterHanlder(byte.MaxValue, this);
			OnMessage(0, OnConnected);
			OnMessage(1, OnConnectError);
			OnMessage(2, OnConnectTimeout);
			OnMessage(3, OnClosed);
		}
		RegisterMessage(EUIMessageID.UIError_LobbyDataPacket, this, UIErrorLobbyDataPacket);
	}

	protected override void UnLoad()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.UnregisterHanlder(byte.MaxValue, this);
		}
		UnregisterMessage(EUIMessageID.UIError_LobbyDataPacket, this);
	}

	public bool OnConnected(UnPacker unpacker)
	{
		Debug.Log("connect lobby success, send login");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UILobbyheart_Open, this, null);
		LoginCmd loginCmd = new LoginCmd();
		loginCmd.m_player_id = COMA_Server_ID.Instance.uintGID;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, loginCmd);
		return true;
	}

	public bool OnConnectError(UnPacker unpacker)
	{
		Debug.Log("connect lobby Error!");
		Debug.Log("=================================\n Reconnecting...");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_ReconnectingLobby, this, COMA_Login.Instance.lstIPAndPort[COMA_Login.Instance.lobbySrvIndex].Key, COMA_Login.Instance.lstIPAndPort[COMA_Login.Instance.lobbySrvIndex].Value);
		return true;
	}

	public bool OnConnectTimeout(UnPacker unpacker)
	{
		Debug.Log("connect lobby Timeout!");
		UIDataBufferCenter.Instance.AddLobbySrvCloseReason(UIDataBufferCenter.ELobbySrvColseReason.TimeOut);
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

	public bool OnClosed(UnPacker unpacker)
	{
		Debug.Log("connect lobby Closed!");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_DisconnectLobby, null, null);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UILobbyheart_Close, this, null);
		if (UIDataBufferCenter.Instance.LobbySrvIsNoReasonClosed())
		{
			UIMessage_CommonBoxData uIMessage_CommonBoxData = new UIMessage_CommonBoxData(1, Localization.instance.Get("fengmian_lianjietishi4"));
			uIMessage_CommonBoxData.Mark = "SrvCloseSuddenly";
			UIGolbalStaticFun.PopCommonMessageBox(uIMessage_CommonBoxData);
		}
		return true;
	}

	private bool UIErrorLobbyDataPacket(TUITelegram msg)
	{
		Debug.LogError("!!!!!!Error:Lobby data parse error!!!!");
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
