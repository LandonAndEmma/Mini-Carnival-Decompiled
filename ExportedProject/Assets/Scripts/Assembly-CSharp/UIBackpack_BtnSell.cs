using MessageID;
using UnityEngine;

public class UIBackpack_BtnSell : UIEntity
{
	[SerializeField]
	private bool _bChecked;

	[SerializeField]
	private GameObject _objSel;

	[SerializeField]
	private GameObject _objUnsel;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIBackpack_CaptionTypeButtonSelChanged, this, CaptionTypeTypeButtonSelChanged);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIBackpack_CaptionTypeButtonSelChanged, this);
	}

	private bool CaptionTypeTypeButtonSelChanged(TUITelegram msg)
	{
		UIBackpack_ButtonCaptionType.ECaptionType eCaptionType = (UIBackpack_ButtonCaptionType.ECaptionType)(int)msg._pExtraInfo;
		switch (eCaptionType)
		{
		case UIBackpack_ButtonCaptionType.ECaptionType.Backpack_Unsel:
			if (_bChecked)
			{
				_bChecked = false;
			}
			break;
		case UIBackpack_ButtonCaptionType.ECaptionType.Sell_Unsel:
			if (!_bChecked)
			{
				_bChecked = true;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_CaptionTypeSwitched, this, eCaptionType);
			}
			break;
		}
		_objSel.SetActive(_bChecked);
		_objUnsel.SetActive(!_bChecked);
		return true;
	}

	private void Awake()
	{
		_objSel.SetActive(_bChecked);
		_objUnsel.SetActive(!_bChecked);
	}

	protected override void Tick()
	{
	}
}
