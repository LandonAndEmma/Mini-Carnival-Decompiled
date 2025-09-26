using System;
using System.Collections.Generic;
using LtcpSdk;
using MessageID;
using Protocol;
using Protocol.Binary;
using UnityEngine;

public class UILobbySession : UIEntity
{
	private LtcpSession _session;

	private Dictionary<byte, List<UILobbyMessageHandler>> _dictMsgHandler = new Dictionary<byte, List<UILobbyMessageHandler>>();

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_TIDReady, this, TIDReady);
		RegisterMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, SendMsgToLobbyServer);
		RegisterMessage(EUIMessageID.UI_ReconnectingLobby, this, ReconnectingLobby);
		RegisterMessage(EUIMessageID.UI_DisconnectLobby, this, DisconnectLobby);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_TIDReady, this);
		UnregisterMessage(EUIMessageID.UI_SendMsgToLobbyServer, this);
		UnregisterMessage(EUIMessageID.UI_ReconnectingLobby, this);
		UnregisterMessage(EUIMessageID.UI_DisconnectLobby, this);
	}

	private bool TIDReady(TUITelegram msg)
	{
		_session.Connect((string)msg._pExtraInfo, (int)msg._pExtraInfo2);
		return true;
	}

	private bool SendMsgToLobbyServer(TUITelegram msg)
	{
		BaseCmd cmd = (BaseCmd)msg._pExtraInfo;
		if (_session != null)
		{
			_session.SendProtoCmd(cmd);
		}
		else
		{
			Debug.LogError("_session is null!");
		}
		return true;
	}

	private bool ReconnectingLobby(TUITelegram msg)
	{
		InitSession();
		_session.Connect((string)msg._pExtraInfo, (int)msg._pExtraInfo2);
		return true;
	}

	private bool DisconnectLobby(TUITelegram msg)
	{
		if (_session != null)
		{
			_session.Close();
			_session = null;
			Debug.Log("Close Lobby!");
		}
		return true;
	}

	private void TransmitSystemMessage(byte cmd)
	{
		if (!_dictMsgHandler.ContainsKey(byte.MaxValue))
		{
			return;
		}
		Debug.Log("255--------------------");
		List<UILobbyMessageHandler> list = _dictMsgHandler[byte.MaxValue];
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				UILobbyMessageHandler uILobbyMessageHandler = list[i];
				Protocol.Header header = new Protocol.Header();
				header.m_cCmd = cmd;
				uILobbyMessageHandler.HandleLobbyMessage(header, null);
			}
		}
		else
		{
			Debug.LogWarning("System's Handler List is null!");
		}
	}

	private void OnConnected()
	{
		TransmitSystemMessage(0);
		Debug.Log("connect success, send login");
	}

	private void OnConnectError()
	{
		TransmitSystemMessage(1);
	}

	private void OnConnectTimeout()
	{
		TransmitSystemMessage(2);
	}

	private void OnClosed()
	{
		TransmitSystemMessage(3);
	}

	private void OnData(Protocol.Header header, UnPacker unpacker)
	{
		byte cProtocol = header.m_cProtocol;
		if (_dictMsgHandler.ContainsKey(cProtocol))
		{
			List<UILobbyMessageHandler> list = _dictMsgHandler[cProtocol];
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					UILobbyMessageHandler uILobbyMessageHandler = list[i];
					uILobbyMessageHandler.HandleLobbyMessage(header, unpacker);
				}
			}
			else
			{
				Debug.LogWarning(string.Concat((ProtoID)header.m_cProtocol, "'s Handler List is null!"));
			}
		}
		else
		{
			Debug.LogWarning(string.Concat((ProtoID)header.m_cProtocol, "has not Handler!"));
		}
	}

	private void InitSession()
	{
		_session = new LtcpSession(30f);
		LtcpSession session = _session;
		session.m_callback_connected = (LtcpSession.OnSessionEventCallback)Delegate.Combine(session.m_callback_connected, new LtcpSession.OnSessionEventCallback(OnConnected));
		LtcpSession session2 = _session;
		session2.m_callback_connecterror = (LtcpSession.OnSessionEventCallback)Delegate.Combine(session2.m_callback_connecterror, new LtcpSession.OnSessionEventCallback(OnConnectError));
		LtcpSession session3 = _session;
		session3.m_callback_connecttimeout = (LtcpSession.OnSessionEventCallback)Delegate.Combine(session3.m_callback_connecttimeout, new LtcpSession.OnSessionEventCallback(OnConnectTimeout));
		LtcpSession session4 = _session;
		session4.m_callback_closed = (LtcpSession.OnSessionEventCallback)Delegate.Combine(session4.m_callback_closed, new LtcpSession.OnSessionEventCallback(OnClosed));
		LtcpSession session5 = _session;
		session5.m_callback_data = (LtcpSession.OnSessionDataCallback)Delegate.Combine(session5.m_callback_data, new LtcpSession.OnSessionDataCallback(OnData));
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		InitSession();
	}

	private void Start()
	{
	}

	protected override void Tick()
	{
		if (_session != null)
		{
			_session.Update();
		}
	}

	private void OnApplicationQuit()
	{
		if (_session != null)
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_DisconnectLobby, null, null);
		}
	}

	public int RegisterHanlder(byte id, UILobbyMessageHandler handler)
	{
		if (_dictMsgHandler.ContainsKey(id))
		{
			List<UILobbyMessageHandler> list = _dictMsgHandler[id];
			if (list == null)
			{
				list = new List<UILobbyMessageHandler>();
			}
			list.Add(handler);
		}
		else
		{
			List<UILobbyMessageHandler> list2 = new List<UILobbyMessageHandler>();
			list2.Add(handler);
			_dictMsgHandler.Add(id, list2);
		}
		return 0;
	}

	public int UnregisterHanlder(byte id, UILobbyMessageHandler handler)
	{
		if (_dictMsgHandler.ContainsKey(id))
		{
			List<UILobbyMessageHandler> list = _dictMsgHandler[id];
			if (list != null)
			{
				list.Remove(handler);
			}
		}
		return 0;
	}
}
