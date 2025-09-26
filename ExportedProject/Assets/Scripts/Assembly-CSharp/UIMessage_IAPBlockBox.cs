using MessageID;
using UnityEngine;

public class UIMessage_IAPBlockBox : UIMessage_BoxEntity
{
	[SerializeField]
	private UILabel _descLabel;

	private void Start()
	{
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_CloseIAPBlockBox, this, CloseIAPBlockBox);
		RegisterMessage(EUIMessageID.UI_ChangeTextIAPBlockBox, this, ChangeTextIAPBlockBox);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_CloseIAPBlockBox, this);
		UnregisterMessage(EUIMessageID.UI_ChangeTextIAPBlockBox, this);
	}

	protected override void Tick()
	{
	}

	private bool CloseIAPBlockBox(TUITelegram msg)
	{
		Object.Destroy(base.gameObject);
		return true;
	}

	private bool ChangeTextIAPBlockBox(TUITelegram msg)
	{
		_descLabel.text = (string)msg._pExtraInfo;
		return true;
	}
}
