using MessageID;
using UnityEngine;

public class UIMessage_BlockForTUI : UIMessage_BoxEntity
{
	private void Start()
	{
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_CloseBlockForTUIBox, this, CloseBlockBox);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_CloseBlockForTUIBox, this);
		CloseBlockBox(null);
	}

	protected override void Tick()
	{
	}

	private bool CloseBlockBox(TUITelegram msg)
	{
		Object.Destroy(base.gameObject);
		return true;
	}
}
