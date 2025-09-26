using MessageID;
using NGUI_COMUI;
using UnityEngine;

public class UIMarket_ButtonTabTop : MonoBehaviour
{
	[SerializeField]
	protected NGUI_COMUI.UIMarket.ETabTopButtonsType _curType;

	public NGUI_COMUI.UIMarket.ETabTopButtonsType CurType
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
		if (GetComponent<UICheckbox>().startsChecked)
		{
			OnClick();
		}
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIMarket_TabTopButtonSelChanged, null, CurType);
	}
}
