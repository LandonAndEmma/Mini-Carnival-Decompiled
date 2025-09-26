using System.Collections.Generic;
using MC_UIToolKit;
using MessageID;
using Protocol;
using Protocol.Binary;
using Protocol.Hall.C2S;
using Protocol.Hall.S2C;
using UnityEngine;

public class UILobby_HallProtocolProcessor : UILobbyMessageHandler
{
	private Dictionary<uint, NotifyEnterHallCmd> _playerIDsInHall = new Dictionary<uint, NotifyEnterHallCmd>();

	private bool _isSendChangeSquare;

	private float _fResetTime;

	protected override void Load()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.RegisterHanlder(3, this);
			OnMessage(1, OnEnterHallResult);
			OnMessage(2, OnNotifyLeave);
			OnMessage(3, OnNotifyEnter);
			OnMessage(5, OnRoleMove);
			OnMessage(8, OnRelayData);
		}
		RegisterMessage(EUIMessageID.UI_Hall_RequestAllIDs, this, OnRequestAllIDsInHall);
		RegisterMessage(EUIMessageID.UI_Hall_ChangeSquare_Kamcord, this, Hall_ChangeSquare);
	}

	protected override void UnLoad()
	{
		UILobbySession component = base.transform.root.GetComponent<UILobbySession>();
		if (component != null)
		{
			component.UnregisterHanlder(3, this);
		}
		UnregisterMessage(EUIMessageID.UI_Hall_RequestAllIDs, this);
		UnregisterMessage(EUIMessageID.UI_Hall_ChangeSquare_Kamcord, this);
	}

	public bool OnEnterHallResult(UnPacker unpacker)
	{
		Debug.Log(" ===========OnEnterHallResult");
		EnterHallResultCmd enterHallResultCmd = new EnterHallResultCmd();
		if (!enterHallResultCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (enterHallResultCmd.m_result == 0)
		{
			_playerIDsInHall.Clear();
			_isSendChangeSquare = false;
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Hall_ChangeSquare, null, null);
		}
		else if (enterHallResultCmd.m_result == 1)
		{
			Debug.Log("Not Change Room!!");
			if (!_isSendChangeSquare)
			{
				string str = Localization.instance.Get("guangchang_desc3");
				UIGolbalStaticFun.PopupTipsBox(str);
			}
		}
		return true;
	}

	public bool OnNotifyEnter(UnPacker unpacker)
	{
		NotifyEnterHallCmd notifyEnterHallCmd = new NotifyEnterHallCmd();
		if (!notifyEnterHallCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		_playerIDsInHall.Add(notifyEnterHallCmd.m_info.m_player_id, notifyEnterHallCmd);
		WatchRoleInfo info = notifyEnterHallCmd.m_info;
		Position pos = notifyEnterHallCmd.m_pos;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Hall_RoleEnter, this, info, pos);
		return true;
	}

	public bool OnNotifyLeave(UnPacker unpacker)
	{
		NotifyLeaveHallCmd notifyLeaveHallCmd = new NotifyLeaveHallCmd();
		if (!notifyLeaveHallCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		if (_playerIDsInHall.ContainsKey(notifyLeaveHallCmd.m_who))
		{
			_playerIDsInHall.Remove(notifyLeaveHallCmd.m_who);
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Hall_RoleLeave, this, notifyLeaveHallCmd.m_who);
		return true;
	}

	public bool OnRoleMove(UnPacker unpacker)
	{
		NotifyMoveRoleCmd notifyMoveRoleCmd = new NotifyMoveRoleCmd();
		if (!notifyMoveRoleCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		Position pos = notifyMoveRoleCmd.m_pos;
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Hall_RoleMove, this, notifyMoveRoleCmd.m_who, pos);
		if (_playerIDsInHall.ContainsKey(notifyMoveRoleCmd.m_who))
		{
			_playerIDsInHall[notifyMoveRoleCmd.m_who].m_pos = pos;
		}
		return true;
	}

	public bool OnRelayData(UnPacker unpacker)
	{
		NotifyRelayDataCmd notifyRelayDataCmd = new NotifyRelayDataCmd();
		if (!notifyRelayDataCmd.Parse(unpacker))
		{
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIError_LobbyDataPacket, this, null);
			return true;
		}
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Hall_RoleSit, this, notifyRelayDataCmd);
		return true;
	}

	private bool OnRequestAllIDsInHall(TUITelegram msg)
	{
		return true;
	}

	private bool Hall_ChangeSquare(TUITelegram msg)
	{
		_playerIDsInHall.Clear();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_Hall_ChangeSquare, null, null);
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}

	protected void FixedUpdate()
	{
		if (_playerIDsInHall.Count > 30 && true && !_isSendChangeSquare)
		{
			_isSendChangeSquare = true;
			_fResetTime = Time.time;
			EnterHallCmd extraInfo = new EnterHallCmd();
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_SendMsgToLobbyServer, null, extraInfo);
		}
		if (_isSendChangeSquare && Time.time - _fResetTime > 900f)
		{
			_isSendChangeSquare = false;
		}
	}
}
