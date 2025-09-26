using MessageID;
using UnityEngine;

public class UIJoinGameMgr : UIEntity
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIJoinGameBox_YesClick, this, UIJoinGameBoxYesClick);
		RegisterMessage(EUIMessageID.UIJoinGameBox_NoClick, this, UIJoinGameBoxNoClick);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIJoinGameBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UIJoinGameBox_NoClick, this);
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.transform.gameObject);
	}

	private void Start()
	{
	}

	private bool UIJoinGameBoxYesClick(TUITelegram msg)
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_Notify3DFittingCharc, null, UIMarketFittingRoom3DMgr.EOperType.Hide_Charc);
		if (!COMA_NetworkConnect.Instance.bNetworkConnectLogin)
		{
			Debug.LogError("have not login!!");
		}
		UIJoinGameMessageBoxData uIJoinGameMessageBoxData = msg._pExtraInfo as UIJoinGameMessageBoxData;
		COMA_NetworkConnect.Instance.TryToEnterFriendsRoom(uIJoinGameMessageBoxData.SceneID, uIJoinGameMessageBoxData.RoomID);
		return true;
	}

	private bool UIJoinGameBoxNoClick(TUITelegram msg)
	{
		return true;
	}
}
