using MessageID;
using UnityEngine;

public class UIBackpack_ButtonItemType : MonoBehaviour
{
	[SerializeField]
	protected UIBackpack.EItemType _curType;

	private bool _bShouldCheckOnClick;

	public UIBackpack.EItemType CurType
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

	private void OnEnable()
	{
		_bShouldCheckOnClick = true;
	}

	private void OnDisable()
	{
		_bShouldCheckOnClick = false;
	}

	private void Start()
	{
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
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click_Sort);
		Debug.Log(base.transform.name + "UIBackpack_ItemTypeButtonSelChanged");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIBackpack_ItemTypeButtonSelChanged, null, CurType, base.gameObject);
	}
}
