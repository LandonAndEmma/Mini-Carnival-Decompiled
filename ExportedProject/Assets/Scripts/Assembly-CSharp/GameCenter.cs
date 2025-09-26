using System;
using UnityEngine;

public class GameCenter : MonoBehaviour
{
	private static GameCenter _instance;

	private static string m_strGameCenterId = string.Empty;

	private static bool m_bLoginGameCenter = true;

	private bool bBindingGameCenter;

	private Action<string> handler_loginComplete;

	private Action<string> handler_bindComplete;

	public static GameCenter Instance
	{
		get
		{
			return _instance;
		}
	}

	public string gameCenterID
	{
		get
		{
			return m_strGameCenterId;
		}
		set
		{
			m_strGameCenterId = value;
		}
	}

	private void Awake()
	{
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		GameCenterPlugin.Initialize();
		GameCenterPlugin.Login();
	}

	private void Update()
	{
		if (m_bLoginGameCenter)
		{
			switch (GameCenterPlugin.LoginStatus())
			{
			case GameCenterPlugin.LOGIN_STATUS.LOGIN_STATUS_SUCCESS:
				m_bLoginGameCenter = false;
				m_strGameCenterId = GameCenterPlugin.GetAccount();
				Debug.Log("Login GameCenter ---------------------------------------------- Success:" + m_strGameCenterId);
				GameCenterPlugin.DownloadSelfPhoto();
				if (handler_loginComplete != null)
				{
					handler_loginComplete("Success");
				}
				break;
			case GameCenterPlugin.LOGIN_STATUS.LOGIN_STATUS_ERROR:
				m_bLoginGameCenter = false;
				m_strGameCenterId = string.Empty;
				Debug.Log("Login GameCenter ---------------------------------------------- Error!!");
				if (handler_loginComplete != null)
				{
					handler_loginComplete("Fail");
				}
				break;
			case GameCenterPlugin.LOGIN_STATUS.LOGIN_STATUS_WAIT:
				Debug.Log("Login GameCenter ---------------------------------------------- Wait...");
				break;
			case GameCenterPlugin.LOGIN_STATUS.LOGIN_STATUS_IDLE:
				Debug.Log("Login GameCenter ---------------------------------------------- Idle.");
				break;
			}
		}
		if (!bBindingGameCenter)
		{
			return;
		}
		switch (GameCenterPlugin.LoginStatus())
		{
		case GameCenterPlugin.LOGIN_STATUS.LOGIN_STATUS_SUCCESS:
			bBindingGameCenter = false;
			m_strGameCenterId = GameCenterPlugin.GetAccount();
			if (handler_bindComplete != null)
			{
				handler_bindComplete("Success");
			}
			break;
		case GameCenterPlugin.LOGIN_STATUS.LOGIN_STATUS_ERROR:
			if (handler_bindComplete != null)
			{
				handler_bindComplete("Fail");
			}
			break;
		}
	}

	public void GCLogin(Action<string> completionHandler)
	{
		handler_loginComplete = completionHandler;
		if (GameCenterPlugin.IsSupported())
		{
			if (m_strGameCenterId != string.Empty)
			{
				handler_loginComplete("Success");
			}
			else if (!m_bLoginGameCenter)
			{
				handler_loginComplete("Fail");
			}
		}
		else
		{
			m_bLoginGameCenter = false;
			handler_loginComplete("Fail");
		}
	}

	private void GCCheck(Action<string> completeHandler)
	{
		handler_bindComplete = completeHandler;
		if (GameCenterPlugin.IsSupported())
		{
			Debug.Log("GC -----------> Check & Bind!!");
			bBindingGameCenter = true;
		}
		else
		{
			handler_bindComplete("Fail");
		}
	}
}
