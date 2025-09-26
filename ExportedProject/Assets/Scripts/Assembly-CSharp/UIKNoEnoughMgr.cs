using MessageID;
using UnityEngine;

public class UIKNoEnoughMgr : UIEntity
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "NotEnoughMoney":
			UIMessageDispatch.Instance.SendMessage(EUIMessageID.UI_OpenIAP, null, null);
			break;
		}
		return true;
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	protected override void Tick()
	{
	}
}
