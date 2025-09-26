using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIMarket_ButtonTabLeft : MonoBehaviour
{
	[SerializeField]
	protected NGUI_COMUI.UIMarket.ETabLeftButtonsType _curType;

	private bool _bShouldCheckOnClick;

	public NGUI_COMUI.UIMarket.ETabLeftButtonsType CurType
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
				OnClick();
			}
			_bShouldCheckOnClick = false;
		}
	}

	private void OnClick()
	{
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_TabLeftButtonSelChanged, null, CurType, base.gameObject);
	}
}
