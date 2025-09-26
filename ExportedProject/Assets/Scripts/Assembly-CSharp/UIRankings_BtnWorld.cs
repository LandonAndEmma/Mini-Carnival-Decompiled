using MessageID;
using UnityEngine;

public class UIRankings_BtnWorld : UIEntity
{
	[SerializeField]
	private bool _bChecked;

	[SerializeField]
	private GameObject _objSel;

	[SerializeField]
	private GameObject _objUnsel;

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UIRankings_CaptionTypeButtonSelChanged, this, CaptionTypeTypeButtonSelChanged);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UIRankings_CaptionTypeButtonSelChanged, this);
	}

	private bool CaptionTypeTypeButtonSelChanged(TUITelegram msg)
	{
		UIRankings_ButtonCaptionType.ECaptionType eCaptionType = (UIRankings_ButtonCaptionType.ECaptionType)(int)msg._pExtraInfo;
		switch (eCaptionType)
		{
		case UIRankings_ButtonCaptionType.ECaptionType.World_Unsel:
			if (!_bChecked)
			{
				_bChecked = true;
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIRankings_CaptionTypeSwitched, this, eCaptionType);
			}
			break;
		case UIRankings_ButtonCaptionType.ECaptionType.Friend_Unsel:
			if (_bChecked)
			{
				_bChecked = false;
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
