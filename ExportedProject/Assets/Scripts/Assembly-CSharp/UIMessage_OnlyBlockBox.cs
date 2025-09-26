using MessageID;
using UnityEngine;

public class UIMessage_OnlyBlockBox : UIMessage_BoxEntity
{
	private void Start()
	{
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_CloseBlockBox, this, CloseBlockBox);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_CloseBlockBox, this);
		CloseBlockBox(null);
	}

	protected override void Tick()
	{
	}

	private bool CloseBlockBox(TUITelegram msg)
	{
		if (msg != null && msg._pExtraInfo != null && base.MsgBoxData.Mark != (string)msg._pExtraInfo)
		{
			return false;
		}
		Object.Destroy(base.gameObject);
		return true;
	}
}
