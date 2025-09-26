using System.Collections.Generic;
using MessageID;
using UnityEngine;

public class UIMessageTrasmitMgr : UIEntity
{
	private static UIMessageTrasmitMgr _instance;

	private Dictionary<EUIMessageID, TUITelegram> lstMSG = new Dictionary<EUIMessageID, TUITelegram>();

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_RequireTransmitMsg, this, Transmit);
	}

	protected override void UnLoad()
	{
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.DestroyObject(base.gameObject);
		}
	}

	private bool Transmit(TUITelegram msg)
	{
		EUIMessageID eUIMessageID = (EUIMessageID)(int)msg._pExtraInfo;
		Debug.Log("Transmit : " + eUIMessageID);
		if (lstMSG.ContainsKey(eUIMessageID))
		{
			UIMessageDispatch.Instance.SendMessage(eUIMessageID, this, lstMSG[eUIMessageID]);
			lstMSG.Remove(eUIMessageID);
		}
		else
		{
			Debug.Log("No Transmit Message u Need!!");
		}
		return true;
	}
}
