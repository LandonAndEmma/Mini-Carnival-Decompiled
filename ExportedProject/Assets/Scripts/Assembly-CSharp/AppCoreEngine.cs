using System;
using LtcpSdk;
using Protocol;
using Protocol.Binary;
using Protocol.Role.C2S;
using UnityEngine;

public class AppCoreEngine : MonoBehaviour
{
	private LtcpSession m_session;

	private void OnConnected()
	{
		Debug.Log("connect success, send login");
		LoginCmd loginCmd = new LoginCmd();
		loginCmd.m_player_id = 1u;
		m_session.SendProtoCmd(loginCmd);
	}

	private void OnConnectError()
	{
	}

	private void OnConnectTimeout()
	{
	}

	private void OnClosed()
	{
	}

	private void OnData(Protocol.Header header, UnPacker unpacker)
	{
		Debug.Log("on data");
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		m_session = new LtcpSession(30f);
		LtcpSession session = m_session;
		session.m_callback_connected = (LtcpSession.OnSessionEventCallback)Delegate.Combine(session.m_callback_connected, new LtcpSession.OnSessionEventCallback(OnConnected));
		LtcpSession session2 = m_session;
		session2.m_callback_connecterror = (LtcpSession.OnSessionEventCallback)Delegate.Combine(session2.m_callback_connecterror, new LtcpSession.OnSessionEventCallback(OnConnectError));
		LtcpSession session3 = m_session;
		session3.m_callback_connecttimeout = (LtcpSession.OnSessionEventCallback)Delegate.Combine(session3.m_callback_connecttimeout, new LtcpSession.OnSessionEventCallback(OnConnectTimeout));
		LtcpSession session4 = m_session;
		session4.m_callback_closed = (LtcpSession.OnSessionEventCallback)Delegate.Combine(session4.m_callback_closed, new LtcpSession.OnSessionEventCallback(OnClosed));
		LtcpSession session5 = m_session;
		session5.m_callback_data = (LtcpSession.OnSessionDataCallback)Delegate.Combine(session5.m_callback_data, new LtcpSession.OnSessionDataCallback(OnData));
		m_session.Connect("192.168.4.252", 20000);
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (m_session != null)
		{
			m_session.Update();
		}
	}

	private void OnApplicationQuit()
	{
		if (m_session != null)
		{
			m_session.Close();
		}
	}
}
