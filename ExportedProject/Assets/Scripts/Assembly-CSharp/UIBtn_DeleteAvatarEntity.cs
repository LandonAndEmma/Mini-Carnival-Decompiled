using MessageID;
using UnityEngine;

public class UIBtn_DeleteAvatarEntity : UIEntity
{
	[SerializeField]
	private GameObject _objShow;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIBackpack_SellListSelChanged, this, SellListSelChanged);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIBackpack_SellListSelChanged, this);
	}

	private bool SellListSelChanged(TUITelegram msg)
	{
		UIBackpack_SellInfoBoxData uIBackpack_SellInfoBoxData = (UIBackpack_SellInfoBoxData)msg._pExtraInfo;
		if (uIBackpack_SellInfoBoxData != null)
		{
			_objShow.SetActive(true);
		}
		else
		{
			_objShow.SetActive(false);
		}
		return true;
	}

	private void Awake()
	{
	}

	protected override void Tick()
	{
	}
}
