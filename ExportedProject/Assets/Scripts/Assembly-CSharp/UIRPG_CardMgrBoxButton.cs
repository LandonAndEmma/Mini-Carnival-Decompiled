using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIRPG_CardMgrBoxButton : UIEntity
{
	[SerializeField]
	private NGUI_COMUI.UI_Box _box;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRPG_CardCompoundWindowBoxClick, this, CompoundWindowBoxClick);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRPG_CardCompoundWindowBoxClick, this);
	}

	protected new virtual void Tick()
	{
	}

	private bool CompoundWindowBoxClick(TUITelegram msg)
	{
		if (_box.BoxData != null && _box.BoxData.ItemId == (ulong)msg._pExtraInfo)
		{
			Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%%  From Right Box:======Item=" + _box.BoxData.ItemId);
			UIMessageDispatch.Instance.PostMessage(EUIMessageID.UIContainer_BoxOnClick, null, _box);
		}
		return true;
	}
}
