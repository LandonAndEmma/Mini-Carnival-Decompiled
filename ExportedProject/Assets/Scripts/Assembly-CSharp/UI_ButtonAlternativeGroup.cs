using MessageID;
using UnityEngine;

public class UI_ButtonAlternativeGroup : UIEntity
{
	[SerializeField]
	private GameObject _fstBtn;

	[SerializeField]
	private GameObject _sndBtn;

	[SerializeField]
	private bool _fstSelected = true;

	[SerializeField]
	private int _groupId;

	private UI_ButtonAlternative _fstBtnCmp;

	private UI_ButtonAlternative _sndBtnCmp;

	public int GroupId
	{
		get
		{
			return _groupId;
		}
		set
		{
			_groupId = value;
		}
	}

	private void Awake()
	{
		_fstBtnCmp = _fstBtn.GetComponent<UI_ButtonAlternative>();
		if (_fstBtnCmp == null)
		{
			_fstBtnCmp = _fstBtn.AddComponent<UI_ButtonAlternative>();
		}
		_fstBtnCmp.SetFstBtn();
		_sndBtnCmp = _sndBtn.GetComponent<UI_ButtonAlternative>();
		if (_sndBtnCmp == null)
		{
			_sndBtnCmp = _sndBtn.AddComponent<UI_ButtonAlternative>();
		}
		_fstBtnCmp.SetSelected(_fstSelected);
		_sndBtnCmp.SetSelected(!_fstSelected);
	}

	protected override void Load()
	{
		RegisterMessage(EUIMessageID.UI_ButtonAlternativeClick, this, ButtonAlternativeClick);
	}

	protected override void UnLoad()
	{
		UnregisterMessage(EUIMessageID.UI_ButtonAlternativeClick, this);
	}

	private bool ButtonAlternativeClick(TUITelegram msg)
	{
		bool flag = (bool)msg._pExtraInfo;
		if (flag)
		{
			_sndBtnCmp.SetSelected(false);
		}
		else
		{
			_fstBtnCmp.SetSelected(false);
		}
		UIMessageDispatch.Instance.PostMessage(EUIMessageID.UI_ButtonAlternativeChanged, this, flag ? 1 : 0);
		return true;
	}

	protected override void Tick()
	{
	}
}
