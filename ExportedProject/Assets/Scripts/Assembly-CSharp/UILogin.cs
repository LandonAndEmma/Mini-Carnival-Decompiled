using MessageID;
using UnityEngine;

public class UILogin : UIEntity
{
	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UICOMBox_YesClick, this, OnPopBoxClick_Yes);
		RegisterMessage(EUIMessageID.UICOMBox_NoClick, this, OnPopBoxClick_No);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UICOMBox_YesClick, this);
		UnregisterMessage(EUIMessageID.UICOMBox_NoClick, this);
	}

	private bool OnPopBoxClick_Yes(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "Desc_NeedSync":
			COMA_Login.Instance.NextStep(COMA_Server_ID.Instance.GID);
			break;
		case "Desc_NeedUpdate":
			Application.OpenURL("https://play.google.com/store/apps/details?id=com.trinitigame.android.google.callofminiavatar");
			break;
		case "Desc_NeedDownLoadSupport":
			UIOptions.Support();
			break;
		}
		return true;
	}

	private bool OnPopBoxClick_No(TUITelegram msg)
	{
		UIMessage_CommonBoxData uIMessage_CommonBoxData = msg._pExtraInfo as UIMessage_CommonBoxData;
		Debug.Log(uIMessage_CommonBoxData.MessageBoxID + " " + uIMessage_CommonBoxData.Mark);
		switch (uIMessage_CommonBoxData.Mark)
		{
		case "Desc_NeedSync":
			COMA_Login.Instance.NextStep(COMA_Pref.Instance.playerID);
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

	private void Start()
	{
	}
}
