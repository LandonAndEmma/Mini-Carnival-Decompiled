using MessageID;
using UnityEngine;

public class UIMails_ButtonCaption : MonoBehaviour
{
	[SerializeField]
	protected UIMails.ECaptionType _curType;

	private bool _bShouldCheckOnClick;

	public UIMails.ECaptionType CurType
	{
		get
		{
			return _curType;
		}
		set
		{
			_curType = value;
		}
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		_bShouldCheckOnClick = true;
	}

	private void OnDisable()
	{
		_bShouldCheckOnClick = false;
	}

	private void Update()
	{
		if (_bShouldCheckOnClick)
		{
			if (GetComponent<UICheckbox>().startsChecked)
			{
				GetComponent<UICheckbox>().isChecked = true;
				OnClick();
			}
			_bShouldCheckOnClick = false;
		}
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMails_CaptionTypeChanged, null, CurType);
	}
}
