using MessageID;
using Protocol.Common;
using UnityEngine;

public class UILobbyHeartbeat : UIEntity
{
	private bool _CanSendHeartbeat;

	private float _preSendHeartbeatTime = -60f;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UILobbyheart_Open, this, OpenLobbyheart);
		RegisterMessage(EUIMessageID.UILobbyheart_Close, this, CloseLobbyheart);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UILobbyheart_Open, this);
		UnregisterMessage(EUIMessageID.UILobbyheart_Close, this);
	}

	private bool OpenLobbyheart(TUITelegram msg)
	{
		_CanSendHeartbeat = true;
		return true;
	}

	private bool CloseLobbyheart(TUITelegram msg)
	{
		Debug.Log("CloseLobbyheart");
		_CanSendHeartbeat = false;
		_preSendHeartbeatTime = -60f;
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
		if (_CanSendHeartbeat && Time.time - _preSendHeartbeatTime >= 60f)
		{
			HeartbeatCmd extraInfo = new HeartbeatCmd();
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_SendMsgToLobbyServer, this, extraInfo);
			_preSendHeartbeatTime = Time.time;
			Debug.Log("-----Send Heartbeat! Time=" + Time.time);
		}
	}
}
